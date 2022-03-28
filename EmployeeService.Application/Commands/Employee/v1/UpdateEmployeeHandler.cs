using AutoMapper;
using EmployeeService.Application.App.Interfaces;
using EmployeeService.Message.DTO.v1;
using EmployeeService.Message.Messaging.Request.v1;
using EmployeeService.Message.Messaging.Response.v1;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Application.Commands.Employee.v1
{

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployee, object>
    {
        private readonly ILogger<UpdateEmployeeHandler> _logger;
        private readonly IEmployeeRepository _repository;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public UpdateEmployeeHandler(ILogger<UpdateEmployeeHandler> logger, IEmployeeRepository repository, IMediator mediator, IEventBus eventBus, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mediator = mediator;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<object> Handle(UpdateEmployee request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.EmployeeDetails?.Id))
            {
                const string message = "Employee Id cannot be zero, null or empty.";
                _logger.LogWarning(message);
                throw new ValidationException(message);
            }

            await UpdateEmployee(request.EmployeeDetails);
            
            _ = _eventBus.Publish(new EmployeeUpdated { EmployeeDetails = request.EmployeeDetails });
            
            return request.EmployeeDetails;
        }

        private async Task UpdateEmployee(EmployeeDto employeeDto)
        {
            var query = new GetEmployeeById
            {
                EmployeeId = employeeDto.Id
            };
            var readResult = await _mediator.Send(query);
            var existingEmployeeDto = (EmployeeDto)readResult;
            
            if(existingEmployeeDto != null)
            {                
                await _repository.UpdateAsync(employeeDto.Adapt<EmployeeDto, Domain.EmployeeDemographics.Employee>());
            }
        }
    }
}
