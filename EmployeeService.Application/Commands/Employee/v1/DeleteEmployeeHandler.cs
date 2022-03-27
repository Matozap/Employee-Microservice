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

    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployee, object>
    {
        private readonly ILogger<DeleteEmployeeHandler> _logger;
        private readonly IEmployeeRepository _repository;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public DeleteEmployeeHandler(ILogger<DeleteEmployeeHandler> logger, IEmployeeRepository repository, IMediator mediator, IEventBus eventBus, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mediator = mediator;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<object> Handle(DeleteEmployee request, CancellationToken cancellationToken)
        {
            if (request.EmployeeId == "0")
            {
                const string message = "Employee Id cannot be zero, null or empty.";
                _logger.LogWarning(message);
                throw new ValidationException(message);
            }

            await DeleteEmployeeAsync(request.EmployeeId);

            _ = _eventBus.Publish(new EmployeeDeleted { EmployeeId = request.EmployeeId });

            return request.EmployeeId;
        }

        private async Task DeleteEmployeeAsync(string employeeId)
        {
            var query = new GetEmployeeById
            {
                EmployeeId = employeeId
            };
            var readResult = await _mediator.Send(query);
            var existingEmployeeDto = (EmployeeDto)readResult;
            
            if(existingEmployeeDto != null)
            {                
                await _repository.DeleteAsync(existingEmployeeDto.Adapt<EmployeeDto, Domain.EmployeeDemographics.Employee>());

                _ = _mediator.Send(new ClearCache
                {
                    EmployeeId = employeeId,
                    ClientId = existingEmployeeDto.ClientId
                });
            }
        }
    }
}
