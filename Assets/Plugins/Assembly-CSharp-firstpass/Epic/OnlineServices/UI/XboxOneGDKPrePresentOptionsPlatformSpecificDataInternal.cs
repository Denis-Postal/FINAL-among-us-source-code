using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct XboxOneGDKPrePresentOptionsPlatformSpecificDataInternal : ICallbackInfoInternal, IGettable<XboxOneGDKPrePresentOptionsPlatformSpecificData>, ISettable<XboxOneGDKPrePresentOptionsPlatformSpecificData>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ScreenRenderTarget;

		private IntPtr m_SubmitGraphicsCommands;

		private IntPtr m_ClientData;

		private IntPtr m_ScreenRenderTargetSubRegion;

		private IntPtr m_AdditionalData;

		private static XboxOneGDKSubmitGraphicsCommandsCallbackInternal s_SubmitGraphicsCommands;

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

		public static XboxOneGDKSubmitGraphicsCommandsCallbackInternal SubmitGraphicsCommands => null;

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

		public XboxOneGDKPrePresentAdditionalData? AdditionalData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref XboxOneGDKPrePresentOptionsPlatformSpecificData other)
		{
		}

		public void Set(ref XboxOneGDKPrePresentOptionsPlatformSpecificData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKPrePresentOptionsPlatformSpecificData output)
		{
			output = default;
		}
	}
}
