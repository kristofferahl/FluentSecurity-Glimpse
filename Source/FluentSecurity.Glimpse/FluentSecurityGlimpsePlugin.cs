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

			ISecurityConfiguration configuration;
			try
			{
				configuration = SecurityConfiguration.Current;
			}
			catch (InvalidOperationException)
			{
				return null;
			}
			
			if (configuration != null)
			{
				var sortedPolicyContainers = configuration.PolicyContainers.OrderBy(x => x.ActionName).OrderBy(x => x.ControllerName);
				foreach (var policyContainer in sortedPolicyContainers)
					data.Add(new object[]
					{
						policyContainer.ControllerName,
						policyContainer.ActionName,
						policyContainer.GetPolicies().Select(x => x.GetType().Name.Replace("Policy", String.Empty)).OrderBy(name => name).ToArray()
					});
			}

			return data;
		}

		public void SetupInit() {}

		public string Name
		{
			get { return "Fluent Security"; }
		}
	}
}
