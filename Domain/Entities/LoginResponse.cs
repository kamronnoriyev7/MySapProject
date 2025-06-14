using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities;
public class LoginResponse
{
    public string? SessionId { get; set; }

    [JsonPropertyName("Version")] // SAP'dan kelayotgan nom
    public string? RouteId { get; set; } // Lekin Cookie uchun ROUTEID bo‘ladi
}

