using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Warehouse.Entities;

namespace Warehouse.Models
{
    public class ProductAddViewModel
    {
        public List<Category> Categories { get; set; }
        public Product Product { get; set; }
    }
}