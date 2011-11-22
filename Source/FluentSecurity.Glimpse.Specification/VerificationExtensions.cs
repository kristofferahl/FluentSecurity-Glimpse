using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Glimpse.Assist;
using Machine.Specifications;

namespace FluentSecurity.Glimpse.Specification
{
	public static class VerificationExtensions
	{
		public static void VerifyRow(this GlimpseRow row, string controller, string action, params Type[] expectedPolicies)
		{
			row.Columns.ElementAt(0).Data.ShouldEqual("FluentSecurity.Glimpse.Specification.Controllers." + controller);
			row.Columns.ElementAt(1).Data.ShouldEqual(action);

			var policies = (List<object[]>)row.Columns.ElementAt(2).Data;

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