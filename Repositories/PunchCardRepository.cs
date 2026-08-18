using Microsoft.EntityFrameworkCore;
using NooshRewardsApi.Data;
using NooshRewardsApi.Models;
using NooshRewardsApi.Repositories.Interfaces;

namespace NooshRewardsApi.Repositories
{
    public class PunchCardRepository : IPunchCardRepository
    {
        private readonly RewardsDbContext _context;
        public PunchCardRepository(RewardsDbContext context) { _context = context; }

        public async Task<PunchCard?> GetAsync(int customerId, int rewardRuleId) =>
            await _context.PunchCards
                .Include(p => p.RewardRule)
                .FirstOrDefaultAsync(p => p.CustomerId == customerId && p.RewardRuleId == rewardRuleId);

        public async Task<PunchCard> GetOrCreateAsync(int customerId, int rewardRuleId)
        {
            var existing = await GetAsync(customerId, rewardRuleId);
            if (existing != null) return existing;

            var card = new PunchCard { CustomerId = customerId, RewardRuleId = rewardRuleId };
            await _context.PunchCards.AddAsync(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task SaveAsync(PunchCard card)
        {
            card.LastUpdatedAt = DateTime.UtcNow;
            _context.PunchCards.Update(card);
            await _context.SaveChangesAsync();
        }

        public async Task AddLogAsync(PunchLog log)
        {
            await _context.PunchLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}