using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Configuration;

namespace Warehouse.Entities
{
    //[MetadataType(typeof(SlideMetadata))]
    //public partial class Slide
    //{
    //    public string ImageCustom
    //    {
    //        get
    //        {
    //            if (ConfigurationManager.AppSettings["BaseUrl"] != null)
    //            {
    //                return ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/images/" + this.Image;
    //            }
    //            else
    //                return "/images/" + this.Image;
    //        }
    //    }
    //}

    //sealed class SlideMetadata
    //{
    //    [Display(Name = "Image")]
    //    public string Image
    //    {
    //        get;set;
    //    }
        
    //    [Display(Name = "Số thứ tự")]
    //    public Nullable<byte> Order { get; set; }

    //    [Display(Name = "Tiêu đề")]
    //    public string Title { get; set; }

    //    [Display(Name = "Hiển thị")]
    //    [DefaultValue(true)]
    //    public bool Status { get; set; }
    //}

    //[MetadataType(typeof(InfoShopMetadata))]
    //public partial class InfoShop
    //{
    //    public string LogoCustom
    //    {
    //        get
    //        {
    //            if (ConfigurationManager.AppSettings["BaseUrl"] != null)
    //            {
    //                return ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/images/" + this.Logo;
    //            }
    //            else
    //                return "/images/" + this.Logo;
    //        }
    //    }
    //}

    //sealed class InfoShopMetadata
    //{
    //    [Display(Name = "Tên shop")]
    //    public string ShopName { get; set; }

    //    public string Logo { get; set; }

    //    [Display(Name = "Điện thoại")]
    //    public string Phone { get; set; }

    //    public string Zalo { get; set; }

    //    [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
    //    public string Email { get; set; }

    //    [Display(Name = "Địa chỉ")]
    //    public string Address { get; set; }

    //    [Display(Name = "URL Fanpage FB")]
    //    [Url(ErrorMessage = "URL không đúng định dạng")]
    //    public string Fanpage { get; set; }

    //    [Display(Name = "Giới thiệu về shop")]
    //    public string Introduce_Shop { get; set; }

    //    [Display(Name = "Thông tin liên hệ")]
    //    public string Contact_Info { get; set; }

    //    [Display(Name = "Google Maps")]
    //    public string Google_Map { get; set; }

    //    [Display(Name = "Giới thiệu ngắn")]
    //    public string TextFooter { get; set; }

    //    [Display(Name = "Google Analytics")]
    //    public string GoogleAnalytics { get; set; }

    //    [Display(Name = "Chính sách bán hàng")]
    //    public string SalesPolicy { get; set; }

    //    [Display(Name = "Hướng dẫn mua hàng")]
    //    public string ShoppingGuide { get; set; }
    //}

    //[MetadataType(typeof(MailBoxMetadata))]
    //public partial class MailBox
    //{

    //}
    //sealed class MailBoxMetadata
    //{
    //    [Display(Name = "ID")]
    //    public int Id { get; set; }

    //    [Display(Name = "Họ tên")]
    //    [Required(ErrorMessage = "Bạn chưa nhập họ tên.")]
    //    public string Name { get; set; }

    //    [Display(Name = "Email")]
    //    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
    //    [Required(ErrorMessage = "Bạn chưa nhập email.")]
    //    public string Email { get; set; }

    //    [Display(Name = "Chủ đề")]
    //    [Required(ErrorMessage = "Bạn chưa nhập chủ đề")]
    //    public string Subject { get; set; }

    //    [Display(Name = "Nội dung")]
    //    [Required(ErrorMessage = "Bạn chưa nhập nội dung")]
    //    public string Content { get; set; }

    //    [Display(Name = "Gửi lúc")]
    //    public System.DateTime DateSend { get; set; }
    //}

    //[MetadataType(typeof(NewsMetadata))]
    //public partial class News
    //{
    //    public string ImageCustom
    //    {
    //        get
    //        {
    //            if (ConfigurationManager.AppSettings["BaseUrl"] != null)
    //            {
    //                return ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Photos/News/" + this.Image;
    //            }
    //            else
    //                return "/Photos/News/" + this.Image;
    //        }
    //    }
    //}

    //sealed class NewsMetadata
    //{
    //    [Display(Name = "Id")]
    //    public int Id { get; set; }

    //    [Display(Name = "Tiêu đề")]
    //    [Required(ErrorMessage = "Tiêu đề không được bỏ trống.")]
    //    [StringLength(256, ErrorMessage = "Tiêu đề quá dài. Độ dài ký tự của tiêu đề không được vượt quá 256 ký tự!")]
    //    public string Title { get; set; }

    //    [Display(Name = "Giới thiệu")]
    //    [Required(ErrorMessage = "Nội dung bài viết không được bỏ trống.")]
    //    public string Introduce { get; set; }

    //    [Display(Name = "Nội dung")]
    //    public string Content { get; set; }

    //    [Display(Name = "Hình ảnh")]
    //    public string Image { get; set; }

    //    [Display(Name = "Ngày đăng")]
    //    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    //    public Nullable<System.DateTime> DateSubmitted { get; set; }

    //    [Display(Name = "Người đăng")]
    //    public string Poster { get; set; }

    //    [Display(Name = "Bí danh")]
    //    [Required(ErrorMessage = "Bí danh SEO không được bỏ trống")]
    //    [StringLength(256, ErrorMessage = "Bí danh SEO chỉ được dài tối đa 256 ký tự")]
    //    public string Alias_SEO { get; set; }

    //    [Display(Name = "Lượt xem")]
    //    [DefaultValue(0)]
    //    public int View { get; set; }
    //}

    //[MetadataType(typeof(ProductMetadata))]
    //public partial class Product
    //{
       
