using System.Threading.Tasks;
using EmployeeService.Message.DTO.v1;
using EmployeeService.Message.Messaging.Request.v1;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }
                
        /// <summary>
        ///  - Gets all the employees in the system by given the start row and end row from the parameters.
        /// </summary>
        /// <param name="from">Start row</param>
        /// <param name="to">End Row</param>
        /// <returns>All employees</returns>
        [HttpGet("{from:int}/{to:int}")]
        public async Task<IActionResult> GetAllEmployees(int from, int to)
        {
            var query = new GetAllEmployees
            { 
                FromRow = from,
                ToRow = to
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// - Gets an employee by id (number).
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>Employee</returns>        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var query = new GetEmployeeById
            {
                EmployeeId = id
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// - Gets an employee by client id (text).
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns>Employee</returns>        
        [HttpGet("ClientId/{id}")]
        public async Task<IActionResult> GetEmployeeByClientId(string id)
        {
            var query = new GetEmployeeByClientId
            {
                ClientId = id
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Creates an employee based in the given object. 
        /// </summary>
        /// <param name="employee">EmployeeDto</param>
        /// <returns>Employee</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employee)
        {
            var query = new CreateEmployee
            {
                EmployeeDetails = employee
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        /// <summary>
        /// Updates an employee based in the given object.
        /// </summary>
        /// <param name="employee">EmployeeDto</param>
        /// <returns>Employee</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee)
        {
            var query = new UpdateEmployee
            {
                EmployeeDetails = employee
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Does a soft delete on the employee with the given id and returns "Success" if no exception was raised.
        /// </summary>
        /// <param name="id">Employee Id</param>
        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDelete(string id)
        {
            var query = new SoftDeleteEmployee
            {
                EmployeeId = id
            };
            await _mediator.Send(query);
            return Ok("Success");
        }

        /// <summary>
        /// Does a physical delete on the employee with the given id
        /// </summary>
        /// <param name="id">Employee Id</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var query = new DeleteEmployee
            {
                EmployeeId = id
            };
            await _mediator.Send(query);
            return Ok("Success");
        }
    }
}
