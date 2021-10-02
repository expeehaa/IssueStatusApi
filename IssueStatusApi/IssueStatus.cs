using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueStatusApi {
	public class IssueStatus {
		private const string LabelIssueStatus = "issue status";
		private const string LabelComponent = "component";
		private const string LabelIncident = "incident";

		private readonly GitHubClient _githubClient;

		/// <summary>
		/// Create a new instance of IssueStatusApi.IssueStatus.
		/// </summary>
		/// <param name="githubApiToken">Token for authentication with the Github API. Requires token scope "public_repo" for accessing public repositories.</param>
		public IssueStatus(string githubApiToken = null) {
			_githubClient = new GitHubClient(new ProductHeaderValue("IssueStatusApi.Net"));

			if(!string.IsNullOrWhiteSpace(githubApiToken)) {
				_githubClient.Credentials = new Credentials(githubApiToken);
			}
		}

		/// <summary>
		/// Get a Status object containing all Issue Status issues from the Github repository this instance was initialized with.
		/// 
		/// Only one request is sent to the Github API.
		/// </summary>
		/// <param name="repositoryOwner">Owner of the repository.</param>
		/// <param name="repositoryName">Name of the repository.</param>
		/// <returns>An object containing all Issue Status issues.</returns>
		public async Task<Status> GetStatus(string repositoryOwner, string repositoryName) {
			var request = new RepositoryIssueRequest {
				State = ItemStateFilter.All,
			};
			request.Labels.Add(LabelIssueStatus);

			var issues = await _githubClient.Issue.GetAllForRepository(repositoryOwner, repositoryName, request);
			var components = issues.Where(i => i.Labels.Any(l => l.Name.Equals(LabelIssueStatus, StringComparison.OrdinalIgnoreCase)) && i.Labels.Any(l => l.Name.Equals(LabelComponent, StringComparison.OrdinalIgnoreCase))).Select(issue => new Component(issue.Number, issue.Title, issue.Body, GetComponentStatusFromLabel(issue.Labels))).ToArray();
			var incidents = issues.Where(i => i.Labels.Any(l => l.Name.Equals(LabelIssueStatus, StringComparison.OrdinalIgnoreCase)) && i.Labels.Any(l => l.Name.Equals(LabelIncident, StringComparison.OrdinalIgnoreCase))).Select(issue => new Incident(issue.Number, issue.Title, issue.Body, issue.CreatedAt, issue.ClosedAt)).ToArray();

			return new Status {
				Components = components,
				Incidents = incidents,
			};
		}

		/// <summary>
		/// Compare two sets of Issue Status issues.
		/// </summary>
		/// <param name="oldStatus">The old Issue Status issues.</param>
		/// <param name="newStatus">The new Issue Status issues.</param>
		/// <returns>
		/// An array of the changed Issue Status issues.
		/// 
		/// Each array element contains both the old and the new issue for direct comparison.
		/// If the issue was added/deleted, one may be null.
		/// </returns>
		public static (IIssue Old, IIssue New)[] CompareStatus(Status oldStatus, Status newStatus) {
			return oldStatus.AllIssues.Concat(newStatus.AllIssues).Select(i => i.Id).Distinct().Select(id => (oldStatus.AllIssues.FirstOrDefault(issue => id == issue.Id), newStatus.AllIssues.FirstOrDefault(issue => id == issue.Id))).Where((s) => s.Item1 != s.Item2).ToArray();
		}

		private static ComponentStatus GetComponentStatusFromLabel(IReadOnlyList<Label> labels) {
			var status = Extensions.ComponentStatusMap.Where(kv => labels.Any(l => l.Name.Equals(kv.Key, StringComparison.OrdinalIgnoreCase))).Select(kv => kv.Value);

			return status.Any() ? status.Aggregate((c1, c2) => c1 | c2) : ComponentStatus.Unknown;
		}
	}
}
