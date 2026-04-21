using Microsoft.AspNetCore.Mvc;
using NET7WebAPI_OrgztnApp.Application.Common.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.DTOs;
using NET7WebAPI_OrgztnApp.Application.Commons.Execeptions;
using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Application.Commons.Utilities;
using NET7WebAPI_OrgztnApp.Domain.Commons.Employees.Models;

namespace NET7WebAPI_OrgztnApp.API.Controllers.V2
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// This endpoint gets all the Employees in the system.
        /// </summary>
        /// <respone code="200">Returns paged list of all Employees in the system</respone>
        [ProducesResponseType(typeof(PageList<EmployeeResponse>), 200)]
        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeQueryParameters employeeQueryParameters)
        {
            var employees = await _unitOfWork.Employees.GetEmployeesByQueryAsync(employeeQueryParameters);

            if (employees == null)
            {
                throw new Exception("Error exception: no records returned.");
            }

            return Ok(employees);
        }


        /// <summary>
        /// This endpoint gets a particular Employee from the system based on the provided Employee id.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <response code="200">Gets a Employee successfully.</response>
        /// <response code="404">Could not find the Employee.</response>
        /// <returns>Company</returns>
        [HttpGet("employee/{id:length(22)}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var employee = await _unitOfWork.Employees.GetRecordByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException($"Employee with id: '{id}' does not existed.");
            }

            return Ok(employee);
        }

        /// <summary>
        /// This endpoint Adds an Employee in the system.
        /// </summary>
        /// <param name="employeeRequest">**EmployeeRequest**</param>
        /// <response code="201">Adds an Employee successfullly</response>
        [HttpPost("add-employee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeRequest employeeRequest)
        {
            if (await _unitOfWork.Employees.IsExistedAsync(employeeRequest.Name) == true)
            {
                //check if the request Company Name is NOT UNIQUE
                throw new DuplicateNameException($"Employee with Name '{employeeRequest.Name}' is ALREADY EXISTED.");
            }

            _unitOfWork.Opens_DbConnection_BeginTransaction();

            string employeeId = await _unitOfWork.Employees.AddRecordAsync(
                new Employee     //creating a new Company object & INITIALIZING Company PROPERTIES
                {
                    Name = employeeRequest.Name,
                    Age = employeeRequest.Age,
                    Position = employeeRequest.Position,
                    Salary = employeeRequest.Salary,
                    CreatedOn = DateTime.Now,
                    //ModifiedOn = DateTime.Now,
                    CompanyId = employeeRequest.CompanyId
                }
            );

            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return CreatedAtAction("GetEmployeeById", new { id = employeeId }, employeeRequest);
        }

        /// <summary>
        /// This endpoint Updates a Employee in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="employeeRequest">**EmployeeResponse**</param>
        /// <response code="201">Updates a Employee successfullly</response>
        [HttpPut("update-employee/{id:length(22)}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] EmployeeRequest employeeRequest)
        {
            var employeeToUpdate = await _unitOfWork.Employees.GetRecordByIdAsync(id);

            if (employeeToUpdate == null)
            {
                throw new NotFoundException($"Employee with id: '{id}' does not existed.");
            }

            employeeToUpdate.Name = employeeRequest.Name;
            employeeToUpdate.Age = employeeRequest.Age;
            employeeToUpdate.Position = employeeRequest.Position;
            employeeToUpdate.Salary = employeeRequest.Salary;
            employeeToUpdate.CreatedOn = employeeToUpdate.CreatedOn;
            employeeToUpdate.ModifiedOn = DateTime.Now;
            employeeToUpdate.CompanyId = employeeRequest.CompanyId;

            _unitOfWork.Opens_DbConnection_BeginTransaction();

            await _unitOfWork.Employees.UpdateRecordAsync(employeeToUpdate!);

            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return Ok(employeeToUpdate);
        }


        /// <summary>
        /// This SoftDelete of an Employee in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="isDeleteAssociations">**Boolean**</param>
        /// <response code="201">SoftDeletes an Employee successfullly</response>
        [HttpDelete("delete-employee/{id:length(22)}")]
        public async Task<IActionResult> DeleteEmployee(string id, [FromQuery] bool deleteAssociations)
        {
            var employeeToDelete = await _unitOfWork.Employees.GetRecordByIdAsync(id);

            if (employeeToDelete == null)
            {
                throw new NotFoundException($"Employee with id: '{id}' does not existed.");
            }

            _unitOfWork.Opens_DbConnection_BeginTransaction();

            await _unitOfWork.Employees.SoftDeleteRecordAsync(id, deleteAssociations);

            _unitOfWork.Commits_Transaction_N_Close_DbConnection_InvokeDispose();

            return NoContent();
        }
    }
}