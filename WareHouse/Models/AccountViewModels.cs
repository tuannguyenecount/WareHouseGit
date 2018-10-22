using System.ComponentModel.DataAnnotations;

namespace WareHouse.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập họ tên")]
        [Display(Name = "Họ tên")]
        [StringLength(300, ErrorMessage = "Họ tên tối đa 300 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại.")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^(0\d{9,12})$", ErrorMessage = "Số điện thoại không hợp lệ. Số điện thoại tối thiểu 9 ký tự số và tối đa 12 ký tự số.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ.")]
        [StringLength(300, ErrorMessage = "Địa chỉ quá dài. Địa chỉ tối đa 300 ký tự.")]
        public string Address { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class EditInformationViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập họ tên")]
        [Display(Name = "Họ tên")]
        [StringLength(300, ErrorMessage = "Họ tên tối đa 300 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại.")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^(0\d{9,12})$", ErrorMessage = "Số điện thoại không hợp lệ. Số điện thoại tối thiểu 9 ký tự số và tối đa 12 ký tự số.")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        [StringLength(300, ErrorMessage = "Địa chỉ quá dài.")]
        public string Address { get; set; }

        [Display(Name = "Giới tính")]
        public bool? Gender { get; set; }

    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải tối thiếu 6 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu và nhập lại mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập họ tên")]
        [Display(Name = "Họ tên")]
        [StringLength(300, ErrorMessage = "Họ tên tối đa 300 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại.")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^(0\d{9,12})$", ErrorMessage = "Số điện thoại không hợp lệ. Số điện thoại tối thiểu 9 ký tự số và tối đa 12 ký tự số.")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ.")]
        [StringLength(300, ErrorMessage = "Địa chỉ quá dài. Địa chỉ tối đa 300 ký tự.")]
        public string Address { get; set; }

        
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

         [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới")]
         [StringLength(100, ErrorMessage = "Mật khẩu phải tối thiếu 6 ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
