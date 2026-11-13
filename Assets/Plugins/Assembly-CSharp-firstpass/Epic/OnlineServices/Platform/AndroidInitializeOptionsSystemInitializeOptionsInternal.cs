using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct AndroidInitializeOptionsSystemInitializeOptionsInternal : IGettable<AndroidInitializeOptionsSystemInitializeOptions>, ISettable<AndroidInitializeOptionsSystemInitializeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Reserved;

		private IntPtr m_OptionalInternalDirectory;

		private IntPtr m_OptionalExternalDirectory;

		public IntPtr Reserved
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public Utf8String OptionalInternalDirectory
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String OptionalExternalDirectory
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref AndroidInitializeOptionsSystemInitializeOptions other)
		{
		}

		public void Set(ref AndroidInitializeOptionsSystemInitializeOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AndroidInitializeOptionsSystemInitializeOptions output)
		{
			output = default;
		}
	}
}
