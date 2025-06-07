using MySapProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.Interfaces;

public interface ILoginRepository
{
    Task<string> LoginAsync(LoginRequest loginRequest);
    
}
