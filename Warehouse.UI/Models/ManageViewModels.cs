using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Warehouse.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

    }


    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu hiện mới.")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải tối thiếu 6 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu hiện tại.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu hiện mới.")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải tối thiếu 6 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public class UpdateInfoViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email chưa có giá trị!")]
        public string Email { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Bạn chưa nhập họ tên!")]
        public string FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ!")]
        public string Address { get; set; }

        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "Bạn chưa nhập điện thoại!")]
        public string PhoneNumber { get; set; }

        public string Avatar { get; set; }

        [Display(Name = "Vai trò")]
        public string RoleId { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }


}