    //    public string ImageCustom
    //    {
    //        get
    //        {
    //            if (ConfigurationManager.AppSettings["BaseUrl"] != null)
    //            {
    //                return ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Photos/Product/" + this.Image;
    //            }
    //            else
    //                return "/Photos/Product/" + this.Image;
    //        }
    //    }
    //}
    //sealed class ProductMetadata
    //{
    //    [Display(Name = "Mã sản phẩm")]
    //    public int Id { get; set; }

    //    [Display(Name = "Tên sản phẩm")]
    //    [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống!")]
    //    [StringLength(256, ErrorMessage = "Tên sản phẩm chỉ được dài tối đa 256 ký tự!")]
    //    public string Name { get; set; }

    //    [Display(Name = "Thuộc phân loại")]
    //    public Nullable<int> CategoryId { get; set; }

    //    [Display(Name = "Đơn giá")]
    //    [DefaultValue(0)]
    //    [Required(ErrorMessage = "Bạn chưa nhập đơn giá!")]
    //    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    //    [GreaterThan0(ErrorMessage = "Đơn giá phải > 0")]
    //    public decimal Price { get; set; }

    //    [Display(Name = "Bí danh")]
    //    [Required(ErrorMessage = "Bí danh SEO không được bỏ trống!")]
    //    [StringLength(256, ErrorMessage = "Bí danh SEO chỉ được dài tối đa 256 ký tự!")]
    //    public string Alias_SEO { get; set; }

    //    [Display(Name = "Hình đại diện")]
    //    public string Image { get; set; }

    //    [Display(Name = "Hình slider")]
    //    public string Slider { get; set; }

    //    [Display(Name = "Lượt thích")]
    //    [DefaultValue(0)]
    //    public int Likes { get; set; }

    //    [Display(Name = "Lượt yêu thích")]
    //    [DefaultValue(0)]
    //    public int LoveTurns { get; set; }

    //    [Display(Name = "Ngày tạo")]
    //    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    //    public System.DateTime DateCreated { get; set; }

    //    [Display(Name = "Tình trạng")]
    //    [DefaultValue(true)]
    //    public bool Status { get; set; }

    //    [Display(Name = "Màu sắc")]
    //    public string Color { get; set; }

    //    [Display(Name = "Kích cỡ")]
    //    public string Size { get; set; }

    //    [Display(Name = "Hiển thị")]
    //    public bool Display { get; set; }

    //    [Display(Name = "Nội dung giới thiệu")]
    //    public string Content { get; set; }
    //}

    //[MetadataType(typeof(CategoryMetadata))]
    //public partial class Category
    //{

    //}
    //sealed class CategoryMetadata
    //{
    //    [Display(Name = "Phân loại ID")]
    //    public int Id { get; set; }

    //    [Display(Name = "Tên phân loại")]
    //    [Required(ErrorMessage = "Tên phân loại không được bỏ trống!")]
    //    [StringLength(256, ErrorMessage = "Tên phân loại chỉ được dài tối đa 256 ký tự!")]
    //    public string Name { get; set; }

    //    [Display(Name = "Bí danh SEO")]
    //    [Required(ErrorMessage = "Bí danh SEO không được bỏ trống")]
    //    [StringLength(256, ErrorMessage = "Bí danh SEO chỉ được dài tối đa 256 ký tự")]
    //    public string Alias_SEO { get; set; }
    //}


    //[MetadataType(typeof(PromotionProductMetadata))]
    //public partial class PromotionProduct
    //{

    //}
    //sealed class PromotionProductMetadata
    //{
    //    [Display(Name = "Mã sản phẩm")]
    //    [Required(ErrorMessage = "Chưa xác định sản phẩm nào được khuyến mãi")]
    //    public int ProductId { get; set; }

    //    [Display(Name = "Giá khuyến mãi")]
    //    [DefaultValue(0)]
    //    public int PromotionalPrice { get; set; }

    //}

    //[MetadataType(typeof(OrderMetadata))]
    //public partial class Order
    //{

    //}
    //sealed class OrderMetadata
    //{
    //    [Display(Name = "Id")]
    //    public int Id { get; set; }

    //    [Display(Name = "Họ tên")]
    //    [MaxLength(300, ErrorMessage = "Họ tên quá dài.")]
    //    [Required(ErrorMessage = "Chúng tôi cần biết tên của bạn.")]
    //    public string Name { get; set; }

    //    [Required(ErrorMessage = "Bạn hãy cung cấp email để chúng tôi dễ liên lạc hơn.")]
    //    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    //    public string Email { get; set; }

    //    [Display(Name = "Số điện thoại")]
    //    public string Phone { get; set; }

    //    [Display(Name = "Địa chỉ")]
    //    public string Address { get; set; }

    //    [Display(Name = "Tổng số lượng")]
    //    public byte TotalCount { get; set; }

    //    [Display(Name = "Tổng thành tiền")]
    //    public decimal TotalMoney { get; set; }

    //    [Display(Name = "Ngày đặt hàng")]
    //    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    //    public System.DateTime DateOrder { get; set; }

    //    public string UserId { get; set; }

    //    [Display(Name = "Trạng thái")]
    //    public bool Assigned { get; set; }

    //    [Display(Name = "Đã thanh toán?")]
    //    public bool Paid { get; set; }
    //}

    //public partial class OrderDetail
    //{
    //    public string ProductImageCustom
    //    {
    //        get
    //        {
    //            if (ConfigurationManager.AppSettings["BaseUrl"] != null)
    //            {
    //                return ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Photos/Product/" + this.ProductImage;
    //            }
    //            else
    //                return "/Photos/Product/" + this.ProductImage;
    //        }
    //    }
    //}
}