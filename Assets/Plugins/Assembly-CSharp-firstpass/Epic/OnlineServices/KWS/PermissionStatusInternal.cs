using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct PermissionStatusInternal : IGettable<PermissionStatus>, ISettable<PermissionStatus>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Name;

		private KWSPermissionStatus m_Status;

		public Utf8String Name
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public KWSPermissionStatus Status
		{
			get
			{
				return KWSPermissionStatus.Granted;
			}
			set
			{
			}
		}

		public void Set(ref PermissionStatus other)
		{
		}

		public void Set(ref PermissionStatus? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PermissionStatus output)
		{
			output = default;
		}
	}
}
