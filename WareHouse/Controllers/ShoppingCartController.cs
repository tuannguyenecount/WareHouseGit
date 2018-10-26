using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
namespace WareHouse.Controllers
{
    public class ShoppingCartController : Controller
    {
        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        public List<CartItem> ShoppingCart
        {
            get
            {
                return Session["ShoppingCart"] as List<CartItem>;
            }
        }

        #region Show Cart 
        [Route("gio-hang.html")]
        public ActionResult Index()
        {
            ViewBag.BodyClass = "lang-en country-us currency-usd layout-full-width page-cart tax-display-disabled body-desktop-header-style-w-4";
            return View(ShoppingCart);
        }
        #endregion

        #region Add, Edit, Delete Cart Item
        public ActionResult Add(int id, string color, string size, bool? muangay)
        {
            Product Product = db.Products.SingleOrDefault(m => m.Id == id && m.Display == true);

            if (Product == null)
            {
                return Redirect("/pages/404");
            }

            CartItem item = new CartItem()
            {
                Price = Product.PriceThuc,
                Count = 1,
                Id = id,
                Name = Product.Name,
                Image = Product.Image,
                Alias = Product.Alias_SEO,
                Property = ""
            };

            if (!string.IsNullOrEmpty(color) || !string.IsNullOrEmpty(size)) {
                if (!string.IsNullOrEmpty(color)) {
                    item.Property += "Màu " + color + ".";
                }
                if(!string.IsNullOrEmpty(size)) {
                    item.Property += "Size " + size;
                }
            }

            if (ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == item.Property) != null)
            {
                ShoppingCart.Single(m => m.Id == id && m.Property == item.Property).Count += 1;
            }
            else
            {
                ShoppingCart.Add(item);
            }

            return muangay.HasValue ? RedirectToAction("Index") : RedirectToAction("Details", "Product", new { alias = Product.Alias_SEO });
        }

        [HttpPost]
        public ActionResult Edit(int id, string property, int? Countmoi)
        {
            CartItem itemEdit = ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == property);
            if(itemEdit != null && Countmoi.HasValue && Countmoi >= 1)
            {
                try {
                    Product p = db.Products.Find(id);
                    itemEdit.Count = Countmoi.Value;
                    return RedirectToAction("Index");
                }
                catch {
                    object message = "Xảy ra lỗi khi sửa số lượng!";
                    return View("_ThongBaoLoi", message);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id, string property)
        {
            CartItem item = ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == property);
            if (item != null)
            {
                ShoppingCart.Remove(item);
            }
            return RedirectToAction("Index");
        }
        #endregion
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