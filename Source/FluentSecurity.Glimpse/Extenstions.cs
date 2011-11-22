using System;

namespace FluentSecurity.Glimpse
{
	public static class Extenstions
	{
		public static GlimpseSection AsGlimpseSection(this object o)
		{
			var instance = o as GlimpseSection.Instance;
			if (instance == null)
				throw new InvalidCastException(String.Format("The object is not a glimpse root. Object is of tyoe {0}.", o.GetType()));

			return instance.Data;
		}
	}
}