using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct PS5PrePresentOptionsPlatformSpecificDataInternal : ICallbackInfoInternal, IGettable<PS5PrePresentOptionsPlatformSpecificData>, ISettable<PS5PrePresentOptionsPlatformSpecificData>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ScreenRenderTarget;

		private IntPtr m_SubmitGraphicsCommands;

		private IntPtr m_ClientData;

		private IntPtr m_ScreenRenderTargetSubRegion;

		private static PS5SubmitGraphicsCommandsCallbackInternal s_SubmitGraphicsCommands;

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

		public static PS5SubmitGraphicsCommandsCallbackInternal SubmitGraphicsCommands => null;

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

		public void Set(ref PS5PrePresentOptionsPlatformSpecificData other)
		{
		}

		public void Set(ref PS5PrePresentOptionsPlatformSpecificData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS5PrePresentOptionsPlatformSpecificData output)
		{
			output = default;
		}
	}
}
