using System.Web;
using System.Web.Mvc;

namespace vsts_webhook_with_auth
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
