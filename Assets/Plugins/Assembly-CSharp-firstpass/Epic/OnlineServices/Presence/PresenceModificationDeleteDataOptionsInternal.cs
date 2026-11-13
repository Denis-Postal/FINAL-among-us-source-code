using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PresenceModificationDeleteDataOptionsInternal : ISettable<PresenceModificationDeleteDataOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_RecordsCount;

		private IntPtr m_Records;

		public PresenceModificationDataRecordId[] Records
		{
			set
			{
			}
		}

		public void Set(ref PresenceModificationDeleteDataOptions other)
		{
		}

		public void Set(ref PresenceModificationDeleteDataOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
