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
    public partial class Blog: IEntity
    {
        public int Id { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Required(ErrorMessage = "Bạn chưa nhập {0} bài viết blog!")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Hình")]
        public string Image { get; set; }

        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Bạn chưa nhập {0}!")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tạo")]
        public Nullable<DateTime> DateCreated { get; set; }

        [DefaultValue(true)]
        [Display(Name = "Hiển thị")]
        public Nullable<bool> Display { get; set; }

        [Display(Name = "Bí danh")]
        [StringLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        public string Alias { get; set; }

        [DefaultValue(0)]
        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; }

        [DefaultValue(0)]
        [Display(Name = "Lượt thích")]
        public int LikeCount { get; set; }

        [StringLength(128)]
        [Display(Name = "Tác giả")]
        public string UserId{ get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<BlogTranslation> BlogTranslations { get; set; }
    }
}
