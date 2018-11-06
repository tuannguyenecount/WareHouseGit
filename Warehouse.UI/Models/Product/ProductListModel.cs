using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Warehouse.Entities;

namespace Warehouse.Models
{
    public class ProductListModel
    {
        public int CurrentCategory { get; internal set; }
        public int CurrentPage { get; internal set; }
        public int PageCount { get; internal set; }
        public int PageSize { get; internal set; }
        public PagedList.IPagedList<Product> Products { get; set; }
    }
}