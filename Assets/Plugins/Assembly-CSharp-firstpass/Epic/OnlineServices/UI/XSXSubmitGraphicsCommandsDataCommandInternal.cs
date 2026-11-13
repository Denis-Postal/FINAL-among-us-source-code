using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XSXSubmitGraphicsCommandsDataCommandInternal : IGettable<XSXSubmitGraphicsCommandsDataCommand>, ISettable<XSXSubmitGraphicsCommandsDataCommand>, IDisposable
	{
		private XSXSubmitGraphicsCommandsDataAction m_Action;

		private uint m_DeviceObjectCount;

		private ulong m_ExtraParameter;

		public XSXSubmitGraphicsCommandsDataAction Action
		{
			get
			{
				return XSXSubmitGraphicsCommandsDataAction.SgcdaExecutegraphicscommandlists;
			}
			set
			{
			}
		}

		public uint DeviceObjectCount
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public ulong ExtraParameter
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public void Set(ref XSXSubmitGraphicsCommandsDataCommand other)
		{
		}

		public void Set(ref XSXSubmitGraphicsCommandsDataCommand? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XSXSubmitGraphicsCommandsDataCommand output)
		{
			output = default;
		}
	}
}
