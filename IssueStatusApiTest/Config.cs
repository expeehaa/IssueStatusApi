using System.IO;
using System.Text.Json;

namespace IssueStatusApiTest {
	public class Config {
		public string GithubApiToken { get; set; } = "";

		public static Config GetConfig() {
			var assemblyFile = new FileInfo(typeof(Config).Assembly.Location);
			var configFile = new FileInfo(Path.Combine(assemblyFile.DirectoryName, "config.json"));

			if(!configFile.Exists) {
				using var writer = configFile.CreateText();
				writer.WriteLine(JsonSerializer.Serialize(new Config(), new JsonSerializerOptions {
					WriteIndented = true,
				}));
				writer.Close();
			}

			using var sr = new StreamReader(configFile.OpenRead());
			var text = sr.ReadToEnd();
			return JsonSerializer.Deserialize<Config>(text);
		}
	}
}
