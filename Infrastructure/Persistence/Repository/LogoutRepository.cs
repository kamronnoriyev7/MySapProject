using MySapProject.Application.Interfaces;
using MySapProject.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Infrastructure.Persistence.Repository;

public class LogoutRepository : ILogoutRepository
{
    private readonly HttpClient _httpClient;
    private readonly ISessionStorage _sessionStorage;

    public LogoutRepository(HttpClient httpClient, ISessionStorage sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    public async Task<string> LogoutAsync()
    {
        var sessionId = await _sessionStorage.GetSessionIdAsync();
        var routeId = await _sessionStorage.GetRouteIdAsync();

        if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(routeId))
        {
            throw new Exception("Session info missing. Please login first.");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, "Logout");
        request.Headers.Add("Cookie", $"B1SESSION={sessionId}; ROUTEID={routeId}");

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Logout failed: {response.StatusCode}\n{responseBody}");
        }

        await _sessionStorage.ClearSessionAsync();
        return responseBody;
    }
}