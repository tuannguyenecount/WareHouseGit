using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Warehouse.Models;
namespace Warehouse
{
    public class MvcApplication : System.Web.HttpApplication
    {
        async void AddInfoShop()
        {
            hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();
            if (db.InfoShops.Count() == 0)
            {
                InfoShop infoShop = new InfoShop()
                {
                    ShopName = "Shop Name",
                    Address = "TPHCM"
                };
                db.InfoShops.Add(infoShop);
                await db.SaveChangesAsync();
            }
        }
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            new Task(() => AddInfoShop()).Start();
            Application["WidthImageProduct"] = ConfigurationManager.AppSettings["WidthImageProduct"] != null ? ConfigurationManager.AppSettings["WidthImageProduct"].ToString() : "230";
            Application["HeightImageProduct"] = ConfigurationManager.AppSettings["HeightImageProduct"] != null ? ConfigurationManager.AppSettings["HeightImageProduct"].ToString() : "300";
        }

        protected void Session_Start()
        {
            if(Session["ShoppingCart"] == null)
                Session["ShoppingCart"] = new List<CartItem>();
            hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();
            Session["InfoShop"] = db.InfoShops.First();
        }
    }
}
