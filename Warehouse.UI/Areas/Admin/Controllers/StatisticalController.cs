using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticalController : Controller
    {
        hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            List<Statistical> lst = new List<Statistical>();
            for (short i = 1; i <= 12; i++)
            {
                lst.Add(new Statistical() { Thang = i, DoanhThu = db.Orders.Where(m => m.DateOrder.Year == DateTime.Today.Year && m.DateOrder.Month == i).Select(m => m.TotalMoney).DefaultIfEmpty(0).Sum() });
            }
            return View(lst);
        }
    }
}