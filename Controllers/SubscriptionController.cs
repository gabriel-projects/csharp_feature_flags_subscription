using Api.GRRInnovations.FeatureFlags.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

namespace Api.GRRInnovations.FeatureFlags.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;
        private readonly IConfiguration _configuration;
        private readonly FeatureFlagService _featureFlagService;
        private readonly IConfigurationRefresher _refresher;

        public SubscriptionController(IFeatureManager featureManager, IConfiguration configuration, FeatureFlagService featureFlagService, IConfigurationRefresher refresher)
        {
            _featureManager = featureManager;
            _configuration = configuration;
            _featureFlagService = featureFlagService;
            _refresher = refresher;
        }

        /// <summary>
        /// Endpoint using for AppConfiguration with CustomFilter
        /// </summary>
        /// <returns></returns>
        [HttpGet("CreateOrUpdateSubscription")]
        public IActionResult CreateOrUpdateSubscription()
        {
            var example = _configuration.GetSection("FeatureManagement:EnableUpdateSubscription:EnabledFor:0:Parameters:Values").Get<string[]>();

            var allowedUnits = _featureFlagService.GetFeatureFlagValues("EnableUpdateSubscription");
            if (allowedUnits == null || allowedUnits?.Length == 0)
            {
                return BadRequest(new { Message = "Nenhuma unidade configurada para atualização." });
            }

            return Ok(new { Message = "Atualização permitida para as unidades", Units = allowedUnits });
        }


        /// <summary>
        /// Endpoint using for AppConfiguration with ConfigureRefresh
        /// </summary>
        /// <returns></returns>
        [HttpGet("EnableAdminTools")]
        public async Task<IActionResult> EnableAdminTools()
        {
            await _refresher.TryRefreshAsync();

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

        /// <summary>
        /// todo: criar endpoint para usar com targeting group
        /// https://github.com/microsoft/FeatureManagement-Dotnet/blob/main/examples/TargetingConsoleApp/Program.cs
        /// https://www.daveabrock.com/2020/06/07/custom-filters-in-core-flags/
        /// </summary>
        /// <returns></returns>
        [HttpGet("BrowserFilter")]
        //[FeatureGate("AllowedBrowsers")]
        public async Task<IActionResult> BrowserFilter()
        {
            var enableAdminTools = _configuration.GetSection("FeatureManagement:AllowedBrowsers");

            if (await _featureManager.IsEnabledAsync("AllowedBrowsers"))
                return Ok("Funcionalidade liberada para Chrome!");

            return Ok("Funcionalidade ainda não disponível para seu navegador.");
        }
    }
}
