using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct UpdateSessionOptionsInternal : ISettable<UpdateSessionOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionModificationHandle;

		public SessionModification SessionModificationHandle
		{
			set
			{
			}
		}

		public void Set(ref UpdateSessionOptions other)
		{
		}

		public void Set(ref UpdateSessionOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
