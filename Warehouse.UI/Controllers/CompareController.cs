using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Models;
using Warehouse.Services.Interface;

namespace Warehouse.Controllers
{
    public class CompareController : Controller
    {
        IProductService _productService;

        public CompareController(IProductService productService)
        {
            _productService = productService;
        }

        public List<DetailsProductViewModel> CompareItem
        {
            get
            {
                return (Session["CompareItem"] as List<DetailsProductViewModel>) ?? new List<DetailsProductViewModel>();
            }
        }

        /// <summary>
        /// GET: Compare
        /// </summary>
        /// <returns></returns>
        [Route("so-sanh-san-pham.html")]
        public ActionResult Index()
        {
            return View(CompareItem);
        }

        /// <summary>
        /// Add product to compare
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(int id)
        {
            Product product = _productService.GetById(id);

            if (product == null)
            {
                return Json(new { status = 2, message = "Không tìm thấy sản phẩm!" });
            }
            else
            {
                var check_exist = CompareItem.Where(n => n.Id == product.Id).Count();
                if (check_exist > 0)
                {
                    return Json(new { status = 3, message = "Sản phẩm đã tồn tại!" });
                }
            }

            DetailsProductViewModel compareItem = new DetailsProductViewModel()
            {
                Price = product.PriceNew ?? product.Price,
                Id = id,
                Name = product.Name,
                Image = product.Image,
                Alias = product.Alias_SEO,
            };
            try
            {
                CompareItem.Add(compareItem);
            }
            catch (Exception)
            {
                return Json(new { status = 2, message = "Có lỗi!" });
            }

            return Json(new { status = 1, message = "Thêm thành công." });
        }

        /// <summary>
        ///  Delete item in compare
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            DetailsProductViewModel item = CompareItem.SingleOrDefault(m => m.Id == id);
            if (item != null)
            {
                CompareItem.Remove(item);
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            CompareItem.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}