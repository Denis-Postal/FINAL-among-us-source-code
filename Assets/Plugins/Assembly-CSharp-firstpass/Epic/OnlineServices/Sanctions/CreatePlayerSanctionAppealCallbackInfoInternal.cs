using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CreatePlayerSanctionAppealCallbackInfoInternal : ICallbackInfoInternal, IGettable<CreatePlayerSanctionAppealCallbackInfo>, ISettable<CreatePlayerSanctionAppealCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_ReferenceId;

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

		public Utf8String ReferenceId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref CreatePlayerSanctionAppealCallbackInfo other)
		{
		}

		public void Set(ref CreatePlayerSanctionAppealCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out CreatePlayerSanctionAppealCallbackInfo output)
		{
			output = default;
		}
	}
}
