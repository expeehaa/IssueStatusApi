namespace IssueStatusApi {
	public record Component(int Id, string Title, string Description, ComponentStatus Status) : IIssue;
}
