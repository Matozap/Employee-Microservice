using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EmployeeService.Domain.Location;

namespace EmployeeService.Domain.EmployeeDemographics
{
    [Table("EMPLOYEE")]
    public class Employee: EntityBase
    {
        [Key]
        public string Id { get; set; }

        public string ClientId { get; set; }

        public string SelectedFName { get; set; }

        public string SelectedSurname { get; set; }

        public string SocialInsNumber { get; set; }

        public string DeleteFlag { get; set; }
        
        //public virtual int HomeCityId { get; set; }
        public City HomeCity{ get; set; }
        
        //public virtual int BirthCityId { get; set; }
        public City BirthCity { get; set; }

        public string GenderInd { get; set; }

        public string DisabledInd { get; set; }
        
        public string CompanyId { get; set; }
    }
}
