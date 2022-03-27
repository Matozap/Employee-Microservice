using System.Diagnostics.CodeAnalysis;
using EmployeeService.Message.Messaging.Request.v1;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NID.Messaging.Core.Endpoint;

namespace EmployeeService.Integration
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static async Task Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            await new HostBuilder()
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                configHost.AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(configure => configure.AddConsole());
                services.AddEventEndpoint(hostContext.Configuration, Assembly.GetExecutingAssembly(), typeof(GetAllEmployees).Assembly);
                services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(GetAllEmployees).Assembly, typeof(EventMessage).Assembly);
            })
            .RunConsoleAsync();
        }
    }
}
