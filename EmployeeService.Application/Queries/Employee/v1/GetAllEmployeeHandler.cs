using EmployeeService.Application.App.Interfaces;
using EmployeeService.Message.Messaging.Request.v1;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Mapster;

namespace EmployeeService.Application.Queries.Employee.v1
{
    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployees, object>
    {
        private readonly ILogger<GetAllEmployeesHandler> _logger;
        private readonly IObjectCache _cache;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _repository;

        public GetAllEmployeesHandler(IObjectCache cache, ILogger<GetAllEmployeesHandler> logger, IMapper mapper, IEmployeeRepository repository)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<object> Handle(GetAllEmployees request, CancellationToken cancellationToken)
        {
            if(request.FromRow > request.ToRow)
            {
                throw new ValidationException("FromRow cannot be greater than ToRow.");
            }

            var allEmployees = request.FromRow + request.ToRow == 0;
            const string cacheKey = "Employee:All";

            if (allEmployees)
            {
                request.ToRow = 1000000;
                var cachedValue = await _cache.GetCacheValueAsync<List<Message.DTO.v1.EmployeeDto>>(cacheKey, cancellationToken);
                if (cachedValue != null)
                {
                    _logger.LogInformation($"Cache value found for {cacheKey}");
                    return cachedValue;
                }
            }

            var dataValue = await GetAllEmployees(request.FromRow, request.ToRow);

            if (allEmployees)
                _ = _cache.SetCacheValueAsync(cacheKey, dataValue, cancellationToken);

            return dataValue;
        }

        private async Task<List<Message.DTO.v1.EmployeeDto>> GetAllEmployees(int fromRow, int toRow)
        {
            var allEmployees = await _repository.GetAllAsync(fromRow, toRow);
            return allEmployees.Adapt<List<Message.DTO.v1.EmployeeDto>>();
        }
    }
}
