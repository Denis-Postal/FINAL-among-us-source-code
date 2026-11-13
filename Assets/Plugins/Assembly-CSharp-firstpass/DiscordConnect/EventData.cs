using System.Runtime.InteropServices;

namespace DiscordConnect
{
	[StructLayout((LayoutKind)2)]
	public struct EventData
	{
		[FieldOffset(0)]
		public Result ActivitySetFailed;

		[FieldOffset(0)]
		public ActivityJoinData ActivityJoin;
	}
}
