using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Warehouse.Data.Data;
using Warehouse.Data.Interface;
using Warehouse.Models;
using Warehouse.Services.Interface;
using Warehouse.Services.Services;

namespace Warehouse
{
    public class MvcApplication : System.Web.HttpApplication
    {
       
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["WidthImageProduct"] = ConfigurationManager.AppSettings["WidthImageProduct"].ToString();
            Application["HeightImageProduct"] = ConfigurationManager.AppSettings["HeightImageProduct"].ToString();
            Application["WidthImageBlog"] = ConfigurationManager.AppSettings["WidthImageBlog"].ToString();
            Application["HeightImageBlog"] = ConfigurationManager.AppSettings["HeightImageBlog"].ToString();
        }

        protected void Session_Start()
        {
            if(Session["ShoppingCart"] == null)
                Session["ShoppingCart"] = new List<CartItem>();
            InfoShopDal infoShopDal = new InfoShopDal();
            InfoShopService infoShopService = new InfoShopService(infoShopDal);
            Session["InfoShop"] = infoShopService.GetFirst();
        }
    }
}
