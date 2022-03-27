using System.Threading.Tasks;

namespace EmployeeService.Application.App.Interfaces
{
    public interface IEventBus
    {
        Task Publish<T>(T message);
    }
}
