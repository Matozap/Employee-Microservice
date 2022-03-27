using System.Diagnostics.CodeAnalysis;
using EmployeeService.Message.Messaging.Response.v1;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Integration
{
    [ExcludeFromCodeCoverage]
    public class EmployeeDeletedHandler : IRequestHandler<EmployeeDeleted, object>
    {
        private readonly ILogger<EmployeeDeletedHandler> _logger;
        public EmployeeDeletedHandler(ILogger<EmployeeDeletedHandler> logger)
        {
            _logger = logger;
        }
        public Task<object> Handle(EmployeeDeleted request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message of type 'EmployeeDeleted' received with EmployeeId {EmployeeId}", request.EmployeeId.ToString());
            return Task.FromResult("Success" as object);
        }
    }
}
