using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct GetStatCountOptionsInternal : ISettable<GetStatCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public void Set(ref GetStatCountOptions other)
		{
		}

		public void Set(ref GetStatCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
