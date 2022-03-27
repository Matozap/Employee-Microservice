using MediatR;

namespace EmployeeService.Message.Messaging.Request.v1
{
    public class GetEmployeeByClientId : IRequest<object>
    {
        public string ClientId { get; init; }
    }
}
