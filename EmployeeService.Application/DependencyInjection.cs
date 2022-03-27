using System.Diagnostics.CodeAnalysis;
using EmployeeService.Application.App.Mappers;
using EmployeeService.Domain.EmployeeDemographics;
using EmployeeService.Message.DTO.v1;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeService.Application
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            TypeAdapterConfig<Employee, EmployeeDto>
                .NewConfig()
                .IgnoreNullValues(true)
                .AfterMapping((src, dest) =>
                {
                    if (src.HomeCity?.State?.Country?.CountryName != null)
                        dest.HomeCountryName = src.HomeCity.State.Country.CountryName;
                })
                .AfterMapping((src, dest) =>
                {
                    if (src.HomeCity?.State?.StateName != null)
                        dest.HomeStateName = src.HomeCity.State.StateName;
                })
                .AfterMapping((src, dest) =>
                {
                    if (src.HomeCity?.CityName != null)
                        dest.HomeCityName = src.HomeCity.CityName;
                })
                .AfterMapping((src, dest) =>
                {
                    if (src.BirthCity?.State?.Country?.CountryName != null)
                        dest.BirthCountryName = src.BirthCity.State.Country.CountryName;
                })
                .AfterMapping((src, dest) =>
                {
                    if (src.BirthCity?.State?.StateName != null)
                        dest.BirthStateName = src.BirthCity.State.StateName;
                })
                .AfterMapping((src, dest) =>
                {
                    if (src.BirthCity?.CityName != null)
                        dest.BirthCityName = src.BirthCity.CityName;
                })
                ;
            
            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
