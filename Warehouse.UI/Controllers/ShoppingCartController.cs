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
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="muangay"></param>
        /// <returns></returns>
        public ActionResult Add(int id, string color, string size)
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

            if (!string.IsNullOrEmpty(color) || !string.IsNullOrEmpty(size))
            {
                if (!string.IsNullOrEmpty(color))
                {
                    cartItem.Property += "<p>Màu " + color + "<p/>";
                }
                if (!string.IsNullOrEmpty(size))
                {
                    cartItem.Property += "<p>Size " + size + "</p>";
                }
            }

            if (ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == cartItem.Property) != null)
            {
                ShoppingCart.Single(m => m.Id == id && m.Property == cartItem.Property).Quantity += 1;
            }
            else
            {
                ShoppingCart.Add(cartItem);
            }

            return PartialView(cartItem); /*("_ShoppingCartViewModal", "ShoppingCart", new { id = item.Id });*/
        }

        // Edit Quantity Item
        [HttpPost]
        public ActionResult Edit(int id, string property, int quantity)
        {
            CartItem itemEdit = ShoppingCart.SingleOrDefault(m => m.Id == id && m.Property == property);
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

        public PartialViewResult UpdateShoppingCartPartial()
        {
            return PartialView();
        }
        ////Tính tổng số lượng và tổng tiền
        ////Tính tổng số lượng
        //private int TongSoLuong()
        //{
        //    int iTongSoLuong = 0;
        //    List<CartItem> lstShoppingCart = Session["CartItem"] as List<CartItem>;
        //    if (lstShoppingCart != null)
        //    {
        //        iTongSoLuong = lstShoppingCart.Sum(n => n.Quantity);
        //    }
        //    return iTongSoLuong;
        //}
        ////Tính tổng thành tiền
        //private decimal TongTien()
        //{
        //    decimal dTongTien = 0;
        //    List<CartItem> lstShoppingCart = Session["CartItem"] as List<CartItem>;
        //    if (lstShoppingCart != null)
        //    {
        //        dTongTien = lstShoppingCart.Sum(n => n.Subtotal);
        //    }
        //    return dTongTien;
        //}
        //tạo partial giỏ hàng để có thể hiển thị trên tất cả các layout
        //public ActionResult ShoppingCartPartial()
        //{
        //    if (TongSoLuong() == 0)
        //    {
        //        return PartialView();
        //    }
        //    ViewBag.TongSoLuong = TongSoLuong();
        //    ViewBag.TongTien = TongTien();
        //    return PartialView();
        //}

        //public ActionResult _ShoppingCartViewModal(int Id)
        //{
        //    Product product = _productService.GetById(Id);
        //    if (product == null)
        //        return Content("<p>Sản phẩm không tồn tại!</p>");
        //    QuickViewProductViewModel _ShoppingCartViewModal = new QuickViewProductViewModel()
        //    {
        //        Id = product.Id,
        //        Alias = product.Alias_SEO,
        //        FlagColor = "#eba53d",
        //        ProductFlag = product.Category.Name,
        //        Name = product.Name,
        //        Image = product.Image,
        //        Description = product.Description,
        //        Price = (int)(product.PriceNew ?? product.Price)
        //    };
        //    return PartialView(_ShoppingCartViewModal);
        //}

        #endregion

    }
}