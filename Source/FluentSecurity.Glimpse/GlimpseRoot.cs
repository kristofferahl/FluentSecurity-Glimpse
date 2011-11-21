using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Glimpse
{
	public class GlimpseRoot
	{
		public readonly List<GlimpseRow> Rows = new List<GlimpseRow>();

		public GlimpseRow AddRow()
		{
			var row = new GlimpseRow();
			Rows.Add(row);
			return row;
		}

		public List<object[]> Build()
		{
			var rootList = new Instance(this);
			rootList.AddRange(Rows.Select(r => r.Build()));
			return rootList;
		}

		public class Instance : List<object[]>
		{
			public GlimpseRoot Data { get; private set; }

			public Instance(GlimpseRoot instance)
			{
				Data = instance;
			}
		}
	}
}