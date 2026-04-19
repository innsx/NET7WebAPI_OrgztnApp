using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.Commons.Employees.Models;

namespace NET7WebAPI_OrgztnApp.Application.Commons.Interfaces
{
    /*
     We Have ICompanyRepository IMPLEMENTS IGenericRepository 
    which we SPECIFIED the Type “Employee” that we wanted to ACCESS*/
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //Get Employees by these Employee Query parameters
        //and return emplyees in the EmployeeResponse model 
        //in PageList format output
        public Task<PageList<EmployeeResponse>> GetEmployeesByQueryAsync(EmployeeQueryParameters employeeQueryParameters);
    }
}
