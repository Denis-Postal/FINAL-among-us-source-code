using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	[CallbackIdentity(5211)]
	public struct RequestPlayersForGameProgressCallback_t
	{
		public const int k_iCallback = 5211;

		public EResult m_eResult;

		public ulong m_ullSearchID;
	}
}
