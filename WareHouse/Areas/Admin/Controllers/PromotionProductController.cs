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
    [Authorize(Roles = "Admin")]
    public class PromotionProductController : Controller
    {
        private hotellte_warehouseEntities db = new hotellte_warehouseEntities();


        // GET: Admin/PromotionProduct/Create
        public ActionResult Create(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if(Product == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            ViewBag.ProductId = Product.Id;
            ViewBag.Name = Product.Name;
            ViewBag.Price = Product.Price;
            return View();
        }

        // POST: Admin/PromotionProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PromotionProduct promotionProduct, string Name, string returnUrl)
        {
            try
            {
                PromotionProduct temp = db.PromotionProducts.SingleOrDefault(m => m.ProductId ==  promotionProduct.ProductId);
                if (temp != null)
                {
                    ModelState.AddModelError("", "Sản phẩm này đã được khuyến mãi trước đó với giá " + temp.PromotionalPrice); 
                    ViewBag.Id = promotionProduct.ProductId;
                    ViewBag.Name = Name;
                    ViewBag.Price = temp.Product.Price;
                    return View(promotionProduct);
                }
                Product Product = db.Products.Find(promotionProduct.ProductId);
                if (promotionProduct.PromotionalPrice >= Product.Price)
                {
                    ModelState.AddModelError("", "Giá khuyến mãi phải nhỏ hơn đơn giá cũ. Bạn hãy kiểm tra lại đơn giá cũ và lựa chọn giá khuyến mãi phù hợp");
                    ViewBag.ProductId = promotionProduct.ProductId;
                    ViewBag.Name = Name;
                    ViewBag.Price = Product.Price;
                    return View(promotionProduct);
                }
                if (ModelState.IsValid)
                {
                    db.PromotionProducts.Add(promotionProduct);
                    db.SaveChanges();
                    return Redirect(returnUrl);
                }
                ViewBag.ProductId = promotionProduct.ProductId;
                ViewBag.Name = Name;
                ViewBag.Price = Product.Price;
                return View(promotionProduct);
            }
            catch
            {
                ModelState.AddModelError("", "Xảy ra lỗi trong quá trình xử lý");
                ViewBag.ProductId = promotionProduct.ProductId;
                ViewBag.Price = promotionProduct.Product.Price;
                ViewBag.Name = Name;
                return View(promotionProduct);
            }
        }

        // POST: Admin/PromotionProduct/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            PromotionProduct PromotionProduct = db.PromotionProducts.Find(id);
            db.PromotionProducts.Remove(PromotionProduct);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
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
