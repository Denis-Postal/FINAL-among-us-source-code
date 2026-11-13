using System;

namespace DiscordConnect
{
	public class ErrorException : Exception
	{
		public readonly Result result;

		public ErrorException(Result result)
		{
		}
	}
}
