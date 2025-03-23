using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace Api.GRRInnovations.FeatureFlags
{
    public class Program
    {
        public static IConfigurationRefresher _refresher = null;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var settings = config.Build();

                        var connectionString = settings["ConnectionStrings:AzureAppConfig"];
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(connectionString)
                                    .ConfigureRefresh(refresh =>
                                    {
                                        // SentinelKey: a special key used to trigger the refresh of multiple feature flags simultaneously
                                        refresh.Register("SentinelKey", refreshAll: true).SetRefreshInterval(TimeSpan.FromMinutes(1));
                                    })

                                    // UseFeatureFlags: required to retrieve the latest config; works like refresh registration for individual or all flags
                                    .UseFeatureFlags(x =>
                                    {
                                        // If you don't use a SentinelKey, you must manually define the cache refresh interval
                                        x.SetRefreshInterval(TimeSpan.FromMinutes(1));

                                    });

                            //.SelectSnapshot("feature-flags-snapshot-dev")
                            _refresher = options.GetRefresher();
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
