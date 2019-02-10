using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class BlogTranslationViewModel
    {
        public int BlogId { get; set; }

        public string LanguageId { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Required(ErrorMessage = "Bạn chưa nhập {0} bài viết blog!")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Bạn chưa nhập {0}!")]
        public string Content { get; set; }

        [Display(Name = "Bí danh")]
        [StringLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        public string Alias { get; set; }
    }
}