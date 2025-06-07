using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Added for ILogger
using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySapProject.SAP.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessPartnersController : ControllerBase
    {
        private readonly IBusinessPartnerService _businessPartnerService;
        private readonly ILogger<BusinessPartnersController> _logger; // Added ILogger

        public BusinessPartnersController(IBusinessPartnerService businessPartnerService, ILogger<BusinessPartnersController> logger) // Injected ILogger
        {
            _businessPartnerService = businessPartnerService ?? throw new ArgumentNullException(nameof(businessPartnerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize ILogger
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessPartnerDto>>> GetAllBusinessPartners()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve all business partners.");
                var businessPartners = await _businessPartnerService.GetAllBusinessPartnersAsync();
                _logger.LogInformation("Successfully retrieved all business partners.");
                return Ok(businessPartners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all business partners.");
                return StatusCode(500, "An error occurred while retrieving business partners. Please check logs for details.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessPartnerDto>> GetBusinessPartnerById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("GetBusinessPartnerById called with empty ID.");
                return BadRequest("Business partner ID cannot be empty.");
            }

            try
            {
                _logger.LogInformation("Attempting to retrieve business partner with ID: {BusinessPartnerId}", id);
                var businessPartner = await _businessPartnerService.GetBusinessPartnerByIdAsync(id);
                if (businessPartner == null)
                {
                    _logger.LogWarning("Business partner with ID: {BusinessPartnerId} not found.", id);
                    return NotFound($"Business partner with ID {id} not found.");
                }
                _logger.LogInformation("Successfully retrieved business partner with ID: {BusinessPartnerId}", id);
                return Ok(businessPartner);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument exception while retrieving business partner with ID: {BusinessPartnerId}.", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving business partner with ID: {BusinessPartnerId}.", id);
                return StatusCode(500, $"An error occurred while retrieving business partner with ID {id}. Please check logs for details.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BusinessPartnerDto>> CreateBusinessPartner([FromBody] BusinessPartnerDto businessPartnerDto)
        {
            if (businessPartnerDto == null)
            {
                _logger.LogWarning("CreateBusinessPartner called with null DTO.");
                return BadRequest("Business partner data cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateBusinessPartner called with invalid model state: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Attempting to create a new business partner with CardName: {CardName}", businessPartnerDto.CardName);
                var createdBusinessPartner = await _businessPartnerService.CreateBusinessPartnerAsync(businessPartnerDto);
                _logger.LogInformation("Successfully created business partner with ID: {BusinessPartnerId}", createdBusinessPartner.CardCode);
                return CreatedAtAction(nameof(GetBusinessPartnerById), new { id = createdBusinessPartner.CardCode }, createdBusinessPartner);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument exception while creating business partner with CardName: {CardName}.", businessPartnerDto.CardName);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating business partner with CardName: {CardName}.", businessPartnerDto.CardName);
                return StatusCode(500, "An error occurred while creating the business partner. Please check logs for details.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BusinessPartnerDto>> UpdateBusinessPartner(string id, [FromBody] BusinessPartnerDto businessPartnerDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("UpdateBusinessPartner called with empty ID.");
                return BadRequest("Business partner ID cannot be empty.");
            }
            if (businessPartnerDto == null)
            {
                _logger.LogWarning("UpdateBusinessPartner called with null DTO for ID: {BusinessPartnerId}.", id);
                return BadRequest("Business partner data cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateBusinessPartner called with invalid model state for ID: {BusinessPartnerId}: {ModelState}", id, ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Attempting to update business partner with ID: {BusinessPartnerId}", id);
                var updatedBusinessPartner = await _businessPartnerService.UpdateBusinessPartnerAsync(id, businessPartnerDto);
                if (updatedBusinessPartner == null)
                {
                    // This case might be handled by the service throwing an exception, or returning null if "update if exists"
                    _logger.LogWarning("Business partner with ID: {BusinessPartnerId} not found for update.", id);
                    return NotFound($"Business partner with ID {id} not found for update.");
                }
                _logger.LogInformation("Successfully updated business partner with ID: {BusinessPartnerId}", id);
                return Ok(updatedBusinessPartner);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument exception while updating business partner with ID: {BusinessPartnerId}.", id);
                return BadRequest(ex.Message);
            }
            // Consider specific exception for "Not Found" if the service throws it
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating business partner with ID: {BusinessPartnerId}.", id);
                return StatusCode(500, $"An error occurred while updating business partner with ID {id}. Please check logs for details.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessPartner(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("DeleteBusinessPartner called with empty ID.");
                return BadRequest("Business partner ID cannot be empty.");
            }

            try
            {
                _logger.LogInformation("Attempting to delete business partner with ID: {BusinessPartnerId}", id);
                await _businessPartnerService.DeleteBusinessPartnerAsync(id);
                _logger.LogInformation("Successfully deleted business partner with ID: {BusinessPartnerId}", id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument exception while deleting business partner with ID: {BusinessPartnerId}.", id);
                return BadRequest(ex.Message);
            }
            // Consider specific exception for "Not Found" from service if applicable
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting business partner with ID: {BusinessPartnerId}.", id);
                return StatusCode(500, $"An error occurred while deleting business partner with ID {id}. Please check logs for details.");
            }
        }
    }
}
