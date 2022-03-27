using EmployeeService.Application.Commands.Employee.v1;
using EmployeeService.Message.DTO.v1;
using EmployeeService.Message.Messaging.Request.v1;
using EmployeeService.Test.Mocking;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Test.Commands.Employee.v1
{
    [TestClass]
    public class CreateEmployeeTests
    {
        [TestMethod]
        public async Task CreateEmployeeTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            var classToHandle = new CreateEmployee()
            {
                EmployeeDetails = employeeDto
            };

            var handler = new CreateEmployeeHandler(NullLogger<CreateEmployeeHandler>.Instance,
                MockBuilder.GenerateMockRepository(employee),
                MockBuilder.GenerateMockMapper(employee, employeeDto),
                MockBuilder.GenerateMockEventBus());

            //Act
            var result = (EmployeeDto)await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            result.Should().NotBeNull().And.BeOfType<EmployeeDto>();
            result.ClientId.Should().BeEquivalentTo(employee.ClientId);
        }

        [TestMethod]
        public void CreateEmployeeInvalidClientIdTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            employeeDto.ClientId = null;
            var classToHandle = new CreateEmployee()
            {
                EmployeeDetails = employeeDto
            };

            var handler = new CreateEmployeeHandler(NullLogger<CreateEmployeeHandler>.Instance,
                MockBuilder.GenerateMockRepository(employee),
                MockBuilder.GenerateMockMapper(employee, employeeDto),
                MockBuilder.GenerateMockEventBus());

            //Act
            Func<Task> action = async () => await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            action.Should().ThrowAsync<ValidationException>().WithMessage("*Client Id*");
        }

        [TestMethod]
        public void CreateEmployeeInvalidFNameTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            employeeDto.SelectedFName = null;
            var classToHandle = new CreateEmployee()
            {
                EmployeeDetails = employeeDto
            };

            var handler = new CreateEmployeeHandler(NullLogger<CreateEmployeeHandler>.Instance,
                MockBuilder.GenerateMockRepository(employee),
                MockBuilder.GenerateMockMapper(employee, employeeDto),
                MockBuilder.GenerateMockEventBus());

            //Act
            Func<Task> action = async () => await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            action.Should().ThrowAsync<ValidationException>().WithMessage("*Selected FName*");
        }

        [TestMethod]
        public void CreateEmployeeInvalidSurnameTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            employeeDto.SelectedSurname = null;
            var classToHandle = new CreateEmployee()
            {
                EmployeeDetails = employeeDto
            };

            var handler = new CreateEmployeeHandler(NullLogger<CreateEmployeeHandler>.Instance,
                MockBuilder.GenerateMockRepository(employee),
                MockBuilder.GenerateMockMapper(employee, employeeDto),
                MockBuilder.GenerateMockEventBus());

            //Act
            Func<Task> action = async () => await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            action.Should().ThrowAsync<ValidationException>().WithMessage("*Selected Surname*");
        }
    }
}
