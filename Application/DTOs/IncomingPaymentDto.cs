using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MySapProject.Application.DTOs
{
    public  class IncomingPaymentDto
    {
        public int DocEntry { get; set; }

        public DateTime DocDate { get; set; }

        public string? CardName { get; set; }
        public string? CardCode { get; set; } 

        public decimal CashSum { get; set; }

        public string? DocCurrency { get; set; }

        public string? Remarks { get; set; }
    }
}
