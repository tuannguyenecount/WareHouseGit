using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;
using Warehouse.Areas.Admin.Controllers;
using Warehouse.Controllers;
using Warehouse.Data.Data;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Models;
using Warehouse.Services.Interface;
using Warehouse.Services.Services;

namespace Warehouse
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IProductDal, ProductDal>();

            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<ICategoryDal, CategoryDal>();


            container.RegisterType<ISlideService, SlideService>();
            container.RegisterType<ISlideDal, SlideDal>();

            container.RegisterType<INewsService, NewsService>();
            container.RegisterType<INewsDal, NewsDal>();

            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IOrderDal, OrderDal>();
            container.RegisterType<IOrderDetailService, OrderDetailService>();
            container.RegisterType<IOrderDetailDal, OrderDetailDal>();


            container.RegisterType<IArticleService, ArticleService>();
            container.RegisterType<IArticleDal, ArticleDal>();

            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());

            container.RegisterType<AccountController>(new InjectionConstructor(typeof(IProvinceService), typeof(IDistrictService), typeof(IWardService)));
            container.RegisterType<AspNetUserController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor(typeof(IProvinceService), typeof(IDistrictService), typeof(IWardService)));

            container.RegisterType<IImagesProductService, ImagesProductService>();
            container.RegisterType<IImagesProductDal, ImagesProductDal>();

            container.RegisterType<IProvinceService, ProvinceService>();
            container.RegisterType<IProvinceDal, ProvinceDal>();

            container.RegisterType<IDistrictService, DistrictService>();
            container.RegisterType<IDistrictDal, DistrictDal>();

            container.RegisterType<IWardService, WardService>();
            container.RegisterType<IWardDal, WardDal>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}