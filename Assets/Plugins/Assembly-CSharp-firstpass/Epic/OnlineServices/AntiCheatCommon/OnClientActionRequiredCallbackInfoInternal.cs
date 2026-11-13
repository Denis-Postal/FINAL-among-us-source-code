using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnClientActionRequiredCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientActionRequiredCallbackInfo>, ISettable<OnClientActionRequiredCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_ClientHandle;

		private AntiCheatCommonClientAction m_ClientAction;

		private AntiCheatCommonClientActionReason m_ActionReasonCode;

		private IntPtr m_ActionReasonDetailsString;

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

		public AntiCheatCommonClientAction ClientAction
		{
			get
			{
				return AntiCheatCommonClientAction.Invalid;
			}
			set
			{
			}
		}

		public AntiCheatCommonClientActionReason ActionReasonCode
		{
			get
			{
				return AntiCheatCommonClientActionReason.Invalid;
			}
			set
			{
			}
		}

		public Utf8String ActionReasonDetailsString
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnClientActionRequiredCallbackInfo other)
		{
		}

		public void Set(ref OnClientActionRequiredCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnClientActionRequiredCallbackInfo output)
		{
			output = default;
		}
	}
}
