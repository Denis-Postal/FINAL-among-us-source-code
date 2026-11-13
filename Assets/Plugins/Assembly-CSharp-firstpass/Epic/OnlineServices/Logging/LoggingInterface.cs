namespace Epic.OnlineServices.Logging
{
	public sealed class LoggingInterface
	{
		public static Result SetCallback(LogMessageFunc callback)
		{
			return Result.Success;
		}

		public static Result SetLogLevel(LogCategory logCategory, LogLevel logLevel)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(LogMessageFuncInternal))]
		internal static void LogMessageFuncInternalImplementation(ref LogMessageInternal message)
		{
		}
	}
}
