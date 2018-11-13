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
using Warehouse.Data.Data;
using Warehouse.Services.Interface;
using Warehouse.Entities;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Warehouse.Mod.Admin.Controllers
{
    [Authorize(Roles="Admin, Mod")]
    public class ProductController : Controller
    {
        #region Private Propertys
        int WidthResize, HeightResize;
        private IProductService _productService;
        private ICategoryService _categoryService;
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            this.WidthResize = int.Parse(ConfigurationManager.AppSettings["WidthImageProduct"]);
            this.HeightResize = int.Parse(ConfigurationManager.AppSettings["HeightImageProduct"]);

        }
        #endregion

        #region Constructor

        #endregion

        #region Export Data

        [NonAction]
        void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {

                    //  Create a form to contain the List
                    Table table = new Table();
                    TableRow row = new TableRow();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                    foreach (PropertyDescriptor prop in props)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = prop.Name;
                        hcell.BackColor = System.Drawing.Color.Yellow;
                        row.Cells.Add(hcell);
                    }

                    table.Rows.Add(row);

                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            row.Cells.Add(cell);
                        }
                        table.Rows.Add(row);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    output.Write(sw.ToString());
                }
            }

        }


        public ActionResult ExportToExcel()
        {
            var data = _productService.GetByUser(User.Identity.Name);
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=sanpham.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteHtmlTable(data, Response.Output);
            Response.End();
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion

        #region CRUD
        public ActionResult Index(int? page)
        {
            ViewBag.Message = "Sản phẩm đã đăng";
            List<Product> products = _productService.GetByUser(User.Identity.Name);
            return View(products);
        }

        public ActionResult Details(int? id, int? page)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            if(product == null || product.UserCreated != User.Identity.Name)
            {
                return Redirect("/pages/404");
            }
            ViewBag.Message = "Chi tiết sản phẩm";
           
            
            return View(product);
        }

        public ViewResult Create()
        {
            ViewBag.CategoryID = new SelectList(_categoryService.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Exclude = "Image")] Product model, IEnumerable<HttpPostedFileBase> ImagesProducts, string base64String)
        {
            model.DateCreated = DateTime.Now;
            model.UserCreated = User.Identity.Name;
            model.DateUpdated = null;
            #region Save File From String Base64
            if (!string.IsNullOrEmpty(base64String))
            {
                try
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    string newAvatar = model.Alias_SEO + DateTime.Now.Ticks.ToString() + ".jpg";
                    Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Products/" + newAvatar), base64String);
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
                ModelState.AddModelError("CustomError", raise.Message);
                ViewBag.CategoryID = new SelectList(_categoryService.GetAll(), "Id", "Name", model.CategoryId);
                return View(model);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("CustomError", "Xảy ra lỗi khi thêm sản phẩm. Chi tiết: " + ex.Message);
            }
            ViewBag.CategoryID = new SelectList(_categoryService.GetAll(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            if (product == null || product.UserCreated != User.Identity.Name)
            {
                return Redirect("/pages/404");
            }
            ViewBag.CategoryId = new SelectList(_categoryService.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product, string returnURL, string OldName, string OldAlias, string base64String)
        {
            product.UserUpdated = User.Identity.Name;
            if(OldName != product.Name)
            {
               
            }
            if (OldAlias != product.Alias_SEO)
            {
               
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
                return Redirect(returnURL);
            }
            ViewBag.CategoryId = new SelectList(_categoryService.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
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
            if(product == null || product.UserCreated != User.Identity.Name)
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
            if(product == null || product.UserCreated != User.Identity.Name) {
                return Redirect("/pages/404");
            }
            product.Display = true;
            _productService.Update(product);
            return RedirectToAction("Index");
        }
        #endregion

        #region Change Image Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeImage(int? id, string base64String)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.GetById(id.Value);
            if (product == null || product.UserCreated != User.Identity.Name)
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
                        Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Products/" + newImage), base64String);
                        product.Image = newImage;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("LoiDoiImage", "Lỗi khi lưu hình từ chuỗi base64 " + ex.Message);
                    }
                    try
                    {
                        _productService.Update(product);
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

       
    }
}