using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct RegisterEventParamDefInternal : IGettable<RegisterEventParamDef>, ISettable<RegisterEventParamDef>, IDisposable
	{
		private IntPtr m_ParamName;

		private AntiCheatCommonEventParamType m_ParamType;

		public Utf8String ParamName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public AntiCheatCommonEventParamType ParamType
		{
			get
			{
				return AntiCheatCommonEventParamType.Invalid;
			}
			set
			{
			}
		}

		public void Set(ref RegisterEventParamDef other)
		{
		}

		public void Set(ref RegisterEventParamDef? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RegisterEventParamDef output)
		{
			output = default;
		}
	}
}
