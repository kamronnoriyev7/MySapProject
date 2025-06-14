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
        public string ItemCode { get; set; }       // Unikal mahsulot kodi  
        public string ItemName { get; set; }       // Mahsulot nomi  
        public string ItemType { get; set; }       // Doimiy: "itItems"  
        public string InventoryUOM { get; set; }   // O'lchov birligi (masalan: pcs)  
        public int ItemsGroupCode { get; set; }    // Mahsulot guruhi kodi  
        public string U_TypeGroup { get; set; } 
    }
}
