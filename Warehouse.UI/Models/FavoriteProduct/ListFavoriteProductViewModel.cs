using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class ListFavoriteProductViewModel
    {
        public int ProductId { get; set; }


        public string AspNetUserId { get; set; }


        public Nullable<DateTime> FavoriteDate { get; set; }

        public int Price { get; set; }

        // Name product
        public string Name { get; set; }

        // Image 
        public string Image { get; set; }


        // Image FullUrl
        public string FullUrlImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Photos/Products/" + this.Image;
            }
        }

        public string Alias_SEO
        {
            get;set;
        }
    }
}