using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Models;
namespace Warehouse.Controllers
{
    [RoutePrefix("bai-viet")]
    public class ArticleController : Controller
    {
        readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [Route("{alias}.html")]
        public ActionResult Details(string alias)
        {
            Article article = _articleService.GetByAlias(alias);

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