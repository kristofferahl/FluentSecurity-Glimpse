using System;

namespace FluentSecurity.Glimpse.Assist
{
	public static class Extenstions
	{
		public static GlimpseSection AsGlimpseSection(this object o)
		{
			var section = o as GlimpseSection;
			if (section != null)
				return section;

			var instance = o as GlimpseSection.Instance;
			if (instance != null)
				return instance.Data;
			
			throw new InvalidCastException(String.Format("The object is not a glimpse root. Object is of type {0}.", o.GetType()));
		}
	}
}