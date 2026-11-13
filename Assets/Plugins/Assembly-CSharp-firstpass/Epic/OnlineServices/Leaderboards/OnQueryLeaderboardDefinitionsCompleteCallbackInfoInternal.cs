using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryLeaderboardDefinitionsCompleteCallbackInfo>, ISettable<OnQueryLeaderboardDefinitionsCompleteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		public Result ResultCode
		{
			get
			{
				return Result.Success;
			}
			set
			{
			}
		}

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public void Set(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfo other)
		{
		}

		public void Set(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnQueryLeaderboardDefinitionsCompleteCallbackInfo output)
		{
			output = default;
		}
	}
}
