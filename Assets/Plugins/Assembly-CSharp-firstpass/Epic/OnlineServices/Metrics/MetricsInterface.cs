using System;

namespace Epic.OnlineServices.Metrics
{
	public sealed class MetricsInterface : Handle
	{
		public const int BeginplayersessionApiLatest = 1;

		public const int EndplayersessionApiLatest = 1;

		public MetricsInterface()
		{
		}

		public MetricsInterface(IntPtr innerHandle)
		{
		}

		public Result BeginPlayerSession(ref BeginPlayerSessionOptions options)
		{
			return Result.Success;
		}

		public Result EndPlayerSession(ref EndPlayerSessionOptions options)
		{
			return Result.Success;
		}
	}
}
