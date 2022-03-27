using EmployeeService.Message.DTO.v1;
using MediatR;

namespace EmployeeService.Message.Messaging.Request.v1
{
    public class CreateEmployee : IRequest<object>
    {
        public EmployeeDto EmployeeDetails { get; init; }
    }
}
