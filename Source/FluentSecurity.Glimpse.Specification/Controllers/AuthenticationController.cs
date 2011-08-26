using System.Web.Mvc;

namespace FluentSecurity.Glimpse.Specification.Controllers
{
	public class AuthenticationController : Controller
	{
		public ActionResult LogIn()
		{
			return new EmptyResult();
		}

		public ActionResult LogOut()
		{
			return new EmptyResult();
		}
	}
}