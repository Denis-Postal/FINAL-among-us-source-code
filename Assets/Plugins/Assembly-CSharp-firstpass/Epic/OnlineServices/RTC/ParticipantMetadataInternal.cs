using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct ParticipantMetadataInternal : IGettable<ParticipantMetadata>, ISettable<ParticipantMetadata>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		private IntPtr m_Value;

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

		public Utf8String Value
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref ParticipantMetadata other)
		{
		}

		public void Set(ref ParticipantMetadata? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ParticipantMetadata output)
		{
			output = default;
		}
	}
}
