using System.Diagnostics.CodeAnalysis;
using EmployeeService.Application.App.Interfaces;
using EmployeeService.Application.Queries.Employee.v1;
using EmployeeService.Message.Messaging.Request.v1;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Application.Commands.Employee.v1
{

    [ExcludeFromCodeCoverage]
    public class ClearCacheHandler : IRequestHandler<ClearCache, object>
    {
        private readonly ILogger<ClearCacheHandler> _logger;
        private readonly IObjectCache _cache;

        public ClearCacheHandler(ILogger<ClearCacheHandler> logger, IObjectCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<object> Handle(ClearCache request, CancellationToken cancellationToken)
        {            
            if(request.ClearAll)
            {
                // No method to clear the complete cache as of now - https://github.com/dotnet/runtime/issues/36547
            }
            else
            {
                if (!string.IsNullOrEmpty(request.EmployeeId))
                {
                    const string message = "Clearing employee by id cache.";
                    _logger.LogInformation(message);
                    var cacheKey = GetEmployeeByIdHandler.GetCacheKey(request.EmployeeId.ToString());
                    await _cache.RemoveValueAsync(cacheKey, cancellationToken);
                }

                if (!string.IsNullOrEmpty(request.ClientId))
                {
                    const string message = "Clearing employee by client id cache.";
                    _logger.LogInformation(message);
                    var cacheKey = GetEmployeeByClientIdHandler.GetCacheKey(request.ClientId);
                    await _cache.RemoveValueAsync(cacheKey, cancellationToken);
                }
            }

            
            return true;
        }
    }
}
