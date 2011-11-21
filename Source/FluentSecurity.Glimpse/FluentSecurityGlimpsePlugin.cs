using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;

namespace FluentSecurity.Glimpse
{
	[GlimpsePlugin]
	public class FluentSecurityGlimpsePlugin : IGlimpsePlugin
	{
		public object GetData(HttpContextBase context)
		{
			var configuration = GetSecurityConfiguration();
			if (configuration != null)
			{
				//var topLevel = new GlimpseRoot();
				//topLevel.AddRow().Column("Key").Column("Value");

				//topLevel.AddRow().Column("*Ignore missing configuration*").Column(configuration.IgnoreMissingConfiguration);
				
				var root = new GlimpseRoot();
				root.AddRow().Column("Controller").Column("Action").Column("Policies");

				var sortedPolicyContainers = configuration.PolicyContainers.OrderBy(x => x.ActionName).OrderBy(x => x.ControllerName);
				foreach (var policyContainer in sortedPolicyContainers)
				{
					var policyRows = new GlimpseRoot();
					policyRows.AddRow().Column("Policy").Column("Type");

					var securityPolicies = policyContainer.GetPolicies().OrderBy(x => x.GetType().FullName).Select(x => x.GetType());
					AddPoliciesToPolicyRows(policyRows, securityPolicies);

					root.AddRow()
						.Column(policyContainer.ControllerName)
						.Column(policyContainer.ActionName)
						.Column(policyRows.Build());
				}

				//topLevel.AddRow().Column("*Policies*").Column(root.Build());

				return root.Build();
			}

			return null;
		}

		private static void AddPoliciesToPolicyRows(GlimpseRoot policyRows, IEnumerable<Type> securityPolicies)
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

		public void SetupInit() {}

		public string Name
		{
			get { return "Fluent Security"; }
		}
	}
}
