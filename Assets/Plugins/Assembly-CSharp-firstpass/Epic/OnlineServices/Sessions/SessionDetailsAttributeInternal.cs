using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SessionDetailsAttributeInternal : IGettable<SessionDetailsAttribute>, ISettable<SessionDetailsAttribute>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Data;

		private SessionAttributeAdvertisementType m_AdvertisementType;

		public AttributeData? Data
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public SessionAttributeAdvertisementType AdvertisementType
		{
			get
			{
				return SessionAttributeAdvertisementType.DontAdvertise;
			}
			set
			{
			}
		}

		public void Set(ref SessionDetailsAttribute other)
		{
		}

		public void Set(ref SessionDetailsAttribute? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SessionDetailsAttribute output)
		{
			output = default;
		}
	}
}
