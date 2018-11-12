using System.Web;
using System.Web.Optimization;

namespace Warehouse
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryCDN", "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.easing-1.3.js",
                        "~/Scripts/plugins.js",
                        "~/Scripts/main.js"));



            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-off-canvas-nav.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/normalize.min.css",
                      "~/Content/normalize.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/animate.css",
                      "~/Content/templatemo-misc.css",
                      "~/Content/templatemo-style.css", 
                      "~/Content/bootstrap-off-canvas-nav.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
            bundles.UseCdn = true;
        }
    }
}
