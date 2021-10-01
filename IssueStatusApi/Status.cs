using System.Linq;

namespace IssueStatusApi {
	public class Status {
		public Component[] Components { get; set; }
		public Incident[] Incidents { get; set; }

		public IIssue[] AllIssues => Components.Cast<IIssue>().Concat(Incidents.Cast<IIssue>()).ToArray();
	}
}
