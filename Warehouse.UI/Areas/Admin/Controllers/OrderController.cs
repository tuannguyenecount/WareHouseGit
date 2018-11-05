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

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class OrderController : Controller
    {
        private hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var Orders = db.Orders.Include(d => d.AspNetUser);
            return View(Orders.OrderByDescending(m=>m.Id).ToPagedList(page??1, 10));
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order Order = db.Orders.Find(id.Value);
            ViewBag.dsOrderDetail = db.OrderDetails.Where(m => m.OrderId == Order.Id).ToList();
            if (Order == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            return View(Order);
        }
        
        public int DemDonChuaGiao()
        {
            return db.Orders.Count(o=>o.Assigned == false);
        }

        public ActionResult ThayTrangThai(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order Order = db.Orders.Find(id);
            if (Order == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            
            return View(Order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThayTrangThai(int id, string TrangThai,bool? Paid, string returnURL)
        {
            Order Order = db.Orders.Find(id);
            Order.Assigned = TrangThai == "Assigned" ? true : false;
            Order.Paid = (Paid != null && Paid.Value == true);
            
            db.Entry(Order).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Redirect(returnURL);
        }

        public ActionResult RemoveDeleted(int id)
        {
            Order order = db.Orders.Find(id);
            order.Deleted = false;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order Order = db.Orders.Find(id);
            if (Order == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent", new { area = "" });
            }
            return View(Order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnURL)
        {
            if (Session["Revalidate"] == null)
            {
                object thongbao = "Bạn chưa xác thực mật khẩu lần 2 để thực hiện thao tác xóa này!";
                return View("_ThongBaoLoi", thongbao);
            }
            Order Order = db.Orders.Find(id);
            Order.Deleted = true;
            db.Entry(Order).State = EntityState.Modified;
            db.SaveChanges();
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
