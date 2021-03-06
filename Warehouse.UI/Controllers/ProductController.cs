﻿using Warehouse.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Warehouse.Services.Interface;
using System.Configuration;
using System;
using Warehouse.Common;
using Warehouse.Models;
using PagedList;
using DevTrends.MvcDonutCaching;

namespace Warehouse.Controllers
{

    [RoutePrefix("san-pham")]
    public class ProductController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        /// <summary>
        ///  List Product by Category
        /// </summary>
        /// <param name="aliasCategory"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [Route("danh-muc/{aliasCategory}.html")]
        public ActionResult Index(string aliasCategory, string productListView, string sortName, ENUM.SORT_TYPE? sortType, int? page)
        {
            string languageId = Request.Cookies["lang"].Value;
            Category category = _categoryService.GetByAlias(aliasCategory);
            if (category == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Name = category.Name;
            var products = _productService.GetByCategory(category.Id);
            if(category.Category1 != null && category.Category1.Count > 0)
            {
                foreach(Category category1 in category.Category1)
                {
                    products.AddRange(_productService.GetByCategory(category1.Id));
                }
            }
            if (sortName != null)
            {
                products = _productService.Sorting(products.AsQueryable(), sortName, sortType ?? ENUM.SORT_TYPE.Ascending).ToList();
            }
            ViewBag.CountAll = products.Count;
            ViewBag.Start = (((page ?? 1) - 1) * 20) + 1;
            IPagedList<GridProductViewModel> model = products.Select(p =>
                            new GridProductViewModel()
                            {
                                Id = p.Id,
                                Name = languageId == "vi" ? p.Name : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                                Alias = languageId == "vi" ? p.Alias_SEO : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO),
                                Image = p.Image,
                                SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                                Price = (int)(p.PriceNew ?? p.Price),
                                FlagColor = "#eba53d",
                                ProductFlag = p.Category.Name
                            }).ToPagedList(page ?? 1, 20);
            if (!string.IsNullOrEmpty(productListView) || sortName != null)
            {
                ViewBag.productListView = productListView;
                switch (productListView)
                {
                    case "grid": return PartialView("_GridPartial", model);
                    case "list": return PartialView("_ListPartial", model);
                    default: return PartialView("_GridPartial", model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// chi tiết sản phẩm
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        [Route("{alias}.html")]
        public ActionResult Details(string alias)
        {
            string languageId = Request.Cookies["lang"].Value;
            var product = _productService.GetByAlias(alias);
            if (product == null || product.Display == false)
            {
                return Redirect("/pages/404");
            }

            List<Product> productsRelated = _productService.GetRelatedProducts(product.CategoryId, product.Id);
            int count = productsRelated.Count;
            Random r = new Random();
            int skip = count - 10 > 0 ? r.Next(0, count - 10) : 0;
            ViewBag.productsRelated = productsRelated.Skip(skip).Take(10).ToList().Select(p=> new GridProductViewModel()
            {
                Id = p.Id,
                Name = languageId == "vi" ? p.Name : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                Alias = languageId == "vi" ? p.Alias_SEO : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO),
                Image = p.Image,
                SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                Price = (int)(p.PriceNew ?? p.Price),
                FlagColor = "#eba53d",
                ProductFlag = Warehouse.Language.Product.Index.RelatedProducts
            });

            DetailsProductViewModel _detail = new DetailsProductViewModel()
            {
                Id = product.Id,
                Name = languageId == "vi" ? product.Name : (product.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                Alias = languageId == "vi" ? product.Alias_SEO : (product.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO),
                Category = product.Category,
                Image = product.Image,
                imagesProducts = product.ImagesProducts.ToList(),
                Price = (int)(product.PriceNew ?? product.Price),
                FlagColor = "#eba53d",
                Description = languageId == "vi" ? product.Description : (product.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Description),
                ProductFlag = product.Category.Name,
                Content = languageId == "vi" ? product.Content : (product.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Content),
            };

            ViewBag.CategoryParent = _categoryService.GetById(product.CategoryId).Category2;

            return View(_detail);
        }

        public ActionResult _ContentQuickViewModal(int Id)
        {
            string languageId = Request.Cookies["lang"].Value;
            Product product = _productService.GetById(Id);
            if (product == null || product.Display == false)
                return Content("<p>Not Found</p>");
            QuickViewProductViewModel quickViewProductViewModel = new QuickViewProductViewModel()
            {
                Id = product.Id,
                Name = languageId == "vi" ? product.Name : (product.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                FlagColor = "#eba53d",
                ProductFlag = product.Category.Name,
                Alias = languageId == "vi" ? product.Alias_SEO : (product.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO),
                Image = product.Image,
                Description = languageId == "vi" ? product.Description : (product.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Description),
                Price = (int)(product.PriceNew ?? product.Price)
            };
            return PartialView(quickViewProductViewModel);
        }

        /// <summary>
        /// tìm kiếm sản phẩm
        /// </summary>
        /// <param name="keyword"> từ khóa tìm kiếm</param>
        /// <param name="page"> số trang sau khi tìm kiếm thành công</param>
        /// <returns></returns>
        [Route("ket-qua-tim-kiem.html")]
        public ActionResult Search(string keyword, int? page)
        {
            string languageId = Request.Cookies["lang"].Value;
            var dsProduct = _productService.Search(keyword);
            ViewBag.keyword = keyword;
            IPagedList<GridProductViewModel> model = dsProduct.Select(p =>
                            new GridProductViewModel()
                            {
                                Id = p.Id,
                                Name = languageId == "vi" ? p.Name : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Name),
                                Alias = languageId == "vi" ? p.Alias_SEO : (p.ProductTranslations?.FirstOrDefault(x => x.LanguageId == languageId)?.Alias_SEO),
                                Image = p.Image,
                                SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                                Price = (int)(p.PriceNew ?? p.Price),
                                FlagColor = "#eba53d",
                                ProductFlag = p.Category.Name
                            }).ToPagedList(page ?? 1, 20);
            return View(model);

        }

    }
}