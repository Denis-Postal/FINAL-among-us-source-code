using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct PS4OptionsInternal : IGettable<PS4Options>, ISettable<PS4Options>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Type;

		private IntegratedPlatformManagementFlags m_Flags;

		private IntPtr m_InitOptions;

		public Utf8String Type
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntegratedPlatformManagementFlags Flags
		{
			get
			{
				return (IntegratedPlatformManagementFlags)0;
			}
			set
			{
			}
		}

		public PS4OptionsInitOptions? InitOptions
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PS4Options other)
		{
		}

		public void Set(ref PS4Options? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS4Options output)
		{
			output = default;
		}
	}
}
