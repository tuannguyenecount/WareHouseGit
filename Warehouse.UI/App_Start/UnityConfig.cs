using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Warehouse.Data.Data;
using Warehouse.Data.Interface;
using Warehouse.Entities;
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


            container.RegisterType<IArticleService, ArticleService>();
            container.RegisterType<IArticleDal, ArticleDal>();

            container.RegisterType<ApplicationSignInManager, ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager, ApplicationUserManager>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}