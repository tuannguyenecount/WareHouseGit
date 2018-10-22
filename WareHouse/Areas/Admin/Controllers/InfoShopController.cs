using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;

namespace WareHouse.Areas.Admin.Controllers
{
    public class InfoShopController : Controller
    {
        private hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        readonly List<string> ImageExtensions = new List<string> { ".JPG", ".PNG", ".JPEG" };

        // GET: Admin/InfoShop
        public ActionResult Index()
        {
            return View(db.InfoShops.ToList());
        }

        // GET: Admin/InfoShop/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoShop infoShop = db.InfoShops.Find(id);
            if (infoShop == null)
            {
                return Redirect("/pages/404");
            }
            return View(infoShop);
        }

       

        // GET: Admin/InfoShop/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoShop infoShop = db.InfoShops.Find(id);
            if (infoShop == null)
            {
                return Redirect("/pages/404");
            }
            return View(infoShop);
        }

        // POST: Admin/InfoShop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Logo,ShopName,Phone,Zalo,Email,Address,Fanpage,Introduce_Shop,Contact_Info,Google_Map,TextFooter,GoogleAnalytics,SalesPolicy,ShoppingGuide")] InfoShop infoShop, HttpPostedFileBase file)
        {
            if(file != null && file.ContentLength > 0)
            {
                infoShop.Logo =  DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileName = Server.MapPath("~/images/" + infoShop.Logo);
                Functions.UpLoadImage(fileName, file, this.ModelState, "Logo");
            }
            if (ModelState.IsValid)
            {
                db.Entry(infoShop).State = EntityState.Modified;
                db.SaveChanges();
                Session["InfoShop"] = infoShop;
                return RedirectToAction("Index");
            }
            return View(infoShop);
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
