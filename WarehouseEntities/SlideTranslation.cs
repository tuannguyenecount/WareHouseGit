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
    [Table("SlideTranslation")]
    public partial class SlideTranslation : IEntity
    {
        [Key]
        [Column(Order = 1)]
        public int SlideId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string LanguageId { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        public string Title { get; set; }

        public virtual Slide Slide { get; set; }
        public virtual Language Language { get; set; }
    }
}
