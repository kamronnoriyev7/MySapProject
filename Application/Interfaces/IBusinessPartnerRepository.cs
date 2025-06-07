using MySapProject.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces
{
    public interface IBusinessPartnerRepository
    {
        Task<BusinessPartnerDto> GetByIdAsync(string id);
        Task<IEnumerable<BusinessPartnerDto>> GetAllAsync();
        Task<BusinessPartnerDto> CreateAsync(BusinessPartnerDto businessPartner);
        Task<BusinessPartnerDto> UpdateAsync(string id, BusinessPartnerDto businessPartner);
        Task DeleteAsync(string id);
    }
}
