using System.Diagnostics.CodeAnalysis;
using EmployeeService.Message.Messaging.Response.v1;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Integration
{
    [ExcludeFromCodeCoverage]
    public class EmployeeCreatedHandler : IRequestHandler<EmployeeCreated, object>
    {
        private readonly ILogger<EmployeeCreatedHandler> _logger;
        public EmployeeCreatedHandler(ILogger<EmployeeCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task<object> Handle(EmployeeCreated request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message of type 'EmployeeCreated' received with EmployeeId {EmployeeId}", request.EmployeeDetails.Id.ToString());
            return Task.FromResult("Success" as object);
        }
    }
}
