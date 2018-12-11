namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("OrderDetail")]
    public partial class OrderDetail : IEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int? ProductId { get; set; }

        [StringLength(256)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [StringLength(300)]
        [Display(Name = "Hình ảnh")]
        public string ProductImage { get; set; }

        [StringLength(256)]
        [Display(Name = "Bí danh")]
        public string ProductAlias { get; set; }

        [StringLength(500)]
        [Display(Name = "Thông tin")]
        public string ProductProperty { get; set; }

        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá bán")]
        public decimal? Price { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Thành tiền")]
        public decimal? Money { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
