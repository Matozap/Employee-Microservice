using System.Reflection;
using EmployeeService.Application.App.Interfaces;
using EmployeeService.Application.Queries.Employee.v1;
using EmployeeService.Infrastructure.Data.Cache;
using EmployeeService.Infrastructure.Data.Context;
using EmployeeService.Infrastructure.Data.Queue;
using EmployeeService.Infrastructure.Data.Repository;
using EmployeeService.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NID.Messaging.Core.Endpoint;

namespace EmployeeService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<EmployeeContext>(options => options.UseInMemoryDatabase(databaseName: "Employee"));
            services.AddDbContext<EmployeeContext>(options => options.UseCosmos(configuration["ConnectionStrings:DefaultConnection"],
                    configuration["AppSettings:Database"])
            );
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IObjectCache, ObjectCache>();            
            
            services.AddDistributedMemoryCache();
                        
            services.AddMediatR(Assembly.GetExecutingAssembly().GetType(), typeof(IEmployeeRepository), typeof(GetAllEmployeesHandler), typeof(EventBusEndpoint));
            
            if (configuration["Messaging:Disabled"] != "true")
            {
                services.AddEventEndpoint(configuration, Assembly.GetExecutingAssembly(), typeof(IEmployeeRepository).Assembly);
                services.AddScoped<IEventBus>(service => ActivatorUtilities.CreateInstance<EventBus>(service, new object[] { configuration }));
            }
            else
            {
                services.AddScoped<IEventBus, EventBus>();
            }

            services.EnsureDatabaseIsSeeded();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebApiExceptionHandler(env);            
            return app;
        }
    }
}
