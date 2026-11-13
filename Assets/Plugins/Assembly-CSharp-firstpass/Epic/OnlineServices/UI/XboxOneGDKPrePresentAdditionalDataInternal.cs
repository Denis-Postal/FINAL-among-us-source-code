using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct XboxOneGDKPrePresentAdditionalDataInternal : IGettable<XboxOneGDKPrePresentAdditionalData>, ISettable<XboxOneGDKPrePresentAdditionalData>, IDisposable
	{
		private int m_ApiVersion;

		private int m_ScreenRenderTargetResourceState;

		public int ScreenRenderTargetResourceState
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref XboxOneGDKPrePresentAdditionalData other)
		{
		}

		public void Set(ref XboxOneGDKPrePresentAdditionalData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKPrePresentAdditionalData output)
		{
			output = default;
		}
	}
}
