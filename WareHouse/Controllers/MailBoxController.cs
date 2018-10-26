using WareHouse.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WareHouse.Controllers
{
    public class MailBoxController : Controller
    {
        [Route("lien-he")]
        public ActionResult Add()
        {
            return View(new MailBox());
        }
        // GET: MailBox
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("lien-he")]
        public ActionResult Add(MailBox model, FormCollection form)
        {

            model.DateSend = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Xảy ra lỗi. Bạn vui lòng thử lại sau.");
                return View(model);
            }
            try
            {
                hotellte_warehouseEntities db = new hotellte_warehouseEntities();
                db.MailBoxes.Add(model);
                db.SaveChanges();
                ViewBag.Message = "Gửi thành công.";
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "Xảy ra lỗi. Bạn vui lòng thử lại sau.");
                return View(model);
            }
        }
    }
}