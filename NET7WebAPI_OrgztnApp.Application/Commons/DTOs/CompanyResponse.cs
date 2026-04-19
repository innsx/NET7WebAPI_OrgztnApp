namespace NET7WebAPI_OrgztnApp.Application.Commons.DTOs
{
    //we add DTO object as a RETURN Object instead of RETURNING the ENTITY MODEL object:
    public class CompanyResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

    }
}
