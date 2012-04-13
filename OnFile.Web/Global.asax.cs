using System.Web.Mvc;
using System.Web.Routing;
using OnFile.Infra;
using OnFile.Web.Controllers;
using OnFile.Web.Models;
using ServiceStack.WebHost.Endpoints;
using System.Web.Optimization;

namespace OnFile.Web
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

            routes.IgnoreRoute("api/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("Default", "{controller}/{action}/{id}", 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            new AppHost().Init();
            
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleTable.Bundles.RegisterTemplateBundles();

            Bootstrapper.Run(this);
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("CustomersController", typeof(CustomersController).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
            ServiceStack.Text.JsConfig.IncludeNullValues = true;
            ServiceStack.Text.JsConfig.ThrowOnDeserializationError = true;

            Routes
              .Add<CustomersResponse>("/customers");
        }
    }
}