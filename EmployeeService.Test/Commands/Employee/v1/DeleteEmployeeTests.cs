using EmployeeService.Application.Commands.Employee.v1;
using EmployeeService.Message.Messaging.Request.v1;
using EmployeeService.Test.Mocking;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Test.Commands.Employee.v1
{
    [TestClass]
    public class DeleteEmployeeTests
    {
        [TestMethod]
        public async Task DeleteEmployeeTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            var classToHandle = new DeleteEmployee()
            {
                EmployeeId = employee.Id
            };

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetEmployeeById>()).Returns(employeeDto);

            var handler = new DeleteEmployeeHandler(NullLogger<DeleteEmployeeHandler>.Instance,
                MockBuilder.GenerateMockRepository(employee),
                mediator,
                MockBuilder.GenerateMockEventBus(),
                MockBuilder.GenerateMockMapper(employee, employeeDto));

            //Act
            var result = await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void DeleteEmployeeInvalidEmployeeIdTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            employee.Id = "0";
            var classToHandle = new DeleteEmployee()
            {
                EmployeeId = employee.Id
            };

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetEmployeeById>()).Returns(employeeDto);

            var handler = new DeleteEmployeeHandler(NullLogger<DeleteEmployeeHandler>.Instance,
                MockBuilder.GenerateMockRepository(employee),
                mediator,
                MockBuilder.GenerateMockEventBus(),
                MockBuilder.GenerateMockMapper(employee, employeeDto));

            //Act
            Func<Task> action = async () => await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            action.Should().ThrowAsync<ValidationException>().WithMessage("*Employee Id*");
        }
    }
}
