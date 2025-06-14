using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MySapProject.Application.Services;

public class SessionStorage : ISessionStorage
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task SetSessionAsync(string sessionId, string routeId)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            context.Session.Set("SessionId", Encoding.UTF8.GetBytes(sessionId ?? ""));
            context.Session.Set("RouteId", Encoding.UTF8.GetBytes(routeId ?? ""));
        }
        return Task.CompletedTask;
    }

    public Task<string> GetSessionIdAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.Session.TryGetValue("SessionId", out var value) == true)
        {
            return Task.FromResult(Encoding.UTF8.GetString(value));
        }
        return Task.FromResult<string>(null);
    }

    public Task<string> GetRouteIdAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.Session.TryGetValue("RouteId", out var value) == true)
        {
            return Task.FromResult(Encoding.UTF8.GetString(value));
        }
        return Task.FromResult<string>(null);
    }

    public Task ClearSessionAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            context.Session.Remove("SessionId");
            context.Session.Remove("RouteId");
        }
        return Task.CompletedTask;
    }
}

