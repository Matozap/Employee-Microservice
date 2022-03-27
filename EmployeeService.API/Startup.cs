using System;
using System.IO.Compression;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmployeeService.API.Extensions;
using EmployeeService.Application;
using EmployeeService.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;

namespace EmployeeService.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration.GetEnvironmentConfiguration(env);            
        }
                        
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.DefaultConnectionLimit = 10000;
                                
                services.AddAuthorization();
                services.AddCors();
                services.AddHealthChecks();
                services.AddControllers();
                services.AddMvc();
                
                services.AddApplication();
                services.AddInfrastructure(Configuration);
                
                services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
                services.ConfigureSwagger();

                services.AddResponseCompression();

                services.Configure<GzipCompressionProviderOptions>
                (options => 
                { 
                    options.Level = CompressionLevel.Fastest; 
                }); 
                services.AddApplicationInsightsTelemetry();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Startup] ERROR at ConfigureServices {ex}");
            }
            Console.WriteLine("[Startup] ConfigureServices [DONE]");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {                
                app.UseSerilogLogging();
                if(env.EnvironmentName == "Development")
                    app.UseSwaggerApi();
                
                app.UseApplication();
                app.UseInfrastructure(env);

                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseCors(x => x
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/heartbeat");
                    endpoints.MapControllers();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Startup] ERROR at Configure - {ex}");
            }

            Console.WriteLine("[Startup] Configure [DONE]");
        }
    }
}
