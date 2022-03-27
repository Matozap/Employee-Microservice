using System;
using EmployeeService.Application.App.Interfaces;
using EmployeeService.Domain.EmployeeDemographics;
using EmployeeService.Message.DTO.v1;
using EmployeeService.Message.Messaging.Response.v1;
using AutoFixture;
using AutoMapper;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmployeeService.Domain.Location;

namespace EmployeeService.Test.Mocking
{
    public static class MockBuilder
    {
        private static readonly Fixture Fixture = new();

        public static IMapper GenerateMockMapper(Employee employee, EmployeeDto employeeDto)
        {
            var mapper = Substitute.For<IMapper>();
            mapper.Map<EmployeeDto, Employee>(Arg.Any<EmployeeDto>()).Returns(employee);
            mapper.Map<Employee, EmployeeDto>(Arg.Any<Employee>()).Returns(employeeDto);
            return mapper;
        }

        public static IMapper GenerateMockMapperList(List<EmployeeDto> employeeDtoList)
        {
            var mapper = Substitute.For<IMapper>();
            mapper.Map<List<EmployeeDto>>(Arg.Any<List<Employee>>()).Returns(employeeDtoList);
            return mapper;
        }

        public static IEmployeeRepository GenerateMockRepository(Employee employee = null, int rowCount = 100)
        {
            var mockEmployee = employee ?? GenerateMockEmployee();
            var mockEmployees = GenerateMockEmployeeList(rowCount);
            var repository = Substitute.For<IEmployeeRepository>();
            repository.AddAsync(Arg.Any<Employee>()).Returns(mockEmployee);
            repository.UpdateAsync(mockEmployee).Returns(mockEmployee);
            repository.DeleteAsync(mockEmployee).Returns(mockEmployee);
            repository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(mockEmployees);
            repository.Get(Arg.Any<Expression<Func<Employee, bool>>>()).Returns(mockEmployee);
            repository.GetAsList(Arg.Any<Expression<Func<Employee, bool>>>()).Returns(mockEmployees);
            return repository;
        }

        public static IEventBus GenerateMockEventBus()
        {
            var eventBus = Substitute.For<IEventBus>();
            eventBus.Publish(Arg.Any<EmployeeCreated>()).Returns(Task.CompletedTask);
            return eventBus;
        }

        public static IObjectCache GenerateMockObjectCache<T>() where T : class
        {
            var cache = Substitute.For<IObjectCache>();
            return cache;
        }

        public static List<Employee> GenerateMockEmployeeList(int count)
        {
            return Fixture.Build<Employee>()
                .With(q => q.HomeCity, GenerateMockCity())
                .With(q => q.BirthCity, GenerateMockCity())
                .CreateMany(count)
                .ToList();
        }

        public static List<EmployeeDto> GenerateMockEmployeeDtoList(int count)
        {
            return Fixture.Build<EmployeeDto>().CreateMany(count).ToList();
        }

        public static Employee GenerateMockEmployee()
        {
            return Fixture.Build<Employee>()
                .With(q => q.HomeCity, GenerateMockCity())
                .With(q => q.BirthCity, GenerateMockCity())
                .Create();
        }

        private static City GenerateMockCity()
        {
            return new City
            {
                Id = "1",
                Population = 1000,
                CityName = "Test City",
                State = new State
                {
                    Id = "1",
                    StateCode = "TS",
                    StateName = "Test State",
                    Country = new Country
                    {
                        Id = "1",
                        CountryCode = "TC",
                        CountryName = "Test Country"
                    }
                }
            };
        }

        public static EmployeeDto GenerateMockEmployeeDto()
        {
            return Fixture.Build<EmployeeDto>().Create();
        }

        public static EmployeeDto GenerateMockEmployeeDto(Employee employee)
        {
            return Fixture.Build<EmployeeDto>()
                .Create();
        }
    }
}
