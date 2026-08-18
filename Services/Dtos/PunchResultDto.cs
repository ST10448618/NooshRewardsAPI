namespace NooshRewardsApi.Services.Dtos
{
    public class PunchResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int CurrentPunches { get; set; }
        public int RequiredPunches { get; set; }
        public bool RewardUnlocked { get; set; }
        public string? RewardDescription { get; set; }
    }
}