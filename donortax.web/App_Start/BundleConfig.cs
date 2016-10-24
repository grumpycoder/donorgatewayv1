using System.Web.Optimization;

namespace donortax.web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                "~/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/jqueryval").Include(
                "~/js/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/js/modernizr").Include(
                "~/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                "~/js/bootstrap.js",
                "~/js/respond.js"));

            bundles.Add(new StyleBundle("~/css").Include(
                "~/css/bootstrap.css",
                "~/css/site.css"));

            bundles.Add(new StyleBundle("~/css/splcenter")
                .Include("~/css/bootstrap.css")
                .Include("~/css/font-awesome.css")
                .Include("~/css/splcenter-base.css")
                .Include("~/css/splcenter.css"));

            bundles.Add(new ScriptBundle("~/js/splcenter")
                .Include("~/js/jquery-{version}.js")
                .Include("~/js/bootstrap.js")
                .Include("~/js/splc.js"));
        }
    }
}
