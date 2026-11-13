using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 1024)]
	[CallbackIdentity(349)]
	public struct OverlayBrowserProtocolNavigation_t
	{
		public const int k_iCallback = 349;

		private byte[] rgchURI_;

		public string rgchURI
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
	}
}
