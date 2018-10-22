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
        public List<CartItem> gioHang
        {
            get
            {
                return Session["GioHang"] as List<CartItem>;
            }
        }

        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        // GET: ShoppingCart
        [Route("gio-hang.html")]
        public ActionResult Index()
        {
            return View(gioHang);
        }
       
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

            if (gioHang.SingleOrDefault(m => m.Id == id && m.Property == item.Property) != null)
            {
                gioHang.Single(m => m.Id == id && m.Property == item.Property).Count += 1;
            }
            else
            {
                gioHang.Add(item);
            }

            return muangay.HasValue ? RedirectToAction("Index") : RedirectToAction("Details", "Product", new { alias = Product.Alias_SEO });
        }
        [HttpPost]
        public ActionResult Edit(int id, string property, int? Countmoi)
        {
            CartItem itemEdit = gioHang.SingleOrDefault(m => m.Id == id && m.Property == property);
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
            CartItem item = gioHang.SingleOrDefault(m => m.Id == id && m.Property == property);
            if (item != null)
            {
                gioHang.Remove(item);
            }
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