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
using Warehouse.Entities;

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
                    //Name = b.Product.Name,  // error ladyloading
                    //Image = b.Product.Image // error ladyloading
                }).ToList();
            return View(listFavoriteViewModel);
        }

        [HttpPost]
        public JsonResult Create(int id)
        {
            var userid = "";
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                userid = user.Id;
            }
            else
            {
                return Json(new { status = 3, message = "bạn chưa đăng nhập" });
            }

            var favorite = new FavoriteProduct();
            favorite.ProductId = id;
            favorite.AspNetUserId = userid;
            favorite.FavoriteDate = DateTime.Now;

            var validate = _ifavoriteProductService.GetAll().Where(n => n.AspNetUserId == userid && n.ProductId == id).SingleOrDefault();
            if (validate != null)
                return Json(new { status = 4, message = "sản phẩm đã có trong danh sách yêu thích" });
            else
            {
                try
                {
                    _ifavoriteProductService.Add(favorite);
                    return Json(new { status = 1, message = "Thêm thành công" });
                }
                catch (Exception)
                {

                    return Json(new { status = 2, message = "có lỗi" });
                }
            }
        }
    }
}