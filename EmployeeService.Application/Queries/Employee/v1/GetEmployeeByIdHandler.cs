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
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeById, object>
    {
        private readonly ILogger<GetEmployeeByIdHandler> _logger;
        private readonly IEmployeeRepository _repository;
        private readonly IObjectCache _cache;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandler(IEmployeeRepository repository, IObjectCache cache, ILogger<GetEmployeeByIdHandler> logger, IMapper mapper)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<object> Handle(GetEmployeeById request, CancellationToken cancellationToken)
        {
            if (request.EmployeeId == "0")
            {
                throw new ValidationException("Employee Id must be greater than 0.");
            }

            var cacheKey = GetCacheKey(request.EmployeeId.ToString());
            var cachedValue = await _cache.GetCacheValueAsync<Message.DTO.v1.EmployeeDto>(cacheKey, cancellationToken);
            if (cachedValue != null)
            {
                _logger.LogInformation($"Cache value found for {cacheKey}");
                return cachedValue;
            }

            var dataValue = await GetEmployeeById(request.EmployeeId);

            if(dataValue != null)
            {
                _ = _cache.SetCacheValueAsync(cacheKey, dataValue, cancellationToken);
            }
            
            return dataValue;
        }

        private async Task<Message.DTO.v1.EmployeeDto> GetEmployeeById(string id)
        {
            var entity = await _repository.Get(e => e.Id == id);
            var employeeDto = entity.Adapt<Domain.EmployeeDemographics.Employee, Message.DTO.v1.EmployeeDto>();
            return employeeDto;
        }

        public static string GetCacheKey(string id)
        {
            return $"Employee:id:{id}";
        }
    }
}
