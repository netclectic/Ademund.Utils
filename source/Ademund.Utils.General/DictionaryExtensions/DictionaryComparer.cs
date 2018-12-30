using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ademund.Utils.DictionaryExtensions
{
    public static class DictionaryComparer
	{
		public static bool CheckForEquality<T>(this IEnumerable<T> source, IEnumerable<T> destination)
		{
			Debug.Assert(source != null, "source != null");
			Debug.Assert(destination != null, "destination != null");

			var enumerable = source as IList<T> ?? source.ToList();
			var members = destination as IList<T> ?? destination.ToList();

			if (enumerable.Count != members.Count)
			{
				return false;
			}

			var dictionary = new Dictionary<T, int>();

			foreach (var value in enumerable)
			{
				if (!dictionary.ContainsKey(value))
				{
					dictionary[value] = 1;
				}
				else
				{
					dictionary[value]++;
				}
			}

			foreach (var member in members)
			{
				if (!dictionary.ContainsKey(member))
				{
					return false;
				}

				dictionary[member]--;
			}

			return dictionary.All(kvp => kvp.Value == 0);
		}
	}
}
