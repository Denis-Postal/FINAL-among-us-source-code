using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct PS4PrePresentOptionsPlatformSpecificDataInternal : ICallbackInfoInternal, IGettable<PS4PrePresentOptionsPlatformSpecificData>, ISettable<PS4PrePresentOptionsPlatformSpecificData>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ScreenRenderTarget;

		private IntPtr m_SubmitGraphicsCommands;

		private IntPtr m_ClientData;

		private IntPtr m_ScreenRenderTargetSubRegion;

		private static PS4SubmitGraphicsCommandsCallbackInternal s_SubmitGraphicsCommands;

		public IntPtr ScreenRenderTarget
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public static PS4SubmitGraphicsCommandsCallbackInternal SubmitGraphicsCommands => null;

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

		public Rect? ScreenRenderTargetSubRegion
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PS4PrePresentOptionsPlatformSpecificData other)
		{
		}

		public void Set(ref PS4PrePresentOptionsPlatformSpecificData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS4PrePresentOptionsPlatformSpecificData output)
		{
			output = default;
		}
	}
}
