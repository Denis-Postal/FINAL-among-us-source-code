using UnityEngine;

namespace Innersloth.DebugTool
{
	public static class DebugColorExtensions
	{
		public static Color SetAlpha(this Color c, float alpha)
		{
			return new Color(c.r, c.g, c.b, alpha);
		}
	}
}
