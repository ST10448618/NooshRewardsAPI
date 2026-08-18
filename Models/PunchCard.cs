using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshRewardsApi.Models
{
    /// <summary>One customer's live progress toward one specific RewardRule.</summary>
    public class PunchCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        [Required]
        public int RewardRuleId { get; set; }
        [ForeignKey(nameof(RewardRuleId))]
        public RewardRule? RewardRule { get; set; }

        public int CurrentPunches { get; set; } = 0;
        public int TimesRedeemed { get; set; } = 0;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}