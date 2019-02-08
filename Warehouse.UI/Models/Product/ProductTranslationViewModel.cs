using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class ProductTranslationViewModel
    {
        public int ProductId { get; set; }

        public string LanguageId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [StringLength(256, ErrorMessage = "{0} không được vượt quá {0} ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Display(Name = "Bí danh")]
        public string Alias_SEO { get; set; }

        [StringLength(500, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        public Nullable<DateTime> DateCreated { get; set; }
    }
}