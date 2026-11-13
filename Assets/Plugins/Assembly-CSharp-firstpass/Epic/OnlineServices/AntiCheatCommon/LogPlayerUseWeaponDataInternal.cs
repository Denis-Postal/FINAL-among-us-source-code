using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct LogPlayerUseWeaponDataInternal : IGettable<LogPlayerUseWeaponData>, ISettable<LogPlayerUseWeaponData>, IDisposable
	{
		private IntPtr m_PlayerHandle;

		private IntPtr m_PlayerPosition;

		private IntPtr m_PlayerViewRotation;

		private int m_IsPlayerViewZoomed;

		private int m_IsMeleeAttack;

		private IntPtr m_WeaponName;

		public IntPtr PlayerHandle
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public Vec3f? PlayerPosition
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Quat? PlayerViewRotation
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsPlayerViewZoomed
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public Utf8String WeaponName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref LogPlayerUseWeaponData other)
		{
		}

		public void Set(ref LogPlayerUseWeaponData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LogPlayerUseWeaponData output)
		{
			output = default;
		}
	}
}
