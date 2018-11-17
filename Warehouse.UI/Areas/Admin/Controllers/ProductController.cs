using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using System.Net;
using System.Configuration;
using Warehouse.Services.Interface;
using Warehouse.Entities;
using System.Web.UI.WebControls;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin,Mod")]
    public class ProductController : Controller
    {
        #region Private Propertys
        int WidthResize, HeightResize;
        private IProductService _productService;
        private ICategoryService _categoryService;
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        #endregion

        #region Constructor

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            this.WidthResize = int.Parse(ConfigurationManager.AppSettings["WidthImageProduct"]);
            this.HeightResize = int.Parse(ConfigurationManager.AppSettings["HeightImageProduct"]);

        }


        #endregion

        #region Export Data


        public ActionResult ExportToExcel()
        {
            var data = _productService.GetAll();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=products.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            Warehouse.Common.File.WriteHtmlTable(data, Response.Output);
            Response.End();
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion

        #region CRUD

        public ActionResult Index(string tabActive)
        {
            List<Product> products = _productService.GetAll();
            return View(products);
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            if(product == null)
            {
                return Redirect("/pages/404");
            }            
            return View(product);
        }

        public ViewResult Create()
        {
            ViewBag.Categories = _categoryService.GetParents().OrderBy(c => c.OrderNum).ToList();
            ViewBag.Categories1 = new Dictionary<int, List<Category>>();
            foreach (Category category in ViewBag.Categories)
            {
                (ViewBag.Categories1 as Dictionary<int, List<Category>>).Add(category.Id, _categoryService.GetChilds(category.Id));
            }
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Exclude = "Image,DateCreated,UserCreated,DateUpdated")] Product model, IEnumerable<HttpPostedFileBase> files, string base64String)
        {
            model.DateCreated = DateTime.Now;
            model.UserCreated = User.Identity.Name;
            model.DateUpdated = null;
            if(_productService.CheckUniqueName(model.Name) == false)
            {
                ModelState.AddModelError("Name", "Tên sản phẩm bị trùng với sản phẩm khác. Vui lòng đặt lại.");

            }
            if (_productService.CheckUniqueAlias(model.Alias_SEO) == false)
            {
                ModelState.AddModelError("Alias_SEO", "Bí danh sản phẩm bị trùng với sản phẩm khác. Vui lòng đặt lại.");
            }
            if (model.Price < 0)
            {
                ModelState.AddModelError("Price", "Giá phải >= 0.");
            }
            if (model.PriceNew != null)
            {
                if (model.PriceNew.Value < 0)
                    ModelState.AddModelError("PriceNew", "Giá mới phải >= 0.");
                else if (model.PriceNew.Value < model.Price)
                    ModelState.AddModelError("PriceNew", "Giá mới phải >= giá cũ.");
            }
            #region Save File From String Base64
            if (!string.IsNullOrEmpty(base64String))
            {
                try
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    string newAvatar = model.Alias_SEO + ".jpg";
                    Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Products/" + newAvatar), base64String);
                    model.Image = newAvatar;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            if(files != null && files.Count() > 0 && files.ElementAt(0) != null)
            {
                int i = 1;
                foreach(HttpPostedFileBase httpPostedFileBase in files)
                {
                    string extend = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                    if(ImageExtensions.Contains(extend.ToUpper()))
                    {
                        string image = model.Alias_SEO + "-" + i.ToString() + extend;
                        model.ImagesProducts = new List<ImagesProduct>();
                        model.ImagesProducts.Add(new ImagesProduct()
                        {
                            Image = image,
                            OrderNum = i
                        });
                        try
                        {
                            httpPostedFileBase.SaveAs(Server.MapPath("~/Photos/Products/slide/" + image));
                            i++;
                        }
                        catch(Exception ex)
                        {
                            ModelState.AddModelError("files", ex.Message);
                        }
                    }
                }
            }
            #endregion
            try
            {              
                if (ModelState.IsValid)
                {
                    _productService.Add(model);   
                    return RedirectToAction("Index");
                }
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
                ModelState.AddModelError("", raise.Message);
                ViewBag.Categories = _categoryService.GetParents().OrderBy(c => c.OrderNum).ToList();
                ViewBag.Categories1 = new Dictionary<int, List<Category>>();
                foreach (Category category in ViewBag.Categories)
                {
                    (ViewBag.Categories1 as Dictionary<int, List<Category>>).Add(category.Id, _categoryService.GetChilds(category.Id));
                }
                return View(model);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Xảy ra lỗi khi thêm sản phẩm. Chi tiết: " + ex.Message);
            }
            ViewBag.Categories = _categoryService.GetParents().OrderBy(c => c.OrderNum).ToList();
            ViewBag.Categories1 = new Dictionary<int, List<Category>>();
            foreach (Category category in ViewBag.Categories)
            {
                (ViewBag.Categories1 as Dictionary<int, List<Category>>).Add(category.Id, _categoryService.GetChilds(category.Id));
            }
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            if(product == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Categories = _categoryService.GetParents().OrderBy(c => c.OrderNum).ToList();
            ViewBag.Categories1 = new Dictionary<int, List<Category>>();
            foreach (Category category in ViewBag.Categories)
            {
                (ViewBag.Categories1 as Dictionary<int, List<Category>>).Add(category.Id, _categoryService.GetChilds(category.Id));
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product, string OldName, string OldAlias, string base64String)
        {
            product.DateUpdated = DateTime.Now;
            if(OldName != product.Name)
            {
               if(_productService.CheckUniqueName(product.Name) == false)
               {
                    ModelState.AddModelError("Name", "Tên sản phẩm bị trùng với sản phẩm khác. Vui lòng đặt lại.");
               }
            }
            if (OldAlias != product.Alias_SEO)
            {
                if (_productService.CheckUniqueAlias(product.Alias_SEO) == false)
                {
                    ModelState.AddModelError("Alias_SEO", "Bí danh sản phẩm bị trùng với sản phẩm khác. Vui lòng đặt lại.");
                }
            }
            if(product.Price < 0 )
            {
                ModelState.AddModelError("Price", "Giá phải >= 0.");
            }
            if (product.PriceNew != null)
            {
                if(product.PriceNew.Value < 0)
                    ModelState.AddModelError("PriceNew", "Giá mới phải >= 0.");
                else if(product.PriceNew.Value < product.Price)
                    ModelState.AddModelError("PriceNew", "Giá mới phải >= giá cũ.");
            }
            if (!string.IsNullOrEmpty(base64String))
            {
                try
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    string newAvatar = product.Alias_SEO + DateTime.Now.Ticks.ToString() + ".jpg";
                    Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Products/" + newAvatar), base64String);
                    product.Image = newAvatar;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.Update(product);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("CustomError",ex.Message);
                }
                return RedirectToAction("Details", new { Id = product.Id });
            }
            ViewBag.Categories = _categoryService.GetParents().OrderBy(c => c.OrderNum).ToList();
            ViewBag.Categories1 = new Dictionary<int, List<Category>>();
            foreach (Category category in ViewBag.Categories)
            {
                (ViewBag.Categories1 as Dictionary<int, List<Category>>).Add(category.Id, _categoryService.GetChilds(category.Id));
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, string tabActive)
        {
            Product product = _productService.GetById(Id);
            if (product != null)
                _productService.Delete(Id);
            return RedirectToAction("Index", new { tabActive = tabActive });
        }

        #endregion

        #region Hide Product
        public ActionResult Hide(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            if(product == null)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Id = product.Id;
            ViewBag.Name = product.Name;
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
            Product product = _productService.GetById(int.Parse(form["Id"]));
            product.Display = false;
            _productService.Update(product);
            return RedirectToAction("Index");
        }

        #endregion

        #region Show Product
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            product.Display = true;
            _productService.Update(product);
            return RedirectToAction("Index");
        }
        #endregion

        #region Change Image Product
        [HttpPost]
        public ActionResult ChangeImage(int? id, string base64String)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            if (product == null)
            {
                ModelState.AddModelError("", "Sản phẩm này không tồn tại!");
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
                        Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Products/" + newImage), base64String);
                        product.Image = newImage;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu hình từ chuỗi base64 " + ex.Message);
                    }
                    try
                    {
                        _productService.Update(product);
                        return RedirectToAction("Edit", new { id = product.Id });
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Xảy ra lỗi khi lưu dữ liệu!");
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

        #region Count Product
        [ChildActionOnly]
        public ContentResult CountAll()
        {
            return Content(_productService.CountAll().ToString());
        }
        #endregion

        [HttpPost]
        public ActionResult AddImages(int Id, HttpPostedFileBase[] files)
        {
            if (files != null && files.Count() > 0 && files[0] != null)
            {
                Product product = _productService.GetById(Id);
                if (product != null)
                {
                    int i = 1;
                    if (product.ImagesProducts == null)
                        product.ImagesProducts = new List<ImagesProduct>();
                    List<ImagesProduct> imagesProducts = product.ImagesProducts.ToList();

                    foreach (HttpPostedFileBase file in files)
                    {
                        string extend = System.IO.Path.GetExtension(file.FileName);
                        if (ImageExtensions.Contains(extend.ToUpper()))
                        {

                            imagesProducts.Add(new ImagesProduct()
                            {
                                ProductId = Id,
                                OrderNum = i,
                                Image = product.Alias_SEO + "-" + DateTime.Now.Ticks.ToString() + extend
                            });
                            i++;
                        }
                        
                    }
                    product.ImagesProducts = imagesProducts;
                    _productService.Update(product);
                }
            }
            return RedirectToAction("Edit", new { Id = Id });
        }

        [HttpPost]
        public ActionResult DeleteImage(int Id, int IdImage)
        {
            Product product = _productService.GetById(Id);
            if(product != null)
            {
                ImagesProduct imagesProduct = product.ImagesProducts.FirstOrDefault(p => p.Id == IdImage);
                if(imagesProduct != null)
                {
                    product.ImagesProducts.Remove(imagesProduct);
                }
                _productService.Update(product);
            }
            return RedirectToAction("Details", new { Id = Id });
        }

    }
}