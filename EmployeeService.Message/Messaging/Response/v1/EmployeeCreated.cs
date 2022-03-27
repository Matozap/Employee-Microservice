using EmployeeService.Message.DTO.v1;
using MediatR;

namespace EmployeeService.Message.Messaging.Response.v1
{
    public class EmployeeCreated : IRequest<object>
    {
        public EmployeeDto EmployeeDetails { get; init; }
    }
}
