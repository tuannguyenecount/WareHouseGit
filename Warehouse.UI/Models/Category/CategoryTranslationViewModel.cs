using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class CategoryTranslationViewModel
    {
        public int CategoryId { get; set; }

        public string LanguageId { get; set; }

        [Required(ErrorMessage = "Tên phân loại không được để trống!")]
        [StringLength(256, ErrorMessage = "Tên phân loại chỉ được phép tối đa 256 ký tự!")]
        [Display(Name = "Tên phân loại")]
        public string Name { get; set; }

        [StringLength(256, ErrorMessage = "Bí danh phân loại chỉ được phép tối đa 256 ký tự!")]
        [Display(Name = "Bí danh phân loại")]
        public string Alias_SEO { get; set; }
    }
}