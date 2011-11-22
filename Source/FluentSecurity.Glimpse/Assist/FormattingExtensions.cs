using System;
using System.Linq;

namespace FluentSecurity.Glimpse.Assist
{
	public static class FormattingExtensions
	{
		public static GlimpseRow Bold(this GlimpseRow row)
		{
			var data = row.Columns.Last().Data;
			var formattedData = String.Format("*{0}*", data);
			row.Columns.Last().OverrideData(formattedData);
			return row;
		}

		public static GlimpseRow Selected(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Selected");
			row.Column("selected");
			return row;
		}

		public static GlimpseRow Warn(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Warn");
			row.Column("warn");
			return row;
		}

		private static void VerifyRowOperation(GlimpseRow row, string operation)
		{
			if (row.Columns.Count <= 0)
				throw new InvalidOperationException(String.Format("The operation '{0}' is only valid when row has columns.", operation));
		}
	}
}