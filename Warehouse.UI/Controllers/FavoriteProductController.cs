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

namespace Warehouse.Controllers
{
    [RoutePrefix("favorite")]
    public class FavoriteProductController : Controller
    {
        readonly IFavoriteProductService _ifavoriteProductService;

        public FavoriteProductController(IFavoriteProductService ifavoriteProductService)
        {
            _ifavoriteProductService = ifavoriteProductService;
        }

        [Route("")]
        public ActionResult Index()
        {
            List<ListFavoriteProductViewModel> listBlogViewModel = _ifavoriteProductService.GetAll()
                .OrderByDescending(b => b.FavoriteDate).Select(b => new ListFavoriteProductViewModel()
                {
                    ProductId = b.ProductId,
                    AspNetUserId = b.AspNetUserId,
                    FavoriteDate = b.FavoriteDate
                }).ToList();
            return View(listBlogViewModel);
        }

    }
}