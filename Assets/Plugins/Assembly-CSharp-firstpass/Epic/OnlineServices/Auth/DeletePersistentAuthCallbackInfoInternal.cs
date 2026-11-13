using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct DeletePersistentAuthCallbackInfoInternal : ICallbackInfoInternal, IGettable<DeletePersistentAuthCallbackInfo>, ISettable<DeletePersistentAuthCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

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

		public void Set(ref DeletePersistentAuthCallbackInfo other)
		{
		}

		public void Set(ref DeletePersistentAuthCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out DeletePersistentAuthCallbackInfo output)
		{
			output = default;
		}
	}
}
