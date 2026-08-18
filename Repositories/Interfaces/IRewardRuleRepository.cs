using NooshRewardsApi.Models;

namespace NooshRewardsApi.Repositories.Interfaces
{
    public interface IRewardRuleRepository
    {
        Task<RewardRule> CreateAsync(RewardRule rule);
        Task<List<RewardRule>> GetActiveAsync();
        Task<RewardRule?> GetByIdAsync(int id);
    }
}