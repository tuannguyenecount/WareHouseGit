using Warehouse.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Warehouse.Services.Interface;
using System.Configuration;
using System;
using Warehouse.Common;

namespace Warehouse.UI.Controllers
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
        public ActionResult Index(string aliasCategory, string sortName, ENUM.SORT_TYPE? sortType)
        {
            Category category = _categoryService.GetByAlias(aliasCategory);
            if (category == null)
            {
                return Redirect("/pages/404");
            }
            IQueryable<Product> products = _productService.GetByCategory(category.Id).AsQueryable();
            if (sortName != null)
            {
                products = _productService.Sorting(products, sortName, sortType ?? ENUM.SORT_TYPE.Ascending);
            }
            return View(products.ToList());
        }

        [Route("{alias}.html")]
        public ActionResult Details(string alias)
        {
            Product product = _productService.GetByAlias(alias);
            if (product == null)
            {
                return Redirect("/pages/404");
            }
            if (product.Slider != null)
            {
                ViewBag.Slider = product.Slider.Split(' ').Select(m =>
                ConfigurationManager.AppSettings["BaseUrl"] != null ?
                    ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Photos/Product/" + m : m).ToList();
            }
            List<Product> productsRelated = _productService.GetRelatedProducts(product.CategoryId, product.Id);
            int count = productsRelated.Count;
            Random r = new Random();
            int skip = count - 10 > 0 ? r.Next(0, count - 10) : 0;
            ViewBag.productsRelated = productsRelated.Skip(skip).Take(10).ToList();
            string splitString = ConfigurationManager.AppSettings["split_string"].ToString();
            return View(product);
        }

        //[Route("ket-qua-tim-kiem.html")]
        //[OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "keyword")]
        //public ActionResult SearchResult(string keyword)
        //{
        //    List<Product> dsProduct = db.Products.Where(m => m.Display == true  && m.Name.Contains(keyword)).ToList();
        //        return View(dsProduct);

        //}

        public PartialViewResult _PagedListPartial(string filterName, string filterValue, string sort, int page)
        {
            IEnumerable<Warehouse.Entities.Product> dsProduct = _productService.GetAll().Where(m => m.Display == true).OrderByDescending(m => m.Id).AsEnumerable();
            dsProduct = dsProduct.Where(m => m.Category != null && m.Category.Alias_SEO == filterValue);
            dsProduct = dsProduct.Skip((page - 1) * 8).Take(8);
            return PartialView(dsProduct);
        }


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}