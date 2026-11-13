using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct XboxOneGDKSubmitGraphicsCommandsCallbackInfoInternal : ICallbackInfoInternal, IGettable<XboxOneGDKSubmitGraphicsCommandsCallbackInfo>, ISettable<XboxOneGDKSubmitGraphicsCommandsCallbackInfo>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_CommandCount;

		private IntPtr m_Commands;

		private uint m_DeviceObjectCount;

		private IntPtr m_DeviceObjects;

		private IntPtr m_ClientData;

		public XboxOneGDKSubmitGraphicsCommandsDataCommand[] Commands
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

		public void Set(ref XboxOneGDKSubmitGraphicsCommandsCallbackInfo other)
		{
		}

		public void Set(ref XboxOneGDKSubmitGraphicsCommandsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKSubmitGraphicsCommandsCallbackInfo output)
		{
			output = default;
		}
	}
}
