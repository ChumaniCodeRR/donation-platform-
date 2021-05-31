using System.Web;
using System.Web.Optimization;

namespace Vetro
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bootstrap")
                .Include(
                     "~/Assets/vendor/bootstrap/js/popper.min.js",
                     "~/Assets/vendor/bootstrap/js/bootstrap.js"));
            bundles.Add(new StyleBundle("~/bootstrapstyle").Include(
                "~/Assets/vendor/bootstrap/css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/maincss").Include(
              "~/Assets/css/app.css"));
            bundles.Add(new StyleBundle("~/mainfonts").Include(
              "~/Assets/css/font-face.css"));
            
            //Moved bootstrap css to own style tag in the _layout page.  This is to remove it from the optimizations which was breaking the fonts and icons.
            //bundles.Add(new StyleBundle("~/styles").IncludeDirectory("~/Assets", "*.css", true));
            //bundles.Add(new StyleBundle("~/styles").IncludeDirectory("~/Assets", "*.css", true));


            bundles.Add(new ScriptBundle("~/ng").Include(
                        "~/Assets/ng/angular.min.js",
                        "~/Assets/ng/angular-route.min.js",
                        "~/Assets/ng/angular-cookies.min.js"));

            bundles.Add(new ScriptBundle("~/app").IncludeDirectory("~/Assets/app", "*.js", true));

            bundles.Add(new ScriptBundle("~/jquery").Include(
                        "~/Assets/jquery/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/misc").IncludeDirectory("~/Assets/misc", "*.js", true));
            bundles.Add(new ScriptBundle("~/mainjs").IncludeDirectory("~/Assets/js", "*.js", true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
