using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct LogPlayerUseAbilityOptionsInternal : ISettable<LogPlayerUseAbilityOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlayerHandle;

		private uint m_AbilityId;

		private uint m_AbilityDurationMs;

		private uint m_AbilityCooldownMs;

		public IntPtr PlayerHandle
		{
			set
			{
			}
		}

		public uint AbilityId
		{
			set
			{
			}
		}

		public uint AbilityDurationMs
		{
			set
			{
			}
		}

		public uint AbilityCooldownMs
		{
			set
			{
			}
		}

		public void Set(ref LogPlayerUseAbilityOptions other)
		{
		}

		public void Set(ref LogPlayerUseAbilityOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
