using Microsoft.AspNetCore.Mvc;
using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Domain.Commons.Employees.Models;

namespace NET7WebAPI_OrgztnApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeQueryParameters employeeQueryParameters)
        {
            return Ok(await _unitOfWork.Employees.GetEmployeesByQueryAsync(employeeQueryParameters));
        }


        [HttpGet("get-employee-by-id/{id:length(22)}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            return Ok(await _unitOfWork.Employees.GetRecordByIdAsync(id));
        }

        [HttpPost("add-employee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeRequest employeeRequest)
        {
            if (employeeRequest == null)
            {
                return BadRequest();
            }

            _unitOfWork.Opens_DbConnection_BeginTransaction();

            string employeeId = await _unitOfWork.Employees.AddRecordAsync(
                new Employee     //creating a new Company object & INITIALIZING Company PROPERTIES
                {
                    Name = employeeRequest.Name,
                    Age = employeeRequest.Age,
                    Position = employeeRequest.Position,
                    Salary = employeeRequest.Salary,
                    CreatedOn = employeeRequest.CreatedOn,
                    CompanyId = employeeRequest.CompanyId
                }
            );

            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return NoContent();
        }

        [HttpPut("update-employee/{id:length(22)}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] EmployeeRequest employeeRequest)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (employeeRequest == null)
            {
                return NotFound(employeeRequest);
            }

            var employeeToUpdate = await _unitOfWork.Employees.GetRecordByIdAsync(id);

            if (employeeToUpdate != null)
            {
                employeeToUpdate.Name = employeeRequest.Name;
                employeeToUpdate.Age = employeeRequest.Age;
                employeeToUpdate.Position = employeeRequest.Position;
                employeeToUpdate.Salary = employeeRequest.Salary;
                employeeToUpdate.CompanyId = employeeRequest.CompanyId;
            }

            _unitOfWork.Opens_DbConnection_BeginTransaction();

            await _unitOfWork.Employees.UpdateRecordAsync(employeeToUpdate!);

            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return NoContent();
        }

        [HttpDelete("delete-employee/{id:length(22)}")]
        public async Task<IActionResult> DeleteEmployee(string id, [FromBody] bool deleteAssociations)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employeeToDelete = await _unitOfWork.Employees.GetRecordByIdAsync(id);

            if (employeeToDelete == null)
            {
                return NotFound(employeeToDelete);
            }

            _unitOfWork.Opens_DbConnection_BeginTransaction();

            await _unitOfWork.Employees.SoftDeleteRecordAsync(id, deleteAssociations);

            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return NoContent();
        }
    }
}