using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.DTOs
{
    public class BusinessPartnerDto
    {
        public string CardCode { get; set; } = string.Empty;
        public string CardName { get; set; } = string.Empty;
        public string Phone1 { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
