using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XboxOneGDKIntegratedPlatformOptionsContainerAddOptionsInternal : ISettable<XboxOneGDKIntegratedPlatformOptionsContainerAddOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Options;

		public XboxOneGDKXBLOptions? Options
		{
			set
			{
			}
		}

		public void Set(ref XboxOneGDKIntegratedPlatformOptionsContainerAddOptions other)
		{
		}

		public void Set(ref XboxOneGDKIntegratedPlatformOptionsContainerAddOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
