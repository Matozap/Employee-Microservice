using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace EmployeeService.Application.App.Mappers
{
    [ExcludeFromCodeCoverage]
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Domain.EmployeeDemographics.Employee, Message.DTO.v1.EmployeeDto>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
