using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	[CallbackIdentity(5304)]
	public struct ChangeNumOpenSlotsCallback_t
	{
		public const int k_iCallback = 5304;

		public EResult m_eResult;
	}
}
