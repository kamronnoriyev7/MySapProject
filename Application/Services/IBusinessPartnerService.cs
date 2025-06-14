using MySapProject.Application.DTOs;

namespace MySapProject.Application.Services
{
    public interface IBusinessPartnerService
    {
        Task<IEnumerable<BusinessPartnersGetDto>> GetAllAsync();
        Task<BusinessPartnerDto?> GetByIdAsync(string cardCode);
        Task<IEnumerable<BusinessPartnersGetDto>> GetFilteredAsync(string? filter = null,
                          string? select = null,
                          string? orderBy = null,
                          int? top = null,
                          int? skip = null);
        Task<BusinessPartnerDto> CreateAsync(BusinessPartnerDto partnerDto);
        Task UpdateAsync(string cardCode, BusinessPartnerDto partnerDto);
        Task DeleteAsync(string cardCode);
    }
}
