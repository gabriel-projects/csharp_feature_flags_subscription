using Api.GRRInnovations.FeatureFlags.Extensions;
using Microsoft.FeatureManagement.FeatureFilters;

namespace Api.GRRInnovations.FeatureFlags.Services
{
    public class CustomTargetingContextAccessor : ITargetingContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CustomTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ValueTask<TargetingContext?> GetContextAsync()
        {
            var userAgent = _httpContextAccessor.GetBrowserName();

            var context = new TargetingContext
            {
                //UserId = "user1"
                Groups = [userAgent]
            };

            return new ValueTask<TargetingContext?>(context);
        }
    }

}
