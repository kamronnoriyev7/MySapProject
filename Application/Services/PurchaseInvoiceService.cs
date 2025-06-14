using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MySapProject.Application.Services;
public class PurchaseInvoiceService : IPurchaseInvoiceService
{
    private readonly IPurchaseInvoiceRepository _repository;

    public PurchaseInvoiceService(IPurchaseInvoiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PurchaseInvoiceDto>> GetAllAsync(int skip = 0, int top = 10)
    {
        var json = await _repository.GetAllAsync(skip, top);
        using var doc = JsonDocument.Parse(json);
        var value = doc.RootElement.GetProperty("value");
        return JsonSerializer.Deserialize<List<PurchaseInvoiceDto>>(value.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PurchaseInvoiceDto>();
    }

    public async Task<PurchaseInvoiceDto?> GetByIdAsync(int id)
    {
        var json = await _repository.GetByIdAsync(id);
        return JsonSerializer.Deserialize<PurchaseInvoiceDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<PurchaseInvoiceDto> CreateAsync(PurchaseInvoiceDto dto)
    {
        var entity = ToEntity(dto);
        var json = await _repository.CreateAsync(entity);
        return JsonSerializer.Deserialize<PurchaseInvoiceDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task UpdateAsync(int id, PurchaseInvoiceDto dto)
    {
        var entity = ToEntity(dto);
        await _repository.UpdateAsync(id, entity);
    }

    public async Task CancelAsync(int id)
    {
        await _repository.CancelAsync(id);
    }

    private PurchaseInvoice ToEntity(PurchaseInvoiceDto dto) => new()
    {
        CardCode = dto.CardCode,
        DocDate = dto.DocDate,
        DocCurrency = dto.DocCurrency,
        Comments = dto.Comments,
        DocumentLines = dto.DocumentLines.Select(line => new PurchaseInvoiceLine
        {
            ItemCode = line.ItemCode,
            Quantity = line.Quantity,
            Price = line.Price
        }).ToList()
    };

}