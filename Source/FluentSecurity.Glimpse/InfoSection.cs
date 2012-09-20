using System;
using System.Linq;
using System.Xml.Linq;
using Glimpse.Core.Plugin.Assist;

namespace FluentSecurity.Glimpse
{
	public class InfoSection
	{
		public static TabSection Create(ISecurityConfiguration configuration)
		{
			var section = new TabSection("Key", "Value");

			var availableVersion = TryGetVersionFromGithub();
			section.AddRow()
				.Column("Latest version of Fluent Security").Strong()
				.Column(availableVersion).Strong()
				.Selected();

			var loadedVersion = configuration.GetType().Assembly.FullName;
			section.AddRow()
				.Column("Loaded assembly")
				.Column(loadedVersion);

			section.AddRow()
				.Column("Website")
				.Column(@"<a href='http://fluentsecurity.net/'>http://fluentsecurity.net</a>").Raw();

			section.AddRow()
				.Column("Documentation")
				.Column(@"<a href='http://fluentsecurity.net/wiki'>http://fluentsecurity.net/wiki</a>").Raw();

			section.AddRow()
				.Column("Twitter")
				.Column(@"<a href='http://twitter.com/FluentSecurity'>@FluentSecurity</a>").Raw();

			return section;
		}

		private static string TryGetVersionFromGithub()
		{
			try
			{
				var xml = XDocument.Load("https://raw.github.com/kristofferahl/FluentSecurity/master/Build/Scripts/Build.build");
				var root = xml.Root;
				if (root != null)
				{
					var properties = root.Elements().Where(e => e.Name.LocalName == "property" && e.HasAttributes).ToList();
					if (properties.Any())
					{
						var versionProperty = properties.SingleOrDefault(p => p.FirstAttribute.Value == "project.version.label");
						if (versionProperty != null)
						{
							var versionAttribute = versionProperty.Attribute("value");
							if (versionAttribute != null)
								return String.Format("Fluent Security v. {0}", versionAttribute.Value);
						}
					}
				}
			}
			catch { }
			return "Failed to find available version";
		} 
	}
}