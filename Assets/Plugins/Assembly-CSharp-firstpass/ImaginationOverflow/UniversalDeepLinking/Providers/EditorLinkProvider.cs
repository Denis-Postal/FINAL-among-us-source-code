using System;
using System.Runtime.CompilerServices;

namespace ImaginationOverflow.UniversalDeepLinking.Providers
{
	public class EditorLinkProvider : ILinkProvider
	{
		private static EditorLinkProvider _instance;

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

		public static void SimulateLink(string link)
		{
		}
	}
}
