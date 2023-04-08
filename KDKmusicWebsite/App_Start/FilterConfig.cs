using KDKmusicWebsite.Areas.Admin;
using System.Web;
using System.Web.Mvc;

namespace KDKmusicWebsite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
