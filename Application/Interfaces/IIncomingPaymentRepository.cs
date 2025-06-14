using MySapProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces
{
    public interface IIncomingPaymentRepository
    {
        Task<string> GetAllAsync();
        Task<string> GetByIdAsync(int id);
        Task<string> GetFilteredAsync(string? filter = null, string? select = null, string? orderBy = null, int? top = null, int? skip = null);
        Task<string> CreateAsync(IncomingPayment payment);
        Task<string> UpdateAsync(int id, object updatePayload);
        Task<string> CancelAsync(int id);
        Task<string> GetApprovalTemplatesAsync(int id);
        Task<string> CancelByCurrentSystemDateAsync(int id);
        Task<string> RequestApproveCancellationAsync(int id);
    }
}
