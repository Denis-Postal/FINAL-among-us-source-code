using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct AudioInputStateCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioInputStateCallbackInfo>, ISettable<AudioInputStateCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private RTCAudioInputStatus m_Status;

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

		public ProductUserId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String RoomName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RTCAudioInputStatus Status
		{
			get
			{
				return RTCAudioInputStatus.Idle;
			}
			set
			{
			}
		}

		public void Set(ref AudioInputStateCallbackInfo other)
		{
		}

		public void Set(ref AudioInputStateCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AudioInputStateCallbackInfo output)
		{
			output = default;
		}
	}
}
