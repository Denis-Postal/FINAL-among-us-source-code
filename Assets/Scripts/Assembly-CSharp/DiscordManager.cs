using System;
using System.Collections;
using Discord;
using InnerNet;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordManager : DestroyableSingleton<DiscordManager>
{
	private const long ClientId = 477175586805252107L;

	private const string DeeplinkScheme = "amongus";

	private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public GenericPopup discordPopup;

	[NonSerialized]
	private global::Discord.Discord presence;

	private DateTime? StartTime;

	public void Start()
	{
		if (DestroyableSingleton<DiscordManager>.Instance != this)
		{
			return;
		}
		// Remake: Discord Rich Presence is disabled (offline build). `presence` stays null, so every presence call below is a no-op.
		return;
		/*
		try
		{
			presence = new global::Discord.Discord(477175586805252107L, 1uL);
			ActivityManager activityManager = presence.GetActivityManager();
			activityManager.RegisterSteam(945360u);
			activityManager.OnActivityJoin += HandleJoinRequest;
			SetInMenus();
			SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
			{
				OnSceneChange(scene.name);
			};
		}
		catch
		{
			Debug.LogWarning("DiscordManager: Discord messed up");
		}
		*/
	}

	private void OnSceneChange(string name)
	{
		if (name == "MatchMaking" || name == "MainMenu")
		{
			SetInMenus();
		}
	}

	public void FixedUpdate()
	{
		if (presence == null)
		{
			return;
		}
		try
		{
			presence.RunCallbacks();
		}
		catch (ResultException)
		{
			presence.Dispose();
			presence = null;
		}
	}

	public bool HasValidPartyID()
	{
		return false;
	}

	public bool CanLoginWithDiscord()
	{
		return false;
	}

	public void LoginWithDiscord()
	{
		Debug.Log("DiscordManager: Logging in with discord");
	}

	public void Logout()
	{
	}

	public void SetInMenus()
	{
		StartTime = null;
		if (presence != null)
		{
			ClearPresence();
			Activity activity = new Activity
			{
				State = "In Menus",
				Assets = 
				{
					LargeImage = "icon"
				}
			};
			presence.GetActivityManager().UpdateActivity(activity, (Result r) =>
			{
			});
		}
	}

	public void SetPlayingGame()
	{
		if (!StartTime.HasValue)
		{
			StartTime = DateTime.UtcNow;
		}
		if (presence != null)
		{
			Activity activity = new Activity
			{
				State = "In Game",
				Details = "Playing",
				Assets = 
				{
					LargeImage = "icon"
				},
				Timestamps = 
				{
					Start = ToUnixTime(StartTime.Value)
				}
			};
			presence.GetActivityManager().UpdateActivity(activity, (Result r) =>
			{
			});
		}
	}

	public void SetHowToPlay()
	{
		if (presence != null)
		{
			ClearPresence();
			Activity activity = new Activity
			{
				State = "In Freeplay",
				Assets = 
				{
					LargeImage = "icon"
				}
			};
			presence.GetActivityManager().UpdateActivity(activity, (Result r) =>
			{
			});
		}
	}

	public void SetInLobbyClient(int numPlayers, int maxPlayers, int gameId)
	{
		if (!StartTime.HasValue)
		{
			StartTime = DateTime.UtcNow;
		}
		string id = GameCode.IntToGameName(gameId);
		if (presence != null)
		{
			ClearPresence();
			Activity activity = new Activity
			{
				State = "In Lobby",
				Assets = 
				{
					LargeImage = "icon"
				},
				Timestamps = 
				{
					Start = ToUnixTime(StartTime.Value)
				},
				Party = 
				{
					Size = 
					{
						CurrentSize = numPlayers,
						MaxSize = maxPlayers
					},
					Id = id
				}
			};
			presence.GetActivityManager().UpdateActivity(activity, (Result r) =>
			{
			});
		}
	}

	private void ClearPresence()
	{
		if (presence != null)
		{
			presence.GetActivityManager().ClearActivity((Result r) =>
			{
			});
		}
	}

	public void SetInLobbyHost(int numPlayers, int maxPlayers, int gameId)
	{
		if (!StartTime.HasValue)
		{
			StartTime = DateTime.UtcNow;
		}
		string text = GameCode.IntToGameName(gameId);
		if (presence != null)
		{
			Activity activity = default;
			activity.State = "In Lobby";
			activity.Details = "Hosting a game";
			activity.Assets.LargeImage = "icon";
			activity.Assets.LargeText = "Ask to play!";
			activity.Party.Size.CurrentSize = numPlayers;
			activity.Party.Size.MaxSize = maxPlayers;
			activity.Secrets.Join = "join" + ReverseString(text);
			activity.Secrets.Match = "match" + ReverseString(text);
			activity.Party.Id = text;
			activity.SupportedPlatforms = 7u;
			presence.GetActivityManager().UpdateActivity(activity, (Result r) =>
			{
			});
		}
	}

	public bool CanShareGameOnDiscord()
	{
		return false;
	}

	public void ShareGameOnDiscord()
	{
	}

	private void HandleJoinRequest(string joinSecret)
	{
		if (!joinSecret.StartsWith("join"))
		{
			Debug.LogWarning("DiscordManager: Invalid join secret: " + joinSecret);
			return;
		}
		StopAllCoroutines();
		StartCoroutine(CoJoinGame(joinSecret));
	}

	private IEnumerator CoJoinGame(string joinSecret)
	{
		if (DestroyableSingleton<StoreMenu>.InstanceExists)
		{
			DestroyableSingleton<StoreMenu>.Instance.CloseEntirely();
		}
		while (!DestroyableSingleton<EOSManager>.InstanceExists)
		while (!AmongUsClient.Instance)
		{
			yield return null;
		}
		if (AmongUsClient.Instance.mode != MatchMakerModes.None)
		{
			Debug.LogWarning("DiscordManager: Already connected");
			yield break;
		}
		Debug.Log("DiscordManager: Joining game: " + joinSecret);
		int gameId = GameCode.GameNameToInt(ReverseString(joinSecret.Substring(4)));
		yield return AmongUsClient.Instance.CoFindGameInfoFromCodeAndJoin(gameId);
	}

	public void RequestRespondYes(long userId)
	{
		if (presence != null)
		{
			presence.GetActivityManager().SendRequestReply(userId, ActivityJoinRequestReply.Yes, (Result r) =>
			{
			});
		}
	}

	public void RequestRespondNo(long userId)
	{
		if (presence != null)
		{
			Debug.Log("DiscordManager: responding no to Ask to Join request");
			presence.GetActivityManager().SendRequestReply(userId, ActivityJoinRequestReply.No, (Result r) =>
			{
			});
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (presence != null)
		{
			presence.Dispose();
		}
	}

	private static string ReverseString(string source)
	{
		char[] array = source.ToCharArray();
		Array.Reverse(array);
		return new string(array);
	}

	private static long ToUnixTime(DateTime time)
	{
		return (long)(time - epoch).TotalSeconds;
	}
}
