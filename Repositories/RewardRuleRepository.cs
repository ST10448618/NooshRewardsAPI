using Microsoft.EntityFrameworkCore;
using NooshRewardsApi.Data;
using NooshRewardsApi.Models;
using NooshRewardsApi.Repositories.Interfaces;

namespace NooshRewardsApi.Repositories
{
    public class RewardRuleRepository : IRewardRuleRepository
    {
        private readonly RewardsDbContext _context;
        public RewardRuleRepository(RewardsDbContext context) { _context = context; }

        public async Task<List<RewardRule>> GetActiveAsync() =>
            await _context.RewardRules.Where(r => r.IsActive).ToListAsync();

        public async Task<RewardRule?> GetByIdAsync(int id) =>
            await _context.RewardRules.FindAsync(id);

        public async Task<RewardRule> CreateAsync(RewardRule rule)
        {
            await _context.RewardRules.AddAsync(rule);
            await _context.SaveChangesAsync();
            return rule;
        }
    }
}