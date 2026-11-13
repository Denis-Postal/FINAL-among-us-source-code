using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 144)]
	internal struct OptionsInternal : ISettable<Options>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Reserved;

		private IntPtr m_ProductId;

		private IntPtr m_SandboxId;

		private ClientCredentialsInternal m_ClientCredentials;

		private int m_IsServer;

		private IntPtr m_EncryptionKey;

		private IntPtr m_OverrideCountryCode;

		private IntPtr m_OverrideLocaleCode;

		private IntPtr m_DeploymentId;

		private PlatformFlags m_Flags;

		private IntPtr m_CacheDirectory;

		private uint m_TickBudgetInMilliseconds;

		private IntPtr m_RTCOptions;

		private IntPtr m_IntegratedPlatformOptionsContainerHandle;

		private IntPtr m_SystemSpecificOptions;

		private IntPtr m_TaskNetworkTimeoutSeconds;

		public IntPtr Reserved
		{
			set
			{
			}
		}

		public Utf8String ProductId
		{
			set
			{
			}
		}

		public Utf8String SandboxId
		{
			set
			{
			}
		}

		public ClientCredentials ClientCredentials
		{
			set
			{
			}
		}

		public bool IsServer
		{
			set
			{
			}
		}

		public Utf8String EncryptionKey
		{
			set
			{
			}
		}

		public Utf8String OverrideCountryCode
		{
			set
			{
			}
		}

		public Utf8String OverrideLocaleCode
		{
			set
			{
			}
		}

		public Utf8String DeploymentId
		{
			set
			{
			}
		}

		public PlatformFlags Flags
		{
			set
			{
			}
		}

		public Utf8String CacheDirectory
		{
			set
			{
			}
		}

		public uint TickBudgetInMilliseconds
		{
			set
			{
			}
		}

		public RTCOptions? RTCOptions
		{
			set
			{
			}
		}

		public IntegratedPlatformOptionsContainer IntegratedPlatformOptionsContainerHandle
		{
			set
			{
			}
		}

		public IntPtr SystemSpecificOptions
		{
			set
			{
			}
		}

		public double? TaskNetworkTimeoutSeconds
		{
			set
			{
			}
		}

		public void Set(ref Options other)
		{
		}

		public void Set(ref Options? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
