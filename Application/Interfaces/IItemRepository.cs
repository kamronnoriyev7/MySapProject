using MySapProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces
{
    public interface IItemRepository
    {
        Task<string> GetAllAsync();
        Task<string> GetByIdAsync(string itemCode);
        Task<string> GetFilteredAsync(string? filter, string? select, string? orderBy, int? top, int? skip);
        Task<string> CreateAsync(Item item);
        Task<string> UpdateAsync(string itemCode, Item item);
        Task<string> DeleteAsync(string itemCode);
    }
}
