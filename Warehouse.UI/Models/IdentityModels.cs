using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using Warehouse.Core.Entities;
using Warehouse.Entities;

namespace Warehouse.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(150)]
        [DefaultValue("user.png")]
        [Display(Name = "Hình đại diện")]
        public string Avatar { get; set; }

        [Display(Name = "Ngày đăng ký")]
        public DateTime DateRegister { get; set; }

        [Display(Name = "Tỉnh/Thành")]
        public int? ProvinceId { get; set; }

        [Display(Name = "Quận/Huyện")]
        public int? DistrictId { get; set; }

        [Display(Name = "Phường/Xã")]
        public int? WardId { get; set; }

        public virtual Province Province { get; set; }

        public virtual District District { get; set; }

        public virtual Ward Ward { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}