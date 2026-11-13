using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryAgeGateCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryAgeGateCallbackInfo>, ISettable<QueryAgeGateCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_CountryCode;

		private uint m_AgeOfConsent;

		public Result ResultCode
		{
			get
			{
				return Result.Success;
			}
			set
			{
			}
		}

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

		public Utf8String CountryCode
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint AgeOfConsent
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref QueryAgeGateCallbackInfo other)
		{
		}

		public void Set(ref QueryAgeGateCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryAgeGateCallbackInfo output)
		{
			output = default;
		}
	}
}
