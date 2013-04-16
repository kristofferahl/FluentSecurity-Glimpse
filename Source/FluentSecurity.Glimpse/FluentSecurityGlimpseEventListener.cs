using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;

namespace FluentSecurity.Glimpse
{
	public class FluentSecurityGlimpseEventListener : ISecurityEventListener
	{
		public void Handle(ISecurityEvent securityEvent)
		{
			if (securityEvent is ConfigurationEvent)
				FluentSecurityGlimpsePlugin.ConfigurationEvents.Add(securityEvent);
			else
				FluentSecurityGlimpsePlugin.RuntimeEvents.Enqueue(securityEvent);
		}
	}
}