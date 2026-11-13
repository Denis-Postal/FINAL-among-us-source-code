using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ImaginationOverflow.UniversalDeepLinking
{
	public sealed class DeepLinkManager
	{
		public class UrlEncodingParser : Dictionary<string, string>
		{
			private string Url { get; set; }

			public string Query { get; private set; }

			public UrlEncodingParser(string queryStringOrUrl = null)
			{
			}

			public void SetValues(string key, IEnumerable<string> values)
			{
			}

			public Dictionary<string, string> Parse(string query)
			{
				return null;
			}

			public override string ToString()
			{
				return null;
			}
		}

		[CompilerGenerated]
		private LinkActivationHandler m__activated;

		private ILinkProvider _currProvider;

		private GameObject _go;

		private string _storedActivation;

		public static DeepLinkManager Instance { get; private set; }

		public bool IsSteamBuild { get; set; }

		public event LinkActivationHandler LinkActivated
		{
			add
			{
			}
			remove
			{
			}
		}

		private event LinkActivationHandler _activated
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		static DeepLinkManager()
		{
		}

		private DeepLinkManager()
		{
		}

		public void ManuallyTriggerDeepLink(string args)
		{
		}

		private void RegisterIfNecessary()
		{
		}

		private void CreatePauseGameObject()
		{
		}

		private void _currProvider_LinkReceived(string s)
		{
		}

		private void StoreActivation(string s)
		{
		}

		private bool OnActivated(string s)
		{
			return false;
		}

		private LinkActivation CreateLinkActivation(string s)
		{
			return null;
		}

		internal void GameCameFromPause()
		{
		}
	}
}
