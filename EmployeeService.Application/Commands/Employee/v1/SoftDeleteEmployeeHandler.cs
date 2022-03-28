using System;
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

    public class SoftDeleteEmployeeHandler : IRequestHandler<SoftDeleteEmployee, object>
    {
        private readonly ILogger<SoftDeleteEmployeeHandler> _logger;
        private readonly IEmployeeRepository _repository;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public SoftDeleteEmployeeHandler(ILogger<SoftDeleteEmployeeHandler> logger, IEmployeeRepository repository, IMediator mediator, IEventBus eventBus, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mediator = mediator;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<object> Handle(SoftDeleteEmployee request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.EmployeeId);

            await UpdateEmployee(request.EmployeeId);

            _ = _eventBus.Publish(new EmployeeDeleted { EmployeeId = request.EmployeeId });

            return request.EmployeeId;
        }

        private async Task UpdateEmployee(string employeeId)
        {
            var query = new GetEmployeeById
            {
                EmployeeId = employeeId
            };
            var readResult = await _mediator.Send(query);
            var existingEmployeeDto = (EmployeeDto)readResult;
            
            if(existingEmployeeDto != null)
            {
                existingEmployeeDto.DeleteFlag = "Y";
                await _repository.UpdateAsync(existingEmployeeDto.Adapt<EmployeeDto, Domain.EmployeeDemographics.Employee>()); 
                
                _ = _mediator.Send(new ClearCache
                {
                    EmployeeId = employeeId,
                    ClientId = existingEmployeeDto.ClientId
                });
            }
        }
    }
}
