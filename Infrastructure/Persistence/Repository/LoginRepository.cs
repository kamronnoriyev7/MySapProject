using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MySapProject.Infrastructure.Persistence.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly HttpClient _httpClient;

        public LoginRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(LoginRequest loginRequest)
        {
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Login", content);
            var responseBody = await response.Content.ReadAsStringAsync();


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Xatolik! Kod: {(int)response.StatusCode}, Javob: {responseBody}"
                );
            }

            return responseBody;
        }
    }
}

