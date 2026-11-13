using Newtonsoft.Json;
using System;

public class ServerInfo
{
	public readonly string Name;

	public readonly string Ip;

	public readonly ushort Port;

	public readonly bool UseDtls;

	public int Players;

	public int ConnectionFailures;

	[JsonIgnore]
	public string HttpUrl
	{
		get
		{
			if (Ip.IndexOf("://", StringComparison.Ordinal) != -1)
			{
				return $"{Ip}:{Port}/";
			}
			return $"https://{Ip}:{Port}/";
		}
	}

	public ServerInfo(string name, string ip, ushort port, bool useDtls = true)
	{
		Name = name ?? "Custom";
		Ip = ip;
		Port = port;
		UseDtls = useDtls;
	}

	// TODO(verify): exact format string was lost in the decompile (4 format args); diagnostic only.
	public override string ToString()
	{
		return $"{Name}:{Ip}:{Port} (dtls={UseDtls})";
	}

	public override int GetHashCode()
	{
		return Ip.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is ServerInfo serverInfo)
		{
			return serverInfo.Ip.Equals(Ip);
		}
		return false;
	}
}
