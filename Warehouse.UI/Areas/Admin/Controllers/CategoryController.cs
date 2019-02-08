using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using DevTrends.MvcDonutCaching;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;
        private ILanguageService _languageService;

        public CategoryController(ICategoryService categoryService, ILanguageService languageService)
        {
            _categoryService = categoryService;
            _languageService = languageService;
        }

        #region CRUD
        public ActionResult Index()
        {
            return View(_categoryService.GetParents());
        }

        public ActionResult Details(int Id)
        {
            Category category = _categoryService.GetById(Id);
            if (category == null)
            {
                return Redirect("/pages/404");
            }
            Dictionary<string, string> languages = new Dictionary<string, string>();
            _languageService.GetAll().Where(x => x.Id != "vi")
                            .OrderBy(x => x.SortOrder).ToList().ForEach(x => languages.Add(x.Id, x.Name));
            ViewBag.Languages = languages;
            return View(category);
        }

        public ActionResult _CreateModal()
        {
            var categories = _categoryService.GetAll().Select(c=>new { Id = c.Id, Name = c.Name }).ToList();
            categories.Add(new { Id = 0, Name = "Không có" });
            ViewBag.ParentId = new SelectList(categories.OrderBy(c=>c.Id).ToList(), "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Category category)
        {
            if (_categoryService.CheckExistName(category.Name))
            {
                category.Alias_SEO = Functions.UnicodeToKoDauAndGach(category.Name) + "-" + _categoryService.CountByName(category.Name);
            }
            else
            {
                category.Alias_SEO = Functions.UnicodeToKoDauAndGach(category.Name);
            }
            
            if (category.ParentId == 0)
                category.ParentId = null;
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryService.Add(category);
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems("Shared", "_HeaderMenuPartial");
                    return Json(new { status = 1, message = "Thêm thành công" });
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return Json(new { status = 0, message = Functions.GetAllErrorsPage(ModelState)});
        }

        public ActionResult _EditModal(int Id = 0)
        {
            Category category = _categoryService.GetById(Id);
            if (category == null)
            {
                return Content("<p>Dữ liệu không tồn tại trong hệ thống!</p>");
            }
            var categories = _categoryService.GetAll().Where(x => x.Id != Id).Select(c => new { Id = c.Id, Name = c.Name }).ToList();
            categories.Add(new { Id = 0, Name = "Không có" });
            ViewBag.ParentId = new SelectList(categories.OrderBy(c => c.Id).ToList(), "Id", "Name", category.ParentId);
            return PartialView(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(Category category,string OldName, string OldAlias)
        {
            category.Alias_SEO = Functions.UnicodeToKoDauAndGach(category.Name);
            if (OldName != category.Name)
            {
                if (_categoryService.CheckExistName(category.Name))
                {
                    category.Alias_SEO = Functions.UnicodeToKoDauAndGach(category.Name) + "-" + _categoryService.CountByName(category.Name);
                }
            }

            if (category.ParentId == 0)
                category.ParentId = null;

            if (ModelState.IsValid)
            {
                _categoryService.Update(category);
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems("Shared", "_HeaderMenuPartial");
                return Json(new { status = 1, message = "Sửa thành công" });
            }

            return Json(new { status = 0, message = Functions.GetAllErrorsPage(ModelState) });
        }

        public ActionResult _DeleteModal(int Id = 0)
        {
            Category category = _categoryService.GetById(Id);
            if (category == null)
            {
                return Content("<p>Dữ liệu không tồn tại trong hệ thống!</p>");
            }
            return PartialView(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int Id = 0)
        {
            Category category = _categoryService.GetById(Id);
            if (category == null)
            {
                ModelState.AddModelError("", "Dữ liệu không tồn tại trong hệ thống!");
            }
            else if(category.Category1 != null && category.Category1.Count > 0)
            {
                ModelState.AddModelError("", "Đang có phân loại con tham chiếu. Bạn hãy xóa phân loại con trước!");
            }
            else if(category.Products != null && category.Products.Count > 0)
            {
                ModelState.AddModelError("", "Phân loại này đang chứa sản phẩm. Không thể xóa bây giờ!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.Delete(category);
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems("Shared", "_HeaderMenuPartial");
                    return Json(new { status = 1, message = "Xoá thành công." });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            
            return Json(new { status = 0, message = Functions.GetAllErrorsPage(ModelState) });
        }

        #endregion

        #region Create Translation
        public ActionResult CreateTranslation(int Id, string countrySelect)
        {
            ViewBag.LanguageSelected = _languageService.GetById(countrySelect);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            return View(new CategoryTranslationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTranslation(CategoryTranslationViewModel model)
        {
            if (_categoryService.CheckExistName(model.Name))
            {
                model.Alias_SEO = Functions.UnicodeToKoDauAndGach(model.Name) + "-" + _categoryService.CountByName(model.Name);
            }
            else 
                model.Alias_SEO = Functions.UnicodeToKoDauAndGach(model.Name);

            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.CreateTranslation(new CategoryTranslation()
                    {
                        CategoryId = model.CategoryId,
                        Alias_SEO = model.Alias_SEO,
                        LanguageId = model.LanguageId,
                        Name = model.Name
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems("Shared", "_HeaderMenuPartial");
                    return RedirectToAction("Details", new { id = model.CategoryId, languageSelected = model.LanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.LanguageSelected = _languageService.GetById(model.LanguageId);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            return View(model);
        }
        #endregion

        #region Edit Translation
        public ActionResult EditTranslation(int CategoryId, string LanguageId)
        {
            ViewBag.LanguageSelected = _languageService.GetById(LanguageId);

            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            CategoryTranslation categoryTranslation = _categoryService.GetById(CategoryId).CategoryTranslations.FirstOrDefault(x => x.LanguageId == LanguageId);
            if (categoryTranslation == null)
                return Redirect("/pages/404");

            CategoryTranslationViewModel model = new CategoryTranslationViewModel()
            {
                LanguageId = LanguageId,
                CategoryId = CategoryId,
                Alias_SEO = categoryTranslation.Alias_SEO,
                Name = categoryTranslation.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditTranslation(CategoryTranslationViewModel model)
        {
            if (_categoryService.CheckExistName(model.Name))
            {
                model.Alias_SEO = Functions.UnicodeToKoDauAndGach(model.Name) + "-" + _categoryService.CountByName(model.Name);
            }
            else
                model.Alias_SEO = Functions.UnicodeToKoDauAndGach(model.Name);

            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.EditTranslation(new CategoryTranslation()
                    {
                        CategoryId = model.CategoryId,
                        Alias_SEO = model.Alias_SEO,
                        LanguageId = model.LanguageId,
                        Name = model.Name
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems("Shared", "_HeaderMenuPartial");
                    return RedirectToAction("Details", new { id = model.CategoryId, languageSelected = model.LanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.LanguageSelected = _languageService.GetById(model.LanguageId);

            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            CategoryTranslation categoryTranslation = _categoryService.GetById(model.CategoryId).CategoryTranslations.FirstOrDefault(x => x.LanguageId == model.LanguageId);
            if (categoryTranslation == null)
                return Redirect("/pages/404");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTranslation(int CategoryId, string LanguageId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.DeleteTranslation(CategoryId, LanguageId);
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems("Shared", "_HeaderMenuPartial");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Details", new { id = CategoryId });
        }
        #endregion
    }
}
