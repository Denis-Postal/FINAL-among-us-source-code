using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyJoinGameAcceptedOptionsInternal : ISettable<AddNotifyJoinGameAcceptedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyJoinGameAcceptedOptions other)
		{
		}

		public void Set(ref AddNotifyJoinGameAcceptedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
