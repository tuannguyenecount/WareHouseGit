using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class SlideTranslationViewModel
    {
        public int SlideId { get; set; }

        public string LanguageId { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(256)]
        public string Title { get; set; }
    }
}