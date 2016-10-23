using System.Web;
using System.Web.Optimization;

namespace PresentationBuilder
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


			//bundles.Add(new ScriptBundle("~/bundles/angular").Include(
			//			"~/Scripts/angular-*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
					  "~/Scripts/angular.js"));

			bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
					  "~/Scripts/jquery.blockUI.js",
					  "~/Scripts/jquery.matchHeight.js",
					  "~/Scripts/navigation.js",
					  "~/Scripts/scripts.js",
					  "~/Scripts/presentation.js",
                      "~/Scripts/pdf.js",
                        "~/Scripts/dropzone/dropzone.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-social.css",
                      //"~/Content/bootstrap-social.scss",
                      "~/Content/font-awesome.css",
                      "~/Content/basic.css",
                      "~/Content/dropzone.css"));

			bundles.Add(new StyleBundle("~/Content/homeCss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-social.css",
                      //"~/Content/bootstrap-social.scss",
                      "~/Content/font-awesome.css",
					  "~/Content/home.css"));

			bundles.Add(new StyleBundle("~/Content/internalCss").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css",
					  "~/Content/Internal.css",
					  "~/Content/bootstrap-social.css",
					  "~/Content/bootstrap-social.scss",
					  "~/Content/font-awesome.css",
					  "~/Content/basic.css",
					  "~/Content/dropzone.css"));
        }
    }
}
