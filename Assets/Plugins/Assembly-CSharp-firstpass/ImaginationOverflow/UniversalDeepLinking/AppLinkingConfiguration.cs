using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImaginationOverflow.UniversalDeepLinking
{
	[Serializable]
	public class AppLinkingConfiguration
	{
		[SerializeField]
		private string _steamId;

		[SerializeField]
		private string _displayName;

		[SerializeField]
		private PlatformLinkingConfiguration _globalConfiguration;

		[SerializeField]
		private PlatformLinkingConfiguration[] _customDeepLinkingProtocols;

		public string SteamId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public List<LinkInformation> DeepLinkingProtocols
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public List<LinkInformation> DomainProtocols
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public PlatformLinkingConfiguration[] CustomDeepLinkingProtocols
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string DisplayName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal void EnsureAllPlats()
		{
		}

		public List<LinkInformation> GetPlatformDeepLinkingProtocols(SupportedPlatforms plat, bool includeDefault = false)
		{
			return null;
		}

		public List<LinkInformation> GetPlatformDomainProtocols(SupportedPlatforms plat, bool includeDefault = false)
		{
			return null;
		}

		private List<LinkInformation> GetCustomOrDefault(SupportedPlatforms plat, Func<PlatformLinkingConfiguration, List<LinkInformation>> func, bool includeDefault, List<LinkInformation> global)
		{
			return null;
		}

		public List<LinkInformation> GetCustomDeepLinkingProtocols(SupportedPlatforms value)
		{
			return null;
		}

		public List<LinkInformation> GetCustomDomainAssociation(SupportedPlatforms value)
		{
			return null;
		}

		public void ActivatePlatformOverride(SupportedPlatforms value)
		{
		}

		public void DeactivatePlatformOverride(SupportedPlatforms value)
		{
		}

		private PlatformLinkingConfiguration[] EnsureAllPlats(PlatformLinkingConfiguration[] value)
		{
			return null;
		}
	}
}
