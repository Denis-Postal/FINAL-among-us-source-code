using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SubmitSnapshotCallbackInfoInternal : ICallbackInfoInternal, IGettable<SubmitSnapshotCallbackInfo>, ISettable<SubmitSnapshotCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private uint m_SnapshotId;

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

		public uint SnapshotId
		{
			get
			{
				return 0u;
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

		public void Set(ref SubmitSnapshotCallbackInfo other)
		{
		}

		public void Set(ref SubmitSnapshotCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SubmitSnapshotCallbackInfo output)
		{
			output = default;
		}
	}
}
