using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
namespace WareHouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        public void AnHien(bool an)
        {
                Session["An"] = an;
        }
    }
}