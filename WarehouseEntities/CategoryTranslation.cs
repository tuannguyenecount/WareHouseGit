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
    [Table("CategoryTranslation")]
    public partial class CategoryTranslation : IEntity
    {
        [Key]
        [Column(Order = 1)]
        public int CategoryId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string LanguageId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [StringLength(256, ErrorMessage = "{0} chỉ được phép tối đa {1} ký tự!")]
        [Display(Name = "Tên phân loại")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [StringLength(300, ErrorMessage = "{0} chỉ được phép tối đa {1} ký tự!")]
        [Display(Name = "Bí danh phân loại")]
        public string Alias_SEO { get; set; }

        public virtual Category Category { get; set; }
        public virtual Language Language { get; set; }

    }
}
