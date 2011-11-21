using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Glimpse
{
	public class GlimpseRow
	{
		public readonly List<GlimpseColumn> Columns = new List<GlimpseColumn>();

		public GlimpseRow Column(object o)
		{
			var column = new GlimpseColumn(o);
			Columns.Add(column);
			return this;
		}

		public object[] Build()
		{
			return Columns.Select(c => c.Data).ToArray();
		}
	}
}