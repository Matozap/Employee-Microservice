using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Domain.Location
{
    public class Country
    {
        [Key]
        public string Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        //public virtual List<State> States { get; set; }
    }
}