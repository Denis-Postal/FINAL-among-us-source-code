using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnClientAuthStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientAuthStatusChangedCallbackInfo>, ISettable<OnClientAuthStatusChangedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_ClientHandle;

		private AntiCheatCommonClientAuthStatus m_ClientAuthStatus;

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

		public AntiCheatCommonClientAuthStatus ClientAuthStatus
		{
			get
			{
				return AntiCheatCommonClientAuthStatus.Invalid;
			}
			set
			{
			}
		}

		public void Set(ref OnClientAuthStatusChangedCallbackInfo other)
		{
		}

		public void Set(ref OnClientAuthStatusChangedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnClientAuthStatusChangedCallbackInfo output)
		{
			output = default;
		}
	}
}
