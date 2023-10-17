using food_delivery.Data.Models;
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
            string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                long? actualTokenSeries = _tokenService.GetTokenSeriesByUserId(Guid.Parse(userId));
                string? tokenSeries = context.User.FindFirst(ClaimTypes.Version)?.Value;

                if (actualTokenSeries != null && tokenSeries != null && actualTokenSeries.ToString() == tokenSeries)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

}
