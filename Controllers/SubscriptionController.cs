using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.FeatureManagement;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace Api.GRRInnovations.FeatureFlags.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly FeatureFlagOptions _featureFlags;
        private readonly IFeatureManager _featureManager;
        private readonly IConfiguration _configuration;

        public SubscriptionController(IOptions<FeatureFlagOptions> featureFlags, IFeatureManager featureManager, IConfiguration configuration)
        {
            _featureFlags = featureFlags.Value;
            _featureManager = featureManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> CreateSubscription()
        {
            var featureFilterContext = new FeatureFilterEvaluationContext();
            var allowedUnits = _configuration.GetSection("FeatureManagement:EnableUpdateSubscription:EnabledFor:0:Parameters:Units").Get<string[]>();
            var enableUpdateSubscription = _configuration.GetSection("FeatureManagement:EnableUpdateSubscription");
            bool isFeatureEnabled = await _featureManager.IsEnabledAsync("EnableUpdateSubscription");
            if (!isFeatureEnabled)
            {
                return BadRequest(new { Message = "Atualização de assinatura não está disponível no momento." });
            }

            return Ok();
        }




        public class UnitFilterSettings
        {
            public string[] AllowedUnits { get; set; } = Array.Empty<string>();
        }
    }
}
