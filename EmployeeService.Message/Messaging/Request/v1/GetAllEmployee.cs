using MediatR;

namespace EmployeeService.Message.Messaging.Request.v1
{
    public class GetAllEmployees : IRequest<object>
    {
        public int FromRow { get; init; }
        public int ToRow { get; set; }
    }
}
