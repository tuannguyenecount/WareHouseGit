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
    //[Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public ViewResult Index()
        {
            List<Blog> blogs = _blogService.GetAll();
            return View(blogs);
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _blogService.GetById(Id.Value);
            if (blog == null)
                return Redirect("/pages/404");
            return View(blog);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _blogService.GetById(Id.Value);
            if (blog == null)
                return Redirect("/pages/404");
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "UserId")] Blog blog, string OldTitle, string OldAlias, string base64String)
        {
            blog.UserId = User.Identity.GetUserId();
            if (OldTitle != blog.Title)
            {
                if (_blogService.CheckUniqueTitle(blog.Title) == false)
                {
                    ModelState.AddModelError("Name", "Tiêu đề bị trùng với bài viết khác. Vui lòng đặt lại.");
                }
            }
            if (OldAlias != blog.Alias)
            {
                if (_blogService.CheckUniqueAlias(blog.Alias) == false)
                {
                    ModelState.AddModelError("Alias", "Bí danh bị trùng với bài viết khác. Vui lòng đặt lại.");
                }
            }
            if (blog.ViewCount < 0)
            {
                ModelState.AddModelError("ViewCount", "Lượt xem phải >= 0.");
            }
            if (blog.LikeCount < 0)
            {
                ModelState.AddModelError("LikeCount", "Lượt thích phải >= 0.");
            }
            if (!string.IsNullOrEmpty(base64String))
            {
                try
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    string newAvatar = blog.Alias + DateTime.Now.Ticks.ToString() + ".jpg";
                    Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Blogs/" + newAvatar), base64String);
                    blog.Image = newAvatar;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _blogService.Update(blog);
                    return RedirectToAction("Details", new { Id = blog.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                
            }
            return View(blog);
        }
    }
}