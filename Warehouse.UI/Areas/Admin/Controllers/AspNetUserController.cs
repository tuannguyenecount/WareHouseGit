using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using PagedList.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Warehouse.Data.Interface;
using Warehouse.Services.Interface;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AspNetUserController : Controller
    {
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IProvinceService _provinceService;
        private IDistrictService _districtService;
        private IWardService _wardService;
        private IProductService _productService;

        public AspNetUserController()
        {
           
        }

        public AspNetUserController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        public AspNetUserController(IProvinceService provinceService, IDistrictService districtService, IWardService wardService, IProductService productService)
        {
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;
            _productService = productService;
        }

        #region Public Property 
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

        #endregion

        public PartialViewResult _UserLoggedPartial()
        {
            ApplicationUser applicationUser = UserManager.FindById(UserId);
            IList<string> userRoles = applicationUser.Roles.Select(r => r.RoleId).ToList();
            ViewBag.UserRole = userRoles.First();
            return PartialView(applicationUser);
        }

        public PartialViewResult _UserPanelPartial()
        {
            ApplicationUser applicationUser = UserManager.FindById(UserId);
            return PartialView(applicationUser);
        }

        public ViewResult ProfileUser()
        {
            ApplicationUser model = UserManager.FindById(UserId);
            ViewBag.RoleId = model.Roles.FirstOrDefault()?.RoleId;
            ViewBag.Title = "Hồ sơ cá nhân";
            UpdateInfoViewModel updateInfoViewModel = new UpdateInfoViewModel()
            {
                Address = model.Address,
                UserName = model.UserName,
                FullName = model.FullName,
                Id = model.Id,
                PhoneNumber = model.PhoneNumber,
                RoleId = ViewBag.RoleId,
                Avatar = model.Avatar,
                Email = model.Email,
                ProvinceId = model.ProvinceId,
                DistrictId = model.DistrictId,
                WardId = model.WardId
            };
            ViewBag.ProvinceId = new SelectList(_provinceService.GetAll(), "Id", "Name", updateInfoViewModel.ProvinceId);
            ViewBag.DistrictId = new SelectList(_districtService.GetAll(), "Id", "Name", updateInfoViewModel.DistrictId);
            ViewBag.WardId = new SelectList(_wardService.GetAll(), "Id", "Name", updateInfoViewModel.WardId);
            ViewBag.ProductsUserPost = _productService.GetByUser(model.UserName).Where(p=>p.Display == true).ToList();
            return View(updateInfoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileUser([Bind(Exclude = "UserName")] UpdateInfoViewModel updateInfoViewModel, string OldRole, string RoleId)
        {
            var model = UserManager.FindById(UserId);
            if (ModelState.IsValid)
            {
                model.FullName = updateInfoViewModel.FullName;
                model.Address = updateInfoViewModel.Address;
                model.Email = updateInfoViewModel.Email;
                model.PhoneNumber = updateInfoViewModel.PhoneNumber;
                model.ProvinceId = updateInfoViewModel.ProvinceId;
                model.DistrictId = updateInfoViewModel.DistrictId;
                model.WardId = updateInfoViewModel.WardId;
                UserManager.Update(model);
                return RedirectToAction("ProfileUser", new { Id = updateInfoViewModel.Id });
            }
            ViewBag.RoleId = model.Roles.FirstOrDefault()?.RoleId;
            ViewBag.Title = "Hồ sơ cá nhân";
            ViewBag.ProvinceId = new SelectList(_provinceService.GetAll(), "Id", "Name", updateInfoViewModel.ProvinceId);
            ViewBag.DistrictId = new SelectList(_districtService.GetAll(), "Id", "Name", updateInfoViewModel.DistrictId);
            ViewBag.WardId = new SelectList(_wardService.GetAll(), "Id", "Name", updateInfoViewModel.WardId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(string userId, string base64String)
        {
            if (User.IsInRole("Admin") == false)
            {
                userId = this.UserId;
            }
            if (!string.IsNullOrEmpty(base64String))
            {
                var model = UserManager.FindById(userId);
                try
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    string newAvatar = model.UserName + DateTime.Now.Ticks.ToString() + ".jpg";
                    Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Users/" + newAvatar), base64String);
                    model.Avatar = newAvatar;
                }
                catch (Exception ex)
                {
                    return RedirectToAction("ProfileUser", new { Id = userId });
                }
                UserManager.Update(model);
            }

            return RedirectToAction("ProfileUser", new { Id = userId });
        }

        /// <summary>
        /// Danh sách Admin
        /// </summary>
        /// <param name="afterInsert">Vừa hoàn thành thao tác thêm?</param>
        /// <returns></returns>
        public async Task<ActionResult> ListAdmin(int? afterInsert)
        {
            List<ApplicationUser> applicationUsers = await UserManager.Users.ToListAsync();
            applicationUsers = applicationUsers.Where(u => u.Roles != null && u.Roles.FirstOrDefault(r=>r.RoleId == "Admin") != null ).ToList();
            return View(applicationUsers);
        }

        /// <summary>
        /// Danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Customer()
        {
            List<ApplicationUser> applicationUsers = UserManager.Users.Include(m=>m.District).Include(m=>m.Ward).Include(m=>m.Province).Where(x => x.Roles == null).ToList();
            return View(applicationUsers);
        }


        // GET: Expense/Edit/5
        public ActionResult _EditCustomerModal(string Id)
        {
            ApplicationUser applicationUser = UserManager.FindById(Id);
            if (applicationUser == null)
            {
                return Content("<p>Dữ liệu không tồn tại trong hệ thống!</p>");
            }
            ViewBag.ProvinceId = new SelectList(_provinceService.GetAll(), "Id", "Name", applicationUser.ProvinceId);
            ViewBag.DistrictId = new SelectList(_districtService.GetAll(), "Id", "Name", applicationUser.DistrictId);
            ViewBag.WardId = new SelectList(_wardService.GetAll(), "Id", "Name", applicationUser.WardId);
            return PartialView(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditCustomer([Bind(Include = "Id,FullName,PhoneNumber,Address,ProvinceId,DistrictId,WardId")] ApplicationUser applicationUser, bool Status)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = UserManager.FindById(applicationUser.Id);
                try
                {
                    user.FullName = applicationUser.FullName;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.Address = applicationUser.Address;
                    user.ProvinceId = applicationUser.ProvinceId;
                    user.DistrictId = applicationUser.DistrictId;
                    user.WardId = applicationUser.WardId;
                    user.LockoutEndDateUtc = Status == true ? DateTime.Now.AddDays(-1) : DateTime.Now.AddYears(200);
                    UserManager.Update(user);
                    return Json(new { status = 1, message = "Update success." }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return Json(new { status = 0, message = Functions.GetAllErrorsPage(this.ModelState) });
        }

        /// <summary>
        /// Thêm Quản trị viên
        /// </summary>
        /// <returns></returns>
        public ActionResult _CreateModal()
        {
            return PartialView(new CreateUserViewModel());
        }

        /// <summary>
        /// Thêm Quản trị viên
        /// </summary>
        /// <param name="createUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(CreateUserViewModel createUserViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = createUserViewModel.UserName,
                    Email = createUserViewModel.Email,
                    FullName = createUserViewModel.FullName,
                    Avatar = "user.png",
                    DateRegister = DateTime.Now,
                    EmailConfirmed = true
                };
                try
                {
                    IdentityResult result = await UserManager.CreateAsync(user, createUserViewModel.Password);
                    user.Roles.Add(new IdentityUserRole() { RoleId = "Admin", UserId = user.Id });
                    UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        return Json(new { status = 1, message = "Thêm thành công" });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return Json(new { status = 0, message = Functions.GetAllErrorsPage(ModelState) });
        }

        public ActionResult _DeleteModal(string Id)
        {
            ApplicationUser applicationUser = UserManager.FindById(Id);
            if (applicationUser == null)
            {
                return Content("<p>Dữ liệu không tồn tại trong hệ thống!</p>");
            }
            return PartialView(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(string Id)
        {
            try
            {
                ApplicationUser user = await UserManager.FindByIdAsync(Id);
                await UserManager.DeleteAsync(user);
                return Json(new { status = 1, message = "Update success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { status = 0, message = Functions.GetAllErrorsPage(this.ModelState) }, JsonRequestBehavior.AllowGet);
        }


        public ViewResult Lock(string Id)
        {
            ApplicationUser aspNetUser = UserManager.FindById(Id);
            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Lock")]
        public ActionResult Locked(string Id, string LockoutEndDateUtc)
        {
            try
            {
                ApplicationUser aspNetUser = UserManager.FindById(Id);
                aspNetUser.LockoutEndDateUtc = DateTime.Parse(LockoutEndDateUtc);
                UserManager.Update(aspNetUser);
            }
            catch (FormatException)
            {
                object thongbao = "Thời gian khoá có định dạng không hợp lệ!";
                return View("_ThongBaoLoi", thongbao);
            }
            catch (Exception ex)
            {
                object thongbao = "Xảy ra lỗi " + ex.Message;
                return View("_ThongBaoLoi", thongbao);
            }
            return RedirectToAction("Index");
        }

        public ViewResult UnLocked(string Id)
        {
            ApplicationUser aspNetUser = UserManager.FindById(Id);
            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UnLocked")]
        public ActionResult UnLockedConfirmed(string Id)
        {
            if (Session["Revalidate"] == null)
            {
                object thongbao = "Bạn chưa xác thực mật khẩu lần 2 để thực hiện thao tác xóa này!";
                return View("_ThongBaoLoi", thongbao);
            }
            try
            {
                ApplicationUser applicationUser  = UserManager.FindById(Id);
                applicationUser.LockoutEndDateUtc = DateTime.Today;
                UserManager.Update(applicationUser);
            }
            catch (Exception ex)
            {
                object thongbao = "Xảy ra lỗi " + ex.Message;
                return View("_ThongBaoLoi", thongbao);
            }
            return RedirectToAction("Index");
        }

        public ViewResult ChangePermission(string Id)
        {
            ApplicationUser applicationUser = UserManager.FindById(Id);
            //ViewBag.Roles = new SelectList(UserManager.Role , "Id", "Name", aspNetUser.AspNetRoles.Count > 0 ? aspNetUser.AspNetRoles.First().Id : null);

            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePermission(string UserId, string Role)
        {
            ApplicationUser user = UserManager.FindById(UserId);
            UserManager.AddToRole(UserId, Role);
            return RedirectToAction("Index");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = UserManager.ChangePassword(UserId, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
                if (result.Succeeded)
                {
                    var user = UserManager.FindById(UserId);
                    if (user != null)
                    {
                        SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    }
                    return Json(new { status = 1, message = "Đổi mật khẩu thành công." }, JsonRequestBehavior.AllowGet);
                }
                AddErrors(result);
            }  
            return Json(new { status = 0, message = Functions.GetAllErrorsPage(ModelState)}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SetPassword(SetPasswordViewModel setPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = UserManager.AddPassword(UserId, setPasswordViewModel.NewPassword);
                if (result.Succeeded)
                {
                    var user = UserManager.FindById(UserId);
                    if (user != null)
                    {
                        SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    }
                    return Json(new { status = 1, message = "Đặt lại mật khẩu thành công." });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return Json(new { status = 0, message = Functions.GetAllErrorsPage(ModelState) });
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
