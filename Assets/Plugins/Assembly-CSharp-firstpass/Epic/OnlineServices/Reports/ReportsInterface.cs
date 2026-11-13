using System;

namespace Epic.OnlineServices.Reports
{
	public sealed class ReportsInterface : Handle
	{
		public const int ReportcontextMaxLength = 4096;

		public const int ReportmessageMaxLength = 512;

		public const int SendplayerbehaviorreportApiLatest = 2;

		public ReportsInterface()
		{
		}

		public ReportsInterface(IntPtr innerHandle)
		{
		}

		public void SendPlayerBehaviorReport(ref SendPlayerBehaviorReportOptions options, object clientData, OnSendPlayerBehaviorReportCompleteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnSendPlayerBehaviorReportCompleteCallbackInternal))]
		internal static void OnSendPlayerBehaviorReportCompleteCallbackInternalImplementation(ref SendPlayerBehaviorReportCompleteCallbackInfoInternal data)
		{
		}
	}
}
