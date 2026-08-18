using NooshRewardsApi.Models;

namespace NooshRewardsApi.Data
{
    public static class DbSeeder
    {
        public static void Seed(RewardsDbContext context)
        {
            if (context.RewardRules.Any()) return;

            context.RewardRules.Add(new RewardRule
            {
                Name = "Shawarma Punch Card",
                RequiredPunches = 5,
                RewardDescription = "1 Free Shawarma",
                IsActive = true
            });

            context.SaveChanges();
        }
    }
}