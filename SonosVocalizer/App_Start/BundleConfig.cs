using System.Web;
using System.Web.Optimization;

namespace SonosVocalizer
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/bs").Include("~/Content/bootstrap/css/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/Scripts/bs")
                .Include("~/Scripts/jquery/jquery-1.10.2.js")
                .Include("~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ang")
                .Include("~/Scripts/angular/angular.js"));
        }
    }
}