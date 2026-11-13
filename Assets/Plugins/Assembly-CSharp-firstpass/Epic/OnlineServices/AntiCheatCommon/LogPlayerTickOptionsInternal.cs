using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct LogPlayerTickOptionsInternal : ISettable<LogPlayerTickOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlayerHandle;

		private IntPtr m_PlayerPosition;

		private IntPtr m_PlayerViewRotation;

		private int m_IsPlayerViewZoomed;

		private float m_PlayerHealth;

		private AntiCheatCommonPlayerMovementState m_PlayerMovementState;

		private IntPtr m_PlayerViewPosition;

		public IntPtr PlayerHandle
		{
			set
			{
			}
		}

		public Vec3f? PlayerPosition
		{
			set
			{
			}
		}

		public Quat? PlayerViewRotation
		{
			set
			{
			}
		}

		public bool IsPlayerViewZoomed
		{
			set
			{
			}
		}

		public float PlayerHealth
		{
			set
			{
			}
		}

		public AntiCheatCommonPlayerMovementState PlayerMovementState
		{
			set
			{
			}
		}

		public Vec3f? PlayerViewPosition
		{
			set
			{
			}
		}

		public void Set(ref LogPlayerTickOptions other)
		{
		}

		public void Set(ref LogPlayerTickOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
