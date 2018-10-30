using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
namespace Warehouse.Controllers
{
    public class OrderDetailController : Controller
    {
        hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();
        // GET: OrderDetail
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            List<OrderDetail> dsChiTiet = db.OrderDetails.Where(m => m.OrderId == id).ToList();

            return View(dsChiTiet);
        }
    }
}