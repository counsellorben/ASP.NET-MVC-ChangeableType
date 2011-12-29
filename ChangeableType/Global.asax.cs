using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ChangeableType.Extensions;

namespace ChangeableType
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Changeable", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelValidatorProviders.Providers.Clear();
            ModelValidatorProviders.Providers.Add(new ChangeableDataAnnotationsModelValidatorProvider());
            ChangeableDataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            // must add a reference for every single type to be used
            ModelBinders.Binders.Add(typeof(Changeable<int>), new ChangeableModelBinder());
            ModelBinders.Binders.Add(typeof(Changeable<string>), new ChangeableModelBinder());
            // <double>, <Email>, etc., ad nauseaum

        }
    }
}