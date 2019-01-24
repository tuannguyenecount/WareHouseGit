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
    [Table("ArticleTranslation")]
    public partial class ArticleTranslation : IEntity
    {
        [Key]
        [Column(Order = 1)]
        public int ArticleId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string LanguageId { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Required(ErrorMessage = "Bạn chưa nhập {0}")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Required(ErrorMessage = "Bạn chưa nhập bí danh bài viết")]
        [Display(Name = "Bí danh")]
        public string Alias { get; set; }

        public virtual Article Article { get; set; }
        public virtual Language Language { get; set; }
    }
}
