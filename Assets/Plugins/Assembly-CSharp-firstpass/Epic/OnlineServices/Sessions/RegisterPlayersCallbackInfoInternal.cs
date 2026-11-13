using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct RegisterPlayersCallbackInfoInternal : ICallbackInfoInternal, IGettable<RegisterPlayersCallbackInfo>, ISettable<RegisterPlayersCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_RegisteredPlayers;

		private uint m_RegisteredPlayersCount;

		private IntPtr m_SanctionedPlayers;

		private uint m_SanctionedPlayersCount;

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

		public ProductUserId[] RegisteredPlayers
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ProductUserId[] SanctionedPlayers
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref RegisterPlayersCallbackInfo other)
		{
		}

		public void Set(ref RegisterPlayersCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RegisterPlayersCallbackInfo output)
		{
			output = default;
		}
	}
}
