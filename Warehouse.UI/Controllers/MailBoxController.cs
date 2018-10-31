using Warehouse.Models;
using System;
using System.Web.Mvc;
using Warehouse.Entities;

namespace Warehouse.Controllers
{
    public class MailBoxController : Controller
    {
        [Route("lien-he")]
        public ActionResult Add()
        {
            //return View(new MailBox());
            return View();
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
                //hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();
                //db.MailBoxes.Add(model);
                //db.SaveChanges();
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