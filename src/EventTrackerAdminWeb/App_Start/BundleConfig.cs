using System.Web.Optimization;

namespace EventTrackerAdminWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.*",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                                 "~/Scripts/angular.js",
                                 "~/Scripts/angular-route.js",
                                 "~/Scripts/angular-ui-router.js",
                                 "~/Scripts/angular-sanitizer.js"
                                 ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                      "~/Scripts/bootstrap-datepicker*",
                      "~/Scripts/locales/bootstrap-datepicker.js*"));

            bundles.Add(new ScriptBundle("~/bundles/highchart").Include(
            "~/Scripts/Highcharts-4.0.1/js/highcharts.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker*"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                      "~/Content/themes/base/jquery.ui.core.css",
                      "~/Content/themes/base/jquery.ui.autocomplete.css",
                      "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                          "~/app/app.js",
                          "~/app/controllers/customer.js",
                          "~/app/services/customer.js"
                          ));
        }
    }
}