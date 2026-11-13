using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 80)]
	internal struct InfoInternal : IGettable<Info>, ISettable<Info>, IDisposable
	{
		private int m_ApiVersion;

		private Status m_Status;

		private IntPtr m_UserId;

		private IntPtr m_ProductId;

		private IntPtr m_ProductVersion;

		private IntPtr m_Platform;

		private IntPtr m_RichText;

		private int m_RecordsCount;

		private IntPtr m_Records;

		private IntPtr m_ProductName;

		private IntPtr m_IntegratedPlatform;

		public Status Status
		{
			get
			{
				return Status.Offline;
			}
			set
			{
			}
		}

		public EpicAccountId UserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ProductId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ProductVersion
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Platform
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String RichText
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public DataRecord[] Records
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ProductName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String IntegratedPlatform
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref Info other)
		{
		}

		public void Set(ref Info? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Info output)
		{
			output = default;
		}
	}
}
