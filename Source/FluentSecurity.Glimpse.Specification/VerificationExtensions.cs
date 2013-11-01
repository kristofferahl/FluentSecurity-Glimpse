using System;
using System.Linq;
using Glimpse.Core.Tab.Assist;
using Machine.Specifications;

namespace FluentSecurity.Glimpse.Specification
{
	public static class VerificationExtensions
	{
		public static void VerifyRow(this TabSectionRow row, string controller, string action, params Type[] expectedPolicies)
		{
			row.Columns.ElementAt(0).Data.ShouldEqual("FluentSecurity.Glimpse.Specification.Controllers." + controller);
			row.Columns.ElementAt(1).Data.ShouldEqual(action);

			var policies = (TabSection)row.Columns.ElementAt(2).Data;

			for (var index = 0; index < expectedPolicies.Length; index++)
			{
				var expectedPolicy = expectedPolicies[index];

				policies.Rows.Any(p =>
					p.Columns.ElementAt(0).Data.Equals(expectedPolicy.Name.Replace("Policy", String.Empty)) &&
					p.Columns.ElementAt(1).Data.Equals(expectedPolicy.FullName)
					).ShouldBeTrue();
			}
		}
	}
}