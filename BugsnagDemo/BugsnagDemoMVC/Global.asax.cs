﻿using Bugsnag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BugsnagDemoMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Client Bugsnag { get; set; }

        protected void Application_Start()
        {
            Bugsnag = new Client("9134c4469d16f30f025a1e98f45b3ddb");
            Bugsnag.Config.FilePrefix = new[] { @"e:\GitHub\Bugsnag-NET\" };

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}