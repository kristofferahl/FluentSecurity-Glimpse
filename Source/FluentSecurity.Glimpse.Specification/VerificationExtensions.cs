using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace FluentSecurity.Glimpse.Specification
{
	public static class VerificationExtensions
	{
		public static void VerifyRow(this object[] row, string controller, string action, params Type[] expectedPolicies)
		{
			row.ElementAt(0).ShouldEqual("FluentSecurity.Glimpse.Specification.Controllers." + controller);
			row.ElementAt(1).ShouldEqual(action);

			var policies = (List<object[]>)row.ElementAt(2);

			for (var index = 0; index < expectedPolicies.Length; index++)
			{
				var expectedPolicy = expectedPolicies[index];

				policies.Any(p =>
					p.First().Equals(expectedPolicy.Name.Replace("Policy", String.Empty)) &&
					p.Last().Equals(expectedPolicy.FullName)
					).ShouldBeTrue();
			}
		}
	}
}