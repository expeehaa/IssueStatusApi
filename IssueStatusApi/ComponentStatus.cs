using System;

namespace IssueStatusApi {
	[Flags]
	public enum ComponentStatus {
		Unknown = 0,
		Operational = 1 << 0,
		PerformanceIssues = 1 << 1,
		PartialOutage = 1 << 2,
		MajorOutage = 1 << 3,
	}
}
