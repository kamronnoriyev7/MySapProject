using MySapProject.Application.DTOs;
using MySapProject.Application.Exceptions;
using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using System.Text.Json;

namespace MySapProject.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var jsonResponse = await _repository.GetAllAsync();

            using var document = JsonDocument.Parse(jsonResponse);
            var valueElement = document.RootElement.GetProperty("value");

            var employees = JsonSerializer.Deserialize<List<EmployeeDto>>(valueElement.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return employees ?? new List<EmployeeDto>();
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID cannot be less than or equal to 0", nameof(id));

            var responseString = await _repository.GetByIdAsync(id)
                ?? throw new SapApiException($"Employee with ID '{id}' not found.", 404, null, null);

            var entity = JsonSerializer.Deserialize<Employee>(responseString)
                ?? throw new Exception("Failed to deserialize employee entity.");

            return ToDto(entity);
        }

        public async Task<IEnumerable<EmployeeDto>> GetFilteredAsync(string filter, string select, string orderBy, int? top, int? skip)
        {
            var json = await _repository.GetFilteredAsync(filter, select, orderBy, top, skip);

            using var doc = JsonDocument.Parse(json);
            var value = doc.RootElement.GetProperty("value");

            return JsonSerializer.Deserialize<List<EmployeeDto>>(value.GetRawText(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EmployeeDto>();
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto)
        {
            if (employeeDto == null)
                throw new ArgumentNullException(nameof(employeeDto), "Employee DTO cannot be null.");

            var entity = ToEntity(employeeDto);
            var created = await _repository.CreateAsync(entity)
                ?? throw new SapApiException("Failed to create employee.", 404, null, null);

            var deserialized = JsonSerializer.Deserialize<EmployeeDto>(created)
                ?? throw new Exception("Failed to deserialize created employee.");

            return deserialized;
        }

        public async Task UpdateAsync(int id, EmployeeDto employeeDto)
        {
            if (employeeDto == null)
                throw new ArgumentNullException(nameof(employeeDto));

            var entity = ToEntity(employeeDto);
            var updated = await _repository.UpdateAsync(id, entity)
                ?? throw new SapApiException($"Failed to update employee with ID '{id}'.", 404, null, null);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <=0)
                throw new ArgumentException("Invalid ID", nameof(id));

            var response = await _repository.DeleteAsync(id)
                ?? throw new SapApiException($"Failed to delete employee with ID '{id}'.", 404, null, null);
        }


        private EmployeeDto ToDto(Employee entity) => new EmployeeDto
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            JobTitle = entity.JobTitle,
            WorkCountryCode = entity.WorkCountryCode,
            Remarks = entity.Remarks
        };


        private Employee ToEntity(EmployeeDto dto) => new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            JobTitle = dto.JobTitle,
            Department = dto.Department,
            Branch = dto.Branch,
            WorkCountryCode = dto.WorkCountryCode,
            Remarks = dto.Remarks
        };

    }
}