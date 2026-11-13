using System;

// Offline/community-server remake: this client does not use Innersloth's EOS authentication and must never talk to Innersloth's (or Epic's)
// live services - doing so with a modified client is against their rules and can get the player's account banned.
// Everything that sends HTTP requests to a matchmaker goes through RetryableWebRequest, which refuses blocked hosts.
public static class NetworkGuard
{
	private static readonly string[] BlockedDomains = new string[]
	{
		"among.us", "innersloth.com", "innersloth.io", "epicgames.com", "epicgames.dev", "epicgames.io"
	};

	public const string OfficialServerWarning = "Innersloth's official servers (North America / Europe / Asia) require their EOS login, which this client does not use. "
		+ "Connecting a custom client to them can get your account banned, so they are disabled. Pick a community server instead.";

	public static bool IsBlockedHost(string host)
	{
		if (string.IsNullOrEmpty(host))
		{
			return false;
		}
		host = host.Trim().ToLowerInvariant();
		foreach (string domain in BlockedDomains)
		{
			if (host == domain || host.EndsWith("." + domain, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsBlockedUrl(string urlOrHost)
	{
		if (string.IsNullOrEmpty(urlOrHost))
		{
			return false;
		}
		if (urlOrHost.IndexOf("://", StringComparison.Ordinal) < 0)
		{
			int slash = urlOrHost.IndexOf('/');
			int colon = urlOrHost.IndexOf(':');
			int end = slash >= 0 ? slash : urlOrHost.Length;
			if (colon >= 0 && colon < end)
			{
				end = colon;
			}
			return IsBlockedHost(urlOrHost.Substring(0, end));
		}
		return Uri.TryCreate(urlOrHost, UriKind.Absolute, out Uri uri) && IsBlockedHost(uri.Host);
	}
}
