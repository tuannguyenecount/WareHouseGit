using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WareHouse.Models;
namespace WareHouse.Areas.Admin.Controllers
{
    [Authorize(Roles= "Admin")]
    public class SubscriberController : Controller
    {
        public FileResult Download()
        {
            hotellte_warehouseEntities db = new hotellte_warehouseEntities();
            string filename = Server.MapPath("~/Files/" + "dsnhantin.txt");
            using (StreamWriter w = new StreamWriter(filename))
            {
                foreach (string email in db.MailBoxes.Select(m => m.Email).ToList())
                {
                    w.Write(email + " ");
                }
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(filename);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = "dsnhantin.txt";
            return response;
        }
    }
}