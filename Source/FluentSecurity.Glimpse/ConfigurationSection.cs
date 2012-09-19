using Glimpse.Core.Plugin.Assist;

namespace FluentSecurity.Glimpse
{
	public class ConfigurationSection
	{
		public static TabSection Create(ISecurityConfiguration configuration)
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
	}
}