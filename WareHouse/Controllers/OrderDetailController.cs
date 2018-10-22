using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
namespace WareHouse.Controllers
{
    public class OrderDetailController : Controller
    {
        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
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