using NooshRewardsApi.Services.Dtos;

namespace NooshRewardsApi.Services.Interfaces
{
    public interface IRewardsService
    {
        Task<(string qrToken, string qrImageBase64)> GenerateQrAsync(string phoneNumber, string? fullName, int rewardRuleId);
        Task<PunchResultDto> RedeemScanTokenAsync(string token);
        Task<PunchResultDto> SubmitReceiptAsync(string phoneNumber, string? fullName, int rewardRuleId,
            string receiptReference, decimal amountPaid, DateOnly purchaseDate);
        Task<PunchResultDto> ClaimRewardAsync(string phoneNumber, int rewardRuleId);
    }
}