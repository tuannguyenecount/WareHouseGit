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
    [Table("ProductTranslation")]
    public partial class ProductTranslation : IEntity
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string LanguageId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [StringLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Display(Name = "Bí danh")]
        public string Alias_SEO { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Ngày sửa")]
        public DateTime? DateUpdated { get; set; }

        public virtual Language Language { get; set; }
        public virtual Product Product { get; set; }
    }
}
