using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;

namespace Warehouse.Controllers
{
    public class ArticleController : Controller
    {
       
        [Route("gioi-thieu-shop.html")]
        public ViewResult About()
        {
            InfoShop infoShop = Session["InfoShop"] as InfoShop;
            ViewBag.Title = "Giới Thiệu " + infoShop.ShopName;
            object model = infoShop.Introduce_Shop;
            return View("Article", model);
        }

        [Route("chinh-sach-ban-hang.html")]
        public ViewResult SalesPolicy()
        {
            InfoShop infoShop = Session["InfoShop"] as InfoShop;
            ViewBag.Title = "Chính Sách Bán Hàng";
            object model = infoShop.SalesPolicy;
            return View("Article", model);
        }

        [Route("huong-dan-mua-hang.html")]
        public ViewResult ShoppingGuide()
        {
            InfoShop infoShop = Session["InfoShop"] as InfoShop;
            ViewBag.Title = "Hướng Dẫn Mua Hàng";
            object model = infoShop.ShoppingGuide;
            return View("Article", model);
        }

    }
}