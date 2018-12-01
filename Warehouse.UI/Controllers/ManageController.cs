using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using Warehouse.Entities;
using Warehouse.Models;
using Warehouse.Services.Interface;

namespace Warehouse.Controllers
{
    [Authorize]
    [RoutePrefix("trang-ca-nhan")]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IProvinceService _provinceService;
        private IDistrictService _districtService;
        private IWardService _wardService;
        private IOrderService _orderService;

        public ManageController()
        {
        }

        public ManageController(IProvinceService provinceService, IDistrictService districtService, IWardService wardService, IOrderService orderService)
        {
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _orderService = orderService;
        }

        public string UserId
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Route("")]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Mật khẩu được thay đổi thành công."
                : message == ManageMessageId.SetPasswordSuccess ? "Bạn đã tạo mật khẩu cho tài khoản thành công."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Thiết lập đăng nhập thông qua 2 bước thành công."
                : message == ManageMessageId.Error ? "Xảy ra lỗi khi xử lý."
                : message == ManageMessageId.AddPhoneSuccess ? "Điện thoại đã được thêm thành công."
                : message == ManageMessageId.RemovePhoneSuccess ? "Gỡ bỏ điện thoại khỏi tài khoản thành công."
                : message == ManageMessageId.UpdateInfoSuccess ? "Cập nhật thông tin thành công"
                : "";

           
          

            ApplicationUser applicationUser = await UserManager.FindByIdAsync(UserId);
            UpdateInfoViewModel updateInfoViewModel = new UpdateInfoViewModel()
            {
                Address = applicationUser.Address,
                Email = applicationUser.Email,
                FullName = applicationUser.FullName,
                PhoneNumber = applicationUser.PhoneNumber
            };

            ViewBag.UpdateInfoViewModel = updateInfoViewModel;
            ViewBag.ProvinceId = new SelectList(_provinceService.GetAll(), "Id", "Name", applicationUser.ProvinceId);
            ViewBag.DistrictId = new SelectList(_districtService.GetAll(), "Id", "Name", applicationUser.DistrictId);
            ViewBag.WardId = new SelectList(_wardService.GetAll(), "Id", "Name", applicationUser.WardId);
            return View();
        }

        [ChildActionOnly]
        public async Task<PartialViewResult> _SidebarManage()
        {
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(UserId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(UserId),
                Logins = await UserManager.GetLoginsAsync(UserId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(UserId)
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInformation(UpdateInfoViewModel updateInfoViewModel, int ProvinceId, int DistrictId, int WardId)
        {
            Province province = _provinceService.GetById(ProvinceId);
            if (province == null)
            {
                ModelState.AddModelError("ProvinceId", "Tỉnh/Thành không tồn tại");
            }

            District district = _districtService.GetById(DistrictId);
            if (district == null)
            {
                ModelState.AddModelError("DistrictId", "Quận/Huyện không tồn tại");
            }

            Ward ward = _wardService.GetById(WardId);
            if (province == null)
            {
                ModelState.AddModelError("WardId", "Phường/Xã không tồn tại");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = UserManager.FindById(UserId);
                applicationUser.Email = updateInfoViewModel.Email;
                applicationUser.FullName = updateInfoViewModel.FullName;
                applicationUser.PhoneNumber = updateInfoViewModel.PhoneNumber;
                applicationUser.Address = updateInfoViewModel.Address;
                applicationUser.ProvinceId = ProvinceId;
                applicationUser.DistrictId = DistrictId;
                applicationUser.WardId = WardId;
                await UserManager.UpdateAsync(applicationUser);
                return RedirectToAction("Index", new { message = ManageMessageId.UpdateInfoSuccess });

            }

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(UserId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(UserId),
                Logins = await UserManager.GetLoginsAsync(UserId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(UserId)
            };
            ViewBag.UpdateInfoViewModel = updateInfoViewModel;
            ViewBag.ProvinceId = new SelectList(_provinceService.GetAll(), "Id", "Name", ProvinceId);
            ViewBag.DistrictId = new SelectList(_districtService.GetAll(), "Id", "Name", DistrictId);
            ViewBag.WardId = new SelectList(_wardService.GetAll(), "Id", "Name", WardId);
            return View("Index", model);
        }

        
        [Route("doi-mat-khau")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("doi-mat-khau")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(UserId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        [Route("don-hang-cua-ban")]
        public ActionResult ViewHistory(int? page)
        {
            IPagedList<Order> orders = _orderService.GetHistory(User.Identity.GetUserId()).OrderBy(o => o.Status).ThenByDescending(o => o.Id).ToPagedList(page ?? 1, 10);
            if (page == null)
                return View(orders);
            else
                return PartialView("_PagedListHistory", orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("huy-don-hang")]
        public ActionResult CancelOrder(int Id, int? page)
        {
            try
            {
                Order order = _orderService.GetById(Id);
                if (order != null)
                {
                    order.Status = 4; // Trạng thái bị hủy
                    _orderService.Update(order);
                }
                return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("ViewHistory"));
            }
            catch
            {
                return RedirectToAction("ViewHistory", new { page = page, errormessage = "Xảy ra lỗi khi xóa đơn hàng " + Id });
            }
        }

        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

     

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }


        [Route("dat-mat-khau")]
        public ActionResult SetPassword()
        {
            return View();
        }

        [Route("dat-mat-khau")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            UpdateInfoSuccess,
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}