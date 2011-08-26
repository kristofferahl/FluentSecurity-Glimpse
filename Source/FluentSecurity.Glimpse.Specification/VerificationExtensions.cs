using System.Linq;
using Machine.Specifications;

namespace FluentSecurity.Glimpse.Specification
{
	public static class VerificationExtensions
	{
		public static void VerifyRow(this object[] row, string controller, string action, params string[] expectedPolicies)
		{
			row.ElementAt(0).ShouldEqual("FluentSecurity.Glimpse.Specification.Controllers." + controller);
			row.ElementAt(1).ShouldEqual(action);

			var policies = (object[])row.ElementAt(2);
			
			for (var index = 0; index < expectedPolicies.Length; index++)
			{
				var expectedPolicy = expectedPolicies[index];
				policies.Contains(expectedPolicy).ShouldBeTrue();
			}
		}
	}
}