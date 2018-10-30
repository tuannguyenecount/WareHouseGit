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

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}