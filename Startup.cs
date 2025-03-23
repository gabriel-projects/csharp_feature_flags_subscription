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

            // Optional telemetry with Application Insights
            services.AddApplicationInsightsTelemetry(x => x.ConnectionString = Configuration.GetConnectionString("ApplicationInsights"));

            // Enables support for Azure App Configuration (required for middleware and refresh)
            services.AddAzureAppConfiguration();

            services.AddHttpContextAccessor();

            // For using IFeatureManager dependency injection in the controller
            // Enables Feature Flags with support for custom targeting filters
            services.AddFeatureManagement().AddApplicationInsightsTelemetry().WithTargeting<CustomTargetingContextAccessor>();

            // Strongly-typed configuration binding for specific Feature Flag use
            services.Configure<FeatureFlagConfig<string[]>>(
                Configuration.GetSection("FeatureManagement:EnableUpdateSubscription"));

            // Service that handles feature flag logic
            services.AddScoped<FeatureFlagService>();

            // Logging injection
            services.AddLogging(x =>
            {
                x.AddDebug();
                x.AddConsole();
            });

            // Manual refresher
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

            // Enables the required middleware for using Feature Flags with Azure App Configuration (required for middleware and refresh)
            app.UseAzureAppConfigurations();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
