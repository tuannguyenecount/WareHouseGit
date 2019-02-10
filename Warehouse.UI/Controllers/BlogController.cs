using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using Warehouse.Services.Interface;
using PagedList.Mvc;
using PagedList;
using Warehouse.Entities;
using DevTrends.MvcDonutCaching;

namespace Warehouse.Controllers
{
    [RoutePrefix("blog")]
    public class BlogController : Controller
    {
        readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Route("")]
        [DonutOutputCache(Duration = 86400, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "page")]
        public ActionResult Index(int? page)
        {
            string languageId = Request.Cookies["lang"].Value;
            List<ListBlogViewModel> listBlogViewModel = _blogService.GetListByDisplay(true)
                .Where(x => (languageId != "vi" && x.BlogTranslations.FirstOrDefault(y => y.LanguageId == languageId) != null) || (languageId == "vi"))
                .OrderByDescending(b => b.Id).Select(x => new ListBlogViewModel()
                {
                    Id = x.Id,
                    Title = languageId == "vi" ? x.Title : (x.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Title),
                    Alias = languageId == "vi" ? x.Alias : (x.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Alias),
                    Description = languageId == "vi" ? x.Description : (x.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Description),
                    Image = x.Image,
                    DateCreated = x.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(x.DateCreated.Value) : ""
                }).ToList();
            return View(listBlogViewModel.ToPagedList(page ?? 1, 9));
        }

        [Route("{alias}-{id}.html")]
        public ActionResult Details(int id, string alias)
        {
            string languageId = Request.Cookies["lang"].Value;
            Blog blog = _blogService.GetById(id);
            if (blog == null || blog.Display == false)
                return Redirect("/pages/404");

            DetailsBlogViewModel detailsBlogViewModel = new DetailsBlogViewModel()
            {
                Id = blog.Id,
                Content = languageId == "vi" ? blog.Content : (blog.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Content),
                Alias = languageId == "vi" ? blog.Alias : (blog.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Alias),
                LikeCount = blog.LikeCount,
                DateCreated = blog.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(blog.DateCreated.Value) : "",
                ViewCount = blog.ViewCount,
                Title = languageId == "vi" ? blog.Title : (blog.BlogTranslations?.FirstOrDefault(y => y.LanguageId == languageId)?.Title)
            };
            if(Session["read-post-" + blog.Id] == null)
            {
                Session["read-post-" + blog.Id] = true;
                blog.ViewCount++;
                _blogService.Update(blog);
            }
            return View(detailsBlogViewModel);
        }

        [Route("thich-bai-viet/{Id}")]
        public JsonResult LikeArticle(int Id)
        {
            try
            {
                Blog blog = _blogService.GetById(Id);
                if (blog != null)
                {
                    if (Session["like-post-" + blog.Id] == null)
                    {
                        Session["like-post-" + blog.Id] = true;
                        blog.LikeCount++;
                        _blogService.Update(blog);
                    }
                }
                return Json(new { status = 1, newLikeCount = blog.LikeCount,  message = "Thích bài viết thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = 0, message = "Thích bài viết thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}