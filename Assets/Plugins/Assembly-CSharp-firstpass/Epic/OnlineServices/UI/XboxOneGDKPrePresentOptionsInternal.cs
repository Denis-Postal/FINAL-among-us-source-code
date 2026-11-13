using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XboxOneGDKPrePresentOptionsInternal : ISettable<XboxOneGDKPrePresentOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformSpecificData;

		public XboxOneGDKPrePresentOptionsPlatformSpecificData? PlatformSpecificData
		{
			set
			{
			}
		}

		public void Set(ref XboxOneGDKPrePresentOptions other)
		{
		}

		public void Set(ref XboxOneGDKPrePresentOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
