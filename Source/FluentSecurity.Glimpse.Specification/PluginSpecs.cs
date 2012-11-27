using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Glimpse.Specification.Controllers;
using FluentSecurity.Glimpse.Specification.Policies;
using FluentSecurity.Policy;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;
using Machine.Specifications;

namespace FluentSecurity.Glimpse.Specification
{
	public class When_getting_the_name_of_the_plugin
	{
		private static ITab plugin;
		private static object result;

		Establish context = () =>
			plugin = new FluentSecurityGlimpsePlugin();

		Because of = () =>
		   result = plugin.Name;

		It should_be_Fluent_Security = () =>
		  result.ShouldEqual("Fluent Security");
	}

	public class When_getting_data_before_configuring_fluent_security : ConfigurationSpec
	{
		Establish context = () =>
			SecurityConfigurator.Reset();

		Because of = () =>
		  result = plugin.GetData(null);

		It should_return_null = () =>
		  result.ShouldBeNull();
	}

	public class When_displaying_basic_information_after_configuring_fluent_security : ConfigurationSpec
	{
		Because of = () =>
		  result = plugin.GetData(null);

		It should_have_headers_key_and_value = () =>
		{
			MainHeader().Columns.ElementAt(0).Data.ShouldEqual("Section");
			MainHeader().Columns.ElementAt(1).Data.ShouldEqual("Content");
		};

		It should_have_loaded_version_of_fluent_security = () =>
		{
			Section(Sections.FluentSecurity).Columns.ElementAt(0).Data.ShouldEqual("*Info*");
			
			FluentSecuritySection().Rows.ElementAt(0).Columns.ElementAt(0).Data.ShouldEqual("Key");
			FluentSecuritySection().Rows.ElementAt(0).Columns.ElementAt(1).Data.ShouldEqual("Value");
		};

		private static TabSection FluentSecuritySection()
		{
			return Section(Sections.FluentSecurity).Columns.ElementAt(1).Data as TabSection;
		}
	}

	public class When_displaying_configuration_info_after_configuring_fluent_security : ConfigurationSpec
	{
		Because of = () =>
		  result = plugin.GetData(null);

		It should_have_configuration_as_heading = () =>
			Section(Sections.Configuration).Columns.ElementAt(0).Data.ShouldEqual("*Configuration*");

		It should_have_headers_key_and_value = () =>
		{
			ConfigurationSection().Rows.ElementAt(0).Columns.ElementAt(0).Data.ShouldEqual("Key");
			ConfigurationSection().Rows.ElementAt(0).Columns.ElementAt(1).Data.ShouldEqual("Value");
		};

		It should_have_ignore_missing_configuration_info = () =>
		{
			ConfigurationSection().Rows.ElementAt(1).Columns.ElementAt(0).Data.ShouldEqual("Ignore missing configuration");
			ConfigurationSection().Rows.ElementAt(1).Columns.ElementAt(1).Data.ShouldEqual("Yes");
		};

		It should_have_service_locator_info = () =>
		{
			ConfigurationSection().Rows.ElementAt(2).Columns.ElementAt(0).Data.ShouldEqual("Service locator");
			ConfigurationSection().Rows.ElementAt(2).Columns.ElementAt(1).Data.ShouldEqual("Service locator has been configued");
		};

		private static TabSection ConfigurationSection()
		{
			return Section(Sections.Configuration).Columns.ElementAt(1).Data as TabSection;
		}
	}

	public class When_displaying_policies_data_after_configuring_fluent_security : ConfigurationSpec
	{
		Because of = () =>
		  result = plugin.GetData(null);

		It should_have_heading_for_policies = () =>
			Section(Sections.Policies).Columns.ElementAt(0).Data.ShouldEqual("*Policies*");

		It should_have_header_for_controller = () =>
			PolicySectionData().Rows.ElementAt(0).Columns.ElementAt(0).Data.ShouldEqual("Controller");

		It should_have_header_for_action = () =>
			PolicySectionData().Rows.ElementAt(0).Columns.ElementAt(1).Data.ShouldEqual("Action");

		It should_have_header_for_policies = () =>
			PolicySectionData().Rows.ElementAt(0).Columns.ElementAt(2).Data.ShouldEqual("Policies");

		It should_have_row_for_admin_index_with_policy_DenyAnonymousAccess = () =>
			PolicySectionData().Rows.ElementAt(1).VerifyRow("AdminController", "Index", typeof(DenyAnonymousAccessPolicy));

		It should_have_row_for_admin_systemmonitor_with_policy_DenyAnonymousAccess_and_LocalHostOnly = () =>
			PolicySectionData().Rows.ElementAt(2).VerifyRow("AdminController", "SystemMonitor", typeof(DenyAnonymousAccessPolicy), typeof(LocalHostOnlyPolicy));

		It should_have_row_for_authentication_login_with_policy_DenyAuthenticatedAccess = () =>
			PolicySectionData().Rows.ElementAt(3).VerifyRow("AuthenticationController", "LogIn", typeof(DenyAuthenticatedAccessPolicy));

		It should_have_row_for_authentication_logout_with_policy_DenyAnonymousAccess = () =>
			PolicySectionData().Rows.ElementAt(4).VerifyRow("AuthenticationController", "LogOut", typeof(DenyAnonymousAccessPolicy));

		It should_have_row_for_home_index_with_policy_Ignore = () =>
			PolicySectionData().Rows.ElementAt(5).VerifyRow("HomeController", "Index", typeof(IgnorePolicy));

		private static TabSection PolicySectionData()
		{
			return Section(Sections.Policies).Columns.ElementAt(1).Data as TabSection;
		}
	}

	public static class Sections
	{
		public static int FluentSecurity = 1;
		public static int Configuration = 2;
		public static int Policies = 3;
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

				configuration.IgnoreMissingConfiguration();
				
				configuration.ResolveServicesUsing(t => new List<object>());

				configuration.For<AdminController>().DenyAnonymousAccess();
				configuration.For<AdminController>(x => x.SystemMonitor()).AddPolicy(new LocalHostOnlyPolicy());
				configuration.For<HomeController>().Ignore();
				configuration.For<AuthenticationController>(x => x.LogIn()).DenyAuthenticatedAccess();
				configuration.For<AuthenticationController>(x => x.LogOut()).DenyAnonymousAccess();
			});

			plugin = new FluentSecurityGlimpsePlugin();
		}

		protected static IEnumerable<TabSectionRow> Rows
		{
			get
			{
				var root = (TabSection) result;
				return root.Rows;
			}
		}

		protected static TabSectionRow MainHeader()
		{
			return Rows.ElementAt(0);
		}

		protected static TabSectionRow Section(int section)
		{
			return Rows.ElementAt(section);
		}
	}
}