using MySapProject.Application.DTOs;
using MySapProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces
{
    public interface IPurchaseInvoiceRepository
    {
        Task<string> GetAllAsync(int skip = 0, int top = 10);
        Task<string> GetByIdAsync(int id);
        Task<string> CreateAsync(PurchaseInvoice entity);
        Task<string> UpdateAsync(int id, PurchaseInvoice entity);
        Task<string> CancelAsync(int id);
    }
}
