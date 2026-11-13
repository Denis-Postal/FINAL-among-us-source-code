using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct PS4SubmitGraphicsCommandsCallbackInfoInternal : ICallbackInfoInternal, IGettable<PS4SubmitGraphicsCommandsCallbackInfo>, ISettable<PS4SubmitGraphicsCommandsCallbackInfo>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_BuffersCount;

		private IntPtr m_DispatchCommandBuffers;

		private IntPtr m_DispatchCommandBuffersSizes;

		private IntPtr m_ClientData;

		private IntPtr m_DispatchCommandBuffersReturnPatches;

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

		public IntPtr DispatchCommandBuffersReturnPatches
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public void Set(ref PS4SubmitGraphicsCommandsCallbackInfo other)
		{
		}

		public void Set(ref PS4SubmitGraphicsCommandsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS4SubmitGraphicsCommandsCallbackInfo output)
		{
			output = default;
		}
	}
}
