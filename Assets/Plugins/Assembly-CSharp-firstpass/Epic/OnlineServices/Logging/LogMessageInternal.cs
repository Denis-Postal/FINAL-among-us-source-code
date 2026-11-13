using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Logging
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LogMessageInternal : IGettable<LogMessage>, ISettable<LogMessage>, IDisposable
	{
		private IntPtr m_Category;

		private IntPtr m_Message;

		private LogLevel m_Level;

		public Utf8String Category
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Message
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LogLevel Level
		{
			get
			{
				return LogLevel.Off;
			}
			set
			{
			}
		}

		public void Set(ref LogMessage other)
		{
		}

		public void Set(ref LogMessage? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LogMessage output)
		{
			output = default;
		}
	}
}
