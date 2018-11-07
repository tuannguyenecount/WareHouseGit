using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Models;
using Warehouse.Services.Interface;
namespace Warehouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly INewsService _newsService;
        private readonly ISlideService _slideService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService, INewsService newsService, ISlideService slideService, ICategoryService categoryService)
        {
            _productService = productService;
            _newsService = newsService;
            _slideService = slideService;
            _categoryService = categoryService;
        }

        [Route("")]
        public ActionResult Index()
        {
            ViewBag.NewProducts = _productService.GetNewProducts();
            ViewBag.Slides = _slideService.GetAll();
            ViewBag.News = _newsService.GetNews();          
            return View();
        }

        //[Route("gioi-thieu-shop.html")]
        //public ViewResult About()
        //{
        //    InfoShop infoShop = Session["InfoShop"] as InfoShop;
        //    ViewBag.Title = "Giới Thiệu " + infoShop.ShopName;
        //    object model = infoShop.Introduce_Shop;
        //    return View("Article", model);
        //}

        //[Route("chinh-sach-ban-hang.html")]
        //public ViewResult SalesPolicy()
        //{
        //    InfoShop infoShop = Session["InfoShop"] as InfoShop;
        //    ViewBag.Title = "Chính Sách Bán Hàng";
        //    object model = infoShop.SalesPolicy;
        //    return View("Article", model);
        //}

        //[Route("huong-dan-mua-hang.html")]
        //public ViewResult ShoppingGuide()
        //{
        //    InfoShop infoShop = Session["InfoShop"] as InfoShop;
        //    ViewBag.Title = "Hướng Dẫn Mua Hàng";
        //    object model = infoShop.ShoppingGuide;
        //    return View("Article", model);
        //}

        //[ChildActionOnly]
        ////[OutputCache(Duration = 3600)]
        //public PartialViewResult MainNav()
        //{
        //    return PartialView(db.Categories.ToList());
        //}

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public PartialViewResult _HeaderPartial()
        {
            // Lấy danh sách danh mục để hiện ra menu. Danh sách category được sắp xếp tăng dần theo cột OrderNum
            ViewBag.Categories = _categoryService.Sorting(_categoryService.GetAll(), Common.ENUM.SORT_TYPE.Ascending).ToList();
            return PartialView(Session["InfoShop"]);  // truyền thêm Session lưu thông tin của shop
        }

        //public CaptchaImageResult ShowCaptchaImage()
        //{
        //    return new CaptchaImageResult();
        //}

        [ChildActionOnly]
       // [OutputCache(Duration = 3600)]
        public PartialViewResult _FooterPartial()
        {
            return PartialView(Session["InfoShop"]);
        }

    }
}