using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Diagnostics.Events;
using Glimpse.Core.Tab.Assist;

namespace FluentSecurity.Glimpse
{
	public static class EventsSection
	{
		public static TabSection Build(IList<ISecurityEvent> events)
		{
			var section = CreateTabSection();

			foreach (var securityEvent in events)
				AddRowForEvent(securityEvent, section);

			return section;
		}

		public static TabSection BuildAndDequeueEvents(Queue<ISecurityEvent> queue)
		{
			var section = CreateTabSection();

			while (queue.Any())
			{
				var securityEvent = queue.Dequeue();
				AddRowForEvent(securityEvent, section);
			}

			return section;
		}

		private static TabSection CreateTabSection()
		{
			return new TabSection("Correlation Id", "Message", "Completed in");
		}

		private static void AddRowForEvent(ISecurityEvent @event, TabSection section)
		{
			var milliseconds = @event.CompletedInMilliseconds.HasValue
				? (@event.CompletedInMilliseconds.Value.ToString("0.00") + " ms").Replace(",", ".")
				: null;

			section.AddRow()
				.Column(@event.CorrelationId)
				.Column(@event.Message)
				.Column(milliseconds)
				.WarnIf(@event.CompletedInMilliseconds > 10);
		}
	}
}
