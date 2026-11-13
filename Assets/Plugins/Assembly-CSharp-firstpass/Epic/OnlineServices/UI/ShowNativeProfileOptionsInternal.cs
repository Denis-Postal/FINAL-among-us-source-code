using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct ShowNativeProfileOptionsInternal : ISettable<ShowNativeProfileOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public EpicAccountId TargetUserId
		{
			set
			{
			}
		}

		public void Set(ref ShowNativeProfileOptions other)
		{
		}

		public void Set(ref ShowNativeProfileOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
