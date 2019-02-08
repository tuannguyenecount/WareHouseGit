using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Warehouse.Core.Entities;

namespace Warehouse.Entities
{
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

        [Required(ErrorMessage = "{0} không được để trống!")]
        [StringLength(256, ErrorMessage = "{0} không được vượt quá {0} ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Phân loại")]
        [Required(ErrorMessage = "Bạn chưa chọn phân loại cho sản phẩm!")]
        public int CategoryId { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá bán")]
        public int Price { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá bán mới")]
        public int? PriceNew { get; set; }

        [StringLength(300, ErrorMessage = "Hình ảnh không được vượt quá 300 ký tự!")]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [StringLength(500, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [StringLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự!")]
        [Display(Name = "Bí danh")]
        public string Alias_SEO { get; set; }

        [Display(Name = "Tình trạng")]
        [DefaultValue(false)]
        public bool Status { get; set; }

        [Display(Name = "Hiển thị")]
        [DefaultValue(false)]
        public bool Display { get; set; }

        [StringLength(256)]
        [Display(Name = "Thành viên tạo")]
        public string UserCreated { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Ngày sửa")]
        public DateTime? DateUpdated { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property_Product> Property_Product { get; set; }

        public virtual ICollection<ImagesProduct> ImagesProducts { get; set; }

        public virtual ICollection<ProductTranslation> ProductTranslations { get; set; }

    }
}
