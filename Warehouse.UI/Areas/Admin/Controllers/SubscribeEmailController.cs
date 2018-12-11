using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubscribeEmailController : Controller
    {
        ISubscriberService _subscriberService;

        public SubscribeEmailController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        #region Export List
        public void ExportToTxt()
        {
            List<Subscriber> subscribers = _subscriberService.GetAll();
            string listEmail = "";
            subscribers.ForEach(s => listEmail += (s.Email + " "));
            string filename = Server.MapPath("~/Files/dsnhantin.txt");
            System.IO.File.Create(filename).Close();
            System.IO.File.WriteAllText(filename, listEmail);
            // Append headers
            Response.AppendHeader("content-disposition", "attachment; filename=dsnhantin.txt");
            // Open/Save dialog
            Response.ContentType = "application/octet-stream";
            // Push it!
            Response.TransmitFile(filename);
        }
        #endregion

        #region List
        public ActionResult Index()
        {
            return View(_subscriberService.GetAll());
        }
        #endregion
        [HttpPost]

        #region Delete
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string[] emails)
        {
            foreach(string email in emails)
            {
                _subscriberService.Delete(email);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}