using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities
{
    public class IncomingPayment
    {
        public int DocEntry { get; set; }               // DocEntry
        public DateTime DocDate { get; set; } // DocDate
        public string? CardName { get; set; } // CardName
        public string? CardCode { get; set; } // CardCode
        public decimal CashSum { get; set; }       // DocTotal
        public string? DocCurrency { get; set; } // DocCurrency
        public string? Remarks { get; set; }      // Comments
    }
}
