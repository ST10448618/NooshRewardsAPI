using NooshRewardsApi.Models;

namespace NooshRewardsApi.Repositories.Interfaces
{
    public interface IPunchCardRepository
    {
        Task<PunchCard?> GetAsync(int customerId, int rewardRuleId);
        Task<PunchCard> GetOrCreateAsync(int customerId, int rewardRuleId);
        Task SaveAsync(PunchCard card);
        Task AddLogAsync(PunchLog log);
    }
}