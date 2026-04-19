using NET7WebAPI_OrgztnApp.Domain.Commons.Utilities;

namespace NET7WebAPI_OrgztnApp.Application.Common.DTOs
{
    public class EmployeeQueryParameters : PagingSortingQueryParameters
    {
        //add FilterByName Property
        public string FilterByName { get; set; } = string.Empty;
    }
}
