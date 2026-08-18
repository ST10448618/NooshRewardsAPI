using Microsoft.EntityFrameworkCore;
using NooshRewardsApi.Data;
using NooshRewardsApi.Models;
using NooshRewardsApi.Repositories.Interfaces;

namespace NooshRewardsApi.Repositories
{
    public class ScanTokenRepository : IScanTokenRepository
    {
        private readonly RewardsDbContext _context;
        public ScanTokenRepository(RewardsDbContext context) { _context = context; }

        public async Task<ScanToken> CreateAsync(int customerId, int rewardRuleId, TimeSpan validFor)
        {
            var token = new ScanToken
            {
                Token = Guid.NewGuid().ToString("N"),
                CustomerId = customerId,
                RewardRuleId = rewardRuleId,
                ExpiresAt = DateTime.UtcNow.Add(validFor)
            };
            await _context.ScanTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<ScanToken?> GetByTokenAsync(string token) =>
            await _context.ScanTokens
                .Include(t => t.Customer)
                .Include(t => t.RewardRule)
                .FirstOrDefaultAsync(t => t.Token == token);

        public async Task MarkUsedAsync(ScanToken token)
        {
            token.IsUsed = true;
            _context.ScanTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}