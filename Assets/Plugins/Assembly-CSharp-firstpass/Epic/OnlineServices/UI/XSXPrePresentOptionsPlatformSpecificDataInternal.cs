using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct XSXPrePresentOptionsPlatformSpecificDataInternal : ICallbackInfoInternal, IGettable<XSXPrePresentOptionsPlatformSpecificData>, ISettable<XSXPrePresentOptionsPlatformSpecificData>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ScreenRenderTarget;

		private IntPtr m_SubmitGraphicsCommands;

		private IntPtr m_ClientData;

		private IntPtr m_ScreenRenderTargetSubRegion;

		private IntPtr m_AdditionalData;

		private static XSXSubmitGraphicsCommandsCallbackInternal s_SubmitGraphicsCommands;

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

		public static XSXSubmitGraphicsCommandsCallbackInternal SubmitGraphicsCommands => null;

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

		public XSXPrePresentAdditionalData? AdditionalData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref XSXPrePresentOptionsPlatformSpecificData other)
		{
		}

		public void Set(ref XSXPrePresentOptionsPlatformSpecificData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XSXPrePresentOptionsPlatformSpecificData output)
		{
			output = default;
		}
	}
}
