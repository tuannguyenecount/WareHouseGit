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
                "quantrishop/{controller}/{action}/{id}",
                new { area="Admin", controller = "Statistical", action = "Index", id = UrlParameter.Optional},
                new [] { "WareHouse.Areas.Admin.Controllers" }

            );
        }
    }
}