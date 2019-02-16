using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using System.Configuration;
using Warehouse.Services.Interface;
using Warehouse.Entities;
using DevTrends.MvcDonutCaching;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SlideController : Controller
    {
        private ISlideService _slideService;
        private ILanguageService _languageService;

        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        public SlideController(ISlideService slideService, ILanguageService languageService)
        {
            _slideService = slideService;
            _languageService = languageService;
        }

        #region CRUD
        public ActionResult Index()
        {
            return View(_slideService.GetAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = _slideService.GetById(id.Value);
            if (slide == null)
            {
                return Redirect("/pages/404");
            }
            Dictionary<string, string> languages = new Dictionary<string, string>();

            _languageService.GetAll().Where(x => x.Id != "vi")
                            .OrderBy(x => x.SortOrder).ToList().ForEach(x => languages.Add(x.Id, x.Name));
            ViewBag.Languages = languages;
            return View(slide);
        }

        // GET: Admin/Slide/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slide slide, HttpPostedFileBase file)
        {
            if(file != null && file.ContentLength > 0)
            {
                slide.Image = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileName = Server.MapPath("~/images/" + slide.Image);
                Functions.UpLoadImage(fileName, file, this.ModelState, "Image");
            }
            else
            {
                ModelState.AddModelError("Image", "Bạn chưa chọn hình upload!");
            }
            if (ModelState.IsValid)
            {
                _slideService.Add(slide);
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems();
                return RedirectToAction("Index");
            }

            return View(slide);
        }

        // GET: Admin/Slide/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = _slideService.GetById(id.Value);
            if (slide == null)
            {
                return Redirect("/pages/404");
            }
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slide slide, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                slide.Image = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileName = Server.MapPath("~/images/" + slide.Image);
                Functions.UpLoadImage(fileName, file, this.ModelState, "Image");
            }

            if (ModelState.IsValid)
            {
                _slideService.Update(slide);
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems();
                return RedirectToAction("Index");
            }
            return View(slide);
        }

       
        // POST: Admin/Slide/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slide slide = _slideService.GetById(id);
            if(slide == null)
            {
                ModelState.AddModelError("", "Slide không tồn tại!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _slideService.Delete(id);
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Xảy ra lỗi " + ex.Message + " khi xóa slide này!");
                }
            }
            return View("Index", _slideService.GetAll());
        }
        #endregion

        #region Create Translation
        public ActionResult CreateTranslation(int Id, string countrySelect)
        {
            ViewBag.LanguageSelected = _languageService.GetById(countrySelect);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            return View(new SlideTranslationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTranslation(SlideTranslationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _slideService.CreateTranslation(new SlideTranslation()
                    {
                        SlideId = model.SlideId,
                        LanguageId = model.LanguageId,
                        Title = model.Title
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return RedirectToAction("Details", new { id = model.SlideId, languageSelected = model.LanguageId });
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
        public ActionResult EditTranslation(int SlideId, string LanguageId)
        {
            ViewBag.LanguageSelected = _languageService.GetById(LanguageId);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");
            SlideTranslation slideTranslation = _slideService.GetById(SlideId).SlideTranslations.FirstOrDefault(x => x.LanguageId == LanguageId);
            if (slideTranslation == null)
                return Redirect("/pages/404");
            SlideTranslationViewModel model = new SlideTranslationViewModel()
            {
                LanguageId = LanguageId,
                SlideId = SlideId,
                Title = slideTranslation.Title
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTranslation(SlideTranslationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _slideService.EditTranslation(new SlideTranslation()
                    {
                        SlideId = model.SlideId,
                        LanguageId = model.LanguageId,
                        Title = model.Title,
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return RedirectToAction("Details", new { id = model.SlideId, languageSelected = model.LanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region Delete Translation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTranslation(int SlideId, string LanguageId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _slideService.DeleteTranslation(SlideId, LanguageId);
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Details", new { id = SlideId });
        }
        #endregion
    }
}
