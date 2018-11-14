using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Warehouse.Entities;

namespace Warehouse.Models
{
    public class DetailsProductViewModel
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
        ///  Giả sản phẩm
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        ///  Hình sản phẩm
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Hình fullURL
        /// </summary>
        public string FullUrlImage
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/" + Image;
            }
        }
        /// <summary>
        ///  Danh sách hình slide sản phẩm
        /// </summary>
        public List<ImagesProduct> imagesProducts { get; set; }

        /// <summary>
        /// Danh sách hình slide medium sản phẩm fullURL
        /// </summary>
        public List<string> ListFullUrlImagesProducts
        {
            get
            {
                List<string> listFullUrlImagesProducts = new List<string>();
                foreach (ImagesProduct imagesProduct in imagesProducts)
                {
                    listFullUrlImagesProducts.Add(ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/m/" + imagesProduct.Image);
                }
                return listFullUrlImagesProducts;
            }
        }

        /// <summary>
        /// Danh sách hình slide large sản phẩm fullURL
        /// </summary>
        public List<string> ListFullUrlZoomImagesProducts
        {
            get
            {
                List<string> listFullUrlZoomImagesProducts = new List<string>();
                foreach (ImagesProduct imagesProduct in imagesProducts)
                {
                    listFullUrlZoomImagesProducts.Add(ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Products/l/" + imagesProduct.Image);
                }
                return listFullUrlZoomImagesProducts;
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