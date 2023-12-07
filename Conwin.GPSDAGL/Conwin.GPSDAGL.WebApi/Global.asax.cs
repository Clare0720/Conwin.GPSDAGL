using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Conwin.EntityFramework;
using Conwin.GPSDAGL.Framework;
using Conwin.GPSDAGL.Services.Interfaces;
using Conwin.GPSDAGL.WebApi.App_Start;
using Conwin.GPSDAGL.WebApi.Helper;
using Conwin.GPSDAGL.WebApi.TimerJob;
using Conwin.GPSDAGL.WebApiEx;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
namespace Conwin.GPSDAGL.WebApi
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;
            AreaRegistration.RegisterAllAreas();
            //JobScheduler.Start();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapConfiguration.Configure();
            
            DbContextManager.InitStorage(new WebDbContextStorage(this));
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            var assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            //注册Services
            var servicesAssemblies = assemblies.FirstOrDefault(m => m.GetName().Name == "Conwin.GPSDAGL.Services");
            builder.RegisterAssemblyTypes(servicesAssemblies).AsClosedTypesOf(typeof(IBaseService<>));
            //注册Repository
            var repositoryAssemblies = assemblies.FirstOrDefault(m => m.GetName().Name == "Conwin.GPSDAGL.Repository");
            builder.RegisterAssemblyTypes(repositoryAssemblies).AsClosedTypesOf(typeof(IRepository<>));
            //builder.RegisterType(typeof(Conwin.Framework.BusinessLogger.Impl.BussinessLogger)).As<Conwin.Framework.BusinessLogger.IBussinessLogger>();
            builder.RegisterType(typeof(Conwin.GPSDAGL.Services.CustomBussinessLogger)).As<Conwin.Framework.BusinessLogger.IBussinessLogger>();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //DbInterception.Add(new DbMasterSlaveCommandInterceptor());
            AutoTaskHelper autoTaskHelper = new AutoTaskHelper();
            autoTaskHelper.StartTask();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.End();
            }
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }
    }
}