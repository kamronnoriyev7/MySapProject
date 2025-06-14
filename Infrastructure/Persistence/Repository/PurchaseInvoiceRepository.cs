using MySapProject.Application.DTOs;
using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using MySapProject.Infrastructure.Extensions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MySapProject.Infrastructure.Persistence.Repository;
public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
{
    private readonly HttpClient _httpClient;

    public PurchaseInvoiceRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetAllAsync(int skip = 0, int top = 10)
    {
        var url = $"PurchaseInvoices?$orderby=DocEntry desc&$top={top}&$skip={skip}";
        var response = await _httpClient.GetAsync(url);
        await response.EnsureSuccessOrThrowAsync();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"PurchaseInvoices({id})");
        await response.EnsureSuccessOrThrowAsync();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> CreateAsync(PurchaseInvoice entity)
    {
        var json = JsonSerializer.Serialize(entity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("PurchaseInvoices", content);
        await response.EnsureSuccessOrThrowAsync();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> UpdateAsync(int id, PurchaseInvoice entity)
    {
        var json = JsonSerializer.Serialize(entity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync($"PurchaseInvoices({id})", content);
        await response.EnsureSuccessOrThrowAsync();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> CancelAsync(int id)
    {
        var response = await _httpClient.PostAsync($"PurchaseInvoices({id})/Cancel", null);
        await response.EnsureSuccessOrThrowAsync();
        return await response.Content.ReadAsStringAsync();
    }
}

