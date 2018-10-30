using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        public void ChangeStatusToolBarAdmin(bool hide)
        {
           Session["HideToolBarAdmin"] = hide;
        }
    }
}