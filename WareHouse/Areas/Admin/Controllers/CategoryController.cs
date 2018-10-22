using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;

namespace WareHouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class CategoryController : Controller
    {
        private hotellte_warehouseEntities db = new hotellte_warehouseEntities();

        // GET: Admin/Category
        public ActionResult Index()
        {
            
            return View(db.Categories.ToList());
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Category category)
        {
            
            if (ModelState.IsValid)
            {
                if(db.Categories.FirstOrDefault(m=>m.Name.Trim().ToLower() == category.Name.Trim().ToLower()) != null)
                {
                    ModelState.AddModelError("", "Tên phân loại bị trùng. Vui lòng chọn tên khác.");
                    return View(category);
                }
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            return View(Category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category,string NameCu)
        {
            if (ModelState.IsValid)
            {
                if(db.Categories.FirstOrDefault(m=>m.Name.Trim().ToLower() == category.Name && m.Name != NameCu) != null)
                {
                    ModelState.AddModelError("", "Tên phân loại bị trùng. Vui lòng chọn tên khác.");
                    return View(category);
                }
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {        
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["XacThucLan2"] == null)
            {
                object thongbao = "Bạn chưa xác thực mật khẩu lần 2 để thực hiện thao tác xóa này!";
                return View("_ThongBaoLoi", thongbao);
            }
            Category Category = db.Categories.Find(id);
            if(Category.Products.Count > 0)
            {
                ModelState.AddModelError("", "Không thể xóa phân loại này vì có các sản phẩm thuộc phân loại này.");
                return View("Delete",Category);
            }
            db.Categories.Remove(Category);
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
