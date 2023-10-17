using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace food_delivery.Policies
{
    public class TokenSeriesHandler : AuthorizationHandler<TokenSeriesRequirement>
    {
        private readonly TokenService _tokenService;

        public TokenSeriesHandler(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, TokenSeriesRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string actualTokenSeries = _tokenService.GetTokenSeriesByUserId(Guid.Parse(userId));

                if (actualTokenSeries != null && actualTokenSeries == requirement.TokenSeries)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

}
