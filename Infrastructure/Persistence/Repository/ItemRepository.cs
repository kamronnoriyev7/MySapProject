using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using MySapProject.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MySapProject.Infrastructure.Persistence.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly HttpClient _httpClient;

        public ItemRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("Items");
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetByIdAsync(string itemCode)
        {
            var response = await _httpClient.GetAsync($"Items('{itemCode}')");
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetFilteredAsync(string? filter, string? select, string? orderBy, int? top, int? skip)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(select))
                queryParams.Add($"$select={select}");
            if (!string.IsNullOrWhiteSpace(filter))
                queryParams.Add($"$filter={Uri.EscapeDataString(filter)}");
            if (!string.IsNullOrWhiteSpace(orderBy))
                queryParams.Add($"$orderby={orderBy}");
            if (top.HasValue)
                queryParams.Add($"$top={top.Value}");
            if (skip.HasValue)
                queryParams.Add($"$skip={skip.Value}");

            var url = "Items";
            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateAsync(Item item)
        {
            var json = JsonSerializer.Serialize(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Items", content);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateAsync(string itemCode, Item item)
        {
            var json = JsonSerializer.Serialize(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"Items('{itemCode}')", content);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(string itemCode)
        {
            var response = await _httpClient.DeleteAsync($"Items('{itemCode}')");
            await response.EnsureSuccessOrThrowAsync();

            return response.StatusCode == System.Net.HttpStatusCode.NoContent
                ? "Deleted successfully"
                : await response.Content.ReadAsStringAsync();
        }
    }
}
