using System;
using System.Runtime.CompilerServices;

namespace ImaginationOverflow.UniversalDeepLinking.Providers
{
	public class DummyLinkProvider : ILinkProvider
	{
		[CompilerGenerated]
		private Action<string> m_LinkReceived;

		public event Action<string> LinkReceived
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

		public bool Initialize()
		{
			return false;
		}

		public void PollInfoAfterPause()
		{
		}

		protected virtual void OnLinkReceived(string obj)
		{
		}
	}
}
