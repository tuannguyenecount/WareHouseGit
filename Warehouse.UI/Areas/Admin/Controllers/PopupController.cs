using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKhachSan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PopupController : Controller
    {
     
        //public ActionResult Show()
        //{
        //    Popup popup = db.Popups.FirstOrDefault();
        //    if(popup == null)
        //    {
        //        return Content("");
        //    }
        //    else if(popup.Status == false)
        //    {
        //        return Content("");
        //    }
        //    else
        //    {
        //        if(Session["showPopup"] == null)
        //        {
        //            Session["showPopup"] = true;
        //            return PartialView(popup);
        //        }
        //        else
        //        {
        //            return Content("");
        //        }
        //    }
        //}
        //// GET: Popup
        //[Authorize(Roles = "Admin")]
        //public ActionResult Edit()
        //{
        //    Popup popup = db.Popups.FirstOrDefault();
        //    if(popup == null)
        //    {
        //        return Redirect("/pages/404");
        //    }
        //    return View(popup);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //[ValidateInput(false)]
        //public ActionResult Edit(Popup popup)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        db.Entry(popup).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Edit");
        //    }
        //    return View(popup);
        //}

    }
}