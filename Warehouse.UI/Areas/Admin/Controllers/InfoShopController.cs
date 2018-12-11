using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Models;
using Warehouse.Services.Interface;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InfoShopController : Controller
    {
        private IInfoShopService _infoShopService;

        readonly List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        public InfoShopController(IInfoShopService infoShopService)
        {
            _infoShopService = infoShopService;
        }
        #region CRUD
        public ActionResult Index()
        {
            return View(_infoShopService.GetList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoShop infoShop = _infoShopService.GetFirst();
            if (infoShop == null)
            {
                return Redirect("/pages/404");
            }
            return View(infoShop);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoShop infoShop = _infoShopService.GetFirst();
            if (infoShop == null)
            {
                return Redirect("/pages/404");
            }
            return View(infoShop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(InfoShop infoShop, HttpPostedFileBase file)
        {
            if(file != null && file.ContentLength > 0)
            {
                infoShop.Logo =  DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(file.FileName);
                string fileName = Server.MapPath("~/images/" + infoShop.Logo);
                Functions.UpLoadImage(fileName, file, this.ModelState, "Logo");
            }
            if (ModelState.IsValid)
            {
                _infoShopService.Update(infoShop);
                Session["InfoShop"] = infoShop;
                return RedirectToAction("Index");
            }
            return View(infoShop);
        }
        #endregion
    }
}
