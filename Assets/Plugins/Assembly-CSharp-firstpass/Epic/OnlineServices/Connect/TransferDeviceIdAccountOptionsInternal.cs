using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct TransferDeviceIdAccountOptionsInternal : ISettable<TransferDeviceIdAccountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PrimaryLocalUserId;

		private IntPtr m_LocalDeviceUserId;

		private IntPtr m_ProductUserIdToPreserve;

		public ProductUserId PrimaryLocalUserId
		{
			set
			{
			}
		}

		public ProductUserId LocalDeviceUserId
		{
			set
			{
			}
		}

		public ProductUserId ProductUserIdToPreserve
		{
			set
			{
			}
		}

		public void Set(ref TransferDeviceIdAccountOptions other)
		{
		}

		public void Set(ref TransferDeviceIdAccountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
