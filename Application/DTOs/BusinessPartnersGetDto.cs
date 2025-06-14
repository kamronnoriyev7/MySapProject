using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.DTOs
{
    public class BusinessPartnersGetDto
    {
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public string? CardType { get; set; }
        public int GroupCode { get; set; }
        public string? Phone1 { get; set; }
        public string? ContactPerson { get; set; }
        public double CreditLimit { get; set; }
    }
}
