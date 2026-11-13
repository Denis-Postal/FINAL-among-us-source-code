using System;

namespace ImaginationOverflow.UniversalDeepLinking
{
	public interface ILinkProvider
	{
		event Action<string> LinkReceived;

		bool Initialize();

		void PollInfoAfterPause();
	}
}
