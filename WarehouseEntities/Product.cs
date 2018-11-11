namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("Product")]
    public partial class Product : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Property_Product = new HashSet<Property_Product>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int? CategoryId { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceNew { get; set; }

        [StringLength(300)]
        public string Image { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Content { get; set; }

        [Required]
        [StringLength(256)]
        public string Alias_SEO { get; set; }

        public bool Status { get; set; }

        public bool Display { get; set; }

        [StringLength(256)]
        public string UserCreated { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(256)]
        public string UserUpdated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property_Product> Property_Product { get; set; }

        public virtual ICollection<ImagesProduct> ImagesProducts { get; set; }
    }
}
