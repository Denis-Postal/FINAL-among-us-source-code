using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LogPlayerUseWeaponOptionsInternal : ISettable<LogPlayerUseWeaponOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UseWeaponData;

		public LogPlayerUseWeaponData? UseWeaponData
		{
			set
			{
			}
		}

		public void Set(ref LogPlayerUseWeaponOptions other)
		{
		}

		public void Set(ref LogPlayerUseWeaponOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
