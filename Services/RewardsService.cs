using NooshRewardsApi.Models;
using NooshRewardsApi.Repositories.Interfaces;
using NooshRewardsApi.Services.Dtos;
using NooshRewardsApi.Services.Interfaces;
using QRCoder;

namespace NooshRewardsApi.Services
{
    public class RewardsService : IRewardsService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRewardRuleRepository _rewardRuleRepository;
        private readonly IPunchCardRepository _punchCardRepository;
        private readonly IScanTokenRepository _scanTokenRepository;
        private readonly IReceiptSubmissionRepository _receiptSubmissionRepository;

        private static readonly TimeSpan QrTokenLifetime = TimeSpan.FromMinutes(2);

        public RewardsService(
            ICustomerRepository customerRepository,
            IRewardRuleRepository rewardRuleRepository,
            IPunchCardRepository punchCardRepository,
            IScanTokenRepository scanTokenRepository,
            IReceiptSubmissionRepository receiptSubmissionRepository)
        {
            _customerRepository = customerRepository;
            _rewardRuleRepository = rewardRuleRepository;
            _punchCardRepository = punchCardRepository;
            _scanTokenRepository = scanTokenRepository;
            _receiptSubmissionRepository = receiptSubmissionRepository;
        }

        private async Task<Customer> GetOrCreateCustomerAsync(string phoneNumber, string? fullName)
        {
            var customer = await _customerRepository.GetByPhoneNumberAsync(phoneNumber);
            return customer ?? await _customerRepository.CreateAsync(phoneNumber, fullName);
        }

        // ---------- Pathway A: Staff-scanned QR ----------

        public async Task<(string qrToken, string qrImageBase64)> GenerateQrAsync(
            string phoneNumber, string? fullName, int rewardRuleId)
        {
            var customer = await GetOrCreateCustomerAsync(phoneNumber, fullName);
            var token = await _scanTokenRepository.CreateAsync(customer.Id, rewardRuleId, QrTokenLifetime);

            using var generator = new QRCodeGenerator();
            using var qrData = generator.CreateQrCode(token.Token, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrData);
            var qrBytes = pngQr.GetGraphic(10);

            return (token.Token, Convert.ToBase64String(qrBytes));
        }

        public async Task<PunchResultDto> RedeemScanTokenAsync(string token)
        {
            var scanToken = await _scanTokenRepository.GetByTokenAsync(token);

            if (scanToken == null) return Fail("This QR code is not recognized.");
            if (scanToken.IsUsed) return Fail("This QR code has already been used.");
            if (DateTime.UtcNow > scanToken.ExpiresAt) return Fail("This QR code has expired. Please generate a new one.");

            await _scanTokenRepository.MarkUsedAsync(scanToken);
            return await AddPunchAsync(scanToken.CustomerId, scanToken.RewardRuleId, PunchSource.StaffScan);
        }

        // ---------- Pathway B: Self-service receipt entry ----------

        public async Task<PunchResultDto> SubmitReceiptAsync(
            string phoneNumber, string? fullName, int rewardRuleId,
            string receiptReference, decimal amountPaid, DateOnly purchaseDate)
        {
            var alreadyUsed = await _receiptSubmissionRepository.ExistsAsync(receiptReference, amountPaid, purchaseDate);
            if (alreadyUsed) return Fail("This receipt has already been used to claim a punch.");

            var customer = await GetOrCreateCustomerAsync(phoneNumber, fullName);

            await _receiptSubmissionRepository.AddAsync(new ReceiptSubmission
            {
                CustomerId = customer.Id,
                RewardRuleId = rewardRuleId,
                ReceiptReference = receiptReference,
                AmountPaid = amountPaid,
                PurchaseDate = purchaseDate
            });

            return await AddPunchAsync(customer.Id, rewardRuleId, PunchSource.SelfService);
        }

        // ---------- Shared punch logic — both pathways end up here ----------

        private async Task<PunchResultDto> AddPunchAsync(int customerId, int rewardRuleId, PunchSource source)
        {
            var rule = await _rewardRuleRepository.GetByIdAsync(rewardRuleId);
            if (rule == null || !rule.IsActive) return Fail("This reward is not currently available.");

            var card = await _punchCardRepository.GetOrCreateAsync(customerId, rewardRuleId);
            card.CurrentPunches += 1;
            var rewardUnlocked = card.CurrentPunches >= rule.RequiredPunches;

            await _punchCardRepository.SaveAsync(card);
            await _punchCardRepository.AddLogAsync(new PunchLog
            {
                PunchCardId = card.Id,
                EventType = "Punch",
                Source = source
            });

            return new PunchResultDto
            {
                Success = true,
                Message = rewardUnlocked
                    ? $"Punch added! Reward unlocked: {rule.RewardDescription}"
                    : $"Punch added! {card.CurrentPunches}/{rule.RequiredPunches} toward {rule.RewardDescription}.",
                CurrentPunches = card.CurrentPunches,
                RequiredPunches = rule.RequiredPunches,
                RewardUnlocked = rewardUnlocked,
                RewardDescription = rule.RewardDescription
            };
        }

        // ---------- Claiming a completed reward (resets the card) ----------

        public async Task<PunchResultDto> ClaimRewardAsync(string phoneNumber, int rewardRuleId)
        {
            var customer = await _customerRepository.GetByPhoneNumberAsync(phoneNumber);
            if (customer == null) return Fail("Customer not found.");

            var rule = await _rewardRuleRepository.GetByIdAsync(rewardRuleId);
            if (rule == null) return Fail("Reward not found.");

            var card = await _punchCardRepository.GetAsync(customer.Id, rewardRuleId);
            if (card == null || card.CurrentPunches < rule.RequiredPunches)
                return Fail("This reward has not been earned yet.");

            card.CurrentPunches = 0;
            card.TimesRedeemed += 1;
            await _punchCardRepository.SaveAsync(card);
            await _punchCardRepository.AddLogAsync(new PunchLog
            {
                PunchCardId = card.Id,
                EventType = "Redeemed",
                Source = PunchSource.StaffScan
            });

            return new PunchResultDto
            {
                Success = true,
                Message = $"Reward claimed: {rule.RewardDescription}. Card reset for next round.",
                CurrentPunches = 0,
                RequiredPunches = rule.RequiredPunches,
                RewardUnlocked = false,
                RewardDescription = rule.RewardDescription
            };
        }

        private PunchResultDto Fail(string message) => new PunchResultDto { Success = false, Message = message };
    }
}