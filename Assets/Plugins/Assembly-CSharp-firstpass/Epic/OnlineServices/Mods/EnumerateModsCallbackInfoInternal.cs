using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct EnumerateModsCallbackInfoInternal : ICallbackInfoInternal, IGettable<EnumerateModsCallbackInfo>, ISettable<EnumerateModsCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_LocalUserId;

		private IntPtr m_ClientData;

		private ModEnumerationType m_Type;

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

		public EpicAccountId LocalUserId
		{
			get
			{
				return null;
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

		public ModEnumerationType Type
		{
			get
			{
				return ModEnumerationType.Installed;
			}
			set
			{
			}
		}

		public void Set(ref EnumerateModsCallbackInfo other)
		{
		}

		public void Set(ref EnumerateModsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out EnumerateModsCallbackInfo output)
		{
			output = default;
		}
	}
}
