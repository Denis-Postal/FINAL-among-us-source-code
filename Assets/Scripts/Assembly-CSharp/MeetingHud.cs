using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.InnerNet.GameDataMessages;
using Hazel;
using InnerNet;
using Rewired;
using TMPro;
using UnityEngine;

public class MeetingHud : InnerNetObject, IDisconnectHandler
{
	public struct VoterState
	{
		public byte VoterId;

		public byte VotedForId;

		public bool AmDead => VotedForId == 252;

		public bool SkippedVote => VotedForId == 253;

		public static VoterState Deserialize(MessageReader reader)
		{
			MessageReader messageReader = reader.ReadMessage();
			return new VoterState
			{
				VoterId = messageReader.Tag,
				VotedForId = messageReader.ReadByte()
			};
		}

		public void Serialize(MessageWriter writer)
		{
			writer.StartMessage(VoterId);
			writer.Write(VotedForId);
			writer.EndMessage();
		}
	}

	public enum MeetingStates
	{
		Animating,
		Discussion,
		NotVoted,
		Voted,
		Results,
		Proceeding
	}

	private const int NumColumns = 3;

	private const float ResultsTime = 5f;

	private const float Depth = 0f;

	public static MeetingHud Instance;

	public SpriteRenderer BlackBackground;

	public SpriteRenderer[] OuterMasks;

	public SpriteRenderer[] PlayerColoredParts;

	public MeetingIntroAnimation MeetingIntro;

	public Transform ButtonParent;

	public TextMeshPro TitleText;

	public Vector3 VoteOrigin = new Vector3(-3.6f, 1.75f);

	public Vector3 VoteButtonOffsets = new Vector2(3.6f, -0.91f);

	public PlayerVoteArea SkipVoteButton;

	private PlayerVoteArea[] playerStates = new PlayerVoteArea[0];

	public PlayerVoteArea PlayerButtonPrefab;

	public SpriteRenderer PlayerVotePrefab;

	public Sprite CrackedGlass;

	public SpriteRenderer Glass;

	public PassiveButton ProceedButton;

	public AbilityButton MeetingAbilityButton;

	public AudioClip VoteSound;

	public AudioClip VoteLockinSound;

	public AudioClip VoteEndingSound;

	[SerializeField]
	private Transform meetingContents;

	public MeetingStates state; // made public: PlayerVoteArea (decomp) accesses it

	public GameObject SkippedVoting;

	public SpriteRenderer HostIcon;

	private NetworkedPlayerInfo exiledPlayer;

	private bool wasTie;

	private bool wasOverruled;

	public TextMeshPro TimerText;

	public float discussionTimer;

	private byte reporterId = byte.MaxValue;

	private bool hasForegroundForDeadBeenSet;

	private float resultsStartedAt;

	private int lastSecond = 10;

	private List<JudgeOverrule> judgeOverrulesQueue = new List<JudgeOverrule>();

	[SerializeField]
	private TextMeshPro judgeUsesRemaining;

	[SerializeField]
	private GameObject judgeOverruleDisplay;

	[SerializeField]
	private GameObject judgeGavelPrefab;

	private readonly Logger logger = new Logger(Logger.Category.UI, "MeetingHud");

	[Header("Console Controller Navigation")]
	public UiElement DefaultButtonSelected;

	public UiElement ProceedButtonUi;

	public List<UiElement> ControllerSelectable;

	public bool IsPersistent => false;

	public MeetingStates CurrentState => state;

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
			MeetingAbilityButton.gameObject.SetActive(value: false);
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private void Start()
	{
		BlackBackground.sprite = ShipStatus.Instance.MeetingBackground;
		SetMasksEnabled(enabled: false);
		SpriteRenderer[] playerColoredParts = PlayerColoredParts;
		foreach (SpriteRenderer playerMaterialColors in playerColoredParts)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
		DestroyableSingleton<HudManager>.Instance.Chat.gameObject.SetActive(value: true);
		DestroyableSingleton<HudManager>.Instance.StopOxyFlash();
		DestroyableSingleton<HudManager>.Instance.StopReactorFlash();
		SkipVoteButton.SetPlayerId((byte)253);
		SkipVoteButton.Parent = this;
		Camera.main.GetComponent<FollowerCamera>().Locked = true;
		if (PlayerControl.LocalPlayer.Data.IsDead)
		{
			SetForegroundForDead();
		}
		AmongUsClient.Instance.DisconnectHandlers.AddUnique(this);
		PlayerVoteArea[] array = playerStates;
		foreach (PlayerVoteArea playerVoteArea in array)
		{
			if (!playerVoteArea.AmDead)
			{
				ControllerSelectable.Add(playerVoteArea.PlayerButton);
			}
		}
		DestroyableSingleton<AchievementManager>.Instance.OnMeetingCalled();
	}

	public override void OnDestroy()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		base.OnDestroy();
	}

	public void Update()
	{
		if (state == MeetingStates.Animating)
		{
			return;
		}
		discussionTimer += Time.deltaTime;
		UpdateForeground();
		LogicOptionsNormal logicOptionsNormal = GameManager.Instance.LogicOptions as LogicOptionsNormal;
		switch (state)
		{
		case MeetingStates.Discussion:
		{
			int discussionTime = logicOptionsNormal.GetDiscussionTime();
			if (discussionTimer < (float)discussionTime)
			{
				float f = (float)discussionTime - discussionTimer;
				UpdateTimerText(StringNames.MeetingVotingBegins, Mathf.CeilToInt(f));
				for (int i = 0; i < playerStates.Length; i++)
				{
					playerStates[i].SetDisabled();
				}
				SkipVoteButton.SetDisabled();
				break;
			}
			state = MeetingStates.NotVoted;
			bool active = logicOptionsNormal.GetVotingTime() > 0;
			TimerText.gameObject.SetActive(active);
			for (int j = 0; j < playerStates.Length; j++)
			{
				playerStates[j].SetEnabled();
			}
			SkipVoteButton.SetEnabled();
			break;
		}
		case MeetingStates.NotVoted:
		case MeetingStates.Voted:
		{
			int votingTime = logicOptionsNormal.GetVotingTime();
			if (votingTime > 0)
			{
				float num3 = discussionTimer - (float)logicOptionsNormal.GetDiscussionTime();
				float f2 = Mathf.Max(0f, (float)votingTime - num3);
				UpdateTimerText(StringNames.MeetingVotingEnds, Mathf.CeilToInt(f2));
				if (state == MeetingStates.NotVoted && Mathf.CeilToInt(f2) <= lastSecond)
				{
					lastSecond--;
					StartCoroutine(Effects.PulseColor(TimerText, Color.red, Color.white, 0.25f));
					SoundManager.Instance.PlaySound(VoteEndingSound, loop: false).pitch = Mathf.Lerp(1.5f, 0.8f, (float)lastSecond / 10f);
				}
				if (AmongUsClient.Instance.AmHost && num3 >= (float)votingTime)
				{
					ForceSkipAll();
				}
			}
			break;
		}
		case MeetingStates.Results:
			if (AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame)
			{
				float num = discussionTimer - resultsStartedAt;
				float num2 = Mathf.Max(0f, 5f - num);
				UpdateTimerText(StringNames.MeetingProceeds, Mathf.CeilToInt(num2));
				if (AmongUsClient.Instance.AmHost && num2 <= 0f)
				{
					HandleProceed();
				}
			}
			break;
		}
		if (MeetingAbilityButton.isActiveAndEnabled && (ReInput.players.GetPlayer(0).GetButtonDown(34) || ReInput.players.GetPlayer(0).GetButtonDown(49)))
		{
			MeetingAbilityButton.DoClick();
		}
	}

	private void UpdateTimerText(StringNames key, int value)
	{
		TimerText.text = DestroyableSingleton<TranslationController>.Instance.GetString(key, value);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.UpdateVoteTimerText(key, value);
		}
	}

	private void SetMasksEnabled(bool enabled)
	{
		SpriteRenderer[] outerMasks = OuterMasks;
		for (int i = 0; i < outerMasks.Length; i++)
		{
			outerMasks[i].enabled = enabled;
		}
	}

	public IEnumerator CoIntro(NetworkedPlayerInfo reporter, NetworkedPlayerInfo reportedBody, NetworkedPlayerInfo[] deadBodies)
	{
		SkipVoteButton.SetDisabled();
		transform.SetParent(DestroyableSingleton<HudManager>.Instance.transform);
		transform.localPosition = Vector3.zero;
		meetingContents.localPosition = new Vector3(0f, -10f, 0f);
		DestroyableSingleton<HudManager>.Instance.Chat.ForceClosed();
		DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: false);
		DestroyableSingleton<HudManager>.Instance.MapButton.gameObject.SetActive(value: false);
		bool flag = reportedBody == null;
		MeetingCalledAnimation prefab = (flag ? ShipStatus.Instance.EmergencyOverlay : ShipStatus.Instance.ReportOverlay);
		NetworkedPlayerInfo networkedPlayerInfo = (flag ? reporter : reportedBody);
		DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowMeeting(prefab, networkedPlayerInfo.DefaultOutfit);
		yield return DestroyableSingleton<HudManager>.Instance.KillOverlay.WaitForFinish();
		MeetingIntro.Init(reporter, deadBodies);
		SetMasksEnabled(enabled: true);
		DestroyableSingleton<HudManager>.Instance.MapButton.gameObject.SetActive(value: true);
		yield return Effects.Slide2D(meetingContents, new Vector2(0f, -10f), new Vector2(0f, 0f), 0.25f);
		yield return Effects.Wait(0.5f);
		yield return MeetingIntro.CoRun();
		SetMasksEnabled(enabled: false);
		TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingWhoIsTitle);
		state = MeetingStates.Discussion;
		ControllerManager.Instance.OpenOverlayMenu(name, null, DefaultButtonSelected, ControllerSelectable);
		ConsoleJoystick.SetMode_Menu();
	}

	private IEnumerator CoStartCutscene()
	{
		ConsoleJoystick.SetMode_Task();
		yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.clear, Color.black, 1f);
		ExileController exileController = UnityEngine.Object.Instantiate(ShipStatus.Instance.ExileCutscenePrefab);
		exileController.transform.SetParent(DestroyableSingleton<HudManager>.Instance.transform, worldPositionStays: false);
		exileController.transform.localPosition = new Vector3(0f, 0f, -60f);
		exileController.BeginForGameplay(exiledPlayer, wasTie, wasOverruled);
		DespawnOnDestroy = false;
		if ((bool)MapBehaviour.Instance)
		{
			MapBehaviour.Instance.Close();
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	public void ServerStart(byte reporterPlayerId)
	{
		reporterId = reporterPlayerId;
		PopulateButtons();
		PopulateReporter(reporterPlayerId);
	}

	public void Close()
	{
		NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
		DestroyableSingleton<HudManager>.Instance.Chat.SetVisible(data.IsDead);
		DestroyableSingleton<HudManager>.Instance.Chat.HideBanButton();
		StartCoroutine(CoStartCutscene());
	}

	private void VotingComplete(VoterState[] states, NetworkedPlayerInfo exiled, bool tie, bool wasOverruled, ushort overruleNonce)
	{
		if (state == MeetingStates.Results)
		{
			return;
		}
		state = MeetingStates.Results;
		resultsStartedAt = discussionTimer;
		exiledPlayer = exiled;
		wasTie = tie;
		this.wasOverruled = wasOverruled;
		if (exiled != null)
		{
			exiled.WasEjected = true;
		}
		if (wasOverruled)
		{
			bool anotherJudgeBeatYouToIt = false;
			PlayerId playerId = exiled.PlayerId;
			JudgeRole judgeRole = PlayerControl.LocalPlayer.Data.Role as JudgeRole;
			if (judgeRole != null && judgeRole.HasAlreadyOverruledThisMeeting)
			{
				if (judgeRole.OverruleNonce == overruleNonce)
				{
					judgeRole.ConsumeOverruleVotesUsage();
					if ((byte)playerId == (byte)judgeRole.OverruledPlayerId)
					{
						DataManager.Player.Stats.IncrementStat(StatID.Role_Judge_SuccessfulOverrules);
					}
					else
					{
						DataManager.Player.Stats.IncrementStat(StatID.Role_Judge_FailedOverrules);
					}
				}
				else
				{
					anotherJudgeBeatYouToIt = true;
				}
			}
			ShowJudgeOverrule(anotherJudgeBeatYouToIt);
		}
		SkipVoteButton.gameObject.SetActive(value: false);
		SkippedVoting.gameObject.SetActive(value: true);
		AmongUsClient.Instance.DisconnectHandlers.Remove(this);
		for (int i = 0; i < GameData.Instance.PlayerCount; i++)
		{
			PlayerControl playerControl = GameData.Instance.AllPlayers[i].Object;
			if (playerControl != null && playerControl.Data != null && (bool)playerControl.Data.Role)
			{
				playerControl.Data.Role.OnVotingComplete();
			}
		}
		PopulateResults(states);
		SetupProceedButton();
		try
		{
			VoterState voterState = states.FirstOrDefault((VoterState s) => s.VoterId == PlayerControl.LocalPlayer.PlayerId);
			NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(voterState.VotedForId);
			DestroyableSingleton<AchievementManager>.Instance.OnMeetingVote(PlayerControl.LocalPlayer.Data, playerById);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		if (DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening)
		{
			DestroyableSingleton<HudManager>.Instance.Chat.ForceClosed();
			ControllerManager.Instance.CloseOverlayMenu(DestroyableSingleton<HudManager>.Instance.Chat.name);
		}
		ControllerManager.Instance.CloseOverlayMenu(name);
		ControllerManager.Instance.OpenOverlayMenu(name, null, ProceedButtonUi);
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.MeetingEnded(Time.realtimeSinceStartup - GameData.TimeLastMeetingStarted, exiled);
	}

	public bool Select(int suspectStateIdx)
	{
		LogicOptionsNormal logicOptionsNormal = GameManager.Instance.LogicOptions as LogicOptionsNormal;
		if (discussionTimer < (float)logicOptionsNormal.GetDiscussionTime())
		{
			return false;
		}
		if (PlayerControl.LocalPlayer.Data.IsDead)
		{
			return false;
		}
		SoundManager.Instance.PlaySound(VoteSound, loop: false).volume = 0.8f;
		for (int i = 0; i < playerStates.Length; i++)
		{
			PlayerVoteArea playerVoteArea = playerStates[i];
			if (suspectStateIdx != (byte)playerVoteArea.PlayerId)
			{
				playerVoteArea.ClearButtons();
			}
		}
		if (suspectStateIdx != -1)
		{
			SkipVoteButton.ClearButtons();
		}
		return true;
	}

	public void Confirm(byte suspectStateIdx)
	{
		if (!PlayerControl.LocalPlayer.Data.IsDead)
		{
			SetVoteComplete(complete: true, clearButtons: true);
			MeetingStates meetingStates = state;
			if (meetingStates != MeetingStates.NotVoted)
			{
				_ = 4;
				return;
			}
			state = MeetingStates.Voted;
			CmdCastVote(PlayerControl.LocalPlayer.PlayerId, suspectStateIdx);
		}
	}

	public void HandleDisconnect(PlayerControl pc, DisconnectReasons reason)
	{
		if (playerStates == null || !pc || !GameData.Instance)
		{
			return;
		}
		HandleDisconnectedPlayerVoteArea(pc, reason);
		if (!AmongUsClient.Instance.AmHost)
		{
			return;
		}
		UpdateOverruleQueueFromDisconnection(pc.PlayerId);
		for (int i = 0; i < playerStates.Length; i++)
		{
			PlayerVoteArea playerVoteArea = playerStates[i];
			if (!playerVoteArea.AmDead && playerVoteArea.DidVote && (byte)playerVoteArea.VotedForId == pc.PlayerId)
			{
				playerVoteArea.UnsetVote();
				RpcClearVote(playerVoteArea.PlayerId);
			}
		}
		SetDirtyBit(1u);
		CheckForEndVoting();
		if (state == MeetingStates.Results)
		{
			SetupProceedButton();
		}
	}

	public void HandleDisconnect()
	{
	}

	private void ForceSkipAll()
	{
		for (int i = 0; i < playerStates.Length; i++)
		{
			PlayerVoteArea playerVoteArea = playerStates[i];
			if (!playerVoteArea.DidVote)
			{
				playerVoteArea.SetVoteMissed();
				SetDirtyBit(1u);
			}
		}
		CheckForEndVoting();
	}

	public void CastVote(PlayerId srcPlayerId, PlayerId suspectPlayerId)
	{
		NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(srcPlayerId);
		NetworkedPlayerInfo playerById2 = GameData.Instance.GetPlayerById(suspectPlayerId);
		logger.Debug("PlayerId:" + playerById.PlayerId + " has voted for " + ((playerById2 != null) ? ("PlayerId: " + playerById2.PlayerId) : "No one"));
		int num = playerStates.IndexOf((PlayerVoteArea pv) => (byte)pv.PlayerId == (byte)srcPlayerId);
		PlayerVoteArea playerVoteArea = playerStates[num];
		if (!playerVoteArea.AmDead && !playerVoteArea.DidVote)
		{
			if (PlayerControl.LocalPlayer.PlayerId == (byte)srcPlayerId || AmongUsClient.Instance.NetworkMode != NetworkModes.LocalGame)
			{
				SoundManager.Instance.PlaySound(VoteLockinSound, loop: false);
			}
			playerVoteArea.SetVote(suspectPlayerId);
			SetDirtyBit(1u);
			CheckForEndVoting();
			PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);
		}
	}

	public void MarkPlayerAsHasVoted(PlayerId srcPlayerId)
	{
		int num = playerStates.IndexOf((PlayerVoteArea pv) => (byte)pv.PlayerId == (byte)srcPlayerId);
		PlayerVoteArea playerVoteArea = playerStates[num];
		if (!playerVoteArea.AmDead)
		{
			playerVoteArea.SetHasVoted();
		}
	}

	public void ClearVote(PlayerId voterIdToClear, bool isLocalPlayer)
	{
		if (isLocalPlayer)
		{
			SetVoteComplete(complete: false, clearButtons: false);
			state = MeetingStates.NotVoted;
			ClearJudgeOverrule();
		}
		int num = playerStates.IndexOf((PlayerVoteArea pv) => (byte)pv.PlayerId == (byte)voterIdToClear);
		PlayerVoteArea playerVoteArea = playerStates[num];
		if (!playerVoteArea.AmDead)
		{
			playerVoteArea.UnsetHasVoted();
		}
	}

	public void SetVoteComplete(bool complete, bool clearButtons)
	{
		int num = playerStates.Length;
		for (int i = 0; i < num; i++)
		{
			PlayerVoteArea playerVoteArea = playerStates[i];
			if (clearButtons)
			{
				playerVoteArea.ClearButtons();
			}
			playerVoteArea.VoteComplete = complete;
		}
		if (clearButtons)
		{
			SkipVoteButton.ClearButtons();
		}
		SkipVoteButton.VoteComplete = complete;
		SkipVoteButton.gameObject.SetActive(!complete);
	}

	private void CheckForEndVoting()
	{
		if (playerStates.All((PlayerVoteArea ps) => ps.AmDead || ps.DidVote))
		{
			Dictionary<byte, int> self = CalculateVotes();
			KeyValuePair<byte, int> max = self.MaxPair(out var tie);
			NetworkedPlayerInfo exiled = GameData.Instance.AllPlayers.FirstOrDefault((NetworkedPlayerInfo v) => !tie && v.PlayerId == max.Key);
			bool flag = false;
			ushort overruleNonce = 0;
			if (TryGetWinningOverrule(out var winningOverrule, out var judgePlayerInfo, out var overruledPlayerInfo))
			{
				flag = true;
				overruleNonce = winningOverrule.OverruleNonce;
				exiled = ((overruledPlayerInfo.Role.TeamType != RoleTeamTypes.Impostor) ? judgePlayerInfo : GameData.Instance.GetPlayerById(winningOverrule.OverruledPlayerId));
			}
			VoterState[] array = new VoterState[playerStates.Length];
			for (int num = 0; num < playerStates.Length; num++)
			{
				PlayerVoteArea playerVoteArea = playerStates[num];
				array[num] = new VoterState
				{
					VoterId = playerVoteArea.PlayerId,
					VotedForId = playerVoteArea.VotedForId
				};
			}
			RpcVotingComplete(array, exiled, tie, flag, overruleNonce);
		}
	}

	private Dictionary<byte, int> CalculateVotes()
	{
		Dictionary<byte, int> dictionary = new Dictionary<byte, int>();
		for (int i = 0; i < playerStates.Length; i++)
		{
			PlayerVoteArea playerVoteArea = playerStates[i];
			if ((byte)playerVoteArea.VotedForId != 252 && (byte)playerVoteArea.VotedForId != byte.MaxValue && (byte)playerVoteArea.VotedForId != 254)
			{
				if (dictionary.TryGetValue(playerVoteArea.VotedForId, out var value))
				{
					dictionary[playerVoteArea.VotedForId] = value + 1;
				}
				else
				{
					dictionary[playerVoteArea.VotedForId] = 1;
				}
			}
		}
		return dictionary;
	}

	public void HandleProceed()
	{
		if (AmongUsClient.Instance.AmHost)
		{
			if (state == MeetingStates.Results)
			{
				state = MeetingStates.Proceeding;
				RpcClose();
			}
		}
		else
		{
			StartCoroutine(Effects.SwayX(HostIcon.transform));
		}
	}

	private void SetupProceedButton()
	{
		if (AmongUsClient.Instance.NetworkMode != NetworkModes.OnlineGame)
		{
			TimerText.gameObject.SetActive(value: false);
			ProceedButton.gameObject.SetActive(value: true);
			HostIcon.gameObject.SetActive(value: true);
			NetworkedPlayerInfo host = GameData.Instance.GetHost();
			if (host != null)
			{
				PlayerMaterial.SetColors(host.DefaultOutfit.ColorId, HostIcon);
			}
			else
			{
				HostIcon.enabled = false;
			}
		}
	}

	private void PopulateResults(VoterState[] states)
	{
		TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults);
		int num = 0;
		for (int i = 0; i < playerStates.Length; i++)
		{
			PlayerVoteArea playerVoteArea = playerStates[i];
			playerVoteArea.ClearForResults();
			int num2 = 0;
			for (int j = 0; j < states.Length; j++)
			{
				VoterState voterState = states[j];
				NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(voterState.VoterId);
				if (playerById == null)
				{
					logger.Error($"Couldn't find player info for voter: {voterState.VoterId}");
				}
				else if (i == 0 && voterState.SkippedVote)
				{
					BloopAVoteIcon(playerById, num, SkippedVoting.transform);
					num++;
				}
				else if (voterState.VotedForId == (byte)playerVoteArea.PlayerId)
				{
					BloopAVoteIcon(playerById, num2, playerVoteArea.transform);
					num2++;
				}
			}
		}
	}

	private void BloopAVoteIcon(NetworkedPlayerInfo voterPlayer, int index, Transform parent)
	{
		SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate(PlayerVotePrefab);
		if (GameManager.Instance.LogicOptions.GetAnonymousVotes())
		{
			PlayerMaterial.SetColors(Palette.DisabledGrey, spriteRenderer);
		}
		else
		{
			PlayerMaterial.SetColors(voterPlayer.DefaultOutfit.ColorId, spriteRenderer);
		}
		spriteRenderer.transform.SetParent(parent);
		spriteRenderer.transform.localScale = Vector3.zero;
		PlayerVoteArea component = parent.GetComponent<PlayerVoteArea>();
		if (component != null)
		{
			spriteRenderer.material.SetInt(PlayerMaterial.MaskLayer, component.MaskLayer);
		}
		StartCoroutine(Effects.Bloop((float)index * 0.3f, spriteRenderer.transform));
		parent.GetComponent<VoteSpreader>().AddVote(spriteRenderer);
	}

	private void UpdateForeground()
	{
		if (PlayerControl.LocalPlayer.Data.IsDead && !hasForegroundForDeadBeenSet)
		{
			SetForegroundForDead();
		}
	}

	private void PopulateButtons()
	{
		playerStates = new PlayerVoteArea[GameData.Instance.PlayerCount];
		for (int i = 0; i < playerStates.Length; i++)
		{
			NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AllPlayers[i];
			PlayerVoteArea playerVoteArea = (playerStates[i] = CreateButton(networkedPlayerInfo));
			playerVoteArea.Parent = this;
			playerVoteArea.SetPlayerId(networkedPlayerInfo.PlayerId);
			playerVoteArea.SetDead(networkedPlayerInfo.Disconnected || networkedPlayerInfo.IsDead);
			playerVoteArea.ConfigureOverlay();
		}
		PlayerVoteArea[] array = playerStates;
		foreach (PlayerVoteArea playerVoteArea2 in array)
		{
			if (!playerVoteArea2.AmDead)
			{
				ControllerManager.Instance.AddSelectableUiElement(playerVoteArea2.PlayerButton);
			}
		}
		SortButtons();
	}

	private void PopulateReporter(byte reporterPlayerId)
	{
		int num = playerStates.IndexOf((PlayerVoteArea pv) => (byte)pv.PlayerId == reporterPlayerId);
		if (num < 0)
		{
			logger.Error(string.Format("{0} - could not find index of PlayerVoteArea for {1}", "PopulateReporter", reporterPlayerId));
			return;
		}
		PlayerVoteArea playerVoteArea = playerStates[num];
		if (playerVoteArea == null)
		{
			logger.Error(string.Format("{0} - PlayerVoteArea for {1}, is null", "PopulateReporter", reporterPlayerId));
		}
		else
		{
			playerVoteArea.SetReporter();
		}
	}

	private void SortButtons()
	{
		PlayerVoteArea[] array = (from p in playerStates
			orderby p.AmDead ? 50 : 0, p.PlayerId.Value
			select p).ToArray();
		for (int num = 0; num < array.Length; num++)
		{
			int num2 = num % 3;
			int num3 = num / 3;
			array[num].transform.localPosition = VoteOrigin + new Vector3(VoteButtonOffsets.x * (float)num2, VoteButtonOffsets.y * (float)num3, -0.9f - (float)num3 * 0.01f);
		}
	}

	private PlayerVoteArea CreateButton(NetworkedPlayerInfo playerInfo)
	{
		PlayerVoteArea playerVoteArea = UnityEngine.Object.Instantiate(PlayerButtonPrefab, ButtonParent.transform);
		playerVoteArea.SetCosmetics(playerInfo);
		PlayerNameColor.Set(playerInfo, playerVoteArea.NameText);
		playerVoteArea.transform.localScale = Vector3.one;
		return playerVoteArea;
	}

	public bool DidVote(byte playerId)
	{
		return playerStates.First((PlayerVoteArea p) => (byte)p.PlayerId == playerId).DidVote;
	}

	public int GetVotesRemaining()
	{
		try
		{
			return playerStates.Count((PlayerVoteArea ps) => !ps.AmDead && !ps.DidVote);
		}
		catch
		{
			return 0;
		}
	}

	public void UpdateJudgeAbilityIndicator(bool hasJudgeUsageRemaining, int tasksRemaining)
	{
		if (!hasJudgeUsageRemaining)
		{
			judgeUsesRemaining.gameObject.SetActive(value: false);
			return;
		}
		judgeUsesRemaining.gameObject.SetActive(value: true);
		if (tasksRemaining > 0)
		{
			judgeUsesRemaining.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.JudgeAbilityProgress, tasksRemaining);
		}
		else
		{
			judgeUsesRemaining.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.JudgeAbilityUnlocked);
		}
	}

	public void ClearJudgeUsesIndicator()
	{
		judgeUsesRemaining.text = string.Empty;
	}

	public void RemoveJudgeUsedLabel(PlayerId overruledPlayerId)
	{
		int num = playerStates.IndexOf((PlayerVoteArea pv) => (byte)pv.PlayerId == (byte)overruledPlayerId);
		playerStates[num].RemoveJudgeUsedLabel();
	}

	private void HandleDisconnectedPlayerVoteArea(PlayerControl pc, DisconnectReasons reason)
	{
		int num = playerStates.IndexOf((PlayerVoteArea pv) => (byte)pv.PlayerId == pc.PlayerId);
		if (num < 0)
		{
			logger.Error(string.Format("{0} - could not find index of PlayerVoteArea for {1}, after disconnecting with reason {2}", "HandleDisconnectedPlayerVoteArea", pc.PlayerId, reason));
			return;
		}
		PlayerVoteArea playerVoteArea = playerStates[num];
		if (playerVoteArea == null)
		{
			logger.Error(string.Format("{0} - PlayerVoteArea for {1}, after disconnecting with reason {2}, is null", "HandleDisconnectedPlayerVoteArea", pc.PlayerId, reason));
			return;
		}
		playerVoteArea.ClearButtons();
		playerVoteArea.SetDead(isDead: true);
	}

	private void ClearJudgeOverrule()
	{
		JudgeRole judgeRole = PlayerControl.LocalPlayer.Data.Role as JudgeRole;
		if (!(judgeRole == null))
		{
			judgeRole.ClearOverrule();
		}
	}

	private void SetJudgeOverrule(PlayerId judgePlayerId, PlayerId targetPlayerId, ushort overruleNonce)
	{
		if (!AmongUsClient.Instance.AmHost)
		{
			return;
		}
		NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AllPlayers.Find((NetworkedPlayerInfo p) => p.PlayerId == (byte)judgePlayerId);
		if (networkedPlayerInfo == null)
		{
			logger.Error(string.Format("{0} - Could not find player with id {1}", "SetJudgeOverrule", judgePlayerId));
			return;
		}
		if (networkedPlayerInfo.Disconnected)
		{
			logger.Error(string.Format("{0} - Judge with id {1} already disconnected", "SetJudgeOverrule", judgePlayerId));
			return;
		}
		NetworkedPlayerInfo networkedPlayerInfo2 = GameData.Instance.AllPlayers.Find((NetworkedPlayerInfo p) => p.PlayerId == (byte)targetPlayerId);
		if (networkedPlayerInfo2 == null)
		{
			logger.Error(string.Format("{0} - Judge w/ id {1} - could not find player with id {2}", "SetJudgeOverrule", judgePlayerId, targetPlayerId));
			return;
		}
		if (networkedPlayerInfo2.Disconnected)
		{
			logger.Error(string.Format("{0} - Judge with id {1} - target with id {2} already disconnected", "SetJudgeOverrule", judgePlayerId, targetPlayerId));
			return;
		}
		AddOrUpdateJudgeOverrule(judgePlayerId, targetPlayerId, overruleNonce);
		int num = playerStates.IndexOf((PlayerVoteArea pv) => (byte)pv.PlayerId == (byte)judgePlayerId);
		playerStates[num].SetHasVoted();
		PlayerControl.LocalPlayer.RpcSendChatNote(judgePlayerId, ChatNoteTypes.DidVote);
		SetDirtyBit(1u);
		CheckForEndVoting();
	}

	private void AddOrUpdateJudgeOverrule(PlayerId judgePlayerId, PlayerId targetPlayerId, ushort overruleNonce)
	{
		int count = judgeOverrulesQueue.Count;
		for (int i = 0; i < count; i++)
		{
			JudgeOverrule judgeOverrule = judgeOverrulesQueue[i];
			if ((byte)judgeOverrule.JudgePlayerId == (byte)judgePlayerId)
			{
				judgeOverrule.OverruledPlayerId = targetPlayerId;
				judgeOverrule.OverruleNonce = overruleNonce;
				return;
			}
		}
		judgeOverrulesQueue.Add(new JudgeOverrule(targetPlayerId, judgePlayerId, overruleNonce));
	}

	private void UpdateOverruleQueueFromDisconnection(PlayerId disconnectedPlayerId)
	{
		for (int num = judgeOverrulesQueue.Count - 1; num >= 0; num--)
		{
			JudgeOverrule judgeOverrule = judgeOverrulesQueue[num];
			if ((byte)judgeOverrule.JudgePlayerId == (byte)disconnectedPlayerId)
			{
				judgeOverrulesQueue.RemoveAt(num);
			}
			else if ((byte)judgeOverrule.OverruledPlayerId == (byte)disconnectedPlayerId)
			{
				judgeOverrule.OverruledPlayerId = PlayerId.Invalid;
				playerStates.FirstOrDefault((PlayerVoteArea voteState) => (byte)voteState.PlayerId == (byte)judgeOverrule.JudgePlayerId).UnsetHasVoted();
				RpcClearVote(judgeOverrule.JudgePlayerId);
			}
		}
	}

	private bool TryGetWinningOverrule(out JudgeOverrule winningOverrule, out NetworkedPlayerInfo judgePlayerInfo, out NetworkedPlayerInfo overruledPlayerInfo)
	{
		winningOverrule = null;
		judgePlayerInfo = null;
		overruledPlayerInfo = null;
		int count = judgeOverrulesQueue.Count;
		for (int i = 0; i < count; i++)
		{
			JudgeOverrule judgeOverrule = judgeOverrulesQueue[i];
			judgePlayerInfo = GameData.Instance.GetPlayerById(judgeOverrule.JudgePlayerId);
			overruledPlayerInfo = GameData.Instance.GetPlayerById(judgeOverrule.OverruledPlayerId);
			if (!(judgePlayerInfo == null) && !(overruledPlayerInfo == null) && !judgePlayerInfo.Disconnected && !overruledPlayerInfo.Disconnected)
			{
				winningOverrule = judgeOverrule;
				return true;
			}
		}
		return false;
	}

	private void ShowJudgeOverrule(bool anotherJudgeBeatYouToIt)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(judgeGavelPrefab, transform);
		if (anotherJudgeBeatYouToIt)
		{
			JudgeGavel component = gameObject.GetComponent<JudgeGavel>();
			if (component != null)
			{
				component.AddAnotherJudgeBeatYouToItText();
			}
		}
	}

	private void SetForegroundForDead()
	{
		hasForegroundForDeadBeenSet = true;
		SkipVoteButton.gameObject.SetActive(value: false);
		Glass.sprite = CrackedGlass;
		Glass.color = Palette.White_75Alpha;
	}

	public void RpcClose()
	{
		if (AmongUsClient.Instance.AmClient)
		{
			Close();
		}
		RpcCloseMeetingMessage rpcMessage = new RpcCloseMeetingMessage(NetId);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void CmdCastVote(byte playerId, byte suspectIdx)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			CastVote(playerId, suspectIdx);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 24, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write(playerId);
		messageWriter.Write(suspectIdx);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void CmdQueueOverruleVotes(PlayerId judgePlayerId, PlayerId targetPlayerId, ushort overruleNonce)
	{
		if (AmongUsClient.Instance.AmLocalHost || AmongUsClient.Instance.AmModdedHost)
		{
			SetJudgeOverrule(judgePlayerId, targetPlayerId, overruleNonce);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 66, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write(judgePlayerId);
		messageWriter.Write(targetPlayerId);
		messageWriter.Write(overruleNonce);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	private void RpcVotingComplete(VoterState[] states, NetworkedPlayerInfo exiled, bool tie, bool wasOverruled, ushort overruleNonce)
	{
		if (AmongUsClient.Instance.AmHost || AmongUsClient.Instance.AmLocalHost || AmongUsClient.Instance.AmModdedHost)
		{
			VotingComplete(states, exiled, tie, wasOverruled, overruleNonce);
		}
		RpcVotingCompleteMessage rpcMessage = new RpcVotingCompleteMessage(NetId, states, (exiled != null) ? exiled.PlayerId : byte.MaxValue, tie, wasOverruled, overruleNonce);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	private void RpcClearVote(PlayerId voterPlayerId)
	{
		if (PlayerControl.LocalPlayer.Data.PlayerId == (byte)voterPlayerId)
		{
			ClearVote(voterPlayerId, isLocalPlayer: true);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 25, SendOption.Reliable);
		messageWriter.Write(voterPlayerId);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public override void ClearOrDecrementDirt()
	{
		ClearDirtyBits();
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		writer.WritePacked(playerStates.Length);
		for (int i = 0; i < playerStates.Length; i++)
		{
			PlayerVoteArea playerVoteArea = playerStates[i];
			writer.StartMessage(playerVoteArea.PlayerId);
			playerStates[i].Serialize(writer);
			writer.EndMessage();
		}
		writer.WritePacked(judgeOverrulesQueue.Count);
		if (judgeOverrulesQueue.Count > 0)
		{
			foreach (JudgeOverrule item in judgeOverrulesQueue)
			{
				item.Serialize(writer);
			}
		}
		return true;
	}

	public override void Deserialize(MessageReader reader, bool initialState)
	{
		if (initialState)
		{
			PopulateButtons();
		}
		int num = reader.ReadPackedInt32();
		for (int i = 0; i < num; i++)
		{
			MessageReader msg = reader.ReadMessage();
			PlayerVoteArea playerVoteArea = playerStates.FirstOrDefault((PlayerVoteArea ps) => (byte)ps.PlayerId == msg.Tag);
			if ((bool)playerVoteArea)
			{
				playerVoteArea.Deserialize(msg);
				if (playerVoteArea.DidReport)
				{
					reporterId = playerVoteArea.PlayerId;
				}
			}
		}
		int num2 = reader.ReadPackedInt32();
		if (num2 > 0)
		{
			judgeOverrulesQueue.Clear();
			for (int num3 = 0; num3 < num2; num3++)
			{
				JudgeOverrule judgeOverrule = new JudgeOverrule();
				judgeOverrule.Deserialize(reader);
				judgeOverrulesQueue.Add(judgeOverrule);
			}
		}
		if (initialState)
		{
			PopulateReporter(reporterId);
		}
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		switch ((RpcCalls)callId)
		{
		case RpcCalls.CloseMeeting:
			Close();
			break;
		case RpcCalls.CastVote:
		{
			byte b = reader.ReadByte();
			byte b2 = reader.ReadByte();
			CastVote(b, b2);
			break;
		}
		case RpcCalls.VotingComplete:
		{
			VoterState[] array = new VoterState[reader.ReadPackedInt32()];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = VoterState.Deserialize(reader);
			}
			NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(reader.ReadByte());
			bool tie = reader.ReadBoolean();
			bool flag = reader.ReadBoolean();
			ushort overruleNonce2 = reader.ReadUInt16();
			VotingComplete(array, playerById, tie, flag, overruleNonce2);
			break;
		}
		case RpcCalls.ClearVote:
		{
			PlayerId playerId = reader.ReadByte();
			ClearVote(playerId, PlayerControl.LocalPlayer.Data.PlayerId == (byte)playerId);
			break;
		}
		case RpcCalls.QueueOverruleVotes:
		{
			PlayerId judgePlayerId = reader.ReadByte();
			PlayerId targetPlayerId = reader.ReadByte();
			ushort overruleNonce = reader.ReadUInt16();
			SetJudgeOverrule(judgePlayerId, targetPlayerId, overruleNonce);
			break;
		}
		}
	}
}
