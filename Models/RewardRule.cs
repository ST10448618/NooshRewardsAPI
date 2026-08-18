using System.ComponentModel.DataAnnotations;

namespace NooshRewardsApi.Models
{
    /// <summary>Defines one punch-card reward, e.g. "Buy 5 Shawarmas Get 1 Free".</summary>
    public class RewardRule
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int RequiredPunches { get; set; }

        [Required, MaxLength(200)]
        public string RewardDescription { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}