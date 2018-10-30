using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class UniqueAliasProduct : ValidationAttribute
    {
        private readonly hotellte_WarehouseEntities hotellte_WarehouseEntities = new hotellte_WarehouseEntities();


   
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            return hotellte_WarehouseEntities.Products.SingleOrDefault(m => m.Alias_SEO == value.ToString()) == null;
        }
    }
}