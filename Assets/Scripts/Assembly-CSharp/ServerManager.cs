using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hazel;
using Hazel.UPnP;
using Innersloth.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ServerManager : DestroyableSingleton<ServerManager>
{
	[JsonObject]
	private struct JsonServerData
	{
		public int CurrentRegionIdx;

		public IRegionInfo[] Regions;

		internal void CleanAndMerge(IRegionInfo[] defaultRegions)
		{
			List<IRegionInfo> list = Regions.Where((IRegionInfo r) => r.Validate()).ToList();
			for (int num = defaultRegions.Length - 1; num >= 0; num--)
			{
				IRegionInfo region = defaultRegions[num];
				int num2 = list.FindIndex((IRegionInfo r) => r.Name.Equals(region.Name, StringComparison.OrdinalIgnoreCase));
				if (num2 == -1)
				{
					list.Insert(0, region);
				}
				else
				{
					list[num2] = region;
				}
			}
			Regions = list.ToArray();
		}
	}

	private struct PingWrapper
	{
		public IRegionInfo Region;

		public Ping Ping;

		public PingWrapper(IRegionInfo region, Ping ping)
		{
			Region = region;
			Ping = ping;
		}
	}

	public static readonly IRegionInfo[] DefaultRegions = new IRegionInfo[3]
	{
		new StaticHttpRegionInfo("North America", StringNames.ServerNA, "matchmaker.among.us", new ServerInfo[1]
		{
			new ServerInfo("Http-1", "https://matchmaker.among.us", 443, useDtls: false)
		}),
		new StaticHttpRegionInfo("Europe", StringNames.ServerEU, "matchmaker-eu.among.us", new ServerInfo[1]
		{
			new ServerInfo("Http-1", "https://matchmaker-eu.among.us", 443, useDtls: false)
		}),
		new StaticHttpRegionInfo("Asia", StringNames.ServerAS, "matchmaker-as.among.us", new ServerInfo[1]
		{
			new ServerInfo("Http-1", "https://matchmaker-as.among.us", 443, useDtls: false)
		})
	};

	private const bool useDtls = false;

	private const float PingTimeoutSeconds = 5f;

	private string serverInfoFileJson;

	private UpdateState state;

	public IRegionInfo CurrentRegion { get; private set; }

	public ServerInfo CurrentUdpServer { get; private set; }

	public bool IsHttp => CurrentRegion is StaticHttpRegionInfo;

	public string TargetServer => CurrentRegion.TargetServer;

	public IRegionInfo[] AvailableRegions { get; private set; }

	private ServerInfo[] AvailableServers => CurrentRegion.Servers;

	public string UdpNetAddress => CurrentUdpServer.Ip;

	public ushort UdpNetPort => CurrentUdpServer.Port;

	public bool UdpUseDtls => CurrentUdpServer.UseDtls;

	internal void AddOrUpdateRegion(IRegionInfo newRegion)
	{
		if (DefaultRegions.All((IRegionInfo r) => !r.Name.Equals(newRegion.Name, StringComparison.OrdinalIgnoreCase)))
		{
			IEnumerable<IRegionInfo> source = AvailableRegions.Where((IRegionInfo r) => !r.Name.Equals(newRegion.Name, StringComparison.OrdinalIgnoreCase));
			if (source.Count() == AvailableRegions.Length)
			{
				Debug.Log("Added new region: " + newRegion.Name);
				AvailableRegions = AvailableRegions.Append(newRegion).ToArray();
			}
			else
			{
				Debug.Log("Updated existing region: " + newRegion.Name);
				AvailableRegions = source.Append(newRegion).ToArray();
			}
			CurrentRegion = newRegion;
			CurrentUdpServer = newRegion.Servers.Random();
			SaveServers();
		}
	}

	public override void Awake()
	{
		base.Awake();
		if (!(DestroyableSingleton<ServerManager>.Instance != this))
		{
			serverInfoFileJson = Path.Combine(PlatformPaths.persistentDataPath, "regionInfo.json");
			LoadServers();
			Task.Run((Action)HandleUpnp);
		}
	}

	private void HandleUpnp()
	{
		try
		{
			using UPnPHelper uPnPHelper = new UPnPHelper(NullLogger.Instance);
			try
			{
				uPnPHelper.DeleteForwardingRule(22024);
			}
			catch
			{
			}
			try
			{
				uPnPHelper.DeleteForwardingRule(22023);
			}
			catch
			{
			}
		}
		catch
		{
		}
	}

	[ContextMenu("Reselect Server")]
	internal void ReselectServer()
	{
		Debug.Log("ServerManager::ReselectServer");
		CurrentRegion = CurrentRegion ?? DefaultRegions[0].Duplicate();
		if (AvailableServers.All((ServerInfo s) => s.Players == 0))
		{
			AvailableServers.Shuffle();
		}
		CurrentUdpServer = (from s in AvailableServers
			orderby s.ConnectionFailures, s.Players
			select s).First();
		Debug.Log($"Selected server: {CurrentUdpServer}");
		state = UpdateState.Success;
	}

	public IEnumerator ReselectRegionFromDefaults()
	{
		Debug.Log("ServerManager::ReselectRegionFromDefaults");
		AvailableRegions = MergeCommunityRegions(DefaultRegions);
		List<Task<IPAddress[]>> dnsLookups = new List<Task<IPAddress[]>>();
		for (int i = 0; i < DefaultRegions.Length; i++)
		{
			IRegionInfo regionInfo = DefaultRegions[i];
			dnsLookups.Add(Dns.GetHostAddressesAsync(regionInfo.PingServer));
		}
		while (dnsLookups.Any((Task<IPAddress[]> task) => !task.IsCompleted))
		{
			yield return null;
		}
		List<PingWrapper> pings = new List<PingWrapper>();
		for (int num = 0; num < DefaultRegions.Length; num++)
		{
			IRegionInfo regionInfo2 = DefaultRegions[num];
			if (!(regionInfo2.PingServer == "127.0.0.1"))
			{
				IPAddress[] result;
				try
				{
					result = dnsLookups[num].ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
				}
				catch (Exception ex)
				{
					Debug.LogError("DNS could not resolve " + regionInfo2.PingServer + " for reason: " + ex.Message);
					continue;
				}
				if (result == null || result.Length == 0)
				{
					Debug.LogError("DNS - no IPs resolved for " + regionInfo2.PingServer);
				}
				else
				{
					pings.Add(new PingWrapper(regionInfo2, new Ping(result.Random().ToString())));
				}
			}
		}
		float timeElapsedSeconds = 0f;
		while (pings.Count > 0 && timeElapsedSeconds < 5f && !pings.Any((PingWrapper p) => p.Ping.isDone && p.Ping.time >= 0))
		{
			yield return null;
			timeElapsedSeconds += Time.deltaTime;
		}
		IRegionInfo regionInfo3 = DefaultRegions.First();
		int num2 = int.MaxValue;
		foreach (PingWrapper item in pings)
		{
			if (item.Ping.isDone && item.Ping.time >= 0)
			{
				Debug.Log("Ping time: " + item.Region.Name + " @ " + item.Ping.time);
				if (item.Ping.time < num2)
				{
					regionInfo3 = item.Region;
					num2 = item.Ping.time;
				}
			}
			item.Ping.DestroyPing();
		}
		CurrentRegion = regionInfo3.Duplicate();
		ReselectServer();
		SaveServers();
	}

	public IEnumerator WaitForServers()
	{
		while (state == UpdateState.Connecting)
		{
			yield return null;
		}
	}

	public static bool IsOfficialRegion(IRegionInfo region)
	{
		return region != null && DefaultRegions.Any((IRegionInfo r) => r.Name.Equals(region.Name, StringComparison.OrdinalIgnoreCase));
	}

	internal static IRegionInfo[] MergeCommunityRegions(IRegionInfo[] regions)
	{
		List<IRegionInfo> list = new List<IRegionInfo>(regions);
		foreach (IRegionInfo community in CommunityRegions)
		{
			if (!list.Any((IRegionInfo r) => r.Name.Equals(community.Name, StringComparison.OrdinalIgnoreCase)))
			{
				list.Add(community);
			}
		}
		return list.ToArray();
	}

	internal void SetRegion(IRegionInfo region)
	{
		CurrentRegion = region;
		ReselectServer();
		SaveServers();
	}

	public void SaveServers()
	{
		try
		{
			FileIO.WriteAllText(contents: JsonConvert.SerializeObject(new JsonServerData
			{
				CurrentRegionIdx = AvailableRegions.IndexOf((IRegionInfo r) => r.Name == CurrentRegion.Name),
				Regions = AvailableRegions
			}, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			}), path: serverInfoFileJson);
		}
		catch
		{
		}
	}

	public void LoadServers()
	{
		Debug.Log("ServerManager::LoadServers");
		if (FileIO.Exists(serverInfoFileJson))
		{
			try
			{
				JsonServerData jsonServerData = JsonConvert.DeserializeObject<JsonServerData>(FileIO.ReadAllText(serverInfoFileJson), new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.Auto
				});
				jsonServerData.CleanAndMerge(DefaultRegions);
				AvailableRegions = MergeCommunityRegions(jsonServerData.Regions);
				CurrentRegion = AvailableRegions[jsonServerData.CurrentRegionIdx.Wrap(AvailableRegions.Length)];
				CurrentUdpServer = CurrentRegion.Servers.Random();
				state = UpdateState.Success;
				SaveServers();
				return;
			}
			catch (Exception ex)
			{
				Debug.Log($"Couldn't load regions: {ex}");
				Debug.LogException(ex, this);
			}
		}
		AvailableRegions = MergeCommunityRegions(DefaultRegions);
		CurrentRegion = AvailableRegions[0];
		CurrentUdpServer = CurrentRegion.Servers.Random();
		state = UpdateState.Success;
		SaveServers();
	}

	internal bool TrackServerFailure(string networkAddress)
	{
		ServerInfo srv = AvailableServers.FirstOrDefault((ServerInfo s) => s.Ip == networkAddress);
		if (srv != null)
		{
			srv.ConnectionFailures++;
			ServerInfo serverInfo = AvailableServers.OrderBy((ServerInfo s) => s.Players).FirstOrDefault((ServerInfo s) => s.ConnectionFailures < srv.ConnectionFailures);
			if (serverInfo != null)
			{
				CurrentUdpServer = serverInfo;
				AmongUsClient.Instance.SetEndpoint(serverInfo.Ip, serverInfo.Port, serverInfo.UseDtls);
				Debug.Log("Attempting another server: " + serverInfo.Name);
				return true;
			}
		}
		return false;
	}

	public ServerManager()
	{
		AvailableRegions = MergeCommunityRegions(DefaultRegions);
	}

	public static readonly IRegionInfo[] CommunityRegions;

	private static IRegionInfo Community(string name, string url)
	{
		// same layout as the community regionInfo.json files: Name, PingServer = url, one "http-1" server on 443, no DTLS, TranslateName 1003
		return new StaticHttpRegionInfo(name, StringNames.NoTranslation, url, new ServerInfo[1]
		{
			new ServerInfo("http-1", url, 443, useDtls: false)
		});
	}

	static ServerManager()
	{
		CommunityRegions = new IRegionInfo[9];
		// Community servers from https://ehr.gurge44.eu/regionInfo.json (Impostor-compatible HTTP matchmakers)
		// skeld.net, as in the user's regionInfo.json (the name carries a rich-text colour tag)
		CommunityRegions[0] = new StaticHttpRegionInfo("<color=\"orange\">skeld.net", StringNames.NoTranslation, "159.223.173.35", new ServerInfo[1]
		{
			new ServerInfo("NetworkManager", "https://play.skeld.net", 443, useDtls: false)
		});
		CommunityRegions[1] = Community("GG-US", "https://play-us.gurge44.eu");
		CommunityRegions[2] = Community("GG-HU", "https://play-hu.gurge44.eu");
		CommunityRegions[3] = Community("AOU-EU", "https://eu.allofus.dev");
		CommunityRegions[4] = Community("Niko-NA", "https://au-us.niko233.top");
		CommunityRegions[5] = Community("Niko-AS", "https://au-as.niko233.top");
		CommunityRegions[6] = Community("Modded EU", "https://au-eu.duikbo.at");
		CommunityRegions[7] = Community("Modded NA", "https://aumods.org");
		CommunityRegions[8] = Community("Modded Asia", "https://au-as.duikbo.at");
	}
}
