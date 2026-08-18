using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NooshRewardsApi.Auth
{
    public class AdminKeyFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;
        public AdminKeyFilter(IConfiguration configuration) { _configuration = configuration; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var providedKey = context.HttpContext.Request.Headers["X-Admin-Key"].ToString();
            var actualKey = _configuration["Admin:ApiKey"];

            if (string.IsNullOrEmpty(providedKey) || providedKey != actualKey)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid admin key." });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}