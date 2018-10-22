using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
namespace WareHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticalController : Controller
    {
        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            List<ThongKeDoanhThu> lst = new List<ThongKeDoanhThu>();
            for (short i = 1; i <= 12; i++)
            {
                lst.Add(new ThongKeDoanhThu() { Thang = i, DoanhThu = db.Orders.Where(m => m.DateOrder.Year == DateTime.Today.Year && m.DateOrder.Month == i).Select(m => m.TotalMoney).DefaultIfEmpty(0).Sum() });
            }
            return View(lst);
        }
    }
}