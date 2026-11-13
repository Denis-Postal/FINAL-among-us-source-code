using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using Hazel;
using InnerNet;
using UnityEngine;

public abstract class LogicOptions : GameLogicComponent
{
	private readonly GameOptionsFactory gameOptionsFactory= new GameOptionsFactory(new UnityLogger());

	protected abstract IGameOptions currentGameOptions { get; }

	public byte MapId => currentGameOptions.MapId;

	public int MaxPlayers => currentGameOptions.MaxPlayers;

	public int NumImpostors => currentGameOptions.NumImpostors;

	public bool IsDefaults => currentGameOptions.IsDefaults;

	public LogicOptions(GameManager manager)
		: base(manager)
	{
	}

	public virtual int GetAdjustedNumImpostors(int playerCount)
	{
		return currentGameOptions.GetAdjustedNumImpostors(playerCount);
	}

	public virtual void SetRecommendations(int numPlayers, NetworkModes networkMode)
	{
		currentGameOptions.SetRecommendations(numPlayers, networkMode);
		SyncOptions();
	}

	public void SyncOptions()
	{
		if (AmongUsClient.Instance.AmHost)
		{
			SetDirty();
			if (PlayerControl.LocalPlayer != null)
			{
				PlayerControl.LocalPlayer.RpcSyncSettings(gameOptionsFactory.ToBytes(currentGameOptions, AprilFoolsMode.IsAprilFoolsModeToggledOn));
			}
		}
	}

	protected abstract void SetGameOptions(IGameOptions newOptions);

	public virtual bool GetGhostsDoTasks()
	{
		if (!currentGameOptions.TryGetBool(BoolOptionNames.GhostsDoTasks, out var value))
		{
			return false;
		}
		return value;
	}

	public virtual float GetKillCooldown()
	{
		if (!currentGameOptions.TryGetFloat(FloatOptionNames.KillCooldown, out var value))
		{
			return 1f;
		}
		return value;
	}

	public virtual float GetKillDistance()
	{
		float[] floatArray = currentGameOptions.GetFloatArray(FloatArrayOptionNames.KillDistances);
		int value = currentGameOptions.GetInt(Int32OptionNames.KillDistance);
		return floatArray[Mathf.Clamp(value, 0, floatArray.Length - 1)];
	}

	public virtual float GetPlayerSpeedMod(PlayerControl pc)
	{
		if (!currentGameOptions.TryGetFloat(FloatOptionNames.PlayerSpeedMod, out var value))
		{
			return 1f;
		}
		return value;
	}

	public virtual bool GetConfirmImpostor()
	{
		if (!currentGameOptions.TryGetBool(BoolOptionNames.ConfirmImpostor, out var value))
		{
			return false;
		}
		return value;
	}

	public virtual int GetEmergencyCooldown()
	{
		if (!currentGameOptions.TryGetInt(Int32OptionNames.EmergencyCooldown, out var value))
		{
			return 1;
		}
		return value;
	}

	public virtual int GetNumEmergencyMeetings()
	{
		if (!currentGameOptions.TryGetInt(Int32OptionNames.NumEmergencyMeetings, out var value))
		{
			return 0;
		}
		return value;
	}

	public virtual bool GetVisualTasks()
	{
		if (!currentGameOptions.TryGetBool(BoolOptionNames.VisualTasks, out var value))
		{
			return false;
		}
		return value;
	}

	public virtual bool GetAnonymousVotes()
	{
		if (!currentGameOptions.TryGetBool(BoolOptionNames.AnonymousVotes, out var value))
		{
			return false;
		}
		return value;
	}

	public virtual TaskBarMode GetTaskBarMode()
	{
		if (!currentGameOptions.TryGetInt(Int32OptionNames.TaskBarMode, out var value))
		{
			return TaskBarMode.Normal;
		}
		return (TaskBarMode)value;
	}

	public virtual float GetEngineerCooldown()
	{
		return GetRoleFloat(FloatOptionNames.EngineerCooldown);
	}

	public virtual float GetEngineerInVentTime()
	{
		return GetRoleFloat(FloatOptionNames.EngineerInVentMaxTime);
	}

	public bool GetRoleBool(BoolOptionNames roleBoolName)
	{
		if (!currentGameOptions.TryGetBool(roleBoolName, out var value))
		{
			return false;
		}
		return value;
	}

	public float GetRoleFloat(FloatOptionNames roleFloatName)
	{
		if (!currentGameOptions.TryGetFloat(roleFloatName, out var value))
		{
			return 1f;
		}
		return value;
	}

	public float GetRoleInt(Int32OptionNames roleIntName)
	{
		int value;
		return currentGameOptions.TryGetInt(roleIntName, out value) ? value : 0;
	}

	public virtual bool GetShowCrewmateNames()
	{
		return true;
	}

	public override bool Serialize(MessageWriter writer)
	{
		writer.WriteBytesAndSize(gameOptionsFactory.ToBytes(currentGameOptions, AprilFoolsMode.IsAprilFoolsModeToggledOn));
		return true;
	}

	public override void Deserialize(MessageReader reader)
	{
		IGameOptions gameOptions = gameOptionsFactory.FromNetworkMessageWithSize(reader);
		SetGameOptions(gameOptions);
		GameOptionsManager.Instance.CurrentGameOptions = gameOptions;
		if (GameOptionsManager.Instance.CurrentGameOptions.TryClearAprilFoolsMode())
		{
			AprilFoolsMode.IsAprilFoolsModeToggledOn = true;
		}
		else
		{
			AprilFoolsMode.IsAprilFoolsModeToggledOn = false;
		}
	}
}
