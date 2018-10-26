using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
using PagedList.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;

namespace WareHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AspNetUserController : Controller
    {
        private hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        public JsonResult GetNameAndEmailUser(string term)
        {
            List<string> dsMail = db.AspNetUsers.Where(m => m.Email.ToUpper().Contains(term.ToUpper())).Select(m=>m.Email).ToList();
            List<string> dsTen = db.AspNetUsers.Where(m => m.FullName.ToUpper().Contains(term.ToUpper())).Select(m => m.FullName).ToList();
            List<string> result = new List<string>();
            result.AddRange(dsMail);
            result.AddRange(dsTen);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ShowList(string sort, string sortName, string UserType,string UserType2, int? page, string tukhoa)
        {
            var dsThanhVien = db.AspNetUsers.Include(m => m.AspNetRoles).Include(m=>m.Orders);
            if (sort == null)
            {
                dsThanhVien = dsThanhVien.OrderBy(m => m.DateRegister);
                ViewBag.sortNext = "GiamDan";
                ViewBag.sortName = "DateRegister";
                ViewBag.sortCurrent = sort;
            }
            else
            {
                if (sort == "GiamDan")
                {
                    if (sortName == "DateRegister")
                    {
                        dsThanhVien = dsThanhVien.OrderByDescending(m => m.DateRegister);

                    }
                    else if (sortName == "FullName")
                    {
                        dsThanhVien = dsThanhVien.OrderByDescending(m => m.FullName);
                    }
                    else if (sortName == "Email")
                    {
                        dsThanhVien = dsThanhVien.OrderByDescending(m => m.Email);
                    }
                    ViewBag.sortNext = "TangDan";

                }
                else
                {
                    if (sortName == "DateRegister")
                    {
                        dsThanhVien = dsThanhVien.OrderBy(m => m.DateRegister);

                    }
                    else if (sortName == "FullName")
                    {
                        dsThanhVien = dsThanhVien.OrderBy(m => m.FullName);
                    }
                    else if (sortName == "Email")
                    {
                        dsThanhVien = dsThanhVien.OrderBy(m => m.Email);
                    }
                    ViewBag.sortNext = "TangDan";
                    ViewBag.sortNext = "GiamDan";
                }
                ViewBag.sortName = sortName;
                ViewBag.sortCurrent = sort;
            }

            if (UserType != null)
            {
                if (UserType == "Locked")
                {
                    dsThanhVien = dsThanhVien.Where(m => m.LockoutEndDateUtc != null && m.LockoutEndDateUtc.Value > DateTime.Now);
                    ViewBag.ThongTin = "Thành viên bị khóa";
                }
                ViewBag.UserType = UserType;
                
            }

            if (tukhoa != null)
            {
                dsThanhVien = dsThanhVien.Where(m => m.FullName.ToUpper().Contains(tukhoa.ToUpper()) || m.Email.ToUpper().Contains(tukhoa.ToUpper()));
                ViewBag.tuKhoa = tukhoa;
            }

            ViewBag.Page = page ?? 1;
            return PartialView("_ListPartial", dsThanhVien.ToPagedList(page ?? 1, 5));
        }
        

        public ActionResult Index()
        {
            List<UserType> dsUserType = new List<UserType>() {
                new UserType(){ Id= "Locked", Name="Bị khóa" }
            };
            ViewBag.UserType = new SelectList(dsUserType,"Id", "Name");
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return Redirect("/pages/404");
            }
            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Phone,Address,Gender")] AspNetUser model)
        {
            AspNetUser user = db.AspNetUsers.Find(model.Id);
            try
            {           
                user.FullName = model.FullName;
                user.Phone = model.Phone;
                user.Address = model.Address;
                user.Gender = model.Gender;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(user);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeImage(string id, string base64String)
        {
            AspNetUser user = await db.AspNetUsers.FindAsync(id);
            if (user == null)
            {
                ModelState.AddModelError("LoiDoiImage", "Thành viên này không tồn tại!");
            }
            else
            {
                #region Save File From String Base64
                if (!string.IsNullOrEmpty(base64String))
                {
                    try
                    {
                        base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                        string newAvatar = user.Id + DateTime.Now.Ticks.ToString() + ".jpg";
                        Functions.SaveFileFromBase64(Server.MapPath("~/Photos/ThanhVien/" + newAvatar), base64String);
                        user.Avatar = newAvatar;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LoiDoiImage", "Lỗi khi lưu hình từ chuỗi base64 " + ex.Message);
                    }
                    try
                    {
                        db.Entry(user).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Edit", new { id = user.Id });
                    }
                    catch
                    {
                        ModelState.AddModelError("LoiDoiImage", "Xảy ra lỗi khi lưu dữ liệu!");
                    }

                }
                #endregion
                else
                {
                    ModelState.AddModelError("LoiDoiImage", "Bạn chưa chọn hình muốn đổi!");
                }
            }
           
            return View("Edit", user);
        }

        public ViewResult Lock(string userId)
        {
           AspNetUser aspNetUser = db.AspNetUsers.Find(userId);
           return View(aspNetUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Lock")]
        public ActionResult Locked(string userId, string LockoutEndDateUtc)
        {
            try
            {
                AspNetUser aspNetUser = db.AspNetUsers.Find(userId);
                aspNetUser.LockoutEndDateUtc = DateTime.Parse(LockoutEndDateUtc);
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
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

        public ViewResult UnLocked(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnLocked(AspNetUser user)
        {
            if (Session["Revalidate"] == null)
            {
                object thongbao = "Bạn chưa xác thực mật khẩu lần 2 để thực hiện thao tác xóa này!";
                return View("_ThongBaoLoi", thongbao);
            }
            try
            {
                AspNetUser aspNetUser = db.AspNetUsers.Find(user.Id);
                aspNetUser.LockoutEndDateUtc = DateTime.Today;
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                object thongbao = "Xảy ra lỗi " + ex.Message;
                return View("_ThongBaoLoi", thongbao);
            }
            return RedirectToAction("Index");
        }

        public ViewResult ChangePermission(string id)
        {
             AspNetUser aspNetUser = db.AspNetUsers.Include(m=>m.AspNetRoles).Single(m=>m.Id == id);
             ViewBag.Roles = new SelectList(db.AspNetRoles.ToList(), "Id", "Name", aspNetUser.AspNetRoles.Count > 0 ? aspNetUser.AspNetRoles.First().Id : null);

            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePermission(string id, string Quyen)
        {
            AspNetRole role = db.AspNetRoles.Include(m => m.AspNetUsers).Single(m => m.Id == Quyen);
            AspNetUser user = db.AspNetUsers.Include(m=>m.AspNetRoles).Single(m=>m.Id == id);
            
            if (user.AspNetRoles.Count > 0)
            {
                user.AspNetRoles.Clear();
            }
            role.AspNetUsers.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
