using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics
{
	[StructLayout((LayoutKind)2, Pack = 4, Size = 12)]
	internal struct BeginPlayerSessionOptionsAccountIdInternal : IGettable<BeginPlayerSessionOptionsAccountId>, ISettable<BeginPlayerSessionOptionsAccountId>, IDisposable
	{
		[FieldOffset(0)]
		private MetricsAccountIdType m_AccountIdType;

		[FieldOffset(4)]
		private IntPtr m_Epic;

		[FieldOffset(4)]
		private IntPtr m_External;

		public EpicAccountId Epic
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String External
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref BeginPlayerSessionOptionsAccountId other)
		{
		}

		public void Set(ref BeginPlayerSessionOptionsAccountId? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out BeginPlayerSessionOptionsAccountId output)
		{
			output = default;
		}
	}
}
