using NET7WebAPI_OrgztnApp.Domain.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.BaseModels;
using NET7WebAPI_OrgztnApp.Domain.Commons.Employees.Models;

namespace NET7WebAPI_OrgztnApp.Domain.Commons.Company.Models
{

    //In Organisation.Domain.Companies.Models, we add an Company.cs class &
    //specify Property with correspondent attribute name

    [TableName("tblCompanies")]
    public class Company : IDbEntity
    {
        [PrimaryKey]
        [ColumnName("Id")]
        public string Id { get; set; } = ShortGuid.NewGuid();


        [DistinguishingUniqueKey]
        [ColumnName("Name")]
        public string Name { get; set; } = string.Empty;


        [ColumnName("Address")]
        public string Address { get; set; } = string.Empty;


        [ColumnName("Country")]
        public string Country { get; set; } = string.Empty;


        [ColumnName("IsDeleted")]
        public bool IsDeleted { get; set; }


        [Navigation(typeof(Employee), "CompanyId")]
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    }
}
