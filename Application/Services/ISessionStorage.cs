using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services;

public interface ISessionStorage
{
    Task SetSessionAsync(string sessionId, string routeId);
    Task<string> GetSessionIdAsync();
    Task<string> GetRouteIdAsync();
    Task ClearSessionAsync();
}
