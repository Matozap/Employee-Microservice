using System.Diagnostics.CodeAnalysis;
using EmployeeService.Message.Messaging.Response.v1;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Integration
{
    [ExcludeFromCodeCoverage]
    public class EmployeeUpdatedHandler : IRequestHandler<EmployeeUpdated, object>
    {
        private readonly ILogger<EmployeeUpdatedHandler> _logger;
        public EmployeeUpdatedHandler(ILogger<EmployeeUpdatedHandler> logger)
        {
            _logger = logger;
        }
        public Task<object> Handle(EmployeeUpdated request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message of type 'EmployeeUpdated' received with EmployeeId {EmployeeId}", request.EmployeeDetails.Id.ToString());
            return Task.FromResult("Success" as object);
        }
    }
}
