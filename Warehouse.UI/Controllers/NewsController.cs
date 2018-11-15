using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
namespace Warehouse.Controllers
{
    [RoutePrefix("tin-tuc")]
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index(int? page)
        {
            //var dsTin = db.News.OrderByDescending(m => m.Id);
            //return View(dsTin.ToPagedList(page ?? 1, 10));
            return View();
        }
        [Route("xem-tin/{alias}.html")]
        public ActionResult Details(string alias)
        {
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 86400)]
        public ActionResult _FeaturedArticlesPartial()
        {
            // return PartialView(db.News.OrderByDescending(m=>m.View).Take(10).ToList());
            return PartialView();
        }
    }
}