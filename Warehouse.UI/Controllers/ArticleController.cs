﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Models;
namespace Warehouse.Controllers
{
    [RoutePrefix("thong-tin")]
    public class ArticleController : Controller
    {
        readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [Route("{alias}-{id}.html")]
        public ActionResult Details(int id, string alias)
        {
            Article article = _articleService.GetById(id);

            if (article == null)
                return Redirect("/pages/404");

            ArticleDetailsViewModel articleDetailsViewModel = new ArticleDetailsViewModel()
            {
                Content = article.Content,
                Title = article.Title
            };

            return View(articleDetailsViewModel);
        }


    }
}