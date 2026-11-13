using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct OnDisplaySettingsUpdatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnDisplaySettingsUpdatedCallbackInfo>, ISettable<OnDisplaySettingsUpdatedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private int m_IsVisible;

		private int m_IsExclusiveInput;

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

		public bool IsVisible
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool IsExclusiveInput
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref OnDisplaySettingsUpdatedCallbackInfo other)
		{
		}

		public void Set(ref OnDisplaySettingsUpdatedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnDisplaySettingsUpdatedCallbackInfo output)
		{
			output = default;
		}
	}
}
