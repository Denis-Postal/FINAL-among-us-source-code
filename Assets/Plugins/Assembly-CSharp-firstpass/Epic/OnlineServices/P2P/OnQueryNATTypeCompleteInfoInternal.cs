using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnQueryNATTypeCompleteInfoInternal : ICallbackInfoInternal, IGettable<OnQueryNATTypeCompleteInfo>, ISettable<OnQueryNATTypeCompleteInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private NATType m_NATType;

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

		public NATType NATType
		{
			get
			{
				return NATType.Unknown;
			}
			set
			{
			}
		}

		public void Set(ref OnQueryNATTypeCompleteInfo other)
		{
		}

		public void Set(ref OnQueryNATTypeCompleteInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnQueryNATTypeCompleteInfo output)
		{
			output = default;
		}
	}
}
