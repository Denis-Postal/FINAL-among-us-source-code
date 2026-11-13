using System;
using System.Runtime.CompilerServices;

namespace ImaginationOverflow.UniversalDeepLinking.Providers
{
	public class WindowsLinkProvider : ILinkProvider
	{
		private readonly bool _steamBuild;

		private string _scheme;

		[CompilerGenerated]
		private Action<string> m__linkReceived;

		private event Action<string> _linkReceived
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

		public event Action<string> LinkReceived
		{
			add
			{
			}
			remove
			{
			}
		}

		public WindowsLinkProvider(bool steamBuild)
		{
		}

		public bool Initialize()
		{
			return false;
		}

		private string GetExe(bool fromSteam, string steamAppId, out string args)
		{
			args = null;
			return null;
		}

		private void CheckArguments()
		{
		}

		public void PollInfoAfterPause()
		{
		}

		protected virtual void OnLinkReceived(string obj)
		{
		}
	}
}
