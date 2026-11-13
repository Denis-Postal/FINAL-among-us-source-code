using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Reports
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct SendPlayerBehaviorReportOptionsInternal : ISettable<SendPlayerBehaviorReportOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ReporterUserId;

		private IntPtr m_ReportedUserId;

		private PlayerReportsCategory m_Category;

		private IntPtr m_Message;

		private IntPtr m_Context;

		public ProductUserId ReporterUserId
		{
			set
			{
			}
		}

		public ProductUserId ReportedUserId
		{
			set
			{
			}
		}

		public PlayerReportsCategory Category
		{
			set
			{
			}
		}

		public Utf8String Message
		{
			set
			{
			}
		}

		public Utf8String Context
		{
			set
			{
			}
		}

		public void Set(ref SendPlayerBehaviorReportOptions other)
		{
		}

		public void Set(ref SendPlayerBehaviorReportOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
