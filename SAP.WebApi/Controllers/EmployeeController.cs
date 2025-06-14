using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySapProject.Application.DTOs;
using MySapProject.Application.Services;

namespace MySapProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest("ID cannot be null.");
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound($"Employee with ID '{id}' not found.");
            return Ok(employee);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredAsync(
            [FromQuery] string? filter = null,
            [FromQuery] string? select = null,
            [FromQuery] string? orderBy = null,
            [FromQuery] int? top = null,
            [FromQuery] int? skip = null)
        {
            var employees = await _employeeService.GetFilteredAsync(filter, select, orderBy, top, skip);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
                return BadRequest("Employee data cannot be null.");
            var createdEmployee = await _employeeService.CreateAsync(employeeDto);
            return Ok(createdEmployee); 
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] EmployeeDto employeeDto)
        {
            if (id <=0)
                return BadRequest("ID must be a positive integer.");
            if (employeeDto == null)
                return BadRequest("Employee data cannot be null.");

            var existingEmployee = await _employeeService.GetByIdAsync(id);
            if (existingEmployee == null)
                return NotFound($"Employee with ID '{id}' not found.");

            await _employeeService.UpdateAsync(id, employeeDto);
            return Ok($"Employee with ID '{id}' has been updated successfully.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest("ID must be a positive integer.");
            var existingEmployee = await _employeeService.GetByIdAsync(id);
            if (existingEmployee == null)
                return NotFound($"Employee with ID '{id}' not found.");
            await _employeeService.DeleteAsync(id);
            return NoContent(); 
        }
    }
}
