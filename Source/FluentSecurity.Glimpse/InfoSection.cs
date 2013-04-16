using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Glimpse.Core.Tab.Assist;

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
				var buildScript = new WebClient().DownloadString("https://raw.github.com/kristofferahl/FluentSecurity/master/build.ps1");
				if (!String.IsNullOrEmpty(buildScript))
				{
					var rows = buildScript.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					var filteredRows = rows.Where(row => row.Contains("=") && (row.Contains("$version") || row.Contains("$label")));
					var dictionary = new Dictionary<string, string>();
					foreach (var filteredRow in filteredRows)
					{
						var key = filteredRow.Split('=')[0].Trim();
						var value = filteredRow.Split('=')[1].Trim().Trim('\'');
						dictionary.Add(key, value);
					}
					var version = dictionary["$version"];
					var label = dictionary["$label"];

					var fullVersion = !String.IsNullOrEmpty(label)
						? String.Join("-", version, label)
						: version;
					return String.Format("FluentSecurity v. {0}.", fullVersion);
				}
			}
			catch { }
			return null;
			return "Failed to find available version";
		} 
	}
}