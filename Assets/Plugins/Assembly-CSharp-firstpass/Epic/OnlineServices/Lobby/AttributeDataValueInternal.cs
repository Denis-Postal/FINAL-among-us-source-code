using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)2, Pack = 8, Size = 16)]
	internal struct AttributeDataValueInternal : IGettable<AttributeDataValue>, ISettable<AttributeDataValue>, IDisposable
	{
		[FieldOffset(0)]
		private long m_AsInt64;

		[FieldOffset(0)]
		private double m_AsDouble;

		[FieldOffset(0)]
		private int m_AsBool;

		[FieldOffset(0)]
		private IntPtr m_AsUtf8;

		[FieldOffset(8)]
		private AttributeType m_ValueType;

		public long? AsInt64
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public double? AsDouble
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool? AsBool
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String AsUtf8
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref AttributeDataValue other)
		{
		}

		public void Set(ref AttributeDataValue? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AttributeDataValue output)
		{
			output = default;
		}
	}
}
