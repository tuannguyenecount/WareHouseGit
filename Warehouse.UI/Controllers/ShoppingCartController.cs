using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Models;
using Warehouse.Services.Interface;

namespace Warehouse.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        public ShoppingCartController(IProductService productService)
        {
            _productService = productService;
        }

        //Get Session["ShoppingCart"]
        public List<CartItem> ShoppingCart
        {
            get
            {
                return Session["ShoppingCart"] as List<CartItem>;
            }
        }

        /// <summary>
        /// Show Cart Page
        /// </summary>
        /// <returns></returns>
        [Route("gio-hang.html")]
        public ActionResult Index()
        {
            return View(ShoppingCart);
        }

        /// <summary>
        /// Add product to Cart
        /// </summary>
        /// <param name="id"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="muangay"></param>
        /// <returns></returns>
        public ActionResult Add(int id, string color, string size, bool? muangay)
        {
            Product Product = _productService.GetById(id);

            if (Product == null)
            {
                return Redirect("/pages/404");
            }

            CartItem item = new CartItem()
            {
                Price = Product.PriceNew ?? Product.Price,
                Quantity = 1,
                Id = id,
                Name = Product.Name,
                Image = Product.Image,
                Alias = Product.Alias_SEO,
                Property = ""
            };

            if (!string.IsNullOrEmpty(color) || !string.IsNullOrEmpty(size)) {
                if (!string.IsNullOrEmpty(color)) {
                    item.Property += "<p>Màu " + color + "<p/>";
                }
                if(!string.IsNullOrEmpty(size)) {
                    item.Property += "<p>Size " + size+"</p>";
                }
            }

            if (ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == item.Property) != null)
            {
                ShoppingCart.Single(m => m.Id == id && m.Property == item.Property).Quantity += 1;
            }
            else
            {
                ShoppingCart.Add(item);
            }

            return muangay.HasValue ? RedirectToAction("Index") : RedirectToAction("Details", "Product", new { alias = Product.Alias_SEO });
        }

        // Edit Quantity Item
        [HttpPost]
        public ActionResult Edit(int id, string property, int quantity)
        {
            CartItem itemEdit = ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == property);
            if(itemEdit != null && quantity > 0)
            {
                itemEdit.Quantity = quantity;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        ///  Delete item in Cart
        /// </summary>
        /// <param name="id"></param>
        /// <param name="property"></param>
        /// <returns></returns>
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
    }
}