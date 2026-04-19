using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.Commons.Company.Models;
namespace NET7WebAPI_OrgztnApp.Application.Commons.Interfaces
{
    /*
     We Have ICompanyRepository IMPLEMENTS IGenericRepository 
    which we SPECIFIED the Type “Company” that we wanted to ACCESS*/
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        //we add a GetCompaniesByQueryAsnyc() with pass-in CompanyQueryParameters
        //& Return type of PageList<CompanyResponse>
        public Task<PageList<CompanyResponse>> GetCompaniesByQueryAsync(CompanyQueryParameters queryParameters);
    }
}
