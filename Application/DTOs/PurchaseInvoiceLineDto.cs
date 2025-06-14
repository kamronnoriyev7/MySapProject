using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.DTOs
{
    public class PurchaseInvoiceLineDto
    {
        public string ItemCode { get; set; } = null!;
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
