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

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #region CRUD
        public ActionResult Index(int? level)
        {
            level = level ?? 1;
            switch(level)
            {
                case 1: return View(_categoryService.GetParents());
                case 2:
                    {
                        return View(_categoryService.GetAll().Where(p => p.ParentId != null).OrderBy(p => p.ParentId).ThenBy(c => c.OrderNum).ToList());
                    }
            }
            return View(_categoryService.GetParents());
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
            if (!_categoryService.CheckExistName(category.Name))
            {
                ModelState.AddModelError("", "Tên phân loại bị trùng. Vui lòng chọn tên khác.");
            }
            if (!_categoryService.CheckExistName(category.Alias_SEO))
            {
                ModelState.AddModelError("", "Bí danh bị trùng. Vui lòng chọn bí danh khác.");
            }
            if (category.ParentId == 0)
                category.ParentId = null;
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryService.Add(category);
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
            if(OldName != category.Name)
            {
                if (!_categoryService.CheckExistName(category.Name))
                {
                    ModelState.AddModelError("", "Tên phân loại bị trùng. Vui lòng chọn tên khác.");
                }
            }
            if (OldAlias != category.Alias_SEO)
            {
                if (!_categoryService.CheckExistName(category.Alias_SEO))
                {
                    ModelState.AddModelError("", "Bí danh bị trùng. Vui lòng chọn bí danh khác.");
                }
            }
            if (category.ParentId == 0)
                category.ParentId = null;
            if (ModelState.IsValid)
            {
                _categoryService.Update(category);
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
            try
            {
                _categoryService.Delete(category);
                return Json(new { status = 1, message = "Xoá thành công." });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            
            return Json(new { status = 0, message = Functions.GetAllErrorsPage(ModelState) });
        }
        #endregion

    }
}
