using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySapProject.Domain.Entities
{
    public class Item
    {
        public string ItemCode { get; set; }      
        public string ItemName { get; set; }   
        public string ItemType { get; set; }     
        public string InventoryUOM { get; set; }   
        public int ItemsGroupCode { get; set; }   
        public string U_TypeGroup { get; set; } 
    }
}
