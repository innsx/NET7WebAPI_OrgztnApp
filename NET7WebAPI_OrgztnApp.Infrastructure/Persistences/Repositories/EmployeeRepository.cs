using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Application.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.Commons.Employees.Models;
using NET7WebAPI_OrgztnApp.Infrastructure.Persistences.DataContexts;
using System.Reflection;

namespace NET7WebAPI_OrgztnApp.Infrastructure.Persistences.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        /*in this constructor, We pass DapperDataContext obj instance to the 
         * BASE GenericRepository.cs class to INITIALIZE and ASSIGN it to a local _dapperDataContext field
         in the BASE GenericRepository.cs class*/
        public EmployeeRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }

        public async Task<PageList<EmployeeResponse>> GetEmployeesByQueryAsync(EmployeeQueryParameters employeeQueryParameters)
        {
            string[] selectedColumns = new string[] 
            { 
                "Id", "Name", "Age", "Position", "Salary", "CreatedOn", "ModifiedOn", "CompanyId"             
            };

            //using the pass-in EmployeeQueryParameters
            //and we specified the columns that we needed
            //that we've declared in DTO EmployeeResponse 
            var employees = (await GetAllRecordsAsync(employeeQueryParameters, selectedColumns))
                                .AsQueryable() //converts the return results into Queryable objects
                                .Select(employee => new EmployeeResponse  
                                {
                                    //manually mapping the selected PROPERTIES & assigned them to its EmployeeResponse property
                                    Id = employee.Id,
                                    Name = employee.Name,
                                    Age = employee.Age,
                                    Position = employee.Position,
                                    Salary = employee.Salary,
                                    CreatedOn = employee.CreatedOn,
                                    ModifiedOn = employee.ModifiedOn,
                                    CompanyId = employee.CompanyId

                                }); //in future, we will use Mapester tool to AUTOMATIC converting an OBJECT into another OBJECT


            if (!string.IsNullOrEmpty(employeeQueryParameters.FilterByName))
            {
                //Filtering by FilterByName property
                employees = employees.Where(property => property.Name.ToLowerInvariant()
                                                        .Contains(
                                                            employeeQueryParameters.FilterByName
                                                            .Trim()
                                                            .ToLowerInvariant()
                                                        )
                );
            }


            //we IMPLEMENT the OrderByCustom( ) SORTING LOGIC:
            if (!string.IsNullOrEmpty(employeeQueryParameters.SortBy))
            {
                string toLowerCase = employeeQueryParameters.SortBy.ToLower();

                PropertyInfo[] properties = typeof(Employee).GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    var propertyName = propertyInfo.Name.ToLower();

                    if (propertyName == toLowerCase)
                    {
                        //we use the SortBy value pass it as input parameter & use it to Sort By
                        employees = employees.OrderByCustom(propertyInfo.Name, employeeQueryParameters.SortOrder);
                    }
                }
            }


            //DEMO only: manually hard-coded employees total Counts = 200000000; from tblEmployee table
            //OPTION: we can also do a CALL to "await GetTotalCountAsync();" to RETURN tblEmployee total counts,
            //because of the 2 millions records, its RESOURCES INTESIVE, we do not wanted that
            int employeesTotalCount = await GetTotalRecordCountsAsync();    //= 200000000;

            //this line will get CALL every time; 
            //so therefore this will create HEAVY TRAFFIC RESULTED IN lanency
            //SOLUTION to this is: we can request it ONLY ONCE and
            //"CACHE" it and save the response in-memory,
            //so we can ACCESS the return reponse from in-memory instead
            //of from PageList.cs STATIC Create() method which it REFERENCED the object
            var outputPagedFormat = PageList<EmployeeResponse>.Create
            (
                employees,
                employeeQueryParameters.PageNumber,
                employeeQueryParameters.PageSize,
                employeesTotalCount);

            return outputPagedFormat;
        }
    }
}





//string firstCharToUpper = char.ToUpper(employeeQueryParameters.SortBy[0]) + employeeQueryParameters.SortBy.Substring(1);

////here, we’re using REFLECTION to get the specified PROPERTY
//var property = typeof(Employee).GetProperty(firstCharToUpper);

//if (property != null)
//{
//    //we use the SortBy value pass it as input parameter & use it to Sort By
//    employees = employees.OrderByCustom(employeeQueryParameters.SortBy, employeeQueryParameters.SortOrder);
//}
