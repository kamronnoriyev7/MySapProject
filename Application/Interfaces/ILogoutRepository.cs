using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces
{
    public interface ILogoutRepository
    {
        Task<string> LogoutAsync();
    }
}
