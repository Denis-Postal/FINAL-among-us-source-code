using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyStatByNameOptionsInternal : ISettable<CopyStatByNameOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private IntPtr m_Name;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public Utf8String Name
		{
			set
			{
			}
		}

		public void Set(ref CopyStatByNameOptions other)
		{
		}

		public void Set(ref CopyStatByNameOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
