using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)2, Pack = 8, Size = 24)]
	internal struct LogEventParamPairParamValueInternal : IGettable<LogEventParamPairParamValue>, ISettable<LogEventParamPairParamValue>, IDisposable
	{
		[FieldOffset(0)]
		private AntiCheatCommonEventParamType m_ParamValueType;

		[FieldOffset(8)]
		private IntPtr m_ClientHandle;

		[FieldOffset(8)]
		private IntPtr m_String;

		[FieldOffset(8)]
		private uint m_UInt32;

		[FieldOffset(8)]
		private int m_Int32;

		[FieldOffset(8)]
		private ulong m_UInt64;

		[FieldOffset(8)]
		private long m_Int64;

		[FieldOffset(8)]
		private Vec3fInternal m_Vec3f;

		[FieldOffset(8)]
		private QuatInternal m_Quat;

		[FieldOffset(8)]
		private float m_Float;

		public IntPtr? ClientHandle
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String String
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint? UInt32
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int? Int32
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ulong? UInt64
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public long? Int64
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Vec3f Vec3f
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public Quat Quat
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public float? Float
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref LogEventParamPairParamValue other)
		{
		}

		public void Set(ref LogEventParamPairParamValue? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LogEventParamPairParamValue output)
		{
			output = default;
		}
	}
}
