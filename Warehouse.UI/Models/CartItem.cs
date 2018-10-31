using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace Warehouse.Models
{
    /// <summary>
    /// CartItem is object of ShoppingCart
    /// </summary>
    public class CartItem
    {
        // Id product
        public int Id { get; set; }

        // Name product
        public string Name { get; set; }

        // Property product (color, size, ...)
        public string Property { get; set; }

        // Alias product
        public string Alias { get; set; }

        // Quantity
        public int Quantity { get; set; }

        // Price
        public decimal Price { get; set; }

        // Image 
        public string Image { get; set; }

        // Subtotal money
        public decimal Subtotal
        {
            get
            {
                return Quantity * Price;
            }
        }

        // Image FullUrl
        public string ImageCustom
        {
            get
            {
                if(ConfigurationManager.AppSettings["BaseUrl"] != null)
                {
                    return ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Photos/Product/" + this.Image;
                }
                else
                {
                    return "/Photos/Product/" + this.Image;
                }
            }
        }
       
    }

}