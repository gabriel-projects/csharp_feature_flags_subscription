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

            services.AddAzureAppConfiguration(); //necessary for the use with capture refresh/update flags in azure
            services.AddFeatureManagement(); //for using wiht depency injection IFeatureManager
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

            //app.UseMiddleware<TargetingHttpContextMiddleware>();
            app.UseAzureAppConfiguration(); //necessary for the use with capture refresh/update flags in azure

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
