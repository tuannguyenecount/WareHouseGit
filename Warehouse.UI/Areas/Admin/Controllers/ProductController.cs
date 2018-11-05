using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Configuration;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class ProductController : Controller
    {
        #region Private Propertys
        hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();
        int WidthResize, HeightResize;
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();
        #endregion

        #region Constructor
        public ProductController():base()
        {
            this.WidthResize = int.Parse(ConfigurationManager.AppSettings["WidthImageProduct"]);
            this.HeightResize = int.Parse(ConfigurationManager.AppSettings["HeightImageProduct"]);
        }
        #endregion

        #region Show List Product
        public PartialViewResult ShowList(int? Category, bool? Display, int? Page,  string sortName, string sortBy)
        {
            ViewBag.Category = Category;
            ViewBag.sortBy = sortBy;
            ViewBag.sortName = sortName;
            ViewBag.Page = Page ?? 1;
            var dsProduct = db.Products.OrderByDescending(m=>m.Id).AsQueryable();
            if (Category != null && Category.Value != 0)
            {
                dsProduct = dsProduct.Where(m => m.CategoryId == Category);
            }
            if(Display != null)
            {
                dsProduct = dsProduct.Where(p => p.Display == Display.Value);
            }
            if (sortName == "Id")
            {
                if (sortBy == "ASC")
                {
                    dsProduct = dsProduct.OrderBy(m => m.Id);
                    ViewBag.SortByNext = "DESC";
                }
                else
                {
                    dsProduct = dsProduct.OrderByDescending(m => m.Id);
                    ViewBag.SortByNext = "ASC";
                }
            }
            if (sortName == "Name")
            {
                if (sortBy == "ASC")
                {
                    dsProduct = dsProduct.OrderBy(m => m.Name);
                    ViewBag.SortByNext = "DESC";
                }
                else
                {
                    dsProduct = dsProduct.OrderByDescending(m => m.Name);
                    ViewBag.SortByNext = "ASC";
                }
            }
            if (sortName == "Category")
            {
                if (sortBy == "ASC")
                {
                    dsProduct = dsProduct.OrderBy(m => m.CategoryId);
                    ViewBag.SortByNext = "DESC";
                }
                else
                {
                    dsProduct = dsProduct.OrderByDescending(m => m.CategoryId);
                    ViewBag.SortByNext = "ASC";
                }
            }
            if (sortName == "Price")
            {
                if (sortBy == "ASC")
                {
                    dsProduct = dsProduct.OrderBy(m => m.Price);
                    ViewBag.SortByNext = "DESC";
                }
                else
                {
                    dsProduct = dsProduct.OrderByDescending(m => m.Price);
                    ViewBag.SortByNext = "ASC";
                }
            }
            if (sortName == "NgayCapNhat")
            {
                if (sortBy == "ASC")
                {
                    dsProduct = dsProduct.OrderBy(m => m.DateCreated);
                    ViewBag.SortByNext = "DESC";
                }
                else
                {
                    dsProduct = dsProduct.OrderByDescending(m => m.DateCreated);
                    ViewBag.SortByNext = "ASC";
                }
            }
            if (sortName == "Display")
            {
                if (sortBy == "ASC")
                {
                    dsProduct = dsProduct.OrderBy(m => m.Display);
                    ViewBag.SortByNext = "DESC";
                }
                else
                {
                    dsProduct = dsProduct.OrderByDescending(m => m.Display);
                    ViewBag.SortByNext = "ASC";
                }
            }
            ViewBag.Page = Page;
            return PartialView("_ListPartial",(dsProduct as IOrderedQueryable<Product>).ThenByDescending(p=>p.Id).ToPagedList(Page ?? 1, 10));
        }
        #endregion

        #region Count Products
        public int DemTongSanPham(bool? Display)
        {
            if (Display == null)
                return db.Products.Count();
            return db.Products.Count(p => p.Display == Display);
        }
        #endregion

        #region Search By Name 
        public PartialViewResult Search(string tukhoa)
        {
            IEnumerable<Product> dsProduct = db.Products.Where(m => m.Name.Contains(tukhoa));
            return PartialView("_SearchPartial", dsProduct);
        }
        #endregion

        #region CRUD
        public ActionResult Index(int? page)
        {
            var categorys = db.Categories.ToList();
            categorys.Add(new Category() { Id = 0, Name = "Tất cả" });
            categorys = categorys.OrderBy(m => m.Id).ToList();
            ViewBag.Category = new SelectList(categorys, "Id", "Name", 0);
            ViewBag.Message = "Quản lý sản phẩm";
            return View();
        }

        public ActionResult Details(int? id, int? page)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id.Value);
            if(Product == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Message = "Chi tiết sản phẩm";
            if (!string.IsNullOrEmpty(Product.Slider))
            {
                ViewBag.ImageSlider = Product.Slider.Split(' ').ToList();
            }
            
            return View(Product);
        }

        public ViewResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Exclude = "Image")] Product model,IEnumerable<HttpPostedFileBase> Imagebosung, string base64String)
        {
            model.DateCreated = DateTime.Now;
            model.Likes = model.LoveTurns = 0;
            UniqueNameProduct uniqueNameProduct = new UniqueNameProduct() { ErrorMessage = "Tên sản phẩm bị trùng!" };
            if(uniqueNameProduct.IsValid(model.Name) == false)
            {
                ModelState.AddModelError("Name", uniqueNameProduct.ErrorMessage);
            }
            UniqueAliasProduct uniqueAliasProduct = new UniqueAliasProduct() { ErrorMessage = "Bí danh sản phẩm bị trùng!" };
            if (uniqueAliasProduct.IsValid(model.Alias_SEO) == false)
            {
                ModelState.AddModelError("Alias_SEO", uniqueAliasProduct.ErrorMessage);
            }
            #region Save File From String Base64
            if (!string.IsNullOrEmpty(base64String))
            {
                try
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    string newAvatar = model.Alias_SEO + DateTime.Now.Ticks.ToString() + ".jpg";
                    Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Product/" + newAvatar), base64String);
                    model.Image = newAvatar;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            #endregion
            try
            {
               
                if (ModelState.IsValid)
                {
                    
                    db.Products.Add(model);
                    db.SaveChanges();
                    if (Imagebosung != null && Imagebosung.ElementAt(0) != null)
                    {
                        int dem = 1;
                        model.Slider = "";
                        foreach (HttpPostedFileBase file in Imagebosung)
                        {
                            string extend = System.IO.Path.GetExtension(file.FileName);
                            if (ImageExtensions.Contains(extend.ToUpper()) == false)
                            {
                                ModelState.AddModelError("CustomError", "Hình slider có chứa file không hợp lệ. File hình phải có đuôi mở rộng là .jpg hoặc .png");
                                ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", model.CategoryId);
                                return View(model);
                            }
                            string filename = model.Alias_SEO + "-" + dem.ToString() + extend;
                            model.Slider += (filename + " ");
                            file.SaveAs(Server.MapPath("~/Photos/Product/") + filename);
                            dem++;
                           
                        }
                        if (model.Slider != null && model.Slider.Length > 0)
                        {
                            model.Slider = ThuVien.XoaKhoangTrangThua(model.Slider);
                        }
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                var categorys = db.Categories.ToList();
                categorys = categorys.OrderBy(m => m.Id).ToList();
                ViewBag.CategoryId = new SelectList(categorys, "Id", "Name", model.CategoryId);
                return View(model);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                        //raise a new exception inserting the current one as the InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                ModelState.AddModelError("CustomError", raise.Message);
                var categorys = db.Categories.ToList();
                categorys = categorys.OrderBy(m => m.Id).ToList();
                ViewBag.CategoryId = new SelectList(categorys, "Id", "Name", model.CategoryId);
                return View(model);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("CustomError", "Xảy ra lỗi khi thêm sản phẩm. Chi tiết: " + ex.Message);
                var categorys = db.Categories.ToList();
                categorys = categorys.OrderBy(m => m.Id).ToList();
                ViewBag.CategoryId = new SelectList(categorys, "Id", "Name", model.CategoryId);
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = await db.Products.FindAsync(id);
            if(Product == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", Product.CategoryId);
            if(!string.IsNullOrEmpty(Product.Slider))
                ViewBag.Slider = Product.Slider.Split(' ').ToList();
            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(Product Product, IEnumerable<HttpPostedFileBase> Imagebosung, string returnURL, string OldName, string OldAlias, string base64String)
        {
            if(Product.PromotionProduct != null)
            {
                if(Product.PromotionProduct.PromotionalPrice >= Product.Price)
                {
                    ModelState.AddModelError("CustomError", "Giá khuyến mãi phải < đơn giá cũ của sản phẩm!");
                    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", Product.CategoryId);
                    if (!string.IsNullOrEmpty(Product.Slider))
                        ViewBag.Slider = Product.Slider.Split(' ').ToList();
                    return View(Product);
                }
            }
            if(OldName != Product.Name)
            {
                UniqueNameProduct uniqueNameProduct = new UniqueNameProduct() { ErrorMessage = "Tên sản phẩm bị trùng!" };
                if (uniqueNameProduct.IsValid(Product.Name) == false)
                {
                    ModelState.AddModelError("CustomError", uniqueNameProduct.ErrorMessage);
                }
            }
            if (OldAlias != Product.Alias_SEO)
            {
                UniqueAliasProduct uniqueAliasProduct = new UniqueAliasProduct() { ErrorMessage = "Bí danh sản phẩm bị trùng!" };
                if (uniqueAliasProduct.IsValid(Product.Alias_SEO) == false)
                {
                    ModelState.AddModelError("CustomError", uniqueAliasProduct.ErrorMessage);
                }
            }
            if (!string.IsNullOrEmpty(base64String))
            {
                try
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    string newAvatar = Product.Alias_SEO + DateTime.Now.Ticks.ToString() + ".jpg";
                    Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Product/" + newAvatar), base64String);
                    Product.Image = newAvatar;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            if (ModelState.IsValid)
            {
                if (Imagebosung != null && Imagebosung.ElementAt(0) != null )
                {
                     if(Imagebosung.ToList().FirstOrDefault(m=> ImageExtensions.Contains(Path.GetExtension(m.FileName).ToUpper()) == false) != null)
                     {
                        ModelState.AddModelError("CustomError","Danh sách hình bổ sung chứa file hình không hợp lệ!");
                        ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", Product.CategoryId);
                        if (!string.IsNullOrEmpty(Product.Slider))
                            ViewBag.Slider = Product.Slider.Split(' ').ToList();
                        return View(Product);
                     }
                    int dem;
                    if (!string.IsNullOrEmpty(Product.Slider))
                    {
                        dem= int.Parse(Product.Slider[Product.Slider.ToString().LastIndexOf(".") - 1].ToString()) + 1;
                    }
                    else
                    {
                        dem = 1;
                    }
                    if (Imagebosung != null)
                    {
                        foreach (HttpPostedFileBase file in Imagebosung)
                        {
                            string extend = Path.GetExtension(file.FileName);
                            string name = Product.Alias_SEO.ToString() + "-" + dem.ToString() + extend;
                            file.SaveAs(Server.MapPath("~/Photos/Product/") + name);
                            dem++;
                            Product.Slider += " " + name;
                        }
                    }
                }
                if (Product.Slider != null && Product.Slider.Length > 0)
                {
                    Product.Slider = ThuVien.XoaKhoangTrangThua(Product.Slider);
                }
                db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
                if (Product.PromotionProduct != null)
                {
                    db.Entry(Product.PromotionProduct).State = System.Data.Entity.EntityState.Modified;
                }
                try
                {
                    await db.SaveChangesAsync();
                }
                catch
                {
                    ModelState.AddModelError("CustomError","Xảy ra lỗi khi lưu dữ liệu!");
                }
                return Redirect(returnURL);
            }
            var categorys = db.Categories.ToList();
            categorys = categorys.OrderBy(m => m.Id).ToList();
            ViewBag.CategoryId = new SelectList(categorys, "Id", "Name", Product.CategoryId);
            if (!string.IsNullOrEmpty(Product.Slider))
                ViewBag.Slider = Product.Slider.Split(' ').ToList();
            return View(Product);
        }

        #endregion

        #region Hide Product
        public ActionResult Hide(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if(Product == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Id = Product.Id;
            ViewBag.Name = Product.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Hide(FormCollection form)
        {
            if (Session["Revalidate"] == null)
            {
                object thongbao = "Bạn chưa xác thực mật khẩu lần 2 để thực hiện thao tác xóa này!";
                return View("_ThongBaoLoi", thongbao);
            }
            Product Product = db.Products.Find(int.Parse(form["Id"]));
            Product.Display = false;
            db.Entry(Product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Show Product
        public ActionResult Show(int id)
        {
            Product product = db.Products.Find(id);
            product.Display = true;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Change Image Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeImage(int id, string base64String)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                ModelState.AddModelError("LoiDoiImage", "Sản phẩm này không tồn tại!");
            }
            else
            {
                #region Save File From String Base64
                if (!string.IsNullOrEmpty(base64String))
                {
                    try
                    {
                        base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                        string newImage = product.Alias_SEO + DateTime.Now.Ticks.ToString() + ".jpg";
                        Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Product/" + newImage), base64String);
                        product.Image = newImage;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LoiDoiImage", "Lỗi khi lưu hình từ chuỗi base64 " + ex.Message);
                    }
                    try
                    {
                        db.Entry(product).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Edit", new { id = product.Id });
                    }
                    catch
                    {
                        ModelState.AddModelError("LoiDoiImage", "Xảy ra lỗi khi lưu dữ liệu!");
                    }

                }
                #endregion
                else
                {
                    ModelState.AddModelError("LoiDoiImage", "Bạn chưa chọn hình muốn đổi!");
                }
            }

            return View("Edit", product);
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}