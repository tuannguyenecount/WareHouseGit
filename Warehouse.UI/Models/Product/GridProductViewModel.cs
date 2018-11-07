using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class GridProductViewModel
    {
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

        // Hình sản phẩm
        public string Image { get; set; }

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