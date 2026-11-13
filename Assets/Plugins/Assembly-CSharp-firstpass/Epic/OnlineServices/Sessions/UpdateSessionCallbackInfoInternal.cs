using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct UpdateSessionCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateSessionCallbackInfo>, ISettable<UpdateSessionCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_SessionName;

		private IntPtr m_SessionId;

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

		public Utf8String SessionName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String SessionId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref UpdateSessionCallbackInfo other)
		{
		}

		public void Set(ref UpdateSessionCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UpdateSessionCallbackInfo output)
		{
			output = default;
		}
	}
}
