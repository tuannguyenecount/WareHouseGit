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
    public class BlogController : Controller
    {
        IBlogService _blogService;
        ILanguageService _languageService;

        public BlogController(IBlogService blogService, ILanguageService languageService)
        {
            _blogService = blogService;
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

            Dictionary<string, string> languages = new Dictionary<string, string>();
            _languageService.GetAll().Where(x => x.Id != "vi")
                            .OrderBy(x => x.SortOrder).ToList().ForEach(x => languages.Add(x.Id, x.Name));
            ViewBag.Languages = languages;

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
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
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
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
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
            _blogService.Delete(Id);
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems();
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
                        var cacheManager = new OutputCacheManager();
                        cacheManager.RemoveItems();
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
        [HttpPost]
        public ContentResult CountDisplay()
        {
            return Content(_blogService.CountDisplay().ToString());
        }
        [HttpPost]
        public ContentResult CountHide()
        {
            return Content(_blogService.CountHide().ToString());
        }
        #endregion

        #region Create Translation
        public ActionResult CreateTranslation(int Id, string countrySelect)
        {
            ViewBag.LanguageSelected = _languageService.GetById(countrySelect);
            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            return View(new BlogTranslationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateTranslation(BlogTranslationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _blogService.CreateTranslation(new BlogTranslation()
                    {
                        BlogId = model.BlogId,
                        Alias = model.Alias,
                        LanguageId = model.LanguageId,
                        Title = model.Title,
                        Description = model.Description,
                        Content = model.Content
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return RedirectToAction("Details", new { id = model.BlogId, languageSelected = model.LanguageId });
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
        public ActionResult EditTranslation(int BlogId, string LanguageId)
        {
            ViewBag.LanguageSelected = _languageService.GetById(LanguageId);

            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            BlogTranslation blogTranslation = _blogService.GetById(BlogId).BlogTranslations.FirstOrDefault(x => x.LanguageId == LanguageId);
            if (blogTranslation == null)
                return Redirect("/pages/404");

            BlogTranslationViewModel model = new BlogTranslationViewModel()
            {
                LanguageId = LanguageId,
                BlogId = BlogId,
                Alias = blogTranslation.Alias,
                Title = blogTranslation.Title,
                Description = blogTranslation.Description,
                Content = blogTranslation.Content
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditTranslation(BlogTranslationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _blogService.EditTranslation(new BlogTranslation()
                    {
                        BlogId = model.BlogId,
                        Alias = model.Alias,
                        LanguageId = model.LanguageId,
                        Title = model.Title,
                        Content = model.Content,
                        Description = model.Description
                    });
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                    return RedirectToAction("Details", new { id = model.BlogId, languageSelected = model.LanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.LanguageSelected = _languageService.GetById(model.LanguageId);

            if (ViewBag.LanguageSelected == null)
                return Redirect("/pages/404");

            BlogTranslation blogTranslation = _blogService.GetById(model.BlogId).BlogTranslations.FirstOrDefault(x => x.LanguageId == model.LanguageId);
            if (blogTranslation == null)
                return Redirect("/pages/404");

            return View(model);
        }
        #endregion

        #region Delete Translation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTranslation(int BlogId, string LanguageId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _blogService.DeleteTranslation(BlogId, LanguageId);
                    var cacheManager = new OutputCacheManager();
                    cacheManager.RemoveItems();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Details", new { id = BlogId });
        }
        #endregion
    }
}