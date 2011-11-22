using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Glimpse.Specification.Controllers;
using FluentSecurity.Glimpse.Specification.Policies;
using FluentSecurity.Policy;
using Glimpse.Core.Extensibility;
using Machine.Specifications;

namespace FluentSecurity.Glimpse.Specification
{
	public class When_getting_the_name_of_the_plugin
	{
		private static IGlimpsePlugin plugin;
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
			MainHeader().Columns[0].Data.ShouldEqual("Section");
			MainHeader().Columns[1].Data.ShouldEqual("Content");
		};

		It should_have_loaded_version_of_fluent_security = () =>
		{
			Section(Sections.FluentSecurity).Columns[0].Data.ShouldEqual("*Fluent Security*");
			
			FluentSecuritySection().Rows[0].Columns[0].Data.ShouldEqual("Key");
			FluentSecuritySection().Rows[0].Columns[1].Data.ShouldEqual("Value");
		};

		private static GlimpseSection FluentSecuritySection()
		{
			return Section(Sections.FluentSecurity).Columns[1].Data.AsGlimpseSection();
		}
	}

	public class When_displaying_configuration_info_after_configuring_fluent_security : ConfigurationSpec
	{
		Because of = () =>
		  result = plugin.GetData(null);

		It should_have_configuration_as_heading = () =>
			Section(Sections.Configuration).Columns[0].Data.ShouldEqual("*Configuration*");

		It should_have_headers_key_and_value = () =>
		{
			ConfigurationSection().Rows[0].Columns[0].Data.ShouldEqual("Key");
			ConfigurationSection().Rows[0].Columns[1].Data.ShouldEqual("Value");
		};

		It should_have_ignore_missing_configuration_info = () =>
		{
			ConfigurationSection().Rows[1].Columns[0].Data.ShouldEqual("Ignore missing configuration");
			ConfigurationSection().Rows[1].Columns[1].Data.ShouldEqual("Yes");
		};

		It should_have_service_locator_info = () =>
		{
			ConfigurationSection().Rows[2].Columns[0].Data.ShouldEqual("Service locator");
			ConfigurationSection().Rows[2].Columns[1].Data.ShouldEqual("Service locator has been configued");
		};

		private static GlimpseSection ConfigurationSection()
		{
			return Section(Sections.Configuration).Columns[1].Data.AsGlimpseSection();
		}
	}

	public class When_displaying_policies_data_after_configuring_fluent_security : ConfigurationSpec
	{
		Because of = () =>
		  result = plugin.GetData(null);

		It should_have_heading_for_policies = () =>
			Section(Sections.Policies).Columns[0].Data.ShouldEqual("*Policies*");

		It should_have_header_for_controller = () =>
			PolicySectionData().Rows[0].Columns[0].Data.ShouldEqual("Controller");

		It should_have_header_for_action = () =>
			PolicySectionData().Rows[0].Columns[1].Data.ShouldEqual("Action");

		It should_have_header_for_policies = () =>
			PolicySectionData().Rows[0].Columns[2].Data.ShouldEqual("Policies");

		It should_have_row_for_admin_index_with_policy_DenyAnonymousAccess = () =>
			PolicySectionData().Rows[1].VerifyRow("AdminController", "Index", typeof(DenyAnonymousAccessPolicy));

		It should_have_row_for_admin_systemmonitor_with_policy_DenyAnonymousAccess_and_LocalHostOnly = () =>
			PolicySectionData().Rows[2].VerifyRow("AdminController", "SystemMonitor", typeof(DenyAnonymousAccessPolicy), typeof(LocalHostOnlyPolicy));

		It should_have_row_for_authentication_login_with_policy_DenyAuthenticatedAccess = () =>
			PolicySectionData().Rows[3].VerifyRow("AuthenticationController", "LogIn", typeof(DenyAuthenticatedAccessPolicy));

		It should_have_row_for_authentication_logout_with_policy_DenyAnonymousAccess = () =>
			PolicySectionData().Rows[4].VerifyRow("AuthenticationController", "LogOut", typeof(DenyAnonymousAccessPolicy));

		It should_have_row_for_home_index_with_policy_Ignore = () =>
			PolicySectionData().Rows[5].VerifyRow("HomeController", "Index", typeof(IgnorePolicy));

		private static GlimpseSection PolicySectionData()
		{
			return Section(Sections.Policies).Columns[1].Data.AsGlimpseSection();
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

		protected static List<GlimpseRow> Rows
		{
			get
			{
				var root = (GlimpseSection.Instance) result;
				return root.Data.Rows;
			}
		}

		protected static GlimpseRow MainHeader()
		{
			return Rows[0];
		}

		protected static GlimpseRow Section(int section)
		{
			return Rows[section];
		}
	}
}