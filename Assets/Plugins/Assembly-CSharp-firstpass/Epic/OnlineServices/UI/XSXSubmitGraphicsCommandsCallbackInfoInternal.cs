using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct XSXSubmitGraphicsCommandsCallbackInfoInternal : ICallbackInfoInternal, IGettable<XSXSubmitGraphicsCommandsCallbackInfo>, ISettable<XSXSubmitGraphicsCommandsCallbackInfo>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_CommandCount;

		private IntPtr m_Commands;

		private uint m_DeviceObjectCount;

		private IntPtr m_DeviceObjects;

		private IntPtr m_ClientData;

		public XSXSubmitGraphicsCommandsDataCommand[] Commands
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ArraySegment<byte>[] DeviceObjects
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

		public void Set(ref XSXSubmitGraphicsCommandsCallbackInfo other)
		{
		}

		public void Set(ref XSXSubmitGraphicsCommandsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XSXSubmitGraphicsCommandsCallbackInfo output)
		{
			output = default;
		}
	}
}
