using MySapProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services
{
    public interface IPurchaseInvoiceService
    {
        Task<IEnumerable<PurchaseInvoiceDto>> GetAllAsync(int skip = 0, int top = 10);
        Task<PurchaseInvoiceDto?> GetByIdAsync(int id);
        Task<PurchaseInvoiceDto> CreateAsync(PurchaseInvoiceDto dto);
        Task UpdateAsync(int id, PurchaseInvoiceDto dto);
        Task CancelAsync(int id);
    }
}
