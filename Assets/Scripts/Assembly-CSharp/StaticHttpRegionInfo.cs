using System;
using System.Linq;

public class StaticHttpRegionInfo : IRegionInfo
{
	public string Name { get; }

	public string PingServer { get; }

	public ServerInfo[] Servers { get; }

	public string TargetServer { get; }

	public StringNames TranslateName { get; }

	public StaticHttpRegionInfo(string name, StringNames translateName, string pingServer, ServerInfo[] servers, string targetServer = null)
	{
		Name = name;
		PingServer = pingServer;
		Servers = servers;
		TargetServer = targetServer;
		TranslateName = translateName;
	}

	public IRegionInfo Duplicate()
	{
		ServerInfo[] copy = new ServerInfo[Servers.Length];
		for (int i = 0; i < copy.Length; i++)
		{
			ServerInfo s = Servers[i];
			copy[i] = new ServerInfo(s.Name, s.Ip, s.Port, s.UseDtls)
			{
				ConnectionFailures = s.ConnectionFailures,
				Players = s.Players
			};
		}
		return new StaticHttpRegionInfo(Name, TranslateName, PingServer, copy, TargetServer);
	}

	public bool Validate()
	{
		if (string.IsNullOrWhiteSpace(PingServer) || Servers == null)
		{
			return false;
		}
		return Servers.All((ServerInfo s) => s != null && !string.IsNullOrWhiteSpace(s.Ip));
	}

	public override int GetHashCode()
	{
		return Name == null ? 0 : Name.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return obj is StaticHttpRegionInfo other && Name != null && Name.Equals(other.Name);
	}
}
