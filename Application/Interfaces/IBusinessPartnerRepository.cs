using MySapProject.Domain.Entities;

namespace MySapProject.Application.Interfaces
{
    public interface IBusinessPartnerRepository
    {
        Task<string> GetAllAsync();

        Task<string> GetByIdAsync(string cardCode);

        Task<string> GetFilteredAsync(
            string? filter = null,
            string? select = null,
            string? orderBy = null,
            int? top = null,
            int? skip = null);

        Task<string> CreateAsync(BusinessPartner partner);

        Task<string> UpdateAsync(string cardCode, BusinessPartner partner);

        Task<string> DeleteAsync(string cardCode);
    }
}
