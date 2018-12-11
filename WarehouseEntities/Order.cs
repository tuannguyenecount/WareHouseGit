namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("Order")]
    public partial class Order : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Tên khách")]
        public string Name { get; set; }

        [StringLength(256)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Phường/Xã")]
        public int? WardId { get; set; }

        [Display(Name = "Tỉnh/Thành")]
        public int? DistrictId { get; set; }

        [Display(Name = "Quận/Huyện")]
        public int? ProvinceId { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Tổng số lượng")]
        public int? TotalQuantity { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Tổng thành tiền")]
        public decimal? TotalMoney { get; set; }

        [Display(Name = "Đã thanh toán?")]
        /// <summary>
        /// Đã thanh toán?
        /// </summary>
        /// 
        public bool Paid { get; set; }

        [Display(Name = "Trạng thái đơn hàng")]
        /// <summary>
        /// Trạng thái đơn hàng (0: chờ xác nhận, 1: đang giao hàng, 2: đã giao, 3: đổi/trả hàng, 4: bị huỷ)
        /// </summary>
        public byte Status { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public DateTime DateOrder { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual District District { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Province Province { get; set; }

        public virtual Ward Ward { get; set; }
    }
}
