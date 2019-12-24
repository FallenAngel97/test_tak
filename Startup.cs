using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(SigmaTestTask.Startup))]

namespace SigmaTestTask
{
    public class Startup
    {
//        using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Http;
//using System.Web.Mvc;
//using System.Web.Optimization;
//using System.Web.Routing;

//namespace SigmaTestTask
//    {
//        public class WebApiApplication : System.Web.HttpApplication
//        {
//            protected void Application_Start()
//            {
//                AreaRegistration.RegisterAllAreas();
//                GlobalConfiguration.Configure(WebApiConfig.Register);
//                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
//                RouteConfig.RegisterRoutes(RouteTable.Routes);
//                BundleConfig.RegisterBundles(BundleTable.Bundles);
//            }
//        }
//    }

    public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}
