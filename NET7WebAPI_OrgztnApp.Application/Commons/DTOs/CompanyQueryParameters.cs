using NET7WebAPI_OrgztnApp.Domain.Commons.Utilities;

namespace NET7WebAPI_OrgztnApp.Application.Common.DTOs
{
    public class CompanyQueryParameters : PagingSortingQueryParameters
    {
        //add FilterBy Properties
        public string FilterByName { get; set; } = string.Empty;

        public string FilterByCountry { get; set; } = string.Empty;
    }
}
