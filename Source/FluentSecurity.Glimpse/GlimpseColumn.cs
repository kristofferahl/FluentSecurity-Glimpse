namespace FluentSecurity.Glimpse
{
	public class GlimpseColumn
	{
		public object Data { get; private set; }

		public GlimpseColumn(object o)
		{
			Data = o;
		}
	}
}