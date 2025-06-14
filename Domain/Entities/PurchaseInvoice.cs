using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities;

public class PurchaseInvoice
{
    public string CardCode { get; set; } = null!;
    public DateTime DocDate { get; set; }
    public string DocCurrency { get; set; } = null!;
    public string? Comments { get; set; }
    public List<PurchaseInvoiceLine> DocumentLines { get; set; } = new();
}
