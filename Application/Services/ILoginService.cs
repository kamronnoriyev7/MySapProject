using MySapProject.Domain.Entities;

namespace MySapProject.Application.Services;

public interface ILoginService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

}