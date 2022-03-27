using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EmployeeService.API.Extensions
{
    public static class SwaggerExtension
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NID - Employee Service API",
                    Version = "v1",
                    Description = "NID - Employee Service API",
                    TermsOfService = new Uri("https://matozap.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nicolas Zapata",
                        Url = new Uri("http://matozap.com")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        public static void UseSwaggerApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Service API - NID");
                c.DisplayRequestDuration();
                c.DefaultModelExpandDepth(1);
                c.DefaultModelsExpandDepth(1);
            });
        }
    }
}
