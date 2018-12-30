using System;
using System.Drawing;

namespace Ademund.Utils.ColorExtensions
{
	public static class ColorExtensions
	{
		public static Color HalfMix(this Color one, Color two)
		{
			return Color.FromArgb(
				(one.A + two.A) >> 1,
				(one.R + two.R) >> 1,
				(one.G + two.G) >> 1,
				(one.B + two.B) >> 1);
		}
	}
}
