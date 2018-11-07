﻿using System;
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
        private readonly IOrderService _orderService;


        public HomeController(IProductService productService, INewsService newsService, ISlideService slideService, ICategoryService categoryService, IOrderService orderService)
        {
            _productService = productService;
            _newsService = newsService;
            _slideService = slideService;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        [Route("")]
        public ActionResult Index()
        {
            ViewBag.NewProducts = _productService.GetNewProducts().Select(
                p=> new GridProductViewModel()
                {
                   Name = p.Name,
                   Alias = p.Alias_SEO,
                   Image = p.Image,
                   Price = (int)(p.PriceNew ?? p.Price),
                   FlagColor = "#969696",
                   ProductFlag = "new"
                }).ToList();

            ViewBag.HotProductsInWeek = _productService.GetHotProductsInWeek().Select(
                p => new GridProductViewModel()
                {
                    Name = p.Name,
                    Alias = p.Alias_SEO,
                    Image = p.Image,
                    Price = (int)(p.PriceNew ?? p.Price),
                    FlagColor = "#ad1f00",
                    ProductFlag = "hot"
                }).ToList(); ;

            ViewBag.Slides = _slideService.GetAll().Where(s=>s.Status == true).OrderBy(s=>s.Order);
            ViewBag.News = _newsService.GetNews();
            return View();
        }

        [OutputCache(Duration = 86400)]
        public PartialViewResult _StatisticalPartial()
        {
            ViewBag.CountAllProduct = _productService.CountAll();
            ViewBag.CountAllCategory = _categoryService.CountAll();
            ViewBag.CountAllOrder = _orderService.CountAll();
            return PartialView();
        }

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


        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public PartialViewResult _HeaderPartial()
        {
            // Lấy danh sách danh mục để hiện ra menu. Danh sách category được sắp xếp tăng dần theo cột OrderNum
            ViewBag.Categories = _categoryService.Sorting(_categoryService.GetAll(), Common.ENUM.SORT_TYPE.Ascending).ToList();
            return PartialView(Session["InfoShop"]);  // truyền thêm Session lưu thông tin của shop
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public PartialViewResult _FooterPartial()
        {
            return PartialView(Session["InfoShop"]);
        }

        [OutputCache(Duration = 86400)]
        public PartialViewResult _GoogleMapPartial()
        {
            return PartialView();
        }

    }
}