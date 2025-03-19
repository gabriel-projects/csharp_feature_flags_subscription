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
                            option.Connect(connectionString);
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
