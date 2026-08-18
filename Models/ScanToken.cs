using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshRewardsApi.Models
{
    /// <summary>Short-lived, single-use token encoded into the customer's QR code.</summary>
    public class ScanToken
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(40)]
        public string Token { get; set; } = string.Empty;

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        [Required]
        public int RewardRuleId { get; set; }
        [ForeignKey(nameof(RewardRuleId))]
        public RewardRule? RewardRule { get; set; }

        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}