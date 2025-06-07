using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Services
{
    public interface ILogoutService
    {
        Task<string> LogoutAndClearSessionAsync();
    }
}
