using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 20)]
	internal struct RectInternal : IGettable<Rect>, ISettable<Rect>, IDisposable
	{
		private int m_ApiVersion;

		private int m_X;

		private int m_Y;

		private uint m_Width;

		private uint m_Height;

		public int X
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int Y
		{
			get
			{
				return 0;
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

		public void Set(ref Rect other)
		{
		}

		public void Set(ref Rect? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Rect output)
		{
			output = default;
		}
	}
}
