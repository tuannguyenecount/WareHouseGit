using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Entities
{
    public partial class Article: IEntity
    {
        public int Id { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Required(ErrorMessage = "Bạn chưa nhập {0}")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Ngày tạo")]
        public Nullable<DateTime> DateCreated { get; set; }

        [Display(Name = "Số thứ tự")]
        public Nullable<int> OrderNum { get; set; }

        [DefaultValue(false)]
        [Display(Name = "Hiển thị")]
        public Nullable<bool> Display { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Required(ErrorMessage = "Bạn chưa nhập bí danh bài viết")]
        [Display(Name = "Bí danh")]
        public string Alias { get; set; }
        
        [DefaultValue(false)]
        [Display(Name = "Bị xóa")]
        public bool Deleted { get; set; }
    }
}
