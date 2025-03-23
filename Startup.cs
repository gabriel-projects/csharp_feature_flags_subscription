using Api.GRRInnovations.FeatureFlags.Models;
using Api.GRRInnovations.FeatureFlags.Services;
using Microsoft.FeatureManagement;

namespace Api.GRRInnovations.FeatureFlags
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddApplicationInsightsTelemetry(x => x.ConnectionString = Configuration.GetConnectionString("ApplicationInsights"));

            services.AddAzureAppConfiguration(); //necessary for the use with capture refresh/update flags in azure

            services.AddHttpContextAccessor();

            //for using for access depency injection IFeatureManager on controller
            //services.AddFeatureManagement();
            services.AddFeatureManagement().WithTargeting<CustomTargetingContextAccessor>(); //fnciona

            services.Configure<FeatureFlagConfig<string[]>>(
                Configuration.GetSection("FeatureManagement:EnableUpdateSubscription"));

            services.AddScoped<FeatureFlagService>();

            services.AddLogging(x =>
            {
                x.AddDebug();
                x.AddConsole();
            });

            services.AddSingleton(Program._refresher);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();

            app.UseAuthorization();

            app.UseAzureAppConfiguration(); //necessary for the use with capture refresh/update flags in azure

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
