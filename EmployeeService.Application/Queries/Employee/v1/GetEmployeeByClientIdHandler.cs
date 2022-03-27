using EmployeeService.Application.App.Interfaces;
using EmployeeService.Message.Messaging.Request.v1;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Mapster;

namespace EmployeeService.Application.Queries.Employee.v1
{
    public class GetEmployeeByClientIdHandler : IRequestHandler<GetEmployeeByClientId, object>
    {
        private readonly ILogger<GetEmployeeByClientIdHandler> _logger;
        private readonly IEmployeeRepository _repository;
        private readonly IObjectCache _cache;
        private readonly IMapper _mapper;

        public GetEmployeeByClientIdHandler(IEmployeeRepository repository, IObjectCache cache, ILogger<GetEmployeeByClientIdHandler> logger, IMapper mapper)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<object> Handle(GetEmployeeByClientId request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ClientId))
            {
                const string message = "Client Id cannot be null or empty.";
                _logger.LogWarning(message);
                throw new ValidationException(message);
            }

            var cacheKey = GetCacheKey(request.ClientId);
            var cachedValue = await _cache.GetCacheValueAsync<Message.DTO.v1.EmployeeDto>(cacheKey, cancellationToken);
            if (cachedValue != null)
            {
                _logger.LogInformation("Cache value found for {CacheKey}",cacheKey);
                return cachedValue;
            }

            var dataValue = await GetEmployeeByClientId(request.ClientId);
            
            if (dataValue != null)
            {
                _ = _cache.SetCacheValueAsync(cacheKey, dataValue, cancellationToken);
            }
            
            return dataValue;
        }

        private async Task<Message.DTO.v1.EmployeeDto> GetEmployeeByClientId(string clientId)
        {
            var entity = await _repository.Get(e => e.ClientId == clientId);
            var employeeDto = entity.Adapt<Domain.EmployeeDemographics.Employee, Message.DTO.v1.EmployeeDto>();
            return employeeDto;
        }

        public static string GetCacheKey(string id)
        {
            return $"Employee:cti:{id}";
        }
    }
}
