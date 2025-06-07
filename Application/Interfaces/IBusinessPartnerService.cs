using MySapProject.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces
{
    public interface IBusinessPartnerService
    {
        Task<BusinessPartnerDto> GetBusinessPartnerByIdAsync(string id);
        Task<IEnumerable<BusinessPartnerDto>> GetAllBusinessPartnersAsync();
        Task<BusinessPartnerDto> CreateBusinessPartnerAsync(BusinessPartnerDto businessPartnerDto);
        Task<BusinessPartnerDto> UpdateBusinessPartnerAsync(string id, BusinessPartnerDto businessPartnerDto);
        Task DeleteBusinessPartnerAsync(string id);
    }
}
