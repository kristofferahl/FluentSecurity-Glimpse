using System;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

namespace FluentSecurity.Glimpse
{
	public class FluentSecurityGlimpsePlugin : AspNetTab, ITabLayout, IDocumentation
	{
		public override object GetData(ITabContext context)
		{
			var configuration = GetSecurityConfiguration();
			if (configuration != null)
			{
				// TODO: See below...
				// * Current configuration events (what was the path that led to the current configurtion)
				//		- Adding policies using "ForAllControllers|ForAllControllersInAssembly(assemblyname)|ForAllControllersInAssemblyContainingType(typename)|..."
				//		- Created policy container for "*Controller" action "SomeAction"
				//		- Added policy (instance|lazy) "*Policy" for "*Controller" action "SomeAction"
				//		- Removed policy "*Policy" for "*Controller" action "SomeAction"
				//		- Setting cache strategy of policy "*Policy" to "*Lifecyle" for "*Controller" action "SomeAction"
				// * Request details (what did Fluent Security do during the most recent request)
				//		- What was the controller and action that was called
				//		- How many policies was in the container that matched that controller action
				//		- How many policies executed
				//		- Which policies executed
				//		- How long did it take to execute them
				//		- What was the result of each policy
				//		- What did Fluent Security do with that result
				//		- What violation handler was selected
				//		- How was that violation handler selected (what conventions was used)
				//		- What was the result of that violation handler
				//		- How long did it take to execute that violation handler
				//		- ...

				var infoSection = InfoSection.Create(configuration);
				var configurationSection = ConfigurationSection.Create(configuration);
				var policiesSection = PoliciesSection.Create(configuration);

				var plugin = Plugin.Create("Section", "Content")
					.Section("Info", infoSection)
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

		public object GetLayout()
		{
			var infoSectionLayout = StructuredLayout.Create(layout => layout.Row(row =>
			{
				row.Cell(1).WidthInPixels(200);
				row.Cell(2).DisableLimit();
			}));

			var configSectionLayout = StructuredLayout.Create(layout =>
				layout.Row(row => row.Cell(1).WidthInPixels(200))
				);

			var policiesSectionLayout = StructuredLayout.Create(layout => layout.Row(row =>
			{
				row.Cell(1).AsCode(CodeType.Csharp);
				row.Cell(2).AsCode(CodeType.Csharp);
				row.Cell(3).DisableLimit();
			}));

			var mainLayout = StructuredLayout.Create(layout =>
			{
				layout.Row(row => {});
				layout.Row(row => row.Cell(2).Layout(infoSectionLayout));
				layout.Row(row => row.Cell(2).Layout(configSectionLayout));
				layout.Row(row => row.Cell(2).Layout(policiesSectionLayout));
			});
			return mainLayout;
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

		public string DocumentationUri
		{
			get { return "http://fluentsecurity.net/wiki"; }
		}
	}

	public interface ITabLayout
	{
		object GetLayout();
	}
}
