using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using Warehouse.Services.Interface;
using PagedList.Mvc;
using PagedList;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net;
using Microsoft.AspNet.Identity.Owin;

namespace Warehouse.Controllers
{
    [RoutePrefix("favorite")]
    public class FavoriteProductController : Controller
    {
        readonly IFavoriteProductService _ifavoriteProductService;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public FavoriteProductController(IFavoriteProductService ifavoriteProductService)
        {
            _ifavoriteProductService = ifavoriteProductService;
        }

        [Route("")]
        public ActionResult Index()
        {
            var userid = "";
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                userid = user.Id;
            }
            else
            {
                RedirectToAction("Login", "Account");
            }



            List<ListFavoriteProductViewModel> listFavoriteViewModel = _ifavoriteProductService.GetAll().Where(n => n.AspNetUserId == userid)
                .OrderByDescending(b => b.FavoriteDate).Select(b => new ListFavoriteProductViewModel()
                {
                    ProductId = b.ProductId,
                    AspNetUserId = userid,
                    FavoriteDate = b.FavoriteDate,
                    //Name = b.Product.Name,
                    //Image = b.Product.Image
                }).ToList();
            return View(listFavoriteViewModel);
        }

    }
}