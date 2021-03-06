using EmployeeService.Application.Queries.Employee.v1;
using EmployeeService.Message.DTO.v1;
using EmployeeService.Message.Messaging.Request.v1;
using EmployeeService.Test.Mocking;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Test.Queries.Employee.v1
{
    [TestClass]
    public class GetEmployeeByClientIdTests
    {
        [TestMethod]
        public async Task GetEmployeeByClientIdTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            var classToHandle = new GetEmployeeByClientId()
            {
                ClientId = employeeDto.ClientId
            };

            var handler = new GetEmployeeByClientIdHandler(MockBuilder.GenerateMockRepository(employee),
                MockBuilder.GenerateMockObjectCache<List<EmployeeDto>>(),
                NullLogger<GetEmployeeByClientIdHandler>.Instance,
                MockBuilder.GenerateMockMapper(employee, employeeDto));

            //Act
            var result = (EmployeeDto)await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetEmployeeByClientIdInvalidRangeTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            var classToHandle = new GetEmployeeByClientId();

            var handler = new GetEmployeeByClientIdHandler(MockBuilder.GenerateMockRepository(employee),
                MockBuilder.GenerateMockObjectCache<List<EmployeeDto>>(),
                NullLogger<GetEmployeeByClientIdHandler>.Instance,
                MockBuilder.GenerateMockMapper(employee, employeeDto));

            //Act
            Func<Task> action = async () => await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            action.Should().ThrowAsync<ValidationException>().WithMessage("*Client Id*");
        }
    }
}
