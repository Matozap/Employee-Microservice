
namespace EmployeeService.Message.DTO.v1
{
    public class EmployeeDto
    {
        public string Id { get; set; }

        public string ClientId { get; set; }

        public string SelectedFName { get; set; }

        public string SelectedSurname { get; set; }

        public string SocialInsNumber { get; set; }

        public string DeleteFlag { get; set; }

        public string HomeCountryName { get; set; }

        public string HomeStateName { get; set; }

        public string HomeCityName { get; set; }
        
        public string HomeCityId { get; set; }

        public string BirthCountryName { get; set; }

        public string BirthStateName { get; set; }

        public string BirthCityName { get; set; }
        
        public string BirthCityId { get; set; }

        public string GenderInd { get; set; }

        public string DisabledInd { get; set; }
        
        public string CompanyId { get; set; }
    }
}
