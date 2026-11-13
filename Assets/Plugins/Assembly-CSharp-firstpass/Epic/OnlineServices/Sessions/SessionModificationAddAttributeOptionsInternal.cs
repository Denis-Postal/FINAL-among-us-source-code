using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SessionModificationAddAttributeOptionsInternal : ISettable<SessionModificationAddAttributeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionAttribute;

		private SessionAttributeAdvertisementType m_AdvertisementType;

		public AttributeData? SessionAttribute
		{
			set
			{
			}
		}

		public SessionAttributeAdvertisementType AdvertisementType
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationAddAttributeOptions other)
		{
		}

		public void Set(ref SessionModificationAddAttributeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
