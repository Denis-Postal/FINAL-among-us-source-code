using UnityEngine;

namespace Innersloth.DebugTool
{
	public struct DebugLogMessage
	{
		public readonly string LogString;

		public readonly string Stacktrace;

		public readonly LogType LogType;

		public DebugLogMessage(string logString, string stacktrace, LogType logType)
		{
			LogString = logString;
			Stacktrace = stacktrace;
			LogType = logType;
		}
	}
}
