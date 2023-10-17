using Microsoft.AspNetCore.Authorization;

namespace food_delivery.Policies
{
    public class TokenSeriesRequirement : IAuthorizationRequirement
    {
        public TokenSeriesRequirement(string tokenSeries)
        {
            TokenSeries = tokenSeries;
        }

        public string TokenSeries { get; }
    }

}
