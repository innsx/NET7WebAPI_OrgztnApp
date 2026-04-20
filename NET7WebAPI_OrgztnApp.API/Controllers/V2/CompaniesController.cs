using Microsoft.AspNetCore.Mvc;
using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Application.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.Commons.Company.Models;

namespace NET7WebAPI_OrgztnApp.API.Controllers.V2
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    [Produces("application/json")]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// This endpoint gets all the companies in the system.
        /// </summary>
        /// <respone code="200">Returns paged list of all companies in the system</respone>
        [ProducesResponseType(typeof(PageList<CompanyResponse>), 200)]
        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyQueryParameters companyQueryParameters)
        {
            return Ok(await _unitOfWork.Companies.GetCompaniesByQueryAsync(companyQueryParameters));
        }


        /// <summary>
        /// This endpoint gets a particular company from the system based on the provided copany id.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <response code="200">Gets a comapny successfully.</response>
        /// <response code="404">Could not find the company.</response>
        /// <returns>Company</returns>
        [HttpGet("company/{id:length(22)}")]
        public async Task<ActionResult<Company>> GetCompanyById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id is null");
            }

            var company = await _unitOfWork.Companies.GetRecordByIdAsync(id);

            return Ok(company);
        }


        /// <summary>
        /// This endpoint Add a company in the system.
        /// </summary>
        /// <param name="companyRequest">**CreateCompanyRequest**</param>
        /// <response code="201">Adds a company successfullly</response>
        [HttpPost("add-company")]
        public async Task<IActionResult> AddCompany([FromBody]CompanyRequest companyRequest)
        {
            if (companyRequest == null)
            {
                return BadRequest("Company is null");
            }

            var companyAddedId = await _unitOfWork.Companies.AddRecordAsync(
                new Company
                {
                    Name = companyRequest.Name,
                    Address = companyRequest.Address,
                    Country = companyRequest.Country,
                }
            );

            return CreatedAtAction("GetCompanyById", new { id = companyAddedId}, companyRequest);
        }


        /// <summary>
        /// This endpoint Update a company in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="companyRequest">**CreateCompanyRequest**</param>
        /// <response code="201">Updates a company successfullly</response>
        [HttpPut("update-company/{id:length(22)}")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] CompanyRequest companyRequest)
        {
            if (companyRequest == null || id == null)
            {
                return BadRequest();
            }

            var companyToUpdate = await _unitOfWork.Companies.GetRecordByIdAsync(id);

            companyToUpdate.Name = companyRequest.Name;
            companyToUpdate.Address = companyRequest.Address;
            companyToUpdate.Country = companyRequest.Country;

            _unitOfWork.Opens_DbConnection_BeginTransaction();
            await _unitOfWork.Companies.UpdateRecordAsync(companyToUpdate);
            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return Ok(companyToUpdate);
        }


        /// <summary>
        /// This endpoint SoftDelete of a company in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="isDeleteAssociations">**CompanyRequest**</param>
        /// <response code="201">SoftDeletes a company successfullly</response>
        [HttpDelete("delete-company/{id:length(22)}")]
        public async Task<IActionResult> DeleteCompany(string id, bool isDeleteAssociations)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var companyToDelete = await _unitOfWork.Companies.GetRecordByIdAsync(id);

            _unitOfWork.Opens_DbConnection_BeginTransaction();
            await _unitOfWork.Companies.SoftDeleteRecordAsync(id, isDeleteAssociations);
            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return Ok(companyToDelete);
        }


        /// <summary>
        /// This endpoint counts and returns total company records in the system.
        /// </summary>
        /// <respone code="200">Returns an integer as Total Comany Counts</respone>
        [HttpGet("company-counts")]
        public async Task<IActionResult> GetCompanyCounts()
        {
            return Ok(await _unitOfWork.Companies.GetTotalRecordCountsAsync());
        }
    }
}
