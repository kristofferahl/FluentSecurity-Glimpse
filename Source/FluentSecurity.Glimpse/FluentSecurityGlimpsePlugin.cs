using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Diagnostics.Events;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

namespace FluentSecurity.Glimpse
{
	public class FluentSecurityGlimpsePlugin : AspNetTab, ITabLayout, IDocumentation
	{
		public static List<ISecurityEvent> ConfigurationEvents = new List<ISecurityEvent>();
		public static Queue<ISecurityEvent> RuntimeEvents = new Queue<ISecurityEvent>();

		public override object GetData(ITabContext context)
		{
			var configuration = GetSecurityConfiguration();
			if (configuration != null)
			{
				// TODO: See below...
				// * Current configuration events (what was the path that led to the current configurtion)
				//		- Adding policies using "ForAllControllers|ForAllControllersInAssembly(assemblyname)|ForAllControllersInAssemblyContainingType(typename)|..."
				//		- Setting cache strategy of policy "*Policy" to "*Lifecyle" for "*Controller" action "SomeAction"
				// * Request details (what did Fluent Security do during the most recent request)
				//		- What was the result of a violation handler
				//		- How long did it take to execute that violation handler

				var infoSection = InfoSection.Create(configuration);
				var configurationSection = ConfigurationSection.Create(configuration, ConfigurationEvents);
				var policiesSection = PoliciesSection.Create(configuration);

				var plugin = Plugin.Create("Section", "Content")
					.Section("Info", infoSection)
					.Section("Configuration", configurationSection)
					.Section("Policies", policiesSection);

				if (RuntimeEvents.Any())
				{
					var runtimeEventsSection = EventsSection.BuildAndDequeueEvents(RuntimeEvents);
					plugin.Section("Events", runtimeEventsSection);
				}

				return plugin;
			}

			return null;
		}

		public override string Name
		{
			get { return "FluentSecurity"; }
		}

		public object GetLayout()
		{
			var mainLayout = TabLayout.Create(layout => layout.Row(row =>
			{
				row.Cell(0).AsKey().WidthInPixels(100);
				row.Cell(1);
			}));
			return mainLayout.Build();
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
}