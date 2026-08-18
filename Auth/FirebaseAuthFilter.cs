using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NooshRewardsApi.Auth
{
    /// <summary>
    /// Verifies a Firebase ID token on every request to a decorated action.
    /// The verified phone number is attached to HttpContext.Items so
    /// Controllers never need to trust a client-supplied phone number again.
    /// </summary>
    public class FirebaseAuthFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Missing or invalid Authorization header." });
                return;
            }

            var idToken = authHeader.Substring("Bearer ".Length);

            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);

                if (!decodedToken.Claims.TryGetValue("phone_number", out var phoneNumber))
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "Token has no verified phone number." });
                    return;
                }

                context.HttpContext.Items["VerifiedPhoneNumber"] = phoneNumber.ToString();
                context.HttpContext.Items["FirebaseUid"] = decodedToken.Uid;
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid or expired token." });
                return;
            }

            await next();
        }
    }
}