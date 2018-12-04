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
    public class BlogController : Controller
    {
        IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #region CRUD
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

        public ViewResult Create()
        {
            return View(new Blog());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog, string base64String)
        {
            blog.UserId = User.Identity.GetUserId();
            blog.DateCreated = DateTime.Now;
            if (_blogService.CheckUniqueTitle(blog.Title) == false)
            {
                ModelState.AddModelError("Title", "Tiêu đề bị trùng với bài viết khác. Vui lòng đặt lại.");
            }
            if (_blogService.CheckUniqueAlias(blog.Alias) == false)
            {
                ModelState.AddModelError("Alias", "Bí danh bị trùng với bài viết khác. Vui lòng đặt lại.");
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
                    string newAvatar = blog.Alias + ".jpg";
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
                    _blogService.Add(blog);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
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
        [ValidateInput(false)]
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
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            Blog blog = _blogService.GetById(Id);
            if (blog != null)
                _blogService.Delete(Id);
            return RedirectToAction("Index");
        }

        #endregion

        #region Change Image 
        [HttpPost]
        public ActionResult ChangeImage(int? id, string base64String)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _blogService.GetById(id.Value);
            if (blog == null)
            {
                ModelState.AddModelError("", "Blog này không tồn tại!");
            }
            else
            {
                #region Save File From String Base64
                if (!string.IsNullOrEmpty(base64String))
                {
                    try
                    {
                        base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                        string newImage = blog.Alias + DateTime.Now.Ticks.ToString() + ".jpg";
                        Functions.SaveFileFromBase64(Server.MapPath("~/Photos/Blogs/" + newImage), base64String);
                        blog.Image = newImage;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu hình từ chuỗi base64 " + ex.Message);
                    }
                    try
                    {
                        _blogService.Update(blog);
                        return RedirectToAction("Edit", new { id = blog.Id });
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }

                }
                #endregion
                else
                {
                    ModelState.AddModelError("", "Bạn chưa chọn hình muốn đổi!");
                }
            }

            return View("Edit", blog);
        }
        #endregion
        
        #region Count 
        [ChildActionOnly]
        public ContentResult CountDisplay()
        {
            return Content(_blogService.CountDisplay().ToString());
        }

        [ChildActionOnly]
        public ContentResult CountHide()
        {
            return Content(_blogService.CountHide().ToString());
        }
        #endregion

    }
}