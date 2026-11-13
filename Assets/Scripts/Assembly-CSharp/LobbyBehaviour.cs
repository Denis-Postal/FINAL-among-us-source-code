using System.Collections;
using AmongUsServer;
using Hazel;
using InnerNet;
using UnityEngine;

public class LobbyBehaviour : InnerNetObject
{
	public static LobbyBehaviour Instance;

	public AudioClip SpawnSound;

	public AnimationClip SpawnInClip;

	public Vector2[] SpawnPositions;

	public AudioClip DropShipSound;

	public AudioClip MapTheme;

	private const string MAP_THEME_NAME = "MapTheme";

	public SkeldShipRoom[] AllRooms;

	private readonly Logger logger = new Logger(Logger.Category.Network, "LobbyBehaviour");

	private float lastFriendsCheckTime;

	private float optionsTimer;

	private int currentExtensionId = -1;

	public void Start()
	{
		Instance = this;
		StartCoroutine(DelayPlayDropshipAmbience());
		Camera main = Camera.main;
		if ((bool)main)
		{
			FollowerCamera component = main.GetComponent<FollowerCamera>();
			if ((bool)component)
			{
				component.shakeAmount = 0.03f;
				component.shakePeriod = 400f;
			}
		}
		LobbyDebugCommands.AddCommands();
	}

	private void Update()
	{
		HandleFriendsDataCaching();
	}

	public void RpcExtendLobbyTimer()
	{
		if (AmongUsClient.Instance.AmHost && !DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 61, SendOption.Reliable);
			messageWriter.WritePacked(currentExtensionId);
			AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
		}
	}

	private IEnumerator DelayPlayDropshipAmbience()
	{
		SoundManager.Instance.StopAllSound();
		yield return new WaitForSeconds(0.5f);
		AudioSource audioSource = SoundManager.Instance.PlayNamedSound("DropShipAmb", DropShipSound, loop: true, SoundManager.Instance.AmbienceChannel);
		audioSource.loop = true;
		audioSource.pitch = 1.2f;
		SoundManager.Instance.CrossFadeSound("MapTheme", MapTheme, 0.07f);
	}

	public void FixedUpdate()
	{
		optionsTimer += Time.fixedDeltaTime;
		if (!(optionsTimer < 0.25f))
		{
			optionsTimer = 0f;
		}
	}

	public override void OnDestroy()
	{
		SoundManager.Instance.StopNamedSound("DropShipAmb");
		SoundManager.Instance.CrossFadeSound("MapTheme", null, 0.07f);
		LobbyDebugCommands.RemoveCommands();
		base.OnDestroy();
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		switch ((RpcCalls)callId)
		{
		case RpcCalls.LobbyTimeExpiring:
		{
			int timeRemainingSeconds = reader.ReadPackedInt32();
			bool flag2 = reader.ReadBoolean();
			int hostId = 255;
			int extensionId = 0;
			int extendedTimeSeconds = 0;
			if (flag2)
			{
				hostId = reader.ReadPackedInt32();
				extensionId = reader.ReadPackedInt32();
				extendedTimeSeconds = reader.ReadPackedInt32();
			}
			HandleLobbyTimerExtensionRequest(timeRemainingSeconds, flag2, hostId, extensionId, extendedTimeSeconds);
			break;
		}
		case RpcCalls.ExtendLobbyTimer:
		{
			int num = reader.ReadPackedInt32();
			bool flag = reader.ReadBoolean();
			ExtensionFailureReasons extensionFailureReasons = ExtensionFailureReasons.Invalid;
			if (!flag)
			{
				extensionFailureReasons = (ExtensionFailureReasons)reader.ReadByte();
				logger.Warning($"Lobby extension {num} failed due to reason {extensionFailureReasons}!");
			}
			else
			{
				LobbyTimerExtended();
			}
			break;
		}
		}
	}

	public override void ClearOrDecrementDirt()
	{
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		return false;
	}

	public override void Deserialize(MessageReader reader, bool initialState)
	{
	}

	private void HandleFriendsDataCaching()
	{
		if (!(GameData.Instance == null) && GameData.Instance.AllPlayers != null && GameData.Instance.AllPlayers.Count > 1 && DestroyableSingleton<FriendsListManager>.InstanceExists && Time.time > lastFriendsCheckTime + 4f)
		{
			DestroyableSingleton<FriendsListManager>.Instance.UpdateFriendsDataWithPlayers(GameData.Instance.AllPlayers);
			lastFriendsCheckTime += Time.time;
		}
	}

	private void HandleLobbyTimerExtensionRequest(int timeRemainingSeconds, bool isExtensionAvailable, int hostId, int extensionId, int extendedTimeSeconds)
	{
		DestroyableSingleton<HudManager>.Instance.ShowLobbyTimer(timeRemainingSeconds);
	}

	private void LobbyTimerExtended()
	{
		DestroyableSingleton<HudManager>.Instance.OnLobbyTimerExtended();
	}
}
