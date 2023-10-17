using Microsoft.AspNetCore.Authorization;

namespace food_delivery.Policies
{
    public class TokenSeriesRequirement : IAuthorizationRequirement
    {
        public string? TokenSeries { get; }
    }

}
