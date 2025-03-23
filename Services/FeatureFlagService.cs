namespace Api.GRRInnovations.FeatureFlags.Services
{
    public class FeatureFlagService
    {
        private readonly IConfiguration _configuration;

        public FeatureFlagService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string[] GetFeatureFlagValues(string featureFlagKey)
        {
            var configuration = _configuration.GetSection($"FeatureManagement:{featureFlagKey}:EnabledFor:0:Parameters:Values").Get<string[]>();

            return configuration;
        }
    }
}
