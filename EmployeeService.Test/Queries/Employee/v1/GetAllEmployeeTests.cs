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
    public class GetAllEmployeesTests
    {
        [TestMethod]
        public async Task GetAllEmployeesTest()
        {
            // Arrange
            const int numberOfRows = 10;
            var employeeDtoList = MockBuilder.GenerateMockEmployeeDtoList(numberOfRows);
            var classToHandle = new GetAllEmployees()
            {
                FromRow = 0,
                ToRow = numberOfRows
            };

            var handler = new GetAllEmployeesHandler(MockBuilder.GenerateMockObjectCache<List<EmployeeDto>>(),
                NullLogger<GetAllEmployeesHandler>.Instance,
                MockBuilder.GenerateMockMapperList(employeeDtoList),
                MockBuilder.GenerateMockRepository(rowCount: numberOfRows));

            //Act
            var result = (List<EmployeeDto>)await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            result.Should().NotBeNull().And.HaveCount(numberOfRows);
        }

        [TestMethod]
        public void GetAllEmployeesInvalidRangeTest()
        {
            // Arrange
            const int numberOfRows = 10;
            var employeeDtoList = MockBuilder.GenerateMockEmployeeDtoList(numberOfRows);
            var classToHandle = new GetAllEmployees()
            {
                FromRow = numberOfRows + 1,
                ToRow = numberOfRows
            };

            var handler = new GetAllEmployeesHandler(MockBuilder.GenerateMockObjectCache<List<EmployeeDto>>(),
                NullLogger<GetAllEmployeesHandler>.Instance,
                MockBuilder.GenerateMockMapperList(employeeDtoList),
                MockBuilder.GenerateMockRepository(rowCount: numberOfRows));

            //Act
            Func<Task> action = async () => await handler.Handle(classToHandle, new CancellationToken());

            //Assert
            action.Should().ThrowAsync<ValidationException>().WithMessage("*greater*");
        }
    }
}
