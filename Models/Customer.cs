using System.ComponentModel.DataAnnotations;

namespace NooshRewardsApi.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}