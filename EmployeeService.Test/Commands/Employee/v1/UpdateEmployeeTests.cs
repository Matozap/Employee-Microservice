using EmployeeService.Application.Commands.Employee.v1;
using EmployeeService.Message.DTO.v1;
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
    public class UpdateEmployeeTests
    {
        [TestMethod]
        public async Task UpdateEmployeeTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            var classToHandle = new UpdateEmployee()
            {
                EmployeeDetails = employeeDto
            };

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetEmployeeById>()).Returns(employeeDto);

            var handler = new UpdateEmployeeHandler(NullLogger<UpdateEmployeeHandler>.Instance,
                MockBuilder.GenerateMockRepository(employee),
                mediator,
                MockBuilder.GenerateMockEventBus(),
                MockBuilder.GenerateMockMapper(employee, employeeDto));

            //Act
            var result = (EmployeeDto)await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            result.Should().NotBeNull().And.BeOfType<EmployeeDto>();
            result.ClientId.Should().BeEquivalentTo(employeeDto.ClientId);
        }

        [TestMethod]
        public void UpdateEmployeeInvalidEmployeeIdTest()
        {
            // Arrange
            var employee = MockBuilder.GenerateMockEmployee();
            var employeeDto = MockBuilder.GenerateMockEmployeeDto();
            employeeDto.Id = "0";
            var classToHandle = new UpdateEmployee()
            {
                EmployeeDetails = employeeDto
            };

            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetEmployeeById>()).Returns(employeeDto);

            var handler = new UpdateEmployeeHandler(NullLogger<UpdateEmployeeHandler>.Instance,
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
