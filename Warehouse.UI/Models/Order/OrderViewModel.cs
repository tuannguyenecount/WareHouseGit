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

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }
    }
}
