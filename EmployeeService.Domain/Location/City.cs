using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Domain.Location
{
    public class City
    {
        [Key]
        public string Id { get; set; }
        public string CityName { get; set; }
        public int Population { get; set; } = 0;
        public State State { get; set; }
        //public int StateId { get; set; }
        //public virtual List<Employee> Employees { get; set; }
    }
}