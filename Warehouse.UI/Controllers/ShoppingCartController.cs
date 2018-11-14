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

        #region ShoppingCart

        //Get Session["ShoppingCart"]
        public List<CartItem> ShoppingCart
        {
            get
            {
                return (Session["ShoppingCart"] as List<CartItem>) ?? new List<CartItem>();
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
        /// <returns></returns>
        public ActionResult Add(int id)
        {
            Product product = _productService.GetById(id);

            if (product == null)
            {
                return Redirect("/pages/404");
            }

            CartItem cartItem = new CartItem()
            {
                Price = product.PriceNew ?? product.Price,
                Quantity = 1,
                Id = id,
                Name = product.Name,
                Image = product.Image,
                Alias = product.Alias_SEO,
                Property = ""
            };

            if (ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == cartItem.Property) != null)
            {
                ShoppingCart.Single(m => m.Id == id && m.Property == cartItem.Property).Quantity += 1;
            }
            else
            {
                ShoppingCart.Add(cartItem);
            }

            return PartialView(cartItem);
        }

        /// <summary>
        /// Edit Quantity Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, int quantity)
        {
            CartItem itemEdit = ShoppingCart.SingleOrDefault(m => m.Id == id);
            if (itemEdit != null && quantity > 0)
            {
                itemEdit.Quantity = quantity;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        ///  Delete item in Cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            CartItem item = ShoppingCart.SingleOrDefault(m => m.Id == id);
            if (item != null)
            {
                ShoppingCart.Remove(item);
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult UpdateShoppingCartPartial()
        {
            return PartialView();
        }

        #endregion

    }
}