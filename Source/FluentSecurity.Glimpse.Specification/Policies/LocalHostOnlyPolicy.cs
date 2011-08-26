using FluentSecurity.Policy;

namespace FluentSecurity.Glimpse.Specification.Policies
{
	public class LocalHostOnlyPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			return PolicyResult.CreateSuccessResult(this);
		}
	}
}