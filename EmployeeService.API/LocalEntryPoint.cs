using EmployeeService.API.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;


namespace EmployeeService.API
{
    public class LocalEntryPoint
    {
        private static IConfiguration _configuration;
        public static async Task Main(string[] args)
        {
            await RunServer();
        }

        private static async Task RunServer()
        {
            Console.WriteLine("[Program] Starting Main");
            _configuration = EnvironmentConfigurationExtension.GetDefaultEnvironmentConfiguration();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();

            try
            {
                Log.Information("Host starting...");
                Console.WriteLine("[Program] Host Starting");
                await CreateHostBuilder(null).Build().RunAsync();
                Console.WriteLine("[Program] Host Stopped Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Program]Host terminated unexpectedly");
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>()
                     .CaptureStartupErrors(true);
             })
             .UseSerilog((hostingContext, loggerConfiguration) => {
                 loggerConfiguration
                     .ReadFrom.Configuration(hostingContext.Configuration)
                     .Enrich.FromLogContext()
                     .Enrich.WithProperty("ApplicationName", typeof(LocalEntryPoint).Assembly.GetName().Name)
                     .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
             });
    }
}
