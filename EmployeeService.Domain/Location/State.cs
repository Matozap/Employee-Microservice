using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Domain.Location
{
    public class State
    {
        [Key]
        public string Id { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public Country Country { get; set; }
        //public int CountryId { get; set; }
        //public virtual List<City> Cities { get; set; }
    }
}