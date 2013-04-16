using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity.Configuration;
using FluentSecurity.Glimpse.SampleApplication.Controllers;
using FluentSecurity.Policy;

namespace FluentSecurity.Glimpse.SampleApplication
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

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			SetupSecurity();
		}

		private void SetupSecurity()
		{
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => true);

				configuration.Advanced.IgnoreMissingConfiguration();

				configuration.For<HomeController>(x => x.Index()).DenyAnonymousAccess();
				configuration.For<HomeController>(x => x.About()).AddPolicy(new FakePolicy()).AddPolicy<PublishingPolicy>();

				configuration.ResolveServicesUsing(t =>
				{
					if (t == typeof(IPolicyViolationHandler))
						return new List<object>
						{
							new DefaultPolicyViolationHandler()
						};

					return new object[0];
				});

				configuration.Scan(scan =>
				{
					scan.AssembliesFromApplicationBaseDirectory();
					scan.LookForProfiles();
				});
			});
			GlobalFilters.Filters.Add(new HandleSecurityAttribute(), 0);
		}
	}

	public class DefaultPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			return new ViewResult
			{
				ViewName = "~/Views/Home/Index.cshtml"
			};
		}
	}

	public class FakePolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			return PolicyResult.CreateSuccessResult(this);
		}
	}

	public class PublishingPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			Diagnostics.Publish.RuntimePolicyEvent(() => "Publishing event from custom policy", context);
			return PolicyResult.CreateFailureResult(this, "Ah ah ah...");
		}
	}

	public class TestProfile1 : SecurityProfile
	{
		public override void Configure()
		{

		}
	}

	public class TestProfile2 : SecurityProfile
	{
		public override void Configure()
		{

		}
	}
}