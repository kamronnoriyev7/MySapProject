using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using MySapProject.Infrastructure.Extensions;
using System.Text;
using System.Text.Json;

namespace MySapProject.Infrastructure.Persistence.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HttpClient _httpClient;

        public EmployeeRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("EmployeesInfo");
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"EmployeesInfo({id})");
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

            var url = "EmployeesInfo";
            if (queryParams.Count > 0) url += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateAsync(Employee employee)
        {
            var json = JsonSerializer.Serialize(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("EmployeesInfo", content);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }
      
        public async Task<string> UpdateAsync(int id, Employee employee)
        {
            var json = JsonSerializer.Serialize(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"EmployeesInfo({id})", content);
            await response.EnsureSuccessOrThrowAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"EmployeesInfo({id})");
            await response.EnsureSuccessOrThrowAsync();

            return response.StatusCode == System.Net.HttpStatusCode.NoContent
                ? "Deleted successfully"
                : await response.Content.ReadAsStringAsync();
        }

       
    }
}