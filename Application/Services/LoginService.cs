using MySapProject.Application.Interfaces;
using MySapProject.Domain.Entities;
using System.Text.Json;

namespace MySapProject.Application.Services;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;
    private readonly ISessionStorage _sessionStorage;

    public LoginService(ILoginRepository loginRepository, ISessionStorage sessionStorage)
    {
        _loginRepository = loginRepository;
        _sessionStorage = sessionStorage;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        if (loginRequest == null)
        {
            throw new ArgumentNullException(nameof(loginRequest), "Login request cannot be null.");
        }

        var result = await _loginRepository.LoginAsync(loginRequest)
                      ?? throw new InvalidOperationException("Login failed. Response from repository is null.");

        var sapLoginResponse = CreateResponse(result);

        var sessionId = sapLoginResponse.SessionId;
        var routeId = sapLoginResponse.RouteId;

        await _sessionStorage.SetSessionAsync(sessionId, routeId);

        return sapLoginResponse;
    }

    private LoginResponse CreateResponse(string jsonResponse)
    {
        if (string.IsNullOrWhiteSpace(jsonResponse))
            throw new ArgumentException("Response is null or empty.");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<LoginResponse>(jsonResponse, options)
                     ?? throw new InvalidOperationException("Failed to parse JSON response.");

        return result;
    }
}
