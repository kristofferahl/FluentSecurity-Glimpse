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

			var order = 0;
			foreach (var securityEvent in events)
			{
				AddRowForEvent(order, securityEvent, section);
				order++;
			}

			return section;
		}

		public static TabSection BuildAndDequeueEvents(Queue<ISecurityEvent> queue)
		{
			var section = CreateTabSection();

			var order = 0;
			while (queue.Any())
			{
				var securityEvent = queue.Dequeue();
				AddRowForEvent(order, securityEvent, section);
				order++;
			}

			return section;
		}

		private static TabSection CreateTabSection()
		{
			return new TabSection("Order", "Correlation Id", "Message", "Completed in");
		}

		private static void AddRowForEvent(int order, ISecurityEvent @event, TabSection section)
		{
			var milliseconds = @event.CompletedInMilliseconds.HasValue
				? (@event.CompletedInMilliseconds.Value.ToString("0") + " ms").Replace(",", ".")
				: null;

			section.AddRow()
				.Column(order)
				.Column(@event.CorrelationId)
				.Column(@event.Message)
				.Column(milliseconds)
				.WarnIf(@event.CompletedInMilliseconds > 10);
		}
	}
}
