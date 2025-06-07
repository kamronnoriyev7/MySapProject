using Microsoft.Extensions.Logging; // Added for ILogger
using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySapProject.Application.Services
{
    public class BusinessPartnerService : IBusinessPartnerService
    {
        private readonly IBusinessPartnerRepository _businessPartnerRepository;
        private readonly ILogger<BusinessPartnerService> _logger; // Added ILogger

        public BusinessPartnerService(IBusinessPartnerRepository businessPartnerRepository, ILogger<BusinessPartnerService> logger) // Injected ILogger
        {
            _businessPartnerRepository = businessPartnerRepository ?? throw new ArgumentNullException(nameof(businessPartnerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize ILogger
        }

        public async Task<BusinessPartnerDto> CreateBusinessPartnerAsync(BusinessPartnerDto businessPartnerDto)
        {
            if (businessPartnerDto == null)
            {
                _logger.LogWarning("CreateBusinessPartnerAsync called with null DTO.");
                throw new ArgumentNullException(nameof(businessPartnerDto));
            }
            if (string.IsNullOrWhiteSpace(businessPartnerDto.CardName))
            {
                _logger.LogWarning("CreateBusinessPartnerAsync called with empty CardName.");
                throw new ArgumentException("Business partner name cannot be empty.", nameof(businessPartnerDto.CardName));
            }

            _logger.LogInformation("Attempting to create business partner via repository with CardName: {CardName}", businessPartnerDto.CardName);
            var result = await _businessPartnerRepository.CreateAsync(businessPartnerDto);
            _logger.LogInformation("Business partner created via repository with CardCode: {CardCode}", result.CardCode);
            return result;
        }

        public async Task DeleteBusinessPartnerAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("DeleteBusinessPartnerAsync called with empty ID.");
                throw new ArgumentException("Business partner ID cannot be empty.", nameof(id));
            }
            _logger.LogInformation("Attempting to delete business partner via repository with ID: {BusinessPartnerId}", id);
            await _businessPartnerRepository.DeleteAsync(id);
            _logger.LogInformation("Business partner deletion command sent to repository for ID: {BusinessPartnerId}", id);
            // If repository indicated "not found", we might log that here.
            // For now, the repository's placeholder doesn't provide this feedback.
        }

        public async Task<IEnumerable<BusinessPartnerDto>> GetAllBusinessPartnersAsync()
        {
            _logger.LogInformation("Attempting to get all business partners from repository.");
            var results = await _businessPartnerRepository.GetAllAsync();
            _logger.LogInformation("Retrieved all business partners from repository. Count: {Count}", results?.Count() ?? 0);
            return results;
        }

        public async Task<BusinessPartnerDto> GetBusinessPartnerByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("GetBusinessPartnerByIdAsync called with empty ID.");
                throw new ArgumentException("Business partner ID cannot be empty.", nameof(id));
            }
            _logger.LogInformation("Attempting to get business partner by ID from repository: {BusinessPartnerId}", id);
            var result = await _businessPartnerRepository.GetByIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Business partner not found in repository for ID: {BusinessPartnerId}", id);
            }
            else
            {
                _logger.LogInformation("Business partner found in repository for ID: {BusinessPartnerId}", id);
            }
            return result;
        }

        public async Task<BusinessPartnerDto> UpdateBusinessPartnerAsync(string id, BusinessPartnerDto businessPartnerDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("UpdateBusinessPartnerAsync called with empty ID.");
                throw new ArgumentException("Business partner ID cannot be empty.", nameof(id));
            }
            if (businessPartnerDto == null)
            {
                _logger.LogWarning("UpdateBusinessPartnerAsync called with null DTO for ID: {BusinessPartnerId}", id);
                throw new ArgumentNullException(nameof(businessPartnerDto));
            }
            if (string.IsNullOrWhiteSpace(businessPartnerDto.CardName))
            {
                _logger.LogWarning("UpdateBusinessPartnerAsync called with empty CardName for ID: {BusinessPartnerId}", id);
                throw new ArgumentException("Business partner name cannot be empty.", nameof(businessPartnerDto.CardName));
            }

            _logger.LogInformation("Attempting to update business partner via repository with ID: {BusinessPartnerId}", id);
            var result = await _businessPartnerRepository.UpdateAsync(id, businessPartnerDto);
            if (result == null)
            {
                _logger.LogWarning("Business partner not updated in repository (possibly not found) for ID: {BusinessPartnerId}", id);
            }
            else
            {
                _logger.LogInformation("Business partner updated via repository for ID: {BusinessPartnerId}", id);
            }
            return result;
        }
    }
}
