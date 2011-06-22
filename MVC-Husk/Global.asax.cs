using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using MVC_Husk.Models;
using MvcMiniProfiler;

namespace MVC_Husk
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
            routes.IgnoreRoute("quartz/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            //set up models so that if the class changes the table gets dropped in DB
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SeasonalProductDBContext>());
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

        }

        //Adding the MiniProfiler to each request to get some timing data and eventually do more granular
        //inspection.  Will only work on local requests
        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        //stop the profiler once done
        protected void Application_EndRequest() 
        { 
            MiniProfiler.Stop(); 
        }

    }
}