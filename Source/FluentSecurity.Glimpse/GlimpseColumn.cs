namespace FluentSecurity.Glimpse
{
	public class GlimpseColumn
	{
		public object Data { get; private set; }

		public GlimpseColumn(object o)
		{
			Data = o;
		}

		internal void OverrideData(object data)
		{
			Data = data;
		}
	}
}