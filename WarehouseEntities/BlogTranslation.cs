using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Entities
{
    [Table("BlogTranslation")]
    public partial class BlogTranslation : IEntity
    {
        [Key]
        [Column(Order = 1)]
        public int BlogId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string LanguageId { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Required(ErrorMessage = "Bạn chưa nhập {0}!")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Bạn chưa nhập nội dung!")]
        public string Content { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Display(Name = "Bí danh")]
        [Required(ErrorMessage = "Bạn chưa nhập {0}!")]
        public string Alias { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Language Language { get; set; }
    }
}
