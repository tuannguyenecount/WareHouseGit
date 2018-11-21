using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class GridProductViewModel
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

        // Giá sản phẩm
        public int Price { get; set; }

        // Hình thứ 1
        public string Image { get; set; }

        // Hình thứ 2
        public string SecondImage { get; set; }

        // Hình thứ 1 fullURL
        public string FullUrlImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/" + Image;
            }
        }

        // Hình thứ 1 fullURL
        public string FullUrlSecondImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/slide/" + SecondImage;
            }
        }

        // Hình thứ 1 lớn
        public string ZoomImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/" + Image;
            }
        }
        // Hình thứ 2 lớn
        public string ZoomSecondImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/" + SecondImage;
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