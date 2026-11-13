using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SetToggleFriendsButtonOptionsInternal : ISettable<SetToggleFriendsButtonOptions>, IDisposable
	{
		private int m_ApiVersion;

		private InputStateButtonFlags m_ButtonCombination;

		public InputStateButtonFlags ButtonCombination
		{
			set
			{
			}
		}

		public void Set(ref SetToggleFriendsButtonOptions other)
		{
		}

		public void Set(ref SetToggleFriendsButtonOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
