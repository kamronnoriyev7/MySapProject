using MySapProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<ItemDto?> GetByIdAsync(string itemCode);
        Task<IEnumerable<ItemDto>> GetFilteredAsync(string filter, string select, string orderBy, int? top, int? skip);
        Task<ItemDto> CreateAsync(ItemDto dto);
        Task UpdateAsync(string itemCode, ItemDto dto);
        Task DeleteAsync(string itemCode);
    }
}
