using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyClientIntegrityViolatedOptionsInternal : ISettable<AddNotifyClientIntegrityViolatedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyClientIntegrityViolatedOptions other)
		{
		}

		public void Set(ref AddNotifyClientIntegrityViolatedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
