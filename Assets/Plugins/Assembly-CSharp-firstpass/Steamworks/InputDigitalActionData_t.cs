using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 1, Size = 2)]
	public struct InputDigitalActionData_t
	{
		public byte bState;

		public byte bActive;
	}
}
