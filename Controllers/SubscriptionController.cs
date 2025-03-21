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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("CreateSubscription")]
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


        /// <summary>
        /// Endpoint using for AppConfiguration with ConfigureRefresh
        /// </summary>
        /// <returns></returns>
        [HttpGet("EnableAdminTools")]
        public async Task<IActionResult> EnableAdminTools()
        {
            var enableAdminTools = _configuration.GetSection("FeatureManagement:EnableAdminTools");
            bool isAdminToolsEnabled = await _featureManager.IsEnabledAsync("EnableAdminTools");

            await Task.CompletedTask;

            return new OkObjectResult(new
            {
                enableAdminTools
            });
        }

        /// <summary>
        /// Endpoint using for AppConfiguration with snapshots flags
        /// </summary>
        /// <returns></returns>
        [HttpGet("EnvsSnapshot")]
        public async Task<IActionResult> EnvsSnapshot()
        {
            bool isEnableDarkMode = await _featureManager.IsEnabledAsync("EnableDarkMode"); //in snapshot
            bool isEnableDashboardV2 = await _featureManager.IsEnabledAsync("EnableDashboardV2"); //in snapshot

            await Task.CompletedTask;

            return new OkObjectResult(new
            {
                isEnableDarkMode = isEnableDarkMode,
                isEnableDashboardV2 = isEnableDashboardV2
            });
        }
        public class UnitFilterSettings
        {
            public string[] AllowedUnits { get; set; } = Array.Empty<string>();
        }
    }
}
