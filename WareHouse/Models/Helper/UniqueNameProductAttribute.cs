using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WareHouse.Models
{
    public class UniqueNameProduct : ValidationAttribute
    {
        private readonly hotellte_warehouseEntities hotellte_warehouseEntities = new hotellte_warehouseEntities();
        
       
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            return hotellte_warehouseEntities.Products.SingleOrDefault(m => m.Name == value.ToString()) == null;
        }
    }
}