using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LogEventParamPairInternal : IGettable<LogEventParamPair>, ISettable<LogEventParamPair>, IDisposable
	{
		private LogEventParamPairParamValueInternal m_ParamValue;

		public LogEventParamPairParamValue ParamValue
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public void Set(ref LogEventParamPair other)
		{
		}

		public void Set(ref LogEventParamPair? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LogEventParamPair output)
		{
			output = default;
		}
	}
}
