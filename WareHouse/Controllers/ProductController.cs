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
        public ActionResult Search(string keyword)
        {
            List<Product> dsProduct = db.Products.Where(m => m.Display == true  && m.Name.Contains(keyword)).ToList();
                return View(dsProduct);
            
        }

        [NonAction]
        IEnumerable<Product> SapXep(IEnumerable<Product> dsProduct,string sort)
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

        public PartialViewResult _PhanTrangPartial(string filterName, string filterValue, string sort, int page)
        {
            IEnumerable<Product> dsProduct = db.Products.Where(m => m.Display == true).OrderByDescending(m => m.Id).AsEnumerable();
            dsProduct = dsProduct.Where(m => m.Category != null && m.Category.Alias_SEO == filterValue);
            dsProduct = SapXep(dsProduct, sort);
            dsProduct = dsProduct.Skip((page - 1) * 8).Take(8);
            return PartialView(dsProduct);
        }

        [Route("phan-loai/{alias}.html")]
        public ActionResult SPTheoLoai(string alias, string sort)
        {
            Category Category = db.Categories.SingleOrDefault(c=>c.Alias_SEO == alias);
            if(Category == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Name = Category.Name;

            IEnumerable<Product> dsProduct = db.Products.AsEnumerable().Where(m => m.Display == true && m.Category != null && m.Category.Alias_SEO == alias).OrderByDescending(m=>m.Id);
            if(sort != null)
            {
               dsProduct =  SapXep(dsProduct, sort);
            }

            return View(dsProduct.Take(8).ToList());
        }

        public string ThichProduct(int id, string CaptchaText)
        {

            try
            {
                if (CaptchaText != HttpContext.Session["captchastring"].ToString())
                {
                    return "Mã xác nhận không đúng!";
                }
 
                Product product = db.Products.Find(id);
                product.Likes++;
                if (Request.IsAuthenticated)
                {
                    string userIDCurrent = User.Identity.GetUserId();
                    UserLikeProduct model = new UserLikeProduct() { ProductId = id, UserId = User.Identity.GetUserId(), DateLike = DateTime.Now };
                    if (db.UserLikeProducts.FirstOrDefault(m=>m.UserId == userIDCurrent && m.ProductId == product.Id) == null)
                    {
                        db.UserLikeProducts.Add(model);
                    }
                }
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return "Bạn đã thích sản phẩm này thành công.";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string YeuThichProduct(int id, string CaptchaText)
        {

            try
            {
                if (CaptchaText != HttpContext.Session["captchastring"].ToString())
                {
                    return "Mã xác nhận không đúng!";
                }

                Product product = db.Products.Find(id);
                product.LoveTurns++;
                if (Request.IsAuthenticated)
                {
                    string userIDCurrent = User.Identity.GetUserId();
                    UserLoveProduct model = new UserLoveProduct() { ProductId = id, UserId = User.Identity.GetUserId(), DateLove = DateTime.Now };
                    if (db.UserLoveProducts.AsEnumerable().FirstOrDefault(m=>m.UserId == userIDCurrent && m.ProductId == product.Id) == null)
                    {
                        db.UserLoveProducts.Add(model);
                    }
                }
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return "Bạn đã yêu thích sản phẩm này thành công.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Authorize]
        [Route("san-phan-ban-yeu-thich.html")]
        public ActionResult XemProductYeuThich()
        {
            string id = User.Identity.GetUserId();
            AspNetUser user = db.AspNetUsers.Single(m=>m.Id == id);
            List<Product> dsProductDuocYeuThich = db.Products.Where(m=>m.Display == true).ToList().Join(user.UserLoveProducts.ToList(), oo => oo.Id, ii => ii.ProductId, (oo, ii) => oo).ToList();
            return View(dsProductDuocYeuThich);
        }

        [Authorize]
        [Route("san-phan-ban-thich.html")]
        public ActionResult XemProductThich()
        {
            string id = User.Identity.GetUserId();
            AspNetUser user = db.AspNetUsers.Single(m => m.Id == id);
            List<Product> dsProductDuocThich = db.Products.Where(m => m.Display == true).ToList().Join(user.UserLikeProducts.ToList(), oo => oo.Id, ii => ii.ProductId, (oo, ii) => oo).ToList();
            return View(dsProductDuocThich);
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