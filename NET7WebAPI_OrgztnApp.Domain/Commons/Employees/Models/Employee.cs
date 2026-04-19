using NET7WebAPI_OrgztnApp.Domain.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.BaseModels;

namespace NET7WebAPI_OrgztnApp.Domain.Commons.Employees.Models
{
    /*
     In Organisation.Domain.Employees.Models, we add an Employee.cs class & 
        specify Property with correspondent attribute name
     */
    [TableName("tblEmployees")]
    public sealed class Employee : IDbEntity
    {
        [PrimaryKey]
        [ColumnName("Id")]
        public string Id { get; set; } = ShortGuid.NewGuid();


        [DistinguishingUniqueKey]
        [ColumnName("Name")]
        public string Name { get; set; } = string.Empty;


        [ColumnName("Age")]
        public int Age { get; set; }


        [ColumnName("Position")]
        public string Position { get; set; } = string.Empty;


        [ColumnName("IsArchived")]
        public bool IsArchived { get; set; }


        [ColumnName("IsDeleted")]
        public bool IsDeleted { get; set; }


        [ColumnName("CreatedOn")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;


        [ColumnName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;


        [ColumnName("Salary")]
        public decimal Salary { get; set; }


        [ForeignKey]
        [ColumnName("CompanyId")]
        public string CompanyId { get; set; } = string.Empty;

    }
}