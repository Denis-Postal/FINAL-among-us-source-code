using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AcknowledgeEventIdOptionsInternal : ISettable<AcknowledgeEventIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private ulong m_UiEventId;

		private Result m_Result;

		public ulong UiEventId
		{
			set
			{
			}
		}

		public Result Result
		{
			set
			{
			}
		}

		public void Set(ref AcknowledgeEventIdOptions other)
		{
		}

		public void Set(ref AcknowledgeEventIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
