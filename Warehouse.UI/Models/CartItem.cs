using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace Warehouse.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Property { get; set; }
        public string Alias { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public decimal Money
        {
            get
            {
                return Count * Price;
            }
        }
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