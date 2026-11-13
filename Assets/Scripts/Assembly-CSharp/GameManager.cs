using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using Hazel;
using InnerNet;
using UnityEngine;

public abstract class GameManager : InnerNetObject, IDisconnectHandler
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<PlayerTask, bool> _003C_003E9__49_0;

		public static Action<DeadBody> _003C_003E9__51_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CCheckTaskCompletion_003Eb__49_0(PlayerTask t)
		{
			return t.IsComplete;
		}

		internal void _003CReviveEveryoneFreeplay_003Eb__51_0(DeadBody b)
		{
			GameObject gameObject = b.gameObject;
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	[SerializeField]
	public GameSettingsCategoryList gameSettingsList;

	public Dictionary<StringNames, BaseGameSetting> AllGameSettingData;

	[SerializeField]
	protected DeadBody[] deadBodyPrefab;

	protected readonly Logger logger = new Logger(Logger.Category.Gameplay, "GameManager");

	protected readonly List<GameLogicComponent> LogicComponents = new List<GameLogicComponent>();

	public GameSettingsCategoryList GameSettingsList => gameSettingsList;

	public static GameManager Instance { get; private set; }

	public bool ShouldCheckForGameEnd { get; set; }

	public bool GameHasStarted { get; set; }

	public LogicGameFlow LogicFlow { get; protected set; }

	public LogicMinigame LogicMinigame { get; protected set; }

	public LogicRoleSelection LogicRoleSelection { get; protected set; }

	public LogicUsables LogicUsables { get; protected set; }

	public LogicOptions LogicOptions { get; protected set; }

	public bool IsPersistent => false;

	public virtual bool RevealTeams => IsHideAndSeek();

	public override bool IsDirty
	{
		get
		{
			foreach (GameLogicComponent logicComponent in LogicComponents)
			{
				if (logicComponent.IsDirty)
				{
					return true;
				}
			}
			return false;
		}
	}

	public abstract void InitComponents();

	public abstract PlayerBodyTypes GetBodyType(PlayerControl player);

	public virtual void StartGame()
	{
		foreach (GameLogicComponent logicComponent in LogicComponents)
		{
			logicComponent.OnGameStart();
		}
		ShouldCheckForGameEnd = true;
		GameHasStarted = true;
		foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
		{
			allPlayer.OnGameStart();
		}
		DestroyableSingleton<HudManager>.Instance.OnGameStart();
	}

	public virtual void EndGame()
	{
		foreach (GameLogicComponent logicComponent in LogicComponents)
		{
			logicComponent.OnGameEnd();
		}
		foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
		{
			allPlayer.OnGameEnd();
		}
		ShouldCheckForGameEnd = false;
		GameHasStarted = false;
	}

	public virtual void OnPlayerDeath(PlayerControl player, bool assignGhostRole)
	{
		LogicRoleSelection.OnPlayerDeath(player, assignGhostRole);
	}

	public virtual void FinishTask(PlayerTask task)
	{
	}

	private void Awake()
	{
		if (Instance != null)
		{
			logger.Warning("There can be only one game manager! Destroying this one...");
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		AmongUsClient.Instance.DisconnectHandlers.AddUnique(this);
		InitComponents();
		AllGameSettingData = new Dictionary<StringNames, BaseGameSetting>();
		foreach (RulesCategory allCategory in gameSettingsList.AllCategories)
		{
			foreach (BaseGameSetting allGameSetting in allCategory.AllGameSettings)
			{
				if (!AllGameSettingData.ContainsKey(allGameSetting.Title))
				{
					AllGameSettingData.Add(allGameSetting.Title, allGameSetting);
				}
			}
		}
		if (!AllGameSettingData.ContainsKey(StringNames.GameNumImpostors))
		{
			IntGameSetting[] allIntSettings = Resources.FindObjectsOfTypeAll<IntGameSetting>();
			foreach (IntGameSetting s in allIntSettings)
			{
				if (s != null && s.Title == StringNames.GameNumImpostors)
				{
					AllGameSettingData[StringNames.GameNumImpostors] = s;
					break;
				}
			}
		}
		if (!(this is NormalGameManager))
		{
			return;
		}
		foreach (RoleBehaviour allRole in DestroyableSingleton<RoleManager>.Instance.AllRoles)
		{
			foreach (BaseGameSetting allGameSetting2 in allRole.AllGameSettings)
			{
				if (!AllGameSettingData.ContainsKey(allGameSetting2.Title))
				{
					AllGameSettingData.Add(allGameSetting2.Title, allGameSetting2);
				}
			}
		}
	}

	public virtual DeadBody GetDeadBody(RoleBehaviour impostorRole)
	{
		return deadBodyPrefab[0];
	}

	public bool CheckTaskCompletion()
	{
		if (DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			if (PlayerControl.LocalPlayer.myTasks.All((PlayerTask t) => t.IsComplete))
			{
				DestroyableSingleton<HudManager>.Instance.ShowPopUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameOverTaskWin));
				ShipStatus.Instance.Begin();
			}
			return false;
		}
		return CheckEndGameViaTasks();
	}

	public bool CheckEndGameViaTasks()
	{
		GameData.Instance.RecomputeTaskCounts();
		if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks)
		{
			RpcEndGame(GameOverReason.CrewmatesByTask, !DataManager.Player.Ads.HasPurchasedAdRemoval);
			return true;
		}
		return false;
	}

	public void ReviveEveryoneFreeplay()
	{
		for (int i = 0; i < GameData.Instance.PlayerCount; i++)
		{
			PlayerControl playerControl = GameData.Instance.AllPlayers[i].Object;
			if (playerControl.Data.IsDead)
			{
				if (playerControl.Data.Role.IsImpostor)
				{
					DestroyableSingleton<RoleManager>.Instance.SetRole(playerControl, RoleTypes.Impostor);
				}
				else
				{
					DestroyableSingleton<RoleManager>.Instance.SetRole(playerControl, RoleTypes.Crewmate);
				}
			}
		}
		UnityEngine.Object.FindObjectsOfType<DeadBody>().ForEach((DeadBody b) =>
		{
			UnityEngine.Object.Destroy(b.gameObject);
		});
	}

	protected T AddGameLogic<T>(T logic) where T : GameLogicComponent
	{
		LogicComponents.Add(logic);
		return logic;
	}

	public GameLogicComponent GetLogicComponent<T>()
	{
		return LogicComponents.FirstOrDefault((GameLogicComponent x) => x is T);
	}

	protected virtual void FixedUpdate()
	{
		if (AmongUsClient.Instance.AmHost && ShouldCheckForGameEnd)
		{
			LogicFlow.CheckEndCriteria();
		}
		if (!GameHasStarted)
		{
			return;
		}
		foreach (GameLogicComponent logicComponent in LogicComponents)
		{
			logicComponent.FixedUpdate();
		}
	}

	public static void DestroyInstance()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(Instance.gameObject);
			Instance = null;
		}
	}

	public override void OnDestroy()
	{
		AmongUsClient.Instance.DisconnectHandlers.Remove(this);
		foreach (GameLogicComponent logicComponent in LogicComponents)
		{
			logicComponent.OnDestroy();
		}
		base.OnDestroy();
	}

	public void HandleDisconnect(PlayerControl pc, DisconnectReasons reason)
	{
		if ((bool)pc && (bool)GameData.Instance)
		{
			OnPlayerDisconnect(pc);
		}
	}

	public void HandleDisconnect()
	{
	}

	protected virtual void OnPlayerDisconnect(PlayerControl pc)
	{
		foreach (GameLogicComponent logicComponent in LogicComponents)
		{
			logicComponent.OnPlayerDisconnect(pc);
		}
	}

	public virtual bool DidHumansWin(GameOverReason reason)
	{
		if (reason != GameOverReason.CrewmatesByTask && reason != GameOverReason.CrewmatesByVote)
		{
			return reason == GameOverReason.ImpostorDisconnect;
		}
		return true;
	}

	public virtual bool DidImpostorsWin(GameOverReason reason)
	{
		if (reason != GameOverReason.ImpostorsByKill && reason != GameOverReason.ImpostorsBySabotage && reason != GameOverReason.ImpostorsByVote)
		{
			return reason == GameOverReason.CrewmateDisconnect;
		}
		return true;
	}

	public abstract MapOptions GetMapOptions();

	public virtual bool CanReportBodies()
	{
		return true;
	}

	public virtual bool SabotagesEnabled()
	{
		return true;
	}

	public virtual bool ShowCrewmatesKilled()
	{
		return IsHideAndSeek();
	}

	public virtual bool IsNormal()
	{
		return false;
	}

	public virtual bool IsHideAndSeek()
	{
		return false;
	}

	public virtual void SetSpecialCosmetics(PlayerControl p)
	{
	}

	public BaseGameSetting GetSettingDataByName(StringNames name)
	{
		AllGameSettingData.TryGetValue(name, out var value);
		return value;
	}

	public void RpcEndGame(GameOverReason endReason, bool showAd)
	{
		ShouldCheckForGameEnd = false;
		logger.Info($"Endgame for {endReason}");
		MessageWriter messageWriter = AmongUsClient.Instance.StartEndGame();
		messageWriter.Write((byte)endReason);
		messageWriter.Write(showAd);
		AmongUsClient.Instance.FinishEndGame(messageWriter);
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		foreach (GameLogicComponent logicComponent in LogicComponents)
		{
			logicComponent.HandleRPC((RpcCalls)callId, reader);
		}
	}

	public override void ClearOrDecrementDirt()
	{
		for (int i = 0; i < LogicComponents.Count; i++)
		{
			GameLogicComponent gameLogicComponent = LogicComponents[i];
			if (gameLogicComponent.IsDirty)
			{
				gameLogicComponent.ClearDirtyFlag();
			}
		}
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		bool result = false;
		for (int i = 0; i < LogicComponents.Count; i++)
		{
			GameLogicComponent gameLogicComponent = LogicComponents[i];
			if (initialState || gameLogicComponent.IsDirty)
			{
				result = true;
				writer.StartMessage((byte)i);
				gameLogicComponent.Serialize(writer);
				writer.EndMessage();
			}
		}
		return result;
	}

	public override void Deserialize(MessageReader reader, bool initialState)
	{
		while (reader.Position < reader.Length)
		{
			MessageReader messageReader = reader.ReadMessage();
			int num = messageReader.Tag;
			if (num < 0 || num > LogicComponents.Count)
			{
				logger.Error("OUT OF BOUNDS IN DESERIALIZE!!");
			}
			else
			{
				LogicComponents[num].Deserialize(messageReader);
			}
		}
	}
}
