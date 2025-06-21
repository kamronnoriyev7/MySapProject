using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities
{
    public class IncomingPayment
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
