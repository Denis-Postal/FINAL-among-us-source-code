using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct InstallModOptionsInternal : ISettable<InstallModOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_Mod;

		private int m_RemoveAfterExit;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public ModIdentifier? Mod
		{
			set
			{
			}
		}

		public bool RemoveAfterExit
		{
			set
			{
			}
		}

		public void Set(ref InstallModOptions other)
		{
		}

		public void Set(ref InstallModOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
