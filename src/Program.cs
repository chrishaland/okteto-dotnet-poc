using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace okteto_dotnet_poc
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var logger =
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            IHost host = null;

            try 
            {
                host = CreateHostBuilder(args).Build();
                await MigrateDatabase(host);
            }
            catch(Exception ex) 
            {
                logger.Fatal(ex, "Unable to create the host");
                Log.CloseAndFlush();
                throw;
            }

            try 
            {
                await host.RunAsync();
                return 0;
            }
            catch(Exception ex) 
            {
                logger.Fatal(ex, "Host shutdown unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
                .CreateDefaultBuilder(args)
                .UseSerilog(ConfigureSerilog)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureAppConfiguration(ConfigureAppConfiguration)
                    .UseStartup<Startup>()
                );

        private static async Task MigrateDatabase(IHost host)
        {
            using var context = host.Services.GetService<Database>();
            await context?.Database.MigrateAsync();
        }

        private static void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder config)
		{
            config.AddJsonFile($"configmap/appsettings.json", optional: true, reloadOnChange: true);
			config.AddJsonFile($"secret/appsettings.json", optional: true, reloadOnChange: true);
        }

        private static void ConfigureSerilog(HostBuilderContext context, LoggerConfiguration config)
		{
			config
				.MinimumLevel.Debug()
				.MinimumLevel.Override("System", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
				.ReadFrom.Configuration(context.Configuration)
				.Enrich.FromLogContext()
			;
		}
    }
}
