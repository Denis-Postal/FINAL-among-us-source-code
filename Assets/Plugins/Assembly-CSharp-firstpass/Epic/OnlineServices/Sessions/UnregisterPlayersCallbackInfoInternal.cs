using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct UnregisterPlayersCallbackInfoInternal : ICallbackInfoInternal, IGettable<UnregisterPlayersCallbackInfo>, ISettable<UnregisterPlayersCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_UnregisteredPlayers;

		private uint m_UnregisteredPlayersCount;

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

		public ProductUserId[] UnregisteredPlayers
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref UnregisterPlayersCallbackInfo other)
		{
		}

		public void Set(ref UnregisterPlayersCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UnregisterPlayersCallbackInfo output)
		{
			output = default;
		}
	}
}
