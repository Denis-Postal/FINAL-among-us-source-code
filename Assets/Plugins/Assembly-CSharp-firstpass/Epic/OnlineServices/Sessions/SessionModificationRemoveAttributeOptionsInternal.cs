using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SessionModificationRemoveAttributeOptionsInternal : ISettable<SessionModificationRemoveAttributeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		public Utf8String Key
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationRemoveAttributeOptions other)
		{
		}

		public void Set(ref SessionModificationRemoveAttributeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
