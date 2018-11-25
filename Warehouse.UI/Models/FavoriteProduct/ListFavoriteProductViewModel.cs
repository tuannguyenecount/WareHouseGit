using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class ListFavoriteProductViewModel
    {
        public int ProductId { get; set; }
        public string AspNetUserId { get; set; }
        public Nullable<DateTime> FavoriteDate { get; set; }
    }
}