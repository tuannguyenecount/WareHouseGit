namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
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
        [StringLength(300)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        public int? WardId { get; set; }

        public int? DistrictId { get; set; }

        public int? ProvinceId { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public int? TotalQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalMoney { get; set; }

        public bool Paid { get; set; }

        public bool Assigned { get; set; }

        public DateTime DateOrder { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual District District { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Province Province { get; set; }

        public virtual Ward Ward { get; set; }
    }
}
