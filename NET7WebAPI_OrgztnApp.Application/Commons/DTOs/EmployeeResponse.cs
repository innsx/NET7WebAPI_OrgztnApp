namespace NET7WebAPI_OrgztnApp.Application.Commons.DTOs
{
    public class EmployeeResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Position { get; set; } = string.Empty; 
        public decimal Salary { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CompanyId { get; set; } = string.Empty;
    }
}
