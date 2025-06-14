using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Application.DTOs
{
    public class ItemDto
    {
        public string? ItemCode { get; set; }      
        public string? ItemName { get; set; }       
        public string? InventoryUOM { get; set; }   
        public int ItemsGroupCode { get; set; }    
        public string? U_TypeGroup { get; set; } 

    }
}
