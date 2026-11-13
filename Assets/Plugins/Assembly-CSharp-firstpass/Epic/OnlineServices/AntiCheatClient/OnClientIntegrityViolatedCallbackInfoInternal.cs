using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnClientIntegrityViolatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientIntegrityViolatedCallbackInfo>, ISettable<OnClientIntegrityViolatedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private AntiCheatClientViolationType m_ViolationType;

		private IntPtr m_ViolationMessage;

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

		public AntiCheatClientViolationType ViolationType
		{
			get
			{
				return AntiCheatClientViolationType.Invalid;
			}
			set
			{
			}
		}

		public Utf8String ViolationMessage
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnClientIntegrityViolatedCallbackInfo other)
		{
		}

		public void Set(ref OnClientIntegrityViolatedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnClientIntegrityViolatedCallbackInfo output)
		{
			output = default;
		}
	}
}
