using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
using System.Configuration;

namespace WareHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SlideController : Controller
    {
        private hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        // GET: Admin/Slide
        public async Task<ActionResult> Index()
        {
            return View(await db.Slides.ToListAsync());
        }


        // GET: Admin/Slide/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Order,Title,Status")] Slide slide, HttpPostedFileBase file)
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
                db.Slides.Add(slide);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(slide);
        }

        // GET: Admin/Slide/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = await db.Slides.FindAsync(id);
            if (slide == null)
            {
                return Redirect("/pages/404");
            }
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Image,Order,Title,Status")] Slide slide, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                slide.Image = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileName = Server.MapPath("~/images/" + slide.Image);
                Functions.UpLoadImage(fileName, file, this.ModelState, "Image");
            }

            if (ModelState.IsValid)
            {
                db.Entry(slide).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(slide);
        }

       
        // POST: Admin/Slide/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Slide slide = await db.Slides.FindAsync(id);
            if(slide == null)
            {
                ModelState.AddModelError("", "Slide không tồn tại!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Slides.Remove(slide);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Xảy ra lỗi " + ex.Message + " khi xóa slide này!");
                }
            }
            return View("Index",await db.Slides.ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
