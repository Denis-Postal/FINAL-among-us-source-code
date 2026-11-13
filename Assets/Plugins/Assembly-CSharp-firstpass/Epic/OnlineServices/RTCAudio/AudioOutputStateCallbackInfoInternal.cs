using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct AudioOutputStateCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioOutputStateCallbackInfo>, ISettable<AudioOutputStateCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private RTCAudioOutputStatus m_Status;

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

		public RTCAudioOutputStatus Status
		{
			get
			{
				return RTCAudioOutputStatus.Idle;
			}
			set
			{
			}
		}

		public void Set(ref AudioOutputStateCallbackInfo other)
		{
		}

		public void Set(ref AudioOutputStateCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AudioOutputStateCallbackInfo output)
		{
			output = default;
		}
	}
}
