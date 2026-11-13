using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnMessageToClientCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnMessageToClientCallbackInfo>, ISettable<OnMessageToClientCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_ClientHandle;

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

		public IntPtr ClientHandle
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

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

		public void Set(ref OnMessageToClientCallbackInfo other)
		{
		}

		public void Set(ref OnMessageToClientCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnMessageToClientCallbackInfo output)
		{
			output = default;
		}
	}
}
