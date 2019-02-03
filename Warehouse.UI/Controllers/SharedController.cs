using DevTrends.MvcDonutCaching;
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
        private readonly IArticleService _articleService;
        private readonly ILanguageService _languageService;

        public SharedController(ICategoryService categoryService, IArticleService articleService, ILanguageService languageService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
            _languageService = languageService;
        }

        [ChildActionOnly]
        public PartialViewResult _HeaderPartial()
        {
            return PartialView(Session["InfoShop"]);  // truyền thêm Session lưu thông tin của shop
        }

        [ChildActionOnly]
        public PartialViewResult _FooterPartial()
        {
            ViewBag.Categories = _categoryService.GetParents();
            return PartialView(Session["InfoShop"]);
        }

       // [ChildActionOnly]
        [DonutOutputCache(Duration = 86400, Location = System.Web.UI.OutputCacheLocation.Server)]
        public PartialViewResult _LanguagePartial()
        {
            var language = _languageService.GetById(Request.Cookies["lang"].Value);
            ViewBag.Language = language;
            return PartialView(_languageService.GetAll().OrderBy(x => x.SortOrder).ToList());
        }

        [HttpPost]
        public ContentResult ChangeLanguage()
        {
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems();
            return Content("success");
        }

        [DonutOutputCache(Duration = 86400 * 30, Location = System.Web.UI.OutputCacheLocation.Server)]
        public PartialViewResult _HeaderMenuPartial()
        {
            // Lấy danh sách danh mục để hiện ra menu. Danh sách category được sắp xếp tăng dần theo cột OrderNum
            ViewBag.Categories = _categoryService.GetAll().OrderBy(p => p.OrderNum).ToList();
            ViewBag.Articles = _articleService.GetListByDisplay(true).OrderBy(a => a.OrderNum).ToList();
            return PartialView();
        }
    }
}