using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	public struct LogEventParamPairParamValue
	{
		private AntiCheatCommonEventParamType m_ParamValueType;

		private IntPtr? m_ClientHandle;

		private Utf8String m_String;

		private uint? m_UInt32;

		private int? m_Int32;

		private ulong? m_UInt64;

		private long? m_Int64;

		private Vec3f m_Vec3f;

		private Quat m_Quat;

		private float? m_Float;

		public AntiCheatCommonEventParamType ParamValueType
		{
			get
			{
				return AntiCheatCommonEventParamType.Invalid;
			}
			private set
			{
			}
		}

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

		public static implicit operator LogEventParamPairParamValue(IntPtr value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(Utf8String value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(string value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(uint value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(int value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(ulong value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(long value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(Vec3f value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(Quat value)
		{
			return default;
		}

		public static implicit operator LogEventParamPairParamValue(float value)
		{
			return default;
		}

		internal void Set(ref LogEventParamPairParamValueInternal other)
		{
		}
	}
}
