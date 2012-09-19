using System;
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

				var infoSection = InfoSection.Create(configuration);
				var configurationSection = ConfigurationSection.Create(configuration);
				var policiesSection = PoliciesSection.Create(configuration);

				var plugin = Plugin.Create("Section", "Content")
					.Section("Fluent Security", infoSection)
					.Section("Configuration", configurationSection)
					.Section("Policies", policiesSection);

				return plugin;
			}

			return null;
		}

		public override string Name
		{
			get { return "Fluent Security"; }
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
	}
}
