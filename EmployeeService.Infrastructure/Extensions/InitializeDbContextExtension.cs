using EmployeeService.Infrastructure.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmployeeService.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class InitializeDbContextExtension
    {
        public static void EnsureDatabaseIsSeeded(this IServiceCollection services)
        {
            try
            {
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                using var serviceScope = serviceProvider.GetService<EmployeeContext>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[EnsureDatabaseIsSeeded] - " + ex.Message + " - " + ex.StackTrace);
            }
        }
    }
}
