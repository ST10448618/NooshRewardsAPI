using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshRewardsApi.Models
{
    public enum PunchSource
    {
        StaffScan,
        SelfService
    }

    public class PunchLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PunchCardId { get; set; }
        [ForeignKey(nameof(PunchCardId))]
        public PunchCard? PunchCard { get; set; }

        public DateTime ScannedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string EventType { get; set; } = "Punch"; // "Punch" or "Redeemed"

        public PunchSource Source { get; set; } = PunchSource.StaffScan;
    }
}