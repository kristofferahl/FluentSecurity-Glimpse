using System.Web.Mvc;

namespace FluentSecurity.Glimpse.Specification.Controllers
{
	public class AdminController : Controller
	{
		public ActionResult Index()
		{
			return new EmptyResult();
		}

		public ActionResult SystemMonitor()
		{
			return new EmptyResult();
		}
	}
}