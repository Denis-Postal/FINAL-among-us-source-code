using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SetClientDetailsOptionsInternal : ISettable<SetClientDetailsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ClientHandle;

		private AntiCheatCommonClientFlags m_ClientFlags;

		private AntiCheatCommonClientInput m_ClientInputMethod;

		public IntPtr ClientHandle
		{
			set
			{
			}
		}

		public AntiCheatCommonClientFlags ClientFlags
		{
			set
			{
			}
		}

		public AntiCheatCommonClientInput ClientInputMethod
		{
			set
			{
			}
		}

		public void Set(ref SetClientDetailsOptions other)
		{
		}

		public void Set(ref SetClientDetailsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
