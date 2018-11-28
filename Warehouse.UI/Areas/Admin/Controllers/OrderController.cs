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

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            return View(_orderService.GetAll());
        }

        // GET: Admin/Order/Details/5
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

        [ChildActionOnly]
        public ContentResult CountOrderWaitConfirm()
        {
            return Content(_orderService.CountOrderWaitConfirm().ToString());
        }
       
        public ActionResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order Order = _orderService.GetById(id.Value);
            if (Order == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            
            return View(Order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(int Id, byte Status)
        {
            Order order = _orderService.GetById(Id);
            order.Status = Status;
            order.Paid = Status == 2 ? true : false; // nếu trạng thái đổi thành đã giao thì tương đương đơn hàng đã được thanh toán
            _orderService.Update(order);
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index"));
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int[] check)
        {
            foreach (int id in check)
            {
                _orderService.Delete(id);
            }
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index"));
        }

    }
}
