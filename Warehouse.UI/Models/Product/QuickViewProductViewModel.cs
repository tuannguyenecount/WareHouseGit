using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Warehouse.Entities;

namespace Warehouse.Models
{
    public class QuickViewProductViewModel
    {
        /// <summary>
        ///  Mã sản phẩm
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Tên sản phẩm
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Bí danh sản phẩm
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        public string Description { get; set; }

        // Giá sản phẩm
        public int Price { get; set; }

        // Hình thứ 1
        public string Image { get; set; }


        // Hình thứ 1 fullURL
        public string FullUrlImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/" + Image;
            }
        }

        // Hình thứ 1 fullURL
        public string FullZoomUrlImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/l/" + Image;
            }
        }



        /// <summary>
        ///  Sản phẩm loại gì (new, hot, sale, none  ...)
        /// </summary>
        public string ProductFlag { get; set; }

        /// <summary>
        ///  Màu loại (new, hot, sale, none ...)
        /// </summary>
        public string FlagColor { get; set; }
    }
}