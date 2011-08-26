using System.Web.Mvc;

namespace FluentSecurity.Glimpse.Specification.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return new EmptyResult();
		}
	}
}