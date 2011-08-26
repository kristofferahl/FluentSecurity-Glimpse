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
			var data = new List<object[]> { new[] { "Controller", "Action", "Policies" } };

			var configuration = GetSecurityConfiguration();
			if (configuration != null)
			{
				var sortedPolicyContainers = configuration.PolicyContainers.OrderBy(x => x.ActionName).OrderBy(x => x.ControllerName);
				foreach (var policyContainer in sortedPolicyContainers)
				{
					var policyRows = new List<object[]> { new object[] { "Policy", "Type" } };
					var securityPolicies = policyContainer.GetPolicies().OrderBy(x => x.GetType().FullName).Select(x => x.GetType());
					
					AddPoliciesToPolicyRows(policyRows, securityPolicies);

					data.Add(new object[]
					{
						policyContainer.ControllerName,
						policyContainer.ActionName,
						policyRows
					});
				}
			}

			return data;
		}

		private static void AddPoliciesToPolicyRows(List<object[]> policyRows, IEnumerable<Type> securityPolicies)
		{
			foreach (var securityPolicy in securityPolicies)
			{
				policyRows.Add(new object[]
				{
					securityPolicy.Name.Replace("Policy", String.Empty),
					securityPolicy.FullName
				});
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
