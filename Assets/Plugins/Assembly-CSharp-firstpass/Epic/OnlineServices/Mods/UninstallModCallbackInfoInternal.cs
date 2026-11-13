using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct UninstallModCallbackInfoInternal : ICallbackInfoInternal, IGettable<UninstallModCallbackInfo>, ISettable<UninstallModCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_LocalUserId;

		private IntPtr m_ClientData;

		private IntPtr m_Mod;

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

		public ModIdentifier? Mod
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref UninstallModCallbackInfo other)
		{
		}

		public void Set(ref UninstallModCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UninstallModCallbackInfo output)
		{
			output = default;
		}
	}
}
