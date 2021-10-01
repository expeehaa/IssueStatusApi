using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueStatusApi {
	public static class Extensions {
		internal static readonly Dictionary<string, ComponentStatus> ComponentStatusMap = Enum.GetValues(typeof(ComponentStatus)).Cast<ComponentStatus>().ToDictionary(cs => cs.ToString().ToSnakeCase(" "), cs => cs);

		public static string ToSnakeCase(this string text, string delimiter = "_")
			=> string.Concat(text.Select((c, i) => i > 0 && char.IsUpper(c) ? $"{delimiter}{c}" : $"{c}")).ToLowerInvariant();
	}
}
