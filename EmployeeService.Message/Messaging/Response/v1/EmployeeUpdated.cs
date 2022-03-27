using EmployeeService.Message.DTO.v1;
using MediatR;

namespace EmployeeService.Message.Messaging.Response.v1
{
    public class EmployeeUpdated : IRequest<object>
    {
        public EmployeeDto EmployeeDetails { get; init; }
    }
}
