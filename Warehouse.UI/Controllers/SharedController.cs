using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Services.Interface;

namespace Warehouse.Controllers
{
    public class SharedController : Controller
    {
        private readonly ICategoryService _categoryService;

        public SharedController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public PartialViewResult _HeaderPartial()
        {
            // Lấy danh sách danh mục để hiện ra menu. Danh sách category được sắp xếp tăng dần theo cột OrderNum
            ViewBag.Categories = _categoryService.GetAll().OrderBy(p=>p.OrderNum).ToList();
            return PartialView(Session["InfoShop"]);  // truyền thêm Session lưu thông tin của shop
        }

        [ChildActionOnly]
        //[OutputCache(Duration = 3600)]
        public PartialViewResult _FooterPartial()
        {
            ViewBag.Categories = _categoryService.GetParents();
            return PartialView(Session["InfoShop"]);
        }
    }
}