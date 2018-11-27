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

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SlideController : Controller
    {
        private ISlideService _slideService;
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        public SlideController(ISlideService slideService)
        {
            _slideService = slideService;
        }

        public ActionResult Index()
        {
            return View(_slideService.GetAll());
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
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Xảy ra lỗi " + ex.Message + " khi xóa slide này!");
                }
            }
            return View("Index", _slideService.GetAll());
        }

    }
}
