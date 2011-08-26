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

			var configuration = SecurityConfiguration.Current;
			if (configuration != null)
			{
				foreach (var policyContainer in configuration.PolicyContainers)
					data.Add(new object[] { policyContainer.ControllerName, policyContainer.ActionName, policyContainer.GetPolicies().Select(x => x.GetType().Name).ToArray() });
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
