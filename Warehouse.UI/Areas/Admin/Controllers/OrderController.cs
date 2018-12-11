using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;
using Warehouse.Services.Interface;
using Warehouse.Entities;
namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class OrderController : Controller
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #region CRUD
        public ActionResult Index(int? page)
        {
            return View(_orderService.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order Order = _orderService.GetById(id.Value);
            if (Order == null)
            {
                return Redirect("/pages/404");
            }
            return View(Order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int[] check)
        {
            foreach (int id in check) {
                _orderService.Delete(id);
            }
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index"));
        }
        #endregion
        #region Count Order Wait Confirm
        [ChildActionOnly]
        public ContentResult CountOrderWaitConfirm()
        {
            return Content(_orderService.CountOrderWaitConfirm().ToString());
        }
        #endregion
        #region Change Status 
        public ActionResult _ChangeStatusModal(int id)
        {
            Order Order = _orderService.GetById(id);
            if (Order == null)
            {
                return Content("<p>Dữ liệu không tồn tại!</p>");
            }           
            return PartialView(Order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _ChangeStatusModal(int Id, byte? Status)
        {
            try
            {
                if(Status == null)
                    return Json(new { status = 0, message = "Bạn chưa chọn trạng thái!" });

                Order order = _orderService.GetById(Id);
                if(order == null)
                    return Json(new { status = 0, message = "Dữ liệu không tồn tại!" });

                order.Status = Status.Value;
                order.Paid = Status.Value == 2 ? true : false; // nếu trạng thái đổi thành đã giao thì tương đương đơn hàng đã được thanh toán
                _orderService.Update(order);

                return Json(new { status = 1, message = "Đổi trạng thái đơn hàng " + Id + " thành công" });
            }
            catch(Exception ex)
            {
                return Json(new { status = 0, message = "Lỗi: " + ex.Message });
            }
        }
        #endregion

    }
}
