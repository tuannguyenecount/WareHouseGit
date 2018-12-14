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
        private readonly IBlogService _blogService;
        private readonly ISlideService _slideService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;


        public HomeController(IProductService productService, IBlogService blogService, ISlideService slideService, ICategoryService categoryService, IOrderService orderService)
        {
            _productService = productService;
            _blogService = blogService;
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

            ViewBag.SaleProduct = _productService.GetAll().Where(p=> p.Display == true && p.PriceNew != null).OrderByDescending(p=>p.Id).Select(
                p => new GridProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Alias = p.Alias_SEO,
                    Image = p.Image,
                    SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                    Price = p.PriceNew.Value,
                    FlagColor = "#ad1f00",
                    ProductFlag = "<span style='text-decoration:line-through'>" + Warehouse.Common.Format.FormatCurrencyVND(p.Price) + "</span>"
                }).ToList();


            ViewBag.Slides = _slideService.GetAll().Where(s => s.Status == true);

            ViewBag.Blogs = _blogService.GetListByDisplay(true).Take(8).Select(b => new ListBlogViewModel()
            {
                Id = b.Id,
                Alias = b.Alias,
                Image = ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/News/" + b.Image,
                Description = b.Description,
                Title = b.Title,
                DateCreated = b.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(b.DateCreated.Value) : ""
            });
            return View();
        }

        public ActionResult _BlogPartial()
        {
            ViewBag.Blogs = _blogService.GetListByDisplay(true).OrderByDescending(b=>b.Id).Take(8).Select(b => new ListBlogViewModel() {
                Id = b.Id,
                Alias = b.Alias,
                Image = ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Blogs/" + b.Image,
                Description = b.Description,
                Title = b.Title,
                DateCreated = b.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(b.DateCreated.Value) : ""
            });
            return PartialView();
        }

        [OutputCache(Duration = 86400)]
        public PartialViewResult _StatisticalPartial()
        {
            ViewBag.CountAllProduct = _productService.CountDisplay();
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