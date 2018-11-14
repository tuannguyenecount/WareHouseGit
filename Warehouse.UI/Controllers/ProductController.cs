using Warehouse.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Warehouse.Services.Interface;
using System.Configuration;
using System;
using Warehouse.Common;
using Warehouse.Models;
using PagedList;

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
            Category category = _categoryService.GetByAlias(aliasCategory);
            if (category == null)
            {
                return Redirect("/pages/404");
            }

            ViewBag.Name = aliasCategory;
            var products = _productService.GetByCategory(category.Id)
                            .Where(p => p.Status == true).OrderByDescending(p => p.Id).ToList();

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
                                Name = p.Name,
                                Alias = p.Alias_SEO,
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
            var product = _productService.GetByAlias(alias);
            if (product == null)
            {
                return Redirect("/pages/404");
            }

            List<Product> productsRelated = _productService.GetRelatedProducts(product.CategoryId, product.Id);
            int count = productsRelated.Count;
            Random r = new Random();
            int skip = count - 10 > 0 ? r.Next(0, count - 10) : 0;
            ViewBag.productsRelated = productsRelated.Skip(skip).Take(10).ToList();

            DetailsProductViewModel _detail = new DetailsProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Alias = product.Alias_SEO,
                Image = product.Image,
                imagesProducts = product.ImagesProducts.ToList(),
                Price = (int)(product.PriceNew ?? product.Price),
                FlagColor = "#eba53d",
                Description = product.Description,
                ProductFlag = product.Category.Name
            };

            return View(_detail);
        }

        public ActionResult _ContentQuickViewModal(int Id)
        {
            Product product = _productService.GetById(Id);
            if (product == null)
                return Content("<p>Sản phẩm không tồn tại!</p>");
            QuickViewProductViewModel quickViewProductViewModel = new QuickViewProductViewModel()
            {
                Id = product.Id,
                Alias = product.Alias_SEO,
                FlagColor = "#eba53d",
                ProductFlag = product.Category.Name,
                Name = product.Name,
                Image = product.Image,
                Description = product.Description,
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
        //[Route("ket-qua-tim-kiem.html")]
        //[OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "keyword")]
        public ActionResult SearchResult(string keyword, int? page)
        {
            var dsProduct = _productService.GetAll().Where(m => m.Display == true && m.Name.Contains(keyword)).ToList();
            ViewBag.keyword = keyword;
            IPagedList<GridProductViewModel> model = dsProduct.Select(p =>
                            new GridProductViewModel()
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Alias = p.Alias_SEO,
                                Image = p.Image,
                                SecondImage = (p.ImagesProducts != null && p.ImagesProducts.Count > 0 ? p.ImagesProducts.ElementAt(0).Image : null),
                                Price = (int)(p.PriceNew ?? p.Price),
                                FlagColor = "#eba53d",
                                ProductFlag = p.Category.Name
                            }).ToPagedList(page ?? 1, 20);
            return View(model);

        }

        public ActionResult _ListPartial(int curentpage = 1)
        {
            int pageSize = 5; // số dòng trên 1 trang
            var products = _productService.GetAll().Where(m => m.Display == true).OrderByDescending(m => m.Id).AsQueryable();
            ProductListModel model = new ProductListModel
            {
                Products = products.ToPagedList(curentpage, pageSize),
                PageCount = (int)Math.Ceiling(products.Count() / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = curentpage
            };

            return PartialView(model);
        }
    }
}