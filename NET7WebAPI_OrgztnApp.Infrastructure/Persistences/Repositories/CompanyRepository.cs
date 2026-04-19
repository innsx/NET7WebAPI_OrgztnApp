using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Application.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.Commons.Company.Models;
using NET7WebAPI_OrgztnApp.Infrastructure.Persistences.DataContexts;
using System.Reflection;

namespace NET7WebAPI_OrgztnApp.Infrastructure.Persistences.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        /*in this constructor, We pass DapperDataContext obj instance to the 
         * BASE GenericRepository.cs class to INITIALIZE and ASSIGN it to a local _dapperDataContext field
         in the BASE GenericRepository.cs class*/
        public CompanyRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }

        public async Task<PageList<CompanyResponse>> GetCompaniesByQueryAsync(CompanyQueryParameters companyQueryParameters)
        {
            string[] selectedColumns = new string[] { "Name", "Address", "Country" };

            //using pass-in companyQueryParameters & the columns that we needed  
            var companies = (await GetAllRecordsAsync(companyQueryParameters, selectedColumns))
                    .AsQueryable() //It converts the IEnumerable<Company> result into an IQueryable<T>;
                                   //This allows the collection to be treated as a remote data source,
                                   //enabling the use of expression trees for building dynamic,
                                   //composable queries. 
                                   //Out-of-Memory Queries: When working with Entity Framework or other ORMs,
                                   //using IQueryable ensures that filtering, sorting,
                                   //and paging are translated into SQL and executed on the database server
                                   //rather than in the application's memory.
                    .Select(company => new CompanyResponse  //now we can use IQueryable<T> select( ) ext method
                    {
                        //and mapping the PROPERTIES
                        Name = company.Name,
                        Address = company.Address,
                        Country = company.Country

                    }); //in future, we will use Mapster tool to AUTOMATIC converting an OBJECT into another OBJECT


            if (!string.IsNullOrEmpty(companyQueryParameters.FilterByName))
            {
                //Filtering by FilterByName property
                companies = companies.Where(property => property.Name.ToLowerInvariant()
                                     .Contains(
                                           companyQueryParameters.FilterByName
                                           .Trim()
                                           .ToLowerInvariant()
                                      )
                );
            }


            if (!string.IsNullOrEmpty(companyQueryParameters.FilterByCountry))
            {
                //Filtering by Country property
                companies = companies.Where(property => property.Country.ToLowerInvariant()
                                     .Contains(
                                        companyQueryParameters.FilterByCountry
                                        .Trim()
                                        .ToLowerInvariant()
                                     )
                );
            }


            //we IMPLEMENT the OrderByCustom( ) SORTING LOGIC:
            if (!string.IsNullOrEmpty(companyQueryParameters.SortBy))
            {
                string toLowerCase = companyQueryParameters.SortBy.ToLower();

                PropertyInfo[] properties = typeof(Company).GetProperties();

                foreach (PropertyInfo propertyInfo in properties)
                {
                    var propertyName = propertyInfo.Name.ToLower();

                    if (propertyName == toLowerCase)
                    {
                        //we use the SortBy value pass it as input parameter & use it to Sort By
                        companies = companies.OrderByCustom(propertyInfo.Name, companyQueryParameters.SortOrder);
                    }
                }
            }

            //manually hard-coded company total Counts from tblCompany table for DEMO only
            //OPTION: we can also do a CALL to a method to RETURN tblCompany total counts,
            //but its RESOURCES INTESIVE, we do not wanted that
            int companyTotalCounts = await GetTotalRecordCountsAsync();  //will use uspGetTotalRecordCounts in GenericRepository.cs to return the counts

            //this line will get CALL every time; 
            //so therefore this will create HEAVY TRAFFIC RESULTED IN lanency
            //SOLUTION to this is: we can request it ONLY ONCE and
            //"CACHE" it and save the response in-memory,
            //so we can ACCESS the return reponse from in-memory instead
            //of from PageList.cs STATIC Create() method which it REFERENCED the object
            PageList<CompanyResponse> companyResponsePagedFormat = PageList<CompanyResponse>.Create
            (                                                                        
                companies,                                                                        
                companyQueryParameters.PageNumber,                                                                        
                companyQueryParameters.PageSize,                                                                        
                companyTotalCounts
            );

            return companyResponsePagedFormat;
        }
    }
}
