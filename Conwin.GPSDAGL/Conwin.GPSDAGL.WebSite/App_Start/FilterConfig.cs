using System.Web;
using System.Web.Mvc;
namespace Conwin.GPSDAGL.WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}