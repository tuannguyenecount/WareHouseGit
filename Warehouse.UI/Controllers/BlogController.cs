using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using Warehouse.Services.Interface;
using PagedList.Mvc;
using PagedList;
namespace Warehouse.Controllers
{
    [RoutePrefix("blog")]
    public class BlogController : Controller
    {
        readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Route("")]
        public ActionResult Index(int? page)
        {
            List<ListBlogViewModel> listBlogViewModel = _blogService.GetListByDisplay(true)
                .OrderByDescending(b => b.Id).Select(b => new ListBlogViewModel()
                {
                    Title = b.Title,
                    Alias = b.Alias,
                    Description = b.Description,
                    Image = b.Image,
                    DateCreated = b.DateCreated.HasValue ? Warehouse.Common.Format.FormatDateTime(b.DateCreated.Value) : ""
                }).ToList();
            return View(listBlogViewModel.ToPagedList(page ?? 1, 9));
        }
    }
}