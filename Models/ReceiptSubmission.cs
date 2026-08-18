using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshRewardsApi.Models
{
    /// <summary>
    /// Self-service punch claim using details printed on an existing till
    /// slip. The Reference+Amount+Date combination must be unique so the
    /// same slip can never be claimed twice — no POS integration required.
    /// </summary>
    public class ReceiptSubmission
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

        [Required, MaxLength(50)]
        public string ReceiptReference { get; set; } = string.Empty;

        [Required, Column(TypeName = "decimal(8,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateOnly PurchaseDate { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}