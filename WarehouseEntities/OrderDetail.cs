namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
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
        public string ProductName { get; set; }

        [StringLength(300)]
        public string ProductImage { get; set; }

        [StringLength(256)]
        public string ProductAlias { get; set; }

        [StringLength(500)]
        public string ProductProperty { get; set; }

        public int? Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [Column(TypeName = "money")]
        public decimal? Money { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
