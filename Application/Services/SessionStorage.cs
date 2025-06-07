using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MySapProject.Application.Services;

public class SessionStorage : ISessionStorage
{
    private string _sessionId;
    private string _routeId;

    public Task SetSessionAsync(string sessionId, string routeId)
    {
        _sessionId = sessionId;
        _routeId = routeId;
        return Task.CompletedTask;
    }

    public Task<string> GetSessionIdAsync() => Task.FromResult(_sessionId);

    public Task<string> GetRouteIdAsync() => Task.FromResult(_routeId);

    public Task ClearSessionAsync()
    {
        _sessionId = null;
        _routeId = null;
        return Task.CompletedTask;
    }
}
