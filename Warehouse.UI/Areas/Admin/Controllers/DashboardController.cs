using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Mod")]
    public class DashboardController : Controller
    {
        // GET: Admin/Home
        public string Index()
        {
            return "Home page admin";
        }
    }
}