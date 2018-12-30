using System.Diagnostics;

namespace Ademund.Utils.IntExtensions
{
    public static class IntExtensions
	{
		public static int InRange(this int x, int lo, int hi)
		{
			Debug.Assert(lo <= hi);
			return x < lo ? lo : (x > hi ? hi : x);
		}

		public static bool IsInRange(this int x, int lo, int hi)
		{
			return x >= lo && x <= hi;
		}
	}
}