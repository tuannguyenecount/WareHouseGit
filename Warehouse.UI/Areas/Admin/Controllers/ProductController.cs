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
using System.IO;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class ProductController : Controller
    {
        #region Private Propertys
        int WidthResize, HeightResize;
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IImagesProductService _imagesProductService;
        private ILanguageService _languageService;
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();
        #endregion

        public ProductController(IProductService productService, ICategoryService categoryService, IImagesProductService imagesProductService, ILanguageService languageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imagesProductService = imagesProductService;
            _languageService = languageService;
            this.WidthResize = int.Parse(ConfigurationManager.AppSettings["WidthImageProduct"]);
            this.HeightResize = int.Parse(ConfigurationManager.AppSettings["HeightImageProduct"]);
        }

        #region Export Data

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
            Dictionary<string, string> languages = new Dictionary<string, string>();

            _languageService.GetAll().Where(x => x.Id != "vi")
                            .OrderBy(x => x.SortOrder).ToList().ForEach(x => languages.Add(x.Id, x.Name));
            ViewBag.Languages = languages;
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
            #region Set default value
            model.DateCreated = DateTime.Now;
            model.UserCreated = User.Identity.Name;
            model.DateUpdated = null;
            #endregion
            #region Check validation
            if(model.CategoryId == 0)
            {
                ModelState.AddModelError("", "Bạn chưa chọn phân loại cho sản phẩm!");
            }
            if (_productService.CheckUniqueName(model.Name) == false)
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
                else if (model.PriceNew.Value >= model.Price)
                    ModelState.AddModelError("PriceNew", "Giá mới phải < giá cũ.");
            }
            #endregion 
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
            else
            {
                model.Image = "no_photo.gif";
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
                    if (files != null && files.Count() > 0 && files.ElementAt(0) != null)
                    {
                        int i = 1;
                        foreach (HttpPostedFileBase file in files)
                        {
                            string extend = System.IO.Path.GetExtension(file.FileName);
                            if (ImageExtensions.Contains(extend.ToUpper()))
                            {
                                _imagesProductService.AddImage(new ImagesProduct()
                                {
                                    ProductId = model.Id,
                                    OrderNum = i,
                                    Image = model.Alias_SEO + "-" + i.ToString() + extend
                                });
                                i++;
                            }
                        }
                    }
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
            #region Response view and error message
            ViewBag.Categories = _categoryService.GetParents().OrderBy(c => c.OrderNum).ToList();
            ViewBag.Categories1 = new Dictionary<int, List<Category>>();
            foreach (Category category in ViewBag.Categories)
            {
                (ViewBag.Categories1 as Dictionary<int, List<Category>>).Add(category.Id, _categoryService.GetChilds(category.Id));
            }
            #endregion
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
            if (product.CategoryId == 0)
            {
                ModelState.AddModelError("", "Bạn chưa chọn phân loại cho sản phẩm!");
            }
            if (OldName != product.Name)
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
                else if(product.PriceNew.Value >= product.Price)
                    ModelState.AddModelError("PriceNew", "Giá mới phải < giá cũ.");
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
                    return RedirectToAction("Details", new { Id = product.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("",ex.Message);
                }
            }

            product.ImagesProducts = _productService.GetById(product.Id).ImagesProducts;
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
                    ModelState.AddModelError("", "Bạn chưa chọn hình muốn đổi!");
                }
            }

            return View("Edit", product);
        }
        #endregion

        #region Count Product
        [ChildActionOnly]
        public ContentResult CountDisplay()
        {
            return Content(_productService.CountDisplay().ToString());
        }

        [ChildActionOnly]
        public ContentResult CountHide()
        {
            return Content(_productService.CountHide().ToString());
        }
        #endregion

        #region Processing List Images

        [HttpPost]
        public ActionResult AddImages(int Id, HttpPostedFileBase[] files)
        {
            if (files != null && files.Count() > 0 && files[0] != null)
            {
                Product product = _productService.GetById(Id);
                if (product != null)
                {
                    int i = 1;
                    foreach (HttpPostedFileBase file in files)
                    {
                        string extend = System.IO.Path.GetExtension(file.FileName);
                        if (ImageExtensions.Contains(extend.ToUpper()))
                        {
                            string Image = product.Alias_SEO + "-" + DateTime.Now.Ticks.ToString() + extend;
                            file.SaveAs(Server.MapPath("~/Photos/Products/slide/" + Image));
                            _imagesProductService.AddImage(new ImagesProduct()
                            {
                                ProductId = Id,
                                OrderNum = i,
                                Image = Image
                            });
                            i++;
                        }
                    }
                }
            }
            return RedirectToAction("Edit", new { Id = Id });
        }

        [HttpPost]
        public ActionResult DeleteImage(int ProductId, int ImageId)
        {
            _imagesProductService.DeleteImage(ImageId);
            return RedirectToAction("Edit", new { Id = ProductId });
        }
        #endregion

        #region Create Translation
        public ActionResult CreateTranslation(int Id, string countrySelect)
        {
            ViewBag.LanguageSelected = _languageService.GetById(countrySelect);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            return View(new ProductTranslationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTranslation(ProductTranslationViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _productService.CreateTranslation(new ProductTranslation()
                    {
                        ProductId = model.ProductId,
                        Content = model.Content,
                        Description = model.Description,
                        Alias_SEO = model.Alias_SEO,
                        LanguageId = model.LanguageId,
                        Name = model.Name
                    });
                    return RedirectToAction("Details", new { id = model.ProductId, languageSelected = model.LanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region Edit Translation
        public ActionResult EditTranslation(int ProductId, string LanguageId)
        {
            ViewBag.LanguageSelected = _languageService.GetById(LanguageId);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");
            ProductTranslation productTranslation = _productService.GetById(ProductId).ProductTranslations.FirstOrDefault(x => x.LanguageId == LanguageId);
            if (productTranslation == null)
                return Redirect("/pages/404");
            ProductTranslationViewModel model = new ProductTranslationViewModel()
            {
                LanguageId = LanguageId,
                ProductId = ProductId,
                Alias_SEO = productTranslation.Alias_SEO,
                Content = productTranslation.Content,
                Description = productTranslation.Description,
                Name = productTranslation.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditTranslation(ProductTranslationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.EditTranslation(new ProductTranslation()
                    {
                        ProductId = model.ProductId,
                        Content = model.Content,
                        Description = model.Description,
                        Alias_SEO = model.Alias_SEO,
                        LanguageId = model.LanguageId,
                        Name = model.Name
                    });
                    return RedirectToAction("Details", new { id = model.ProductId, languageSelected = model.LanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }
        #endregion
    }
}