using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
           // Cache per user
            if (arg == "User")
            {
                // depends on your authentication mechanism
                return "User=" + context.User.Identity.GetUserId();
                //?return "User=" + context.Session.SessionID;
            }

            return base.GetVaryByCustomString(context, arg);
        }
    }


}
