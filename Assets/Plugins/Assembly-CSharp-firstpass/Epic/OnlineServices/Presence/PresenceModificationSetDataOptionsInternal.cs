using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PresenceModificationSetDataOptionsInternal : ISettable<PresenceModificationSetDataOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_RecordsCount;

		private IntPtr m_Records;

		public DataRecord[] Records
		{
			set
			{
			}
		}

		public void Set(ref PresenceModificationSetDataOptions other)
		{
		}

		public void Set(ref PresenceModificationSetDataOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
