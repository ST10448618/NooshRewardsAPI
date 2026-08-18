using Microsoft.AspNetCore.Mvc;
using NooshRewardsApi.Auth;
using NooshRewardsApi.Controllers.Dtos;
using NooshRewardsApi.Services.Interfaces;

namespace NooshRewardsApi.Controllers
{
    [ApiController]
    [Route("api/rewards")]
    public class RewardsController : ControllerBase
    {
        private readonly IRewardsService _rewardsService;
        public RewardsController(IRewardsService rewardsService) { _rewardsService = rewardsService; }

        private string GetVerifiedPhoneNumber() =>
            HttpContext.Items["VerifiedPhoneNumber"] as string
            ?? throw new InvalidOperationException("Phone number not verified.");

        [HttpPost("generate-qr")]
        [ServiceFilter(typeof(FirebaseAuthFilter))]
        public async Task<IActionResult> GenerateQr([FromBody] GenerateQrRequest request)
        {
            var phoneNumber = GetVerifiedPhoneNumber();

            var (token, qrImageBase64) = await _rewardsService.GenerateQrAsync(
                phoneNumber, request.FullName, request.RewardRuleId);

            return Ok(new
            {
                token,
                qrImageBase64,
                qrImageDataUrl = $"data:image/png;base64,{qrImageBase64}"
            });
        }

        [HttpPost("scan")]
        [ServiceFilter(typeof(StaffPinFilter))]
        public async Task<IActionResult> Scan([FromBody] ScanRequest request)
        {
            var result = await _rewardsService.RedeemScanTokenAsync(request.Token);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("submit-receipt")]
        [ServiceFilter(typeof(FirebaseAuthFilter))]
        public async Task<IActionResult> SubmitReceipt([FromBody] SubmitReceiptRequest request)
        {
            var phoneNumber = GetVerifiedPhoneNumber();

            var result = await _rewardsService.SubmitReceiptAsync(
                phoneNumber, request.FullName, request.RewardRuleId,
                request.ReceiptReference, request.AmountPaid, request.PurchaseDate);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("claim")]
        [ServiceFilter(typeof(StaffPinFilter))]
        public async Task<IActionResult> Claim([FromBody] ClaimRequest request)
        {
            // Staff perform the claim in-person after verifying the customer's
            // card on-screen, so this stays staff-authenticated, not customer-authenticated.
            return BadRequest(new { message = "Claim requires a customer phone number — see ClaimByPhone." });
        }

        [HttpPost("claim-by-phone")]
        [ServiceFilter(typeof(StaffPinFilter))]
        public async Task<IActionResult> ClaimByPhone([FromBody] StaffClaimRequest request)
        {
            var result = await _rewardsService.ClaimRewardAsync(request.PhoneNumber, request.RewardRuleId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }

    public class StaffClaimRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public int RewardRuleId { get; set; }
    }
}