using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NooshRewardsApi.Auth
{
    /// <summary>
    /// Simple shared-PIN gate for staff-only endpoints (scanning, claiming).
    /// Not full role-based auth — appropriate for a single small business
    /// with one till/one staff group, not a multi-tenant platform.
    /// </summary>
    public class StaffPinFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;
        public StaffPinFilter(IConfiguration configuration) { _configuration = configuration; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var providedPin = context.HttpContext.Request.Headers["X-Staff-Pin"].ToString();
            var actualPin = _configuration["Staff:ScanPin"];

            if (string.IsNullOrEmpty(providedPin) || providedPin != actualPin)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid staff PIN." });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}