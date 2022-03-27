using MediatR;

namespace EmployeeService.Message.Messaging.Request.v1
{
    public class SoftDeleteEmployee : IRequest<object>
    {
        public string EmployeeId { get; init; }
    }
}
