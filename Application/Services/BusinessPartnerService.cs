using MySapProject.Application.DTOs;
using MySapProject.Application.Exceptions;
using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using System.Text.Json;

namespace MySapProject.Application.Services;

public class BusinessPartnerService : IBusinessPartnerService
{
    private readonly IBusinessPartnerRepository _repository;

    public BusinessPartnerService(IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BusinessPartnersGetDto>> GetAllAsync()
    {
        var jsonResponse = await _repository.GetAllAsync();

        using var document = JsonDocument.Parse(jsonResponse);
        var root = document.RootElement;

        var valueElement = root.GetProperty("value");

        var partners = JsonSerializer.Deserialize<List<BusinessPartnersGetDto>>(valueElement.GetRawText(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return partners ?? new List<BusinessPartnersGetDto>();
    }

    public async Task<BusinessPartnerDto?> GetByIdAsync(string cardCode)
    {
        if (string.IsNullOrWhiteSpace(cardCode))
            throw new ArgumentException("CardCode cannot be null or empty.", nameof(cardCode));

        var responseString = await _repository.GetByIdAsync(cardCode)
            ?? throw new SapApiException($"Business partner with CardCode '{cardCode}' not found.", 404, null, null);

        var entity = System.Text.Json.JsonSerializer.Deserialize<BusinessPartner>(responseString)
            ?? throw new Exception("Failed to deserialize business partner entity.");

        return ToDto(entity);
    }

    public async Task<IEnumerable<BusinessPartnersGetDto>> GetFilteredAsync(
        string? filter = null, string? select = null, string? orderBy = null, int? top = null, int? skip = null)
    {
        var json = await _repository.GetFilteredAsync(filter, select, orderBy, top, skip);

        using var doc = JsonDocument.Parse(json);
        var value = doc.RootElement.GetProperty("value");

        return JsonSerializer.Deserialize<List<BusinessPartnersGetDto>>(value.GetRawText(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new List<BusinessPartnersGetDto>();
    }


    public async Task<BusinessPartnerDto> CreateAsync(BusinessPartnerDto partnerDto)
    {
        if (partnerDto == null) throw new ArgumentNullException(nameof(partnerDto), "Partner DTO cannot be null.");
        var entity = ToEntity(partnerDto);
        var created = await _repository.CreateAsync(entity)
            ?? throw new SapApiException("Failed to create business partner.", 404, null, null);

        var deserializedEntity = System.Text.Json.JsonSerializer.Deserialize<BusinessPartnerDto>(created)
            ?? throw new Exception("Failed to deserialize created business partner.");

        return deserializedEntity;
    }

    public async Task UpdateAsync(string cardCode, BusinessPartnerDto partnerDto)
    {
        if (partnerDto == null) throw new ArgumentNullException(nameof(partnerDto), "Partner DTO cannot be null.");

        if (string.IsNullOrWhiteSpace(cardCode)) throw new ArgumentException("CardCode cannot be null or empty.", nameof(cardCode));

        var entity = ToEntity(partnerDto);
        var updated = await _repository.UpdateAsync(cardCode, entity)
            ?? throw new SapApiException($"Failed to update business partner with CardCode '{cardCode}'.", 404, null, null);
    }

    public async Task DeleteAsync(string cardCode)
    {
        var response = await _repository.DeleteAsync(cardCode)
            ?? throw new SapApiException($"Failed to delete business partner with CardCode '{cardCode}'.", 404, null, null);
    }

    private BusinessPartnerDto ToDto(BusinessPartner entity) => new BusinessPartnerDto
    {
        CardCode = entity.CardCode  ,
        CardName = entity.CardName ,
        CardType = entity.CardType 
    };

    private BusinessPartner ToEntity(BusinessPartnerDto dto) => new BusinessPartner
    {
        CardCode = dto.CardCode ,
        CardName = dto.CardName ,
        CardType = dto.CardType
    };
}
