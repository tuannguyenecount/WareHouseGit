namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("Category")]
    public partial class Category : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Category1 = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        [Display(Name = "Mã phân loại")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phân loại không được để trống!")]
        [StringLength(256, ErrorMessage = "Tên phân loại chỉ được phép tối đa 256 ký tự!")]
        [Display(Name = "Tên phân loại")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bí danh phân loại không được để trống!")]
        [StringLength(256, ErrorMessage = "Bí danh phân loại chỉ được phép tối đa 256 ký tự!")]
        [Display(Name = "Bí danh phân loại")]
        public string Alias_SEO { get; set; }

        [Display(Name = "Phân loại cha")]
        public Nullable<int> ParentId { get; set; }

        [Display(Name = "Số thứ tự")]
        public Nullable<int> OrderNum { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Category1 { get; set; }

        public virtual Category Category2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
