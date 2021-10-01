using IssueStatusApi;
using System;
using System.Linq;

namespace IssueStatusApiTest {
	public class Program {
		public static void Main() {
			var config = Config.GetConfig();

			var issueStatus = new IssueStatus("tadhglewis", "issue-status", config.GithubApiToken);

			var status = issueStatus.GetStatus().GetAwaiter().GetResult();

			Console.WriteLine($"Components\n{string.Join("\n", status.Components.Select(c => c.ToString()))}\n");
			Console.WriteLine($"Incidents\n{string.Join("\n", status.Incidents.OrderByDescending(i => i.CreatedAt).Select(i => i.ToString()))}");

			var noStatusChange = IssueStatus.CompareStatus(status, status);

			Console.ReadLine();
		}
	}
}
