using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PresenceModificationDataRecordIdInternal : IGettable<PresenceModificationDataRecordId>, ISettable<PresenceModificationDataRecordId>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		public Utf8String Key
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PresenceModificationDataRecordId other)
		{
		}

		public void Set(ref PresenceModificationDataRecordId? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PresenceModificationDataRecordId output)
		{
			output = default;
		}
	}
}
