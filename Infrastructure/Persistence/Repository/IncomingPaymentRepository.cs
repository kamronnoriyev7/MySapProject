using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using MySapProject.Infrastructure.Extensions;
using System.Text;
using System.Text.Json;

namespace MySapProject.Infrastructure.Persistence.Repository
{
    public class IncomingPaymentRepository : IIncomingPaymentRepository
    {
        private readonly HttpClient _httpClient;

        public IncomingPaymentRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("IncomingPayments");
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"IncomingPayments({id})");
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetFilteredAsync(string? filter = null, string? select = null, string? orderBy = null, int? top = null, int? skip = null)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(select)) queryParams.Add($"$select={select}");
            if (!string.IsNullOrWhiteSpace(filter)) queryParams.Add($"$filter={Uri.EscapeDataString(filter)}");
            if (!string.IsNullOrWhiteSpace(orderBy)) queryParams.Add($"$orderby={orderBy}");
            if (top.HasValue) queryParams.Add($"$top={top.Value}");
            if (skip.HasValue) queryParams.Add($"$skip={skip.Value}");

            var url = "IncomingPayments";
            if (queryParams.Count > 0) url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateAsync(IncomingPayment payment)
        {
            var json = JsonSerializer.Serialize(payment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("IncomingPayments", content);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateAsync(int id, object updatePayload)
        {
            var json = JsonSerializer.Serialize(updatePayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"IncomingPayments({id})", content);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CancelAsync(int id)
        {
            var response = await _httpClient.PostAsync($"IncomingPayments({id})/Cancel", null);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetApprovalTemplatesAsync(int id)
        {
            var response = await _httpClient.PostAsync($"IncomingPayments({id})/GetApprovalTemplates", null);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CancelByCurrentSystemDateAsync(int id)
        {
            var response = await _httpClient.PostAsync($"IncomingPayments({id})/CancelbyCurrentSystemDate", null);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RequestApproveCancellationAsync(int id)
        {
            var response = await _httpClient.PostAsync($"IncomingPayments({id})/RequestApproveCancellation", null);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }
    }
}