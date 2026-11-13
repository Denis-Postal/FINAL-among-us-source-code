using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XboxOneGDKSubmitGraphicsCommandsDataCommandInternal : IGettable<XboxOneGDKSubmitGraphicsCommandsDataCommand>, ISettable<XboxOneGDKSubmitGraphicsCommandsDataCommand>, IDisposable
	{
		private XboxOneGDKSubmitGraphicsCommandsDataAction m_Action;

		private uint m_DeviceObjectCount;

		private ulong m_ExtraParameter;

		public XboxOneGDKSubmitGraphicsCommandsDataAction Action
		{
			get
			{
				return XboxOneGDKSubmitGraphicsCommandsDataAction.SgcdaExecutegraphicscommandlists;
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

		public void Set(ref XboxOneGDKSubmitGraphicsCommandsDataCommand other)
		{
		}

		public void Set(ref XboxOneGDKSubmitGraphicsCommandsDataCommand? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKSubmitGraphicsCommandsDataCommand output)
		{
			output = default;
		}
	}
}
