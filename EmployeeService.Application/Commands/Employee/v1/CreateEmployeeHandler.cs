using EmployeeService.Application.App.Interfaces;
using EmployeeService.Message.DTO.v1;
using EmployeeService.Message.Messaging.Request.v1;
using EmployeeService.Message.Messaging.Response.v1;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Mapster;

namespace EmployeeService.Application.Commands.Employee.v1
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployee, object>
    {
        private readonly ILogger<CreateEmployeeHandler> _logger;
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;

        public CreateEmployeeHandler(ILogger<CreateEmployeeHandler> logger, IEmployeeRepository repository, IMapper mapper, IEventBus eventBus)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task<object> Handle(CreateEmployee request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.EmployeeDetails?.ClientId))
            {
                const string message = "Client Id cannot be null or empty.";
                _logger.LogWarning(message);
                throw new ValidationException(message);
            }

            if (string.IsNullOrEmpty(request.EmployeeDetails?.SelectedFName))
            {
                const string message = "Selected FName cannot be null or empty.";
                _logger.LogWarning(message);
                throw new ValidationException(message);
            }

            if (string.IsNullOrEmpty(request.EmployeeDetails?.SelectedSurname))
            {
                const string message = "Selected Surname cannot be null or empty.";
                _logger.LogWarning(message);
                throw new ValidationException(message);
            }

            var resultEntity = await CreateEmployee(request.EmployeeDetails);
            var employeeDto = resultEntity.Adapt<Domain.EmployeeDemographics.Employee, EmployeeDto>();
            
            _ = _eventBus.Publish(new EmployeeCreated { EmployeeDetails = employeeDto });

            return employeeDto;
        }

        private async Task<Domain.EmployeeDemographics.Employee> CreateEmployee(EmployeeDto employeeDto)
        {
           
            var entity = employeeDto.Adapt<EmployeeDto, Domain.EmployeeDemographics.Employee>();
            entity.LastUpdtUserId = "system";
            entity.LastUpdtDate= DateTime.Now;
            return await _repository.AddAsync(entity);
        }
    }
}
