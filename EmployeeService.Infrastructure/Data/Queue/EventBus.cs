using EmployeeService.Application.App.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using NID.Messaging.Core.Endpoint;

namespace EmployeeService.Infrastructure.Data.Queue
{
    public sealed class EventBus : IEventBus
    {
        private readonly IEventBusEndpoint _endpoint;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventBus> _logger;

        public EventBus(IEventBusEndpoint endpoint, IConfiguration configuration, ILogger<EventBus> logger)
        {
            _endpoint = endpoint;
            _configuration = configuration;
            _logger = logger;
        }

        public EventBus(IConfiguration configuration, ILogger<EventBus> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Publish<T>(T message)
        {
            try
            {
                if (_endpoint != null)
                {
                    var destination = _configuration["Messaging:Destinations:Employee"];
                    _logger.LogInformation("Publishing changes to {Destination}", destination);
                    await _endpoint.PublishAsync(message, destination);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing changes");
            }
        }
    }
}
