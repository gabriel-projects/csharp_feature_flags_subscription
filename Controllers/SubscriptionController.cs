using Azure.Core;
using Microsoft.AspNetCore.Mvc;
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
        public SubscriptionController(IOptions<FeatureFlagOptions> featureFlags, IFeatureManager featureManager)
        {
            _featureFlags = featureFlags.Value;
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateSubscription()
        {
            bool isFeatureEnabled = await _featureManager.IsEnabledAsync("EnableUpdateSubscription");
            var features = _featureManager.GetFeatureNamesAsync().GetAsyncEnumerator();

            if (!isFeatureEnabled)
            {
                return BadRequest(new { Message = "Atualização de assinatura não está disponível no momento." });
            }

            return Ok();
        }

    }
}
