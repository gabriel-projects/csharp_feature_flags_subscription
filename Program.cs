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
                                        //isso não sobreescreve o refrestinterval, porem se usar uma sentinelKey uma especie de pseudochave, que usamos ela para alterar o valor dela na azure e com isso todas as outras chaves serem atualizadas na proxima busca caso a gente altere demais flags, evitando inconsistencias de precisar manter sob observão diversas keys
                                        refresh.Register("SentinelKey", refreshAll: true).SetRefreshInterval(TimeSpan.FromMinutes(1));
                                    })
                                    .UseFeatureFlags(x =>
                                    {
                                        //caso nao use uma sentinelkey ou monitore alguma chave, devemos configurar aqui o cache das flags
                                        x.SetRefreshInterval(TimeSpan.FromMinutes(1));

                                    }); //UseFeatureFlags: necessary for search the config actual, your use is equais refresh register that configure one on one or all flags

                            //.SelectSnapshot("feature-flags-snapshot-dev")
                            _refresher = options.GetRefresher();
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
