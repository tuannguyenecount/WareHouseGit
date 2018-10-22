using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Store2306.Models;

namespace Store2306.Areas.Admin.Controllers
{
     [Authorize(Roles = "Admin")]
    public class GroupProductController : Controller
    {
        private MenStore2306Entities db = new MenStore2306Entities();

        // GET: Admin/GroupProduct
        public ActionResult Index(int? CategoryId)
        {
            if(CategoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category Category = db.Categories.Find(CategoryId);
            if(Category == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            ViewBag.Name = Category.Name;
            var GroupProducts = db.GroupProducts.Where(m=>m.CategoryId == CategoryId);
            return View(GroupProducts.ToList());
        }

        // GET: Admin/GroupProduct/Create
        public ActionResult Create(int? CategoryId)
        {
            if (CategoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Name = db.Categories.Find(CategoryId).Name;
            ViewBag.CategoryId = CategoryId;
            return View();
        }

        // POST: Admin/GroupProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupProduct GroupProduct)
        {
            if (ModelState.IsValid)
            {
                db.GroupProducts.Add(GroupProduct);
                db.SaveChanges();
                return RedirectToAction("Index", new { CategoryId = GroupProduct.CategoryId});
            }
            ViewBag.CategoryId = GroupProduct.CategoryId;
            return View(GroupProduct);
        }

        // GET: Admin/GroupProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupProduct GroupProduct = db.GroupProducts.Find(id);
            if (GroupProduct == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", GroupProduct.CategoryId);
            return View(GroupProduct);
        }

        // POST: Admin/GroupProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupProduct GroupProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(GroupProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { CategoryId = GroupProduct.CategoryId });
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", GroupProduct.CategoryId);
            return View(GroupProduct);
        }

        // GET: Admin/GroupProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupProduct GroupProduct = db.GroupProducts.Find(id);
            if (GroupProduct == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            
            return View(GroupProduct);
        }

        // POST: Admin/GroupProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["XacThucLan2"] == null)
            {
                object thongbao = "Bạn chưa xác thực mật khẩu lần 2 để thực hiện thao tác xóa này!";
                return View("_ThongBaoLoi", thongbao);
            }
            GroupProduct GroupProduct = db.GroupProducts.Include(m=>m.Products).Single(m=>m.Id == id);
            if(GroupProduct.Products.Count > 0)
            {
                ModelState.AddModelError("", "Không thể xóa nhóm sản phẩm vì có các sản phẩm khác tham chiếu.");
                return View("Delete", GroupProduct);
            }
            db.GroupProducts.Remove(GroupProduct);
            db.SaveChanges();
            return RedirectToAction("Index", new { CategoryId = GroupProduct.CategoryId});
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
