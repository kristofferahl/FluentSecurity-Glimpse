using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Diagnostics.Events;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using Glimpse.Core.Tab.Assist;

namespace FluentSecurity.Glimpse
{
	public class ConfigurationSection
	{
		public static TabSection Create(ISecurityConfiguration configuration, IList<ISecurityEvent> events)
		{
			var section = new TabSection("Key", "Value");

			var ignoreMissingConfiguration = configuration.Runtime.ShouldIgnoreMissingConfiguration;
			var missingConfigurationRow = section.AddRow().Column("Ignore missing configuration");

			if (ignoreMissingConfiguration)
				missingConfigurationRow.Column("Yes").Warn();
			else
				missingConfigurationRow.Column("No");

			var serviceLocatorRow = section.AddRow().Column("Service locator");
			serviceLocatorRow.Column(configuration.Runtime.ExternalServiceLocator != null
				? "Service locator has been configured"
				: "Not configured"
				);

			var profilesConfiguration = configuration.Runtime.Profiles;
			section.Section("Loaded profiles", tabSection =>
			{
				tabSection.AddRow().Column("Order").Column("Profile name");
				var order = 1;
				foreach (var type in profilesConfiguration)
				{
					tabSection.AddRow().Column(order).Column(type.FullName);
					order++;
				}
			});

			var conventionsConfiguration = configuration.Runtime.Conventions;
			section.Section("Applied conventions", tabSection =>
			{
				tabSection.AddRow().Column("Order").Column("Convention").Column("Type");
				AddConventions(conventionsConfiguration.OfType<IPolicyViolationHandlerConvention>(), tabSection);
			});

			section.AddRow().Column("Default cache level").Column(configuration.Runtime.DefaultResultsCacheLifecycle.ToString());

			section.AddRow().Column("Security context modifyer").Column(configuration.Runtime.SecurityContextModifyer != null ? "Yes" : "No");

			if (events.Any())
			{
				var configurationEventsSection = EventsSection.Build(events);
				section.Section("Events", configurationEventsSection);
			}

			return section;
		}

		private static void AddConventions<TConvention>(IEnumerable<TConvention> policyViolationHandlerConventions, TabSection tabSection)
		{
			var order = 1;
			foreach (var convention in policyViolationHandlerConventions)
			{
				tabSection.AddRow().Column(order).Column(convention.GetType().FullName).Column(typeof(TConvention).Name);
				order++;
			}
		}
	}
}