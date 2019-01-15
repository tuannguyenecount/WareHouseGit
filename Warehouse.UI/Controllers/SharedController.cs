﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Services.Interface;

namespace Warehouse.Controllers
{
    public class SharedController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;

       

        public SharedController(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }

        [ChildActionOnly]
    
        public PartialViewResult _HeaderPartial()
        {
            // Lấy danh sách danh mục để hiện ra menu. Danh sách category được sắp xếp tăng dần theo cột OrderNum
            ViewBag.Categories = _categoryService.GetAll().OrderBy(p=>p.OrderNum).ToList();
            ViewBag.Articles = _articleService.GetListByDisplay(true).OrderBy(a => a.OrderNum).ToList();

            return PartialView(Session["InfoShop"]);  // truyền thêm Session lưu thông tin của shop
        }

        [ChildActionOnly]

        public PartialViewResult _FooterPartial()
        {
            ViewBag.Categories = _categoryService.GetParents();
            return PartialView(Session["InfoShop"]);
        }
    }
}