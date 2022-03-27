using MediatR;

namespace EmployeeService.Message.Messaging.Request.v1
{
    public class ClearCache : IRequest<object>
    {
        public string ClientId { get; init; }
        public string EmployeeId { get; init; }
        public bool ClearAll { get; set; }
    }
}
