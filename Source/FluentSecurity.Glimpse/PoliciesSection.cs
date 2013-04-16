using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy;
using Glimpse.Core.Tab.Assist;

namespace FluentSecurity.Glimpse
{
	public class PoliciesSection
	{
		public static TabSection Create(ISecurityConfiguration configuration)
		{
			var section = new TabSection("Controller", "Action", "Policies");

			var sortedPolicyContainers = configuration.PolicyContainers.OrderBy(x => x.ActionName).OrderBy(x => x.ControllerName);
			foreach (var policyContainer in sortedPolicyContainers)
			{
				var policySectionData = new TabSection("Policy", "Type");

				var securityPolicies = policyContainer.GetPolicies().OrderBy(x => x.GetType().FullName).Select(x => x.GetPolicyType());
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
					.Column(securityPolicy.FullName)
					.WarnIf(securityPolicy == typeof(IgnorePolicy));
			}
		} 
	}
}