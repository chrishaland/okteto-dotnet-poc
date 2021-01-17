using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace okteto_dotnet_poc
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
            var databaseOptions = Configuration.GetSection("Database").Get<DatabaseOptions>();
            if (!string.IsNullOrEmpty(databaseOptions?.Server) && !string.IsNullOrEmpty(databaseOptions?.Name)) 
            {
                var connectionString = $"Server={databaseOptions.Server};Database={databaseOptions.Name};User Id={databaseOptions.Username};Password={databaseOptions.Password};MultipleActiveResultSets=true;";
                services.AddDbContext<Database>(options => options.UseSqlServer(connectionString));
            }
            
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
