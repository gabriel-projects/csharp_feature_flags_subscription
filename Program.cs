using Microsoft.Extensions.Options;

namespace Api.GRRInnovations.FeatureFlags
{
    public class Program
    {
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
                        config.AddAzureAppConfiguration(option =>
                        {
                            var connectionString = settings["ConnectionStrings:AzureAppConfig"];

                            option.Connect(connectionString)
                                .ConfigureRefresh(refresh =>
                                {
                                    refresh.Register("FeatureManagement:EnableAdminTools", refreshAll: true)
                                        .SetRefreshInterval(TimeSpan.FromSeconds(10));
                                })
                            .UseFeatureFlags();

                            option.Connect(connectionString)
                                .ConfigureRefresh(refresh =>
                                {
                                    refresh.Register("FeatureManagement:EnableUpdateSubscription", refreshAll: true)
                                        .SetRefreshInterval(TimeSpan.FromSeconds(10));
                                })
                            .UseFeatureFlags();

                            option.Connect(connectionString)
                                    .SelectSnapshot("feature-flags-snapshot-dev").UseFeatureFlags();
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
