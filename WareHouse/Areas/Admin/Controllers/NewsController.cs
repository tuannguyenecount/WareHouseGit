using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
using Microsoft.AspNet.Identity;
using PagedList.Mvc;
using PagedList;
using System.Configuration;

namespace WareHouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    [ValidateInput(false)]
    public class NewsController : Controller
    {
        private hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        public JsonResult LayDsTitle(string term)
        {
            var result = db.News.Where(m => m.Title.ToUpper().Contains(term.ToUpper())).Select(m => m.Title).AsEnumerable();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ShowList(string sort, string sortName, string Poster, int? page, string tukhoa)
        {
            var Newss = db.News.Include(t => t.AspNetUser);
            if (sort == null)
            {
                Newss = Newss.OrderByDescending(m => m.Id);
                ViewBag.sortNext = "TangDan";
                ViewBag.sortName = "Id";
                ViewBag.sortCurrent = sort;
            }
            else
            {
                if (sort == "GiamDan")
                {
                    if (sortName == "Id")
                    {
                        Newss = Newss.OrderByDescending(m => m.Id);

                    }
                    else if (sortName == "Title")
                    {
                        Newss = Newss.OrderByDescending(m => m.Title);
                    }
                    ViewBag.sortNext = "TangDan";
                    
                }
                else
                {
                    if (sortName == "Id")
                    {
                        Newss = Newss.OrderBy(m => m.Id);
                    }
                    else if (sortName == "Title")
                    {
                        Newss = Newss.OrderBy(m => m.Title);
                    }
                    ViewBag.sortNext = "GiamDan";
                }
                ViewBag.sortName = sortName;
                ViewBag.sortCurrent = sort;
            }
            if (Poster != null)
            {
                Newss = Newss.Where(m => m.Poster == Poster);
                ViewBag.Poster = Poster;
            }
            if(tukhoa != null)
            {
                Newss = Newss.Where(m => m.Title.ToUpper().Contains(tukhoa.ToUpper()) || tukhoa == "");
                ViewBag.tuKhoa = tukhoa;
            }
           
            ViewBag.Page = page ?? 1;
            return PartialView("_ListPartial",Newss.ToPagedList(page ?? 1, 3));
        }

        // GET: Admin/News
        public ActionResult Index()
        {
            ViewBag.Poster = new SelectList(db.AspNetUsers.Where(m=>m.AspNetRoles.FirstOrDefault(a=>a.Name == "Admin" || a.Name == "Mod") != null) , "Id", "FullName");      
            return View();
        }

        // GET: Admin/News/Details/5
        public async Task<ActionResult> XemContent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News News = await db.News.FindAsync(id);
            if (News == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            return View(News);
        }

        public PartialViewResult SuaTitle(int id)
        {
            News News = db.News.Find(id);
            return PartialView("_SuaTitlePartial", News);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SuaTitle(int id, string Title, string TitleOld)
        {
            News news = db.News.Find(id);
            if (TitleOld != Title)
            {
                UniqueTitleNew uniqueTitleNew = new UniqueTitleNew() { ErrorMessage = "Tiêu đề tin bị trùng!" };
                if (uniqueTitleNew.IsValid(news.Title) == false)
                {
                    ModelState.AddModelError("Title", uniqueTitleNew.ErrorMessage);
                    Title = uniqueTitleNew.ErrorMessage;
                }
            }
            if (ModelState.IsValid)
            {
                news.Title = Title;
                db.Entry(news).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return Json(Title, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SuaIntroduce(int id)
        {
            News News = db.News.Find(id);
            return PartialView("_SuaIntroducePartial", News);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SuaIntroduce(int id, string Introduce)
        {
            News News = db.News.Find(id);
            News.Introduce = Introduce;
            db.Entry(News).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Json(Introduce, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SuaPoster(int id)
        {
            News News = db.News.Single(m=>m.Id == id);
            ViewBag.Poster = new SelectList(db.AspNetUsers.Include(a=>a.AspNetRoles).Where(m=>m.AspNetRoles.FirstOrDefault(a=>a.Name == "Admin" || a.Name == "Mod") != null) , "Id", "FullName", News.Poster);
            return PartialView("_SuaPosterPartial", News);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SuaPoster(int id, string Poster)
        {
            News News = db.News.Single(m=>m.Id == id);
            News.Poster = Poster;
            db.Entry(News).State = EntityState.Modified;
            
            await db.SaveChangesAsync();
            string Name = db.AspNetUsers.Find(Poster).FullName;
            return Json(Name, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult SuaImage(int id)
        {
            News News = db.News.Find(id);
            ViewBag.Id = News.Id;
            ViewBag.Image = News.Image;
            return PartialView("_SuaImagePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SuaImage(int id, HttpPostedFileBase file)
        {
            News model = await db.News.FindAsync(id);
            if(model == null)
            {
                ModelState.AddModelError("", "Tin không còn tồn tại!");
            }
            if (file != null && file.ContentLength > 0)
            {
                model.Image = model.Alias_SEO +  DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileName = Server.MapPath("~/Photos/News/" + model.Image);
                Functions.UpLoadImage(fileName, file, this.ModelState, "");
            }
            else
            {
                ModelState.AddModelError("", "Bạn chưa chọn hình cẩn sửa!");
            }
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return Redirect(Request.UrlReferrer.ToString() ?? Url.Action("Index"));
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        { 
            return View(new News());
           
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Title,Introduce,Content,Deleted,Alias_SEO")] News model, HttpPostedFileBase file)
        {
            model.DateSubmitted = DateTime.Now;
            model.Poster = User.Identity.GetUserId();

            UniqueNameNew uniqueNameNew = new UniqueNameNew() { ErrorMessage = "Tiêu đề tin bị trùng với tin khác!" };
            if (uniqueNameNew.IsValid(model.Title) == false)
            {
                ModelState.AddModelError("Title", uniqueNameNew.ErrorMessage);
            }
            UniqueAliasNew uniqueAliasNew = new UniqueAliasNew() { ErrorMessage = "Bí danh tin bị trùng với tin khác!" };
            if (uniqueAliasNew.IsValid(model.Alias_SEO) == false)
            {
                ModelState.AddModelError("Alias_SEO", uniqueAliasNew.ErrorMessage);
            }

            if (file != null && file.ContentLength > 0)
            {
                model.Image = model.Alias_SEO + DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileName = Server.MapPath("~/Photos/News/" + model.Image);
                Functions.UpLoadImage(fileName, file, this.ModelState, "Image");
            }

            if (ModelState.IsValid)
            {
                db.News.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<ActionResult> SuaContent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News News = await db.News.FindAsync(id);
            if (News == null)
            {
                return Redirect("/pages/404");
            }
            return View(News);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> SuaContent(News News)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(News).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(News);
        }

        // GET: Admin/News/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News News = await db.News.FindAsync(id);
            if (News == null)
            {
                return Redirect("/pages/404");
            }
            return View(News);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, string returnURL)
        {
            if (Session["XacThucLan2"] == null)
            {
                object thongbao = "Bạn chưa xác thực mật khẩu lần 2 để thực hiện thao tác xóa này!";
                return View("_ThongBaoLoi", thongbao);
            }
            db.News.Remove(db.News.Find(id));
            await db.SaveChangesAsync();
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
