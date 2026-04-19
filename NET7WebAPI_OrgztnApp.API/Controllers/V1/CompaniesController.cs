using Microsoft.AspNetCore.Mvc;
using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Domain.Commons.Company.Models;

namespace NET7WebAPI_OrgztnApp.API.Controllers.V1
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyQueryParameters companyQueryParameters)
        {
            return Ok(await _unitOfWork.Companies.GetCompaniesByQueryAsync(companyQueryParameters));
        }


        [HttpGet("get-company-by-id/{id:length(22)}")]
        public async Task<ActionResult<Company>> GetCompanyById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id is null");
            }

            var company = await _unitOfWork.Companies.GetRecordByIdAsync(id);

            return Ok(company);
        }

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
    }
}
