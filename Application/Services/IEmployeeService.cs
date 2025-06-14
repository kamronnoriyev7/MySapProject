using MySapProject.Application.DTOs;

namespace MySapProject.Application.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int employeeId);
        Task<IEnumerable<EmployeeDto>> GetFilteredAsync(string filter, string select, string orderBy, int? top, int? skip);
        Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto);
        Task UpdateAsync(int employeeId, EmployeeDto employeeDto);
        Task DeleteAsync(int employeeId);
    }
}