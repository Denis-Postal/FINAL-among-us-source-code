using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct PlayerSanctionInternal : IGettable<PlayerSanction>, ISettable<PlayerSanction>, IDisposable
	{
		private int m_ApiVersion;

		private long m_TimePlaced;

		private IntPtr m_Action;

		private long m_TimeExpires;

		private IntPtr m_ReferenceId;

		public long TimePlaced
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public Utf8String Action
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public long TimeExpires
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public Utf8String ReferenceId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PlayerSanction other)
		{
		}

		public void Set(ref PlayerSanction? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PlayerSanction output)
		{
			output = default;
		}
	}
}
