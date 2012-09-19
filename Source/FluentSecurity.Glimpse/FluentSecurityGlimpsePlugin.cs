using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

namespace FluentSecurity.Glimpse
{
	public class FluentSecurityGlimpsePlugin : AspNetTab
	{
		public override object GetData(ITabContext context)
		{
			var configuration = GetSecurityConfiguration();
			if (configuration != null)
			{
				// TODO: See below...
				// * Link to documentation
				// * Link to NuGet
				// * Link to website
				// * Current configuration events (what was the path that led to the current configurtion)
				// * Request details (what did Fluent Security do during the most recent request)
				//		- What was the controller and action that was called
				//		- How many policies was in the container that matched that controller action
				//		- How many policies executed
				//		- Which policies executed
				//		- How long did it take to execute them
				//		- What was the result of each policy
				//		- What did Fluent Security do with that result
				//		- What violation handler was selected
				//		- How was that violation handler selected
				//		- What was the result of that violation handler
				//		- How long did it take to execute that violation handler
				//		- ...

				var infoSection = CreateInfoSection(configuration);
				var configurationSection = CreateConfigurationSection(configuration);
				var policiesSection = CreatePoliciesSection(configuration);

				var plugin = Plugin.Create("Section", "Content")
					.Section("Fluent Security", infoSection)
					.Section("Configuration", configurationSection)
					.Section("Policies", policiesSection);

				return plugin;
			}

			return null;
		}

		private static TabSection CreateInfoSection(ISecurityConfiguration configuration)
		{
			var section = new TabSection("Key", "Value");

			var availableVersion = TryGetVersionFromGithub();
			section.AddRow()
				.Column("Latest version of Fluent Security").Strong()
				.Column(availableVersion).Strong()
				.Selected();
			
			var loadedVersion = configuration.GetType().Assembly.FullName;
			section.AddRow().Column("Loaded assembly").Column(loadedVersion);

			return section;
		}

		private static string TryGetVersionFromGithub()
		{
			try
			{
				var xml = XDocument.Load("https://raw.github.com/kristofferahl/FluentSecurity/master/Build/Scripts/Build.build");
				var root = xml.Root;
				if (root != null)
				{
					var properties = root.Elements().Where(e => e.Name.LocalName == "property" && e.HasAttributes).ToList();
					if (properties.Any())
					{
						var versionProperty = properties.SingleOrDefault(p => p.FirstAttribute.Value == "project.version.label");
						if (versionProperty != null)
						{
							var versionAttribute = versionProperty.Attribute("value");
							if (versionAttribute != null)
								return String.Format("Fluent Security v. {0}", versionAttribute.Value);
						}
					}
				}
			}
			catch { }
			return "Failed to find available version";
		}

		private static TabSection CreateConfigurationSection(ISecurityConfiguration configuration)
		{
			var section = new TabSection("Key", "Value");

			var ignoreMissingConfiguration = configuration.IgnoreMissingConfiguration;
			var missingConfigurationRow = section.AddRow().Column("Ignore missing configuration");

			if (ignoreMissingConfiguration)
				missingConfigurationRow.Column("Yes").Warn();
			else
				missingConfigurationRow.Column("No");

			var serviceLocatorRow = section.AddRow().Column("Service locator");
			serviceLocatorRow.Column(configuration.ExternalServiceLocator != null
				? "Service locator has been configued"
				: "Not configued"
				);

			return section;
		}

		private static TabSection CreatePoliciesSection(ISecurityConfiguration configuration)
		{
			var section = new TabSection("Controller", "Action", "Policies");

			var sortedPolicyContainers = configuration.PolicyContainers.OrderBy(x => x.ActionName).OrderBy(x => x.ControllerName);
			foreach (var policyContainer in sortedPolicyContainers)
			{
				var policySectionData = new TabSection("Policy", "Type");

				var securityPolicies = policyContainer.GetPolicies().OrderBy(x => x.GetType().FullName).Select(x => x.GetType());
				AddPoliciesToPolicySection(policySectionData, securityPolicies);

				section.AddRow()
					.Column(policyContainer.ControllerName)
					.Column(policyContainer.ActionName)
					.Column(policySectionData);
			}
			return section;
		}

		private static void AddPoliciesToPolicySection(TabSection policyRows, IEnumerable<Type> securityPolicies)
		{
			foreach (var securityPolicy in securityPolicies)
			{
				policyRows.AddRow()
					.Column(securityPolicy.Name.Replace("Policy", String.Empty))
					.Column(securityPolicy.FullName);
			}
		}

		private static ISecurityConfiguration GetSecurityConfiguration()
		{
			ISecurityConfiguration configuration;
			try
			{
				configuration = SecurityConfiguration.Current;
			}
			catch (InvalidOperationException)
			{
				return null;
			}
			return configuration;
		}

		public override string Name
		{
			get { return "Fluent Security"; }
		}
	}
}
