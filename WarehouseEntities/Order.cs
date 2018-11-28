namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
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
        public string Name { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public int? WardId { get; set; }

        public int? DistrictId { get; set; }

        public int? ProvinceId { get; set; }

        public string Address { get; set; }

        public int? TotalQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalMoney { get; set; }

        /// <summary>
        /// Đã thanh toán?
        /// </summary>
        public bool Paid { get; set; }

        /// <summary>
        /// Trạng thái đơn hàng (0: chờ xác nhận, 1: đang giao hàng, 2: đã giao, 3: đổi/trả hàng, 4: bị huỷ)
        /// </summary>
        public byte Status { get; set; }

        public DateTime DateOrder { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual District District { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Province Province { get; set; }

        public virtual Ward Ward { get; set; }
    }
}
