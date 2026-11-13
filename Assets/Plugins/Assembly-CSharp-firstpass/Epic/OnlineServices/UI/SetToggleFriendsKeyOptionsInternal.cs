using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SetToggleFriendsKeyOptionsInternal : ISettable<SetToggleFriendsKeyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private KeyCombination m_KeyCombination;

		public KeyCombination KeyCombination
		{
			set
			{
			}
		}

		public void Set(ref SetToggleFriendsKeyOptions other)
		{
		}

		public void Set(ref SetToggleFriendsKeyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
