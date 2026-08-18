using NooshRewardsApi.Models;

namespace NooshRewardsApi.Repositories.Interfaces
{
    public interface IScanTokenRepository
    {
        Task<ScanToken> CreateAsync(int customerId, int rewardRuleId, TimeSpan validFor);
        Task<ScanToken?> GetByTokenAsync(string token);
        Task MarkUsedAsync(ScanToken token);
    }
}