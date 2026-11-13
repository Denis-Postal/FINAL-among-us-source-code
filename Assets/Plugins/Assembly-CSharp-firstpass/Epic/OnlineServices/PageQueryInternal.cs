using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 12)]
	internal struct PageQueryInternal : IGettable<PageQuery>, ISettable<PageQuery>, IDisposable
	{
		private int m_ApiVersion;

		private int m_StartIndex;

		private int m_MaxCount;

		public int StartIndex
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int MaxCount
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref PageQuery other)
		{
		}

		public void Set(ref PageQuery? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PageQuery output)
		{
			output = default;
		}
	}
}
