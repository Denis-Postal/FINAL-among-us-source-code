using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct XSXXBLOptionsInitOptionsInternal : IGettable<XSXXBLOptionsInitOptions>, ISettable<XSXXBLOptionsInitOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ServiceConfigurationID;

		private int m_IsXboxGoldRequired;

		private int m_UseClassicGamertag;

		public Utf8String ServiceConfigurationID
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsXboxGoldRequired
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool UseClassicGamertag
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref XSXXBLOptionsInitOptions other)
		{
		}

		public void Set(ref XSXXBLOptionsInitOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XSXXBLOptionsInitOptions output)
		{
			output = default;
		}
	}
}
