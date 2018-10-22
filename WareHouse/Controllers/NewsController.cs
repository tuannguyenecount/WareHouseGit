using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
namespace WareHouse.Controllers
{
    [RoutePrefix("tin-tuc")]
    public class NewsController : Controller
    {
        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        // GET: News
        public ActionResult Index(int? page)
        {
            var dsTin = db.News.OrderByDescending(m => m.Id);
            return View(dsTin.ToPagedList(page ?? 1, 10));
        }
        [Route("{alias}.html")]
        public async Task<ActionResult> ChiTiet(string alias)
        {
            News News = db.News.SingleOrDefault(n=>n.Alias_SEO == alias);
            if(News == null)
            {
                return RedirectToAction("PageNotFound", "StaticContent");
            }
            if(Session["docbai" + alias] == null)
            {
                Session["docbai" + alias] = true;
                News.View++;
                try
                {
                    db.Entry(News).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                catch
                {

                }
            }
            return View(News);
        }
        public ActionResult _BaiVietQuanTamPartial()
        {
            return PartialView(db.News.OrderByDescending(m=>m.View).Take(10).ToList());
        }
    }
}