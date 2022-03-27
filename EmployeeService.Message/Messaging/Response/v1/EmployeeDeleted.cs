using MediatR;

namespace EmployeeService.Message.Messaging.Response.v1
{
    public class EmployeeDeleted : IRequest<object>
    {
        public string EmployeeId { get; init; }
    }
}
