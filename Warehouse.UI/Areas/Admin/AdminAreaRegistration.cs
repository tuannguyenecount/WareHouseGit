using System.Web.Mvc;

namespace FStore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "AdminArea/{controller}/{action}/{id}",
                new { area="Admin", controller = "Product", action = "Index", id = UrlParameter.Optional},
                new [] { "Warehouse.Areas.Admin.Controllers" }

            );
        }
    }
}