using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 128)]
	internal struct LogPlayerTakeDamageOptionsInternal : ISettable<LogPlayerTakeDamageOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_VictimPlayerHandle;

		private IntPtr m_VictimPlayerPosition;

		private IntPtr m_VictimPlayerViewRotation;

		private IntPtr m_AttackerPlayerHandle;

		private IntPtr m_AttackerPlayerPosition;

		private IntPtr m_AttackerPlayerViewRotation;

		private int m_IsHitscanAttack;

		private int m_HasLineOfSight;

		private int m_IsCriticalHit;

		private uint m_HitBoneId_DEPRECATED;

		private float m_DamageTaken;

		private float m_HealthRemaining;

		private AntiCheatCommonPlayerTakeDamageSource m_DamageSource;

		private AntiCheatCommonPlayerTakeDamageType m_DamageType;

		private AntiCheatCommonPlayerTakeDamageResult m_DamageResult;

		private IntPtr m_PlayerUseWeaponData;

		private uint m_TimeSincePlayerUseWeaponMs;

		private IntPtr m_DamagePosition;

		private IntPtr m_AttackerPlayerViewPosition;

		public IntPtr VictimPlayerHandle
		{
			set
			{
			}
		}

		public Vec3f? VictimPlayerPosition
		{
			set
			{
			}
		}

		public Quat? VictimPlayerViewRotation
		{
			set
			{
			}
		}

		public IntPtr AttackerPlayerHandle
		{
			set
			{
			}
		}

		public Vec3f? AttackerPlayerPosition
		{
			set
			{
			}
		}

		public Quat? AttackerPlayerViewRotation
		{
			set
			{
			}
		}

		public bool IsHitscanAttack
		{
			set
			{
			}
		}

		public bool HasLineOfSight
		{
			set
			{
			}
		}

		public bool IsCriticalHit
		{
			set
			{
			}
		}

		public uint HitBoneId_DEPRECATED
		{
			set
			{
			}
		}

		public float DamageTaken
		{
			set
			{
			}
		}

		public float HealthRemaining
		{
			set
			{
			}
		}

		public AntiCheatCommonPlayerTakeDamageSource DamageSource
		{
			set
			{
			}
		}

		public AntiCheatCommonPlayerTakeDamageType DamageType
		{
			set
			{
			}
		}

		public AntiCheatCommonPlayerTakeDamageResult DamageResult
		{
			set
			{
			}
		}

		public LogPlayerUseWeaponData? PlayerUseWeaponData
		{
			set
			{
			}
		}

		public uint TimeSincePlayerUseWeaponMs
		{
			set
			{
			}
		}

		public Vec3f? DamagePosition
		{
			set
			{
			}
		}

		public Vec3f? AttackerPlayerViewPosition
		{
			set
			{
			}
		}

		public void Set(ref LogPlayerTakeDamageOptions other)
		{
		}

		public void Set(ref LogPlayerTakeDamageOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
