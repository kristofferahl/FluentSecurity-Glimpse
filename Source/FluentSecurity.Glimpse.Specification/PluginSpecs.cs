using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Glimpse.Specification.Controllers;
using FluentSecurity.Glimpse.Specification.Policies;
using Machine.Specifications;

namespace FluentSecurity.Glimpse.Specification
{
	public class When_getting_data_before_configuring_fluent_security : ConfigurationSpec
	{
		Establish context = () =>
			SecurityConfigurator.Reset();

		Because of = () =>
		  result = plugin.GetData(null);

		It should_return_null = () =>
		  result.ShouldBeNull();
	}

	public class When_getting_data_after_configuring_fluent_security : ConfigurationSpec
	{
		Because of = () =>
		  result = plugin.GetData(null);

		It should_have_header_for_controller = () =>
			GetHeaders().ElementAt(0).ShouldEqual("Controller");

		It should_have_header_for_action = () =>
			GetHeaders().ElementAt(1).ShouldEqual("Action");

		It should_have_header_for_policies = () =>
			GetHeaders().ElementAt(2).ShouldEqual("Policies");

		It should_have_row_for_admin_index_with_policy_DenyAnonymousAccess = () =>
			GetRows().ElementAt(0).VerifyRow("AdminController", "Index", "DenyAnonymousAccess");

		It should_have_row_for_admin_systemmonitor_with_policy_DenyAnonymousAccess_and_LocalHostOnly = () =>
			GetRows().ElementAt(1).VerifyRow("AdminController", "SystemMonitor", "DenyAnonymousAccess", "LocalHostOnly");

		It should_have_row_for_authentication_login_with_policy_DenyAuthenticatedAccess = () =>
			GetRows().ElementAt(2).VerifyRow("AuthenticationController", "LogIn", "DenyAuthenticatedAccess");

		It should_have_row_for_authentication_logout_with_policy_DenyAnonymousAccess = () =>
			GetRows().ElementAt(3).VerifyRow("AuthenticationController", "LogOut", "DenyAnonymousAccess");

		It should_have_row_for_home_index_with_policy_Ignore = () =>
			GetRows().ElementAt(4).VerifyRow("HomeController", "Index", "Ignore");
	}

	public abstract class ConfigurationSpec
	{
		protected static FluentSecurityGlimpsePlugin plugin;
		protected static object result;

		protected ConfigurationSpec()
		{
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(() => true);

				configuration.For<AdminController>().DenyAnonymousAccess();
				configuration.For<AdminController>(x => x.SystemMonitor()).AddPolicy(new LocalHostOnlyPolicy());
				configuration.For<HomeController>().Ignore();
				configuration.For<AuthenticationController>(x => x.LogIn()).DenyAuthenticatedAccess();
				configuration.For<AuthenticationController>(x => x.LogOut()).DenyAnonymousAccess();
			});

			plugin = new FluentSecurityGlimpsePlugin();
		}

		protected static object[] GetHeaders()
		{
			return ((List<object[]>)result).First();
		}

		protected static IEnumerable<object[]> GetRows()
		{
			return ((List<object[]>)result).Skip(1);
		}
	}
}