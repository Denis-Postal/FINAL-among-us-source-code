using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnMessageToServerCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnMessageToServerCallbackInfo>, ISettable<OnMessageToServerCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_MessageData;

		private uint m_MessageDataSizeBytes;

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

		public ArraySegment<byte> MessageData
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public void Set(ref OnMessageToServerCallbackInfo other)
		{
		}

		public void Set(ref OnMessageToServerCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnMessageToServerCallbackInfo output)
		{
			output = default;
		}
	}
}
