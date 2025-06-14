using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.DTOs
{
    public class PurchaseInvoiceDto
    {
        public string CardCode { get; set; } = null!;
        public DateTime DocDate { get; set; } = DateTime.Now;
        public string DocCurrency { get; set; } = "USD";
        public string? Comments { get; set; }
        public List<PurchaseInvoiceLineDto> DocumentLines { get; set; } = new();
    }
}
