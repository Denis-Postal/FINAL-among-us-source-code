using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct AudioBeforeRenderCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioBeforeRenderCallbackInfo>, ISettable<AudioBeforeRenderCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_Buffer;

		private IntPtr m_ParticipantId;

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

		public AudioBuffer? Buffer
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ProductUserId ParticipantId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref AudioBeforeRenderCallbackInfo other)
		{
		}

		public void Set(ref AudioBeforeRenderCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AudioBeforeRenderCallbackInfo output)
		{
			output = default;
		}
	}
}
