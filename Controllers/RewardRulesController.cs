using Microsoft.AspNetCore.Mvc;
using NooshRewardsApi.Auth;
using NooshRewardsApi.Controllers.Dtos;
using NooshRewardsApi.Models;
using NooshRewardsApi.Repositories.Interfaces;

namespace NooshRewardsApi.Controllers
{
    [ApiController]
    [Route("api/reward-rules")]
    public class RewardRulesController : ControllerBase
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;
        public RewardRulesController(IRewardRuleRepository rewardRuleRepository)
        {
            _rewardRuleRepository = rewardRuleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            var rules = await _rewardRuleRepository.GetActiveAsync();
            return Ok(rules);
        }

        [HttpPost]
        [ServiceFilter(typeof(AdminKeyFilter))]
        public async Task<IActionResult> Create([FromBody] CreateRewardRuleRequest request)
        {
            var rule = new RewardRule
            {
                Name = request.Name,
                RequiredPunches = request.RequiredPunches,
                RewardDescription = request.RewardDescription,
                IsActive = true
            };

            var created = await _rewardRuleRepository.CreateAsync(rule);
            return Ok(created);
        }
    }
}