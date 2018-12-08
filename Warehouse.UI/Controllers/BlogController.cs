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
        public ActionResult Index(int? page)
        {
            List<ListBlogViewModel> listBlogViewModel = _blogService.GetListByDisplay(true)
                .OrderByDescending(b => b.Id).Select(b => new ListBlogViewModel()
                {
                    Title = b.Title,
                    Alias = b.Alias,
                    Description = b.Description,
                    Image = b.Image,
                    DateCreated = b.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(b.DateCreated.Value) : ""
                }).ToList();
            return View(listBlogViewModel.ToPagedList(page ?? 1, 9));
        }

        [Route("{alias}.html")]
        public ActionResult Details(string alias)
        {
            Blog blog = _blogService.GetByAlias(alias);
            if (blog == null || blog.Display == false)
                return Redirect("/pages/404");

            DetailsBlogViewModel detailsBlogViewModel = new DetailsBlogViewModel()
            {
                Id = blog.Id,
                Content = blog.Content,
                Alias = blog.Alias,
                LikeCount = blog.LikeCount,
                DateCreated = blog.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(blog.DateCreated.Value) : "",
                ViewCount = blog.ViewCount,
                Title = blog.Title
            };
            if(Session["read-post-" + blog.Id] == null)
            {
                Session["read-post-" + blog.Id] = true;
                blog.LikeCount++;
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