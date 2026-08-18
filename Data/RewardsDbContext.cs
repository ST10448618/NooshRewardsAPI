using Microsoft.EntityFrameworkCore;
using NooshRewardsApi.Models;

namespace NooshRewardsApi.Data
{
    public class RewardsDbContext : DbContext
    {
        public RewardsDbContext(DbContextOptions<RewardsDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<RewardRule> RewardRules { get; set; }
        public DbSet<PunchCard> PunchCards { get; set; }
        public DbSet<ScanToken> ScanTokens { get; set; }
        public DbSet<ReceiptSubmission> ReceiptSubmissions { get; set; }
        public DbSet<PunchLog> PunchLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber).IsUnique();

            modelBuilder.Entity<PunchCard>()
                .HasIndex(p => new { p.CustomerId, p.RewardRuleId }).IsUnique();

            modelBuilder.Entity<ScanToken>()
                .HasIndex(t => t.Token).IsUnique();

            modelBuilder.Entity<ReceiptSubmission>()
                .HasIndex(r => new { r.ReceiptReference, r.AmountPaid, r.PurchaseDate }).IsUnique();
        }
    }
}