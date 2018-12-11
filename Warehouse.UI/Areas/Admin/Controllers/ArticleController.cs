using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

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
            if (_articleService.CheckUniqueTitle(Article.Title) == false)
            {
                ModelState.AddModelError("Title", "Tiêu đề bị trùng với bài viết khác. Vui lòng đặt lại.");
            }
            if (_articleService.CheckUniqueAlias(Article.Alias) == false)
            {
                ModelState.AddModelError("Alias", "Bí danh bị trùng với bài viết khác. Vui lòng đặt lại.");
            }
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
        public ActionResult Edit(Article Article, string OldTitle, string OldAlias)
        {
            if (OldTitle != Article.Title)
            {
                if (_articleService.CheckUniqueTitle(Article.Title) == false)
                {
                    ModelState.AddModelError("Title", "Tiêu đề bị trùng với bài viết khác. Vui lòng đặt lại.");
                }
            }
            if (OldAlias != Article.Alias)
            {
                if (_articleService.CheckUniqueAlias(Article.Alias) == false)
                {
                    ModelState.AddModelError("Alias", "Bí danh bị trùng với bài viết khác. Vui lòng đặt lại.");
                }
            }
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
    }
}