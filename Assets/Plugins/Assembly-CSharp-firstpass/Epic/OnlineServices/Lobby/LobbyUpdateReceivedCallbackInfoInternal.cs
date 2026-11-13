using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LobbyUpdateReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyUpdateReceivedCallbackInfo>, ISettable<LobbyUpdateReceivedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LobbyId;

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public Utf8String LobbyId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref LobbyUpdateReceivedCallbackInfo other)
		{
		}

		public void Set(ref LobbyUpdateReceivedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LobbyUpdateReceivedCallbackInfo output)
		{
			output = default;
		}
	}
}
