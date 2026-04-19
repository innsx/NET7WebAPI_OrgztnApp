namespace NET7WebAPI_OrgztnApp.Application.Commons.DTOs
{
    public class EmployeeRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string CompanyId { get; set; } = string.Empty;
    }
}
