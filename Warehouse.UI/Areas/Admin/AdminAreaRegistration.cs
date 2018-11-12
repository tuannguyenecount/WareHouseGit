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
                new { area="Admin", controller = "AspNetUser", action = "ProfileUser", id = UrlParameter.Optional},
                new [] { "Warehouse.Areas.Admin.Controllers" }

            );
        }
    }
}