using System;

namespace IssueStatusApi {
	public record Incident(int Id, string Title, string Description, DateTimeOffset CreatedAt, DateTimeOffset? EndedAt) : IIssue {
		public bool Active => EndedAt is null;
	}
}
