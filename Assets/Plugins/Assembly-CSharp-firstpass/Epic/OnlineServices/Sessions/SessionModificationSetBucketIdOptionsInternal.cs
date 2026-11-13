using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SessionModificationSetBucketIdOptionsInternal : ISettable<SessionModificationSetBucketIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_BucketId;

		public Utf8String BucketId
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationSetBucketIdOptions other)
		{
		}

		public void Set(ref SessionModificationSetBucketIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
