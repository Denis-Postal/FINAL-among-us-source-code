using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ProgressionSnapshot
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct BeginSnapshotOptionsInternal : ISettable<BeginSnapshotOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref BeginSnapshotOptions other)
		{
		}

		public void Set(ref BeginSnapshotOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
