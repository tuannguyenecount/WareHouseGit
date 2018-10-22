using Microsoft.AspNet.Identity;
using WareHouse.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WareHouse.Controllers
{

    public class OrderController : Controller
    {
        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        
        public ViewResult DanhSachDonHang()
        {
            AspNetUser userCurrent = db.AspNetUsers.Single(m => m.UserName == User.Identity.Name);
            ViewBag.dsDonChuaNhanHang = userCurrent.Orders.Where(m=>m.Assigned == false && m.Deleted == false).OrderBy(m=>m.Assigned).ToList();
            ViewBag.dsDonDaNhanHang = userCurrent.Orders.Where(m => m.Assigned == true && m.Deleted == false).OrderBy(m => m.Assigned).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThemOrder([Bind(Exclude = "Paid,Deleted")]Order model, bool? thanhtoantructuyen)
        {
            List<CartItem> ds = Session["GioHang"] as List<CartItem>;
            model.DateOrder = DateTime.Now;
            model.TotalCount = (byte)ds.Sum(m => m.Count);
            model.TotalMoney = ds.Sum(m => m.Money);
            model.Deleted = false;
            if (User.Identity.IsAuthenticated)
            {
                string emailUserCurrent = User.Identity.Name;
                AspNetUser user = db.AspNetUsers.Single(m => m.UserName == emailUserCurrent);
                model.UserId = user.Id;
                model.Name = user.FullName;
                model.Phone = user.Phone;
                model.Email = emailUserCurrent;
                model.Address = user.Address;
            }
            else
            {
                if (string.IsNullOrEmpty(model.Address))
                {
                    ModelState.AddModelError("", "Bạn chưa cung cấp địa chỉ.");
                    return View("ThongBaoLoi");
                }
                if (string.IsNullOrEmpty(model.Phone))
                {
                    ModelState.AddModelError("", "Bạn chưa cung cấp số điện thoại.");
                    return View("ThongBaoLoi");
                }
                if (ThuVien.IsValidEmail(model.Email) == false)
                {
                    ModelState.AddModelError("", "Địa chỉ email không hợp lệ.");
                    return View("ThongBaoLoi");
                }
            }
            #region ModelState Valid
            if(ModelState.IsValid)
            {

                try
                {
                    db.Orders.Add(model);
                    await db.SaveChangesAsync();
                }
                catch
                {
                    ModelState.AddModelError("", "Không thể lưu đơn hàng này!");
                    return View("ThongBaoLoi");
                }
                
                foreach (CartItem item in ds)
                {
                    OrderDetail chiTiet = new OrderDetail()
                    {
                        OrderId = model.Id,
                        ProductImage = item.Image,
                        ProductName = item.Name,
                        ProductAlias = item.Alias,
                        ProductId = item.Id,
                        Count = item.Count,
                        Price = item.Price,
                        Subtotal = item.Money
                    };
                    try
                    {
                        db.OrderDetails.Add(chiTiet);
                        await db.SaveChangesAsync();
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Không thể lưu bản ghi chi tiết số " + chiTiet.Id.ToString());
                    }
                }
                try
                {
                    EmailService email = new EmailService();
                    await email.SendAsync(new IdentityMessage()
                    {
                        Body = "Khách hàng <b>" + model.Name + "</b> vừa đặt đơn hàng #" + model.Id.ToString() + " tại shop",
                        Destination = (Session["InfoShop"] as InfoShop).Email,
                        Subject = "Đơn hàng mới tại shop"
                    });

                }
                catch
                {

                }
                if (thanhtoantructuyen == null || thanhtoantructuyen == false)
                {
                    ds.Clear();
                    Session["GioHang"] = null;
                    if (ModelState.IsValid)
                        return View("ThongBaoThanhCong");
                    else
                        return View("ThongBaoLoi");
                }
                else
                {
                    NL_Checkout nganluong = new NL_Checkout();
                    string urlThanhToan = nganluong.buildCheckoutUrlNew(Url.Action("XacThucThanhToan", "Order", null, Request.Url.Scheme), ConfigurationManager.AppSettings["email_nganluong"].ToString(), "Thanh toán " + model.TotalMoney.ToString("#,##0").Replace(',', '.') + " đồng", model.Id.ToString(), model.TotalMoney.ToString(), "vnd", 1, 0, 0, 0, 0, "Thanh toán " + model.TotalMoney.ToString("#,##0").Replace(',', '.') + " đồng",
                     "Thanh toán " + model.TotalMoney.ToString("#,##0").Replace(',', '.') + " đồng", "");
                    return Redirect(urlThanhToan);
                }
            }
            #endregion
            else
            {
                return View("ThongBaoLoi");
            }
          
        }
        
        public async Task<ActionResult> XacThucThanhToan(string transaction_info, string order_code, int price, string payment_id, string payment_type, string error_text, string secure_code)
        {
            if (error_text == "")
            {
                int Id = int.Parse(order_code);
                try
                {
                    Order Order = db.Orders.Find(Id);
                    Order.Paid = true;
                    db.Entry(Order).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                catch
                {
                    ModelState.AddModelError("","Không thể sửa trạng thái đơn hàng này thành Đã Thanh Toán!");
                    return View("ThongBaoLoi");
                }
                try
                {
                    AspNetUser userCurrent = db.AspNetUsers.Find(User.Identity.GetUserId());
                    historyBankCharging history = new historyBankCharging()
                    {
                        fullname = userCurrent.FullName,
                        email = userCurrent.Email,
                        phone = userCurrent.Phone,
                        date_trans = DateTime.Now,
                        price = price,
                        order_code = order_code,
                        error_text = error_text,
                        transaction_info = transaction_info,
                        payment_id = payment_id,
                        payment_type = payment_type,
                        secure_code = secure_code
                    };
                    db.historyBankChargings.Add(history);
                    await db.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Không thể lưu lịch sử thanh toán! Lỗi " + ex.Message);
                    return View("ThongBaoLoi");
                }
                Session["GioHang"] = null;
                return View("ThongBaoThanhCong");
            }
            else
                return View("Error",new HandleErrorInfo(new Exception(error_text), "Order", "XacThucThanhToan"));
        }
    }
}