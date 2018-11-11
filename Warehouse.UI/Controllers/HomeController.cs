using System;
using System.Collections.Generic;
using System.Configuration;
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

  
        public ActionResult Index()
        {
            ViewBag.NewProducts = _productService.GetNewProducts().Select(
                p => new GridProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Alias = p.Alias_SEO,
                    Image = p.Image,
                    SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null), 
                    Price = (int)(p.PriceNew ?? p.Price),
                    FlagColor = "#969696",
                    ProductFlag = "new"
                }).ToList();

            ViewBag.HotProductsInWeek = _productService.GetHotProductsInWeek().Select(
                p => new GridProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Alias = p.Alias_SEO,
                    Image =  p.Image,
                    SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                    Price = (int)(p.PriceNew ?? p.Price),
                    FlagColor = "#ad1f00",
                    ProductFlag = "hot"
                }).ToList(); ;


            ViewBag.Slides = _slideService.GetAll().Where(s => s.Status == true).OrderBy(s => s.Order);

            ViewBag.News = _newsService.GetNews(10).Select(n => new NewsListViewModel()
            {
                Id = n.Id,
                Alias = n.Alias_SEO,
                Image = ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/News/" + n.Image,
                Introduce = n.Introduce,
                Title = n.Title,
                DateSubmitted = n.DateSubmitted ?? new DateTime(2018, 01, 01)
            });
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

        [OutputCache(Duration = 86400)]
        public PartialViewResult _GoogleMapPartial()
        {
            return PartialView();
        }

    }
}