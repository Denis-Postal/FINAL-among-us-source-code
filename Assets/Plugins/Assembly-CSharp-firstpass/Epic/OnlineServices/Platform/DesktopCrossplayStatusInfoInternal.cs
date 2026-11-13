using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct DesktopCrossplayStatusInfoInternal : IGettable<DesktopCrossplayStatusInfo>, ISettable<DesktopCrossplayStatusInfo>, IDisposable
	{
		private DesktopCrossplayStatus m_Status;

		private int m_ServiceInitResult;

		public DesktopCrossplayStatus Status
		{
			get
			{
				return DesktopCrossplayStatus.Ok;
			}
			set
			{
			}
		}

		public int ServiceInitResult
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref DesktopCrossplayStatusInfo other)
		{
		}

		public void Set(ref DesktopCrossplayStatusInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out DesktopCrossplayStatusInfo output)
		{
			output = default;
		}
	}
}
