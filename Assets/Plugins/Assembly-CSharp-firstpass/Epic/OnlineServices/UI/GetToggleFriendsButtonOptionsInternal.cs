using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetToggleFriendsButtonOptionsInternal : ISettable<GetToggleFriendsButtonOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetToggleFriendsButtonOptions other)
		{
		}

		public void Set(ref GetToggleFriendsButtonOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
