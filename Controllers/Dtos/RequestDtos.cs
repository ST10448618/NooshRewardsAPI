namespace NooshRewardsApi.Controllers.Dtos
{
    public class GenerateQrRequest
    {
        public string? FullName { get; set; }
        public int RewardRuleId { get; set; }
    }

    public class ScanRequest
    {
        public string Token { get; set; } = string.Empty;
    }

    public class SubmitReceiptRequest
    {
        public string? FullName { get; set; }
        public int RewardRuleId { get; set; }
        public string ReceiptReference { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public DateOnly PurchaseDate { get; set; }
    }

    public class ClaimRequest
    {
        public int RewardRuleId { get; set; }
    }

    public class CreateRewardRuleRequest
    {
        public string Name { get; set; } = string.Empty;
        public int RequiredPunches { get; set; }
        public string RewardDescription { get; set; } = string.Empty;
    }
}