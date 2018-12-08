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
    [RoutePrefix("san-pham-yeu-thich")]
    [Authorize]
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
        [Route("xem-danh-sach")]
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            List<ListFavoriteProductViewModel> listFavoriteViewModel = _ifavoriteProductService.GetAll(userid)
                .OrderByDescending(b => b.FavoriteDate).Select(b => new ListFavoriteProductViewModel()
                {
                    ProductId = b.ProductId,
                    AspNetUserId = userid,
                    Price = b.Product.PriceNew ?? b.Product.Price,
                    FavoriteDate = b.FavoriteDate,
                    Alias_SEO = b.Product.Alias_SEO,
                    Name = b.Product.Name,  // error ladyloading
                    Image = b.Product.Image // error ladyloading
                }).ToList();
            return View(listFavoriteViewModel);
        }

        [HttpPost]
        public JsonResult Create(int id)
        {
            var userid = User.Identity.GetUserId();
         
            var favorite = new FavoriteProduct();
            favorite.ProductId = id;
            favorite.AspNetUserId = userid;
            favorite.FavoriteDate = DateTime.Now;

            var validate = _ifavoriteProductService.Get(userid, id);
            if (validate == null) 
            { 
                try
                {
                    _ifavoriteProductService.Add(favorite);
                }
                catch (Exception)
                {
                    return Json(new { status = 2, message = "Có lỗi" });
                }
            }
            return Json(new { status = 1, message = "Thêm thành công" });
        }

        [Route("remove/{ProductId}")]
        public ActionResult RemoveFavorite(int ProductId)
        {
            FavoriteProduct favoriteProduct = _ifavoriteProductService.Get(User.Identity.GetUserId(), ProductId);
            if(favoriteProduct != null) {
                _ifavoriteProductService.Delete(favoriteProduct);
            }
            return RedirectToAction("Index");
        }
    }
}