using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities
{
    public class SAPSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string CompanyDB { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
