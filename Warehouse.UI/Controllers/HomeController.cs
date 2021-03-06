﻿using DevTrends.MvcDonutCaching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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

        [DonutOutputCache(Duration = 86400, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            string languageId = Request.Cookies["lang"].Value;

            ViewBag.NewProducts = _productService.GetNewProducts().Select(
                p => new GridProductViewModel()
                {
                    Id = p.Id,
                    Name = languageId == "vi" ? p.Name : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                    Alias = languageId == "vi" ? p.Alias_SEO :  (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO),
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
                    Name = languageId == "vi" ? p.Name :(p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                    Alias = languageId == "vi" ? p.Alias_SEO : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO),
                    Image =  p.Image,
                    SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                    Price = (int)(p.PriceNew ?? p.Price),
                    FlagColor = "#ad1f00",
                    ProductFlag = "hot"
                }).ToList(); ;

            ViewBag.SaleProduct = _productService.GetSaleProducts().Select(
                p => new GridProductViewModel()
                {
                    Id = p.Id,
                    Name = languageId == "vi" ? p.Name : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                    Alias = languageId == "vi" ? p.Alias_SEO : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO ),
                    Image = p.Image,
                    SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                    Price = p.PriceNew.Value,
                    FlagColor = "#ad1f00",
                    ProductFlag = "<span style='text-decoration:line-through'>" + Warehouse.Common.Format.FormatCurrencyVND(p.Price) + "</span>"
                }).ToList();

            ViewBag.Slides = _slideService.GetAll().Where(s => s.Status == true).OrderBy(x => x.Order);

            return View();
        }

        public ActionResult _BlogPartial()
        {
            string languageId = Request.Cookies["lang"].Value;
            ViewBag.Blogs = _blogService.GetListByDisplay(true)
                .Where(x => (languageId != "vi" && x.BlogTranslations.FirstOrDefault(y => y.LanguageId == languageId) != null) || (languageId == "vi"))
                .OrderByDescending(b => b.Id).Take(8)
                .Select(x => new ListBlogViewModel()
                {
                    Id = x.Id,
                    Alias = languageId == "vi" ? x.Alias : (x.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Alias),
                    Image = ConfigurationManager.AppSettings["BaseUrl"] + "/Photos/Blogs/" + x.Image,
                    Description = languageId == "vi" ? x.Description : (x.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Description),
                    Title = languageId == "vi" ? x.Title : (x.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Title),
                    DateCreated = x.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(x.DateCreated.Value) : ""
                });
            return PartialView();
        }

        [DonutOutputCache(Duration = 86400)]
        public PartialViewResult _StatisticalPartial()
        {
            ViewBag.CountAllProduct = _productService.CountDisplay();
            ViewBag.CountAllCategory = _categoryService.CountAll();
            ViewBag.CountAllOrder = _orderService.CountAll();
            return PartialView();
        }

        public PartialViewResult _GoogleMapPartial()
        {
            return PartialView();
        }

    }
}