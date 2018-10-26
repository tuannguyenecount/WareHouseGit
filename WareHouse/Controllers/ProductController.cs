using WareHouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Configuration;

namespace WareHouse.Controllers
{
    
    [RoutePrefix("san-pham")]
    public class ProductController : Controller
    {
        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        
        [Route("{alias}.html")]
        public ActionResult Details(string alias)
        {
            //Product Product = db.Products.SingleOrDefault(p=>p.Alias_SEO == alias && p.Display == true);
            //if(Product == null)
            //{
            //    return Redirect("/pages/404");
            //}
            //if (Product.Slider != null)
            //{
            //    ViewBag.Slider = Product.Slider.Split(' ').Select(m=> 
            //    ConfigurationManager.AppSettings["BaseUrl"] != null ? 
            //        ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Photos/Product/" + m : m).ToList();
            //}
            //List<Product> ProductCungNhom = db.Products.Where(m => m.Display == true && m.CategoryId == Product.CategoryId && m.Alias_SEO != alias).ToList();
            //int count = ProductCungNhom.Count;
            //Random r = new Random();
            //int skip = count - 10 > 0 ? r.Next(0, count - 10) : 0; 
            //ViewBag.ProductLienQuan = ProductCungNhom.Skip(skip).Take(10).ToList();
            //ViewBag.CoTheBanThich = ProductCungNhom.OrderByDescending(m => m.LoveTurns + m.Likes).Take(10).ToList();

            //string splitString = ConfigurationManager.AppSettings["split_string"].ToString();
            //ViewBag.ListMauSac = Product.Color != null ? Product.Color.Split(splitString.ToCharArray()) : null;
            //ViewBag.ListSize = Product.Size != null ? Product.Size.Split(splitString.ToCharArray()) : null;
            //return View(Product);
            return View();
        }

        [Route("ket-qua-tim-kiem.html")]
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "keyword")]
        public ActionResult SearchResult(string keyword)
        {
            List<Product> dsProduct = db.Products.Where(m => m.Display == true  && m.Name.Contains(keyword)).ToList();
                return View(dsProduct);
            
        }

        [NonAction]
        IEnumerable<Product> Sorting(IEnumerable<Product> dsProduct,string sort)
        {
            if(sort == "GiaTangDan")
            {
                dsProduct = dsProduct.OrderBy(m => m.PromotionProduct != null ? m.PromotionProduct.PromotionalPrice : m.Price );
            }
            else if(sort == "GiaGiamDan")
            {
                dsProduct = dsProduct.OrderByDescending(m => m.PromotionProduct != null ? m.PromotionProduct.PromotionalPrice : m.Price);
            }
            return dsProduct;
        }

        public PartialViewResult _PagedListPartial(string filterName, string filterValue, string sort, int page)
        {
            IEnumerable<Product> dsProduct = db.Products.Where(m => m.Display == true).OrderByDescending(m => m.Id).AsEnumerable();
            dsProduct = dsProduct.Where(m => m.Category != null && m.Category.Alias_SEO == filterValue);
            dsProduct = Sorting(dsProduct, sort);
            dsProduct = dsProduct.Skip((page - 1) * 8).Take(8);
            return PartialView(dsProduct);
        }

        [Route("danh-muc/{aliasCategory}.html")]
        public ActionResult Index(string aliasCategory, string sort)
        {
            Category Category = db.Categories.SingleOrDefault(c=>c.Alias_SEO == aliasCategory);
            if(Category == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Name = Category.Name;
            IEnumerable<Product> dsProduct = db.Products.AsEnumerable().Where(m => m.Display == true && m.Category != null && m.Category.Alias_SEO == aliasCategory).OrderByDescending(m=>m.Id);
            if(sort != null)
            {
               dsProduct =  Sorting(dsProduct, sort);
            }
            return View(dsProduct.Take(8).ToList());
        }
 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}