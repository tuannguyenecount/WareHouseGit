using DevTrends.MvcDonutCaching;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Models;
using Warehouse.Services.Interface;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticleController : Controller
    {
        IArticleService _articleService;
        ILanguageService _languageService;


        public ArticleController(IArticleService articleService, ILanguageService languageService)
        {
            _articleService = articleService;
            _languageService = languageService;
        }

        #region Get Alias 
        public ContentResult GetAlias(string Title)
        {
            return Content(Functions.UnicodeToKoDauAndGach(Title));
        }
        #endregion

        #region CRUD
        public ViewResult Index()
        {
            List<Article> Articles = _articleService.GetAll();
            return View(Articles);
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article Article = _articleService.GetById(Id.Value);
            if (Article == null)
                return Redirect("/pages/404");

            Dictionary<string, string> languages = new Dictionary<string, string>();
            _languageService.GetAll().Where(x => x.Id != "vi")
                            .OrderBy(x => x.SortOrder).ToList().ForEach(x => languages.Add(x.Id, x.Name));
            ViewBag.Languages = languages;

            return View(Article);
        }

        public ViewResult Create()
        {
            return View(new Article());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Article Article)
        {
            Article.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.Add(Article);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(Article);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article Article = _articleService.GetById(Id.Value);
            if (Article == null)
                return Redirect("/pages/404");
            return View(Article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Article Article)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.Update(Article);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                
            }
            return View(Article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            _articleService.Delete(Id);
            return RedirectToAction("Index");
        }

        #endregion

        #region Create Translation
        public ActionResult CreateTranslation(int Id, string countrySelect)
        {
            ViewBag.LanguageSelected = _languageService.GetById(countrySelect);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            return View(new ArticleTranslationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTranslation(ArticleTranslationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.CreateTranslation(new ArticleTranslation()
                    {
                        ArticleId = model.ArticleId,
                        Alias = model.Alias,
                        LanguageId = model.LanguageId,
                        Title = model.Title,
                        Content = model.Content
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return RedirectToAction("Details", new { id = model.ArticleId, languageSelected = model.LanguageId });
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
        public ActionResult EditTranslation(int ArticleId, string LanguageId)
        {
            ViewBag.LanguageSelected = _languageService.GetById(LanguageId);

            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            ArticleTranslation articleTranslation = _articleService.GetById(ArticleId).ArticleTranslations.FirstOrDefault(x => x.LanguageId == LanguageId);
            if (articleTranslation == null)
                return Redirect("/pages/404");

            ArticleTranslationViewModel model = new ArticleTranslationViewModel()
            {
                LanguageId = LanguageId,
                ArticleId = ArticleId,
                Alias = articleTranslation.Alias,
                Title = articleTranslation.Title,
                Content = articleTranslation.Content
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditTranslation(ArticleTranslationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.EditTranslation(new ArticleTranslation()
                    {
                        ArticleId = model.ArticleId,
                        Alias = model.Alias,
                        LanguageId = model.LanguageId,
                        Title = model.Title,
                        Content = model.Content
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return RedirectToAction("Details", new { id = model.ArticleId, languageSelected = model.LanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.LanguageSelected = _languageService.GetById(model.LanguageId);

            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            ArticleTranslation articleTranslation = _articleService.GetById(model.ArticleId).ArticleTranslations.FirstOrDefault(x => x.LanguageId == model.LanguageId);
            if (articleTranslation == null)
                return Redirect("/pages/404");

            return View(model);
        }
        #endregion

        #region Delete Translation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTranslation(int ArticleId, string LanguageId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.DeleteTranslation(ArticleId, LanguageId);
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Details", new { id = ArticleId });
        }
        #endregion
    }
}