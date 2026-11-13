using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OptionsInternal : IGettable<Options>, ISettable<Options>, IDisposable
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

		public IntPtr InitOptions
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public void Set(ref Options other)
		{
		}

		public void Set(ref Options? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Options output)
		{
			output = default;
		}
	}
}
