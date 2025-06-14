using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services;
public interface ISessionStorage
{
    Task<string?> GetSessionIdAsync();
    Task<string?> GetRouteIdAsync();
    Task SetSessionAsync(string sessionId, string? routeId = null);
    Task ClearSessionAsync();
}
