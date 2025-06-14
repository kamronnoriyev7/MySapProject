using MySapProject.Application.DTOs;

namespace MySapProject.Application.Services
{
    public interface IIncomingPaymentService
    {
        Task<IEnumerable<IncomingPaymentDto>> GetAllAsync();
        Task<IncomingPaymentDto?> GetByIdAsync(int id);
        Task<IEnumerable<IncomingPaymentDto>> GetFilteredAsync(string? filter, string? select, string? orderBy, int? top, int? skip);
        Task<IncomingPaymentDto> CreateAsync(IncomingPaymentDto dto);
        Task UpdateAsync(int id, object updatePayload);
        Task CancelAsync(int id);
        Task<string> GetApprovalTemplatesAsync(int id);
        Task<string> CancelByCurrentSystemDateAsync(int id);
        Task<string> RequestApproveCancellationAsync(int id);
    }
}