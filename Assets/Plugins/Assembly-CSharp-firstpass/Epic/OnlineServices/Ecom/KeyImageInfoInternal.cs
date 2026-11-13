using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct KeyImageInfoInternal : IGettable<KeyImageInfo>, ISettable<KeyImageInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Type;

		private IntPtr m_Url;

		private uint m_Width;

		private uint m_Height;

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

		public Utf8String Url
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint Width
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public uint Height
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref KeyImageInfo other)
		{
		}

		public void Set(ref KeyImageInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out KeyImageInfo output)
		{
			output = default;
		}
	}
}
