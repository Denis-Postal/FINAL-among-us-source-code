using System.Runtime.InteropServices;

namespace DiscordConnect
{
	public class Logging
	{
		[PreserveSig]
		private static extern void logging_init();

		public static void Init()
		{
		}
	}
}
