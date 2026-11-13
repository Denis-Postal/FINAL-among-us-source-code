using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImaginationOverflow.UniversalDeepLinking
{
	[Serializable]
	public class PlatformLinkingConfiguration
	{
		[SerializeField]
		private List<LinkInformation> _domainProtocols;

		[SerializeField]
		private List<LinkInformation> _deepLinkingProtocols;

		[SerializeField]
		private bool _initialized;

		public bool IsInitialized
		{
			get
			{
				return false;
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

		public PlatformLinkingConfiguration(bool init = false)
		{
		}
	}
}
