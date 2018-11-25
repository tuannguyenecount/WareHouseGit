using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Warehouse.Models.Order
{
    public partial class OrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} Không được để trống.")]
        public string Name { get; set; }

        [StringLength(256)]
        [Required(ErrorMessage = "{0} Không được để trống.")]
        [DataType((DataType.EmailAddress))]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} Không được để trống.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} Không được để trống.")]
        public string Address { get; set; }

        public int? TotalQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalMoney { get; set; }

        public bool Paid { get; set; }

        public bool Assigned { get; set; }

        [Required(ErrorMessage = "{0} Không được để trống.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//áp dụng cho hiểu chỉnh.
        public DateTime DateOrder { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }
    }
}
