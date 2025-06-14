using MySapProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<string> GetAllAsync();
        Task<string> GetByIdAsync(int id);
        Task<string> GetFilteredAsync(string? filter = null, string? select = null, string? orderBy = null, int? top = null, int? skip = null);
        Task<string> CreateAsync(Employee employee);
        Task<string> DeleteAsync(int id);
        Task<string> UpdateAsync(int id, Employee employee);
    }
}
