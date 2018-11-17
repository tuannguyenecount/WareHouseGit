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

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [StringLength(256,ErrorMessage = "Tên sản phẩm không được vượt quá 256 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Phân loại")]
        public int? CategoryId { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá bán")]
        public int Price { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá bán mới")]
        public int? PriceNew { get; set; }

        [StringLength(300, ErrorMessage = "Hình ảnh không được vượt quá 300 ký tự!")]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự!")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Bí danh không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Bí danh không được vượt quá 256 ký tự!")]
        [Display(Name = "Bí danh SEO")]
        public string Alias_SEO { get; set; }

        [Display(Name = "Tình trạng")]
        public bool Status { get; set; }

        [Display(Name = "Hiển thị")]
        public bool Display { get; set; }

        [StringLength(256)]
        [Display(Name = "Thành viên tạo")]
        public string UserCreated { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Ngày sửa")]
        public DateTime? DateUpdated { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property_Product> Property_Product { get; set; }

        public virtual ICollection<ImagesProduct> ImagesProducts { get; set; }
    }
}
