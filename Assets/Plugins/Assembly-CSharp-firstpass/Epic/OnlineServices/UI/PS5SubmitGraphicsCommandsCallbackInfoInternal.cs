using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct PS5SubmitGraphicsCommandsCallbackInfoInternal : ICallbackInfoInternal, IGettable<PS5SubmitGraphicsCommandsCallbackInfo>, ISettable<PS5SubmitGraphicsCommandsCallbackInfo>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_BuffersCount;

		private IntPtr m_DispatchCommandBuffers;

		private IntPtr m_DispatchCommandBuffersSizes;

		private IntPtr m_ClientData;

		public uint BuffersCount
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public IntPtr DispatchCommandBuffers
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public uint? DispatchCommandBuffersSizes
		{
			get
			{
				return null;
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

		public void Set(ref PS5SubmitGraphicsCommandsCallbackInfo other)
		{
		}

		public void Set(ref PS5SubmitGraphicsCommandsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS5SubmitGraphicsCommandsCallbackInfo output)
		{
			output = default;
		}
	}
}
