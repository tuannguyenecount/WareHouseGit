using Microsoft.AspNet.Identity;
using Warehouse.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;
using Microsoft.AspNet.Identity.Owin;
using Warehouse.Services.Interface;
using Warehouse.Models.Order;

namespace Warehouse.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationUserManager _userManager;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;
        private IProductService _productService;

        public OrderController(ApplicationUserManager userManager, IOrderService orderService, IOrderDetailService orderDetailService, IProductService productService)
        {
            UserManager = userManager;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _productService = productService;
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize]
        public ViewResult Index()
        {
            //ViewBag.ListUnAssigned = db.Orders.Where(m=>m.Assigned == false && m.Deleted == false && m.AspNetUser != null && m.AspNetUser.UserName == User.Identity.Name).OrderBy(m=>m.Assigned).ToList();
            //ViewBag.ListAssigned = db.Orders.Where(m => m.Assigned == true && m.Deleted == false && m.AspNetUser != null && m.AspNetUser.UserName == User.Identity.Name).OrderBy(m => m.Assigned).ToList();
            return View();
        }

        public ActionResult Checkout()
        {

            // kiểm tra đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            //Kiểm tra giỏ hàng
            if (Session["ShoppingCart"] == null)
            {
                RedirectToAction("Index", "Home");
            }

            List<CartItem> ds = Session["ShoppingCart"] as List<CartItem>;

            var model = new OrderViewModel();
            model.DateOrder = DateTime.Now;
            model.TotalQuantity = (byte)ds.Sum(m => m.Quantity);
            model.TotalMoney = ds.Sum(m => m.Subtotal);

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Checkout(OrderViewModel model)
        {
            List<CartItem> ds = Session["ShoppingCart"] as List<CartItem>;
            model.DateOrder = DateTime.Now;
            model.TotalQuantity = (byte)ds.Sum(m => m.Quantity);
            model.TotalMoney = ds.Sum(m => m.Subtotal);

            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                model.UserId = user.Id;
                if (model.Name == null)
                    model.Name = user.FullName;
                if (model.Phone == null)
                    model.Phone = user.PhoneNumber;
                model.Email = user.Email;
                if (model.Address == null)
                    model.Address = user.Address;
            }
            //else
            //{
            //    if (string.IsNullOrEmpty(model.Address))
            //    {
            //        ModelState.AddModelError("", "Bạn chưa cung cấp địa chỉ.");
            //        return View("OrderError");
            //    }
            //    if (string.IsNullOrEmpty(model.Phone))
            //    {
            //        ModelState.AddModelError("", "Bạn chưa cung cấp số điện thoại.");
            //        return View("OrderError");
            //    }
            //    if (Functions.IsValidEmail(model.Email) == false)
            //    {
            //        ModelState.AddModelError("", "Địa chỉ email không hợp lệ.");
            //        return View("OrderError");
            //    }
            //}



            #region ModelState Valid
            if (ModelState.IsValid)
            {

                try
                {
                    _orderService.Add(model);
                }
                catch
                {
                    ModelState.AddModelError("", "Không thể lưu đơn hàng này!");
                    return View("OrderError");
                }

                foreach (CartItem item in ds)
                {
                    OrderDetail detail = new OrderDetail()
                    {
                        OrderId = model.Id,
                        ProductImage = item.Image,
                        ProductName = item.Name,
                        ProductAlias = item.Alias,
                        ProductId = item.Id,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Money = item.Subtotal
                    };
                    try
                    {
                        _orderDetailService.Add(detail);
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Không thể lưu bản ghi chi tiết số " + detail.Id.ToString());
                    }
                }
                //try
                //{
                //    EmailService email = new EmailService();
                //    await email.SendAsync(new IdentityMessage()
                //    {
                //        Body = "Khách hàng <b>" + model.Name + "</b> vừa đặt đơn hàng #" + model.Id.ToString() + " tại shop",
                //        Destination = (Session["InfoShop"] as InfoShop).Email,
                //        Subject = "Đơn hàng mới tại shop"
                //    });

                //}
                //catch
                //{

                //}
                //if (onlinePayment == null || onlinePayment == false)
                //{
                //    ds.Clear();
                //    Session["ShoppingCart"] = null;
                //    if (ModelState.IsValid)
                //        return View("OrderSuccess");
                //    else
                //        return View("OrderError");
                //}
                //else
                //{
                //    NL_Checkout nganluong = new NL_Checkout();
                //    string urlThanhToan = nganluong.buildCheckoutUrlNew(Url.Action("Confirm", "Order", null, Request.Url.Scheme), ConfigurationManager.AppSettings["email_nganluong"].ToString(), "Thanh toán " + model.TotalMoney.ToString("#,##0").Replace(',', '.') + " đồng", model.Id.ToString(), model.TotalMoney.ToString(), "vnd", 1, 0, 0, 0, 0, "Thanh toán " + model.TotalMoney.ToString("#,##0").Replace(',', '.') + " đồng",
                //     "Thanh toán " + model.TotalMoney.ToString("#,##0").Replace(',', '.') + " đồng", "");
                //    return Redirect(urlThanhToan);
                //}
                return View("OrderSuccess");
            }
            #endregion
            else
            {
                return View("OrderError");
            }

        }

        public ActionResult Confirm(string transaction_info, string order_code, int price, string payment_id, string payment_type, string error_text, string secure_code)
        {
            if (error_text == "")
            {
                int Id = int.Parse(order_code);
                try
                {
                    //Order Order = db.Orders.Find(Id);
                    //Order.Paid = true;
                    //db.Entry(Order).State = System.Data.Entity.EntityState.Modified;
                    //await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Order", "Confirm"));
                }
                try
                {
                    ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                    historyBankCharging history = new historyBankCharging()
                    {
                        fullname = user.FullName,
                        email = user.Email,
                        phone = user.PhoneNumber,
                        date_trans = DateTime.Now,
                        price = price,
                        order_code = order_code,
                        error_text = error_text,
                        transaction_info = transaction_info,
                        payment_id = payment_id,
                        payment_type = payment_type,
                        secure_code = secure_code
                    };
                    //db.historyBankChargings.Add(history);
                    //await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Không thể lưu lịch sử thanh toán! Lỗi " + ex.Message);
                    return View("OrderError");
                }
                Session["ShoppingCart"] = null;
                return View("OrderSuccess");
            }
            else
                return View("Error", new HandleErrorInfo(new Exception(error_text), "Order", "Confirm"));
        }
    }
}