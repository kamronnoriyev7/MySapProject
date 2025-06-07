using MySapProject.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services;

public class LogoutService : ILogoutService
{
    private readonly ILogoutRepository _logoutRepository;
    private readonly ISessionStorage _sessionStorage;

    public LogoutService(ILogoutRepository logoutRepository, ISessionStorage sessionStorage)
    {
        _logoutRepository = logoutRepository;
        _sessionStorage = sessionStorage;
    }

    public async Task<string> LogoutAndClearSessionAsync()
    {
        var result = await _logoutRepository.LogoutAsync();

        await _sessionStorage.ClearSessionAsync(); 

        return result;
    }
}
