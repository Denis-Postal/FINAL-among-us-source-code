using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct AttributeDataInternal : IGettable<AttributeData>, ISettable<AttributeData>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		private AttributeDataValueInternal m_Value;

		public Utf8String Key
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public AttributeDataValue Value
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public void Set(ref AttributeData other)
		{
		}

		public void Set(ref AttributeData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AttributeData output)
		{
			output = default;
		}
	}
}
