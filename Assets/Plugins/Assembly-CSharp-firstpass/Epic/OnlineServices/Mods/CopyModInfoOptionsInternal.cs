using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyModInfoOptionsInternal : ISettable<CopyModInfoOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private ModEnumerationType m_Type;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public ModEnumerationType Type
		{
			set
			{
			}
		}

		public void Set(ref CopyModInfoOptions other)
		{
		}

		public void Set(ref CopyModInfoOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
