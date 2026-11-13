using System.Collections.Generic;
using Hazel;
using UnityEngine;

public class LogicHnSDangerLevel : GameLogicComponent
{
	private DangerMeter dangerMeter;

	private List<PlayerControl> impostors;

	private float scaryMusicDistance;

	private float veryScaryMusicDistance;

	private float dangerLevel1;

	private float dangerLevel2;

	private HideAndSeekManager hnsManager;

	private bool firstMusicActivation;

	private float firstCrossfadeCountdown;

	public LogicHnSDangerLevel(HideAndSeekManager manager)
		: base(manager)
	{
		hnsManager = manager;
	}

	public override void OnGameStart()
	{
		firstMusicActivation = true;
		if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor)
		{
			dangerMeter = DestroyableSingleton<HudManager>.Instance.DangerMeter;
			dangerMeter.gameObject.SetActive(value: true);
		}
		impostors = new List<PlayerControl>();
		foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
		{
			if (allPlayerControl.Data?.Role != null && allPlayerControl.Data.Role.IsImpostor)
			{
				impostors.Add(allPlayerControl);
			}
		}
		scaryMusicDistance = hnsManager.LogicOptionsHnS.GetScaryMusicDistance() * hnsManager.LogicOptionsHnS.PlayerSpeedBase;
		veryScaryMusicDistance = hnsManager.LogicOptionsHnS.GetVeryScaryMusicDistance() * hnsManager.LogicOptionsHnS.PlayerSpeedBase;
		if (scaryMusicDistance < veryScaryMusicDistance)
		{
			float num = veryScaryMusicDistance;
			float num2 = scaryMusicDistance;
			scaryMusicDistance = num;
			veryScaryMusicDistance = num2;
		}
	}

	public override void OnPlayerDisconnect(PlayerControl pc)
	{
		if (pc.Data.Role.IsImpostor)
		{
			impostors?.Remove(pc);
		}
	}

	public override void OnGameEnd()
	{
		impostors = null;
	}

	public override void FixedUpdate()
	{
		PlayerControl localPlayer = PlayerControl.LocalPlayer;
		if (impostors == null || localPlayer == null || impostors.Count <= 0)
		{
			return;
		}
		float num = float.MaxValue;
		foreach (PlayerControl impostor in impostors)
		{
			if (!(impostor == null))
			{
				float sqrMagnitude = (impostor.transform.position - localPlayer.transform.position).sqrMagnitude;
				if (sqrMagnitude < scaryMusicDistance && num > sqrMagnitude)
				{
					num = sqrMagnitude;
				}
			}
		}
		if (ShipStatus.Instance.HideCountdown > 0f)
		{
			dangerLevel1 = 0f;
			dangerLevel2 = 0f;
		}
		else
		{
			if (firstMusicActivation)
			{
				firstMusicActivation = false;
				firstCrossfadeCountdown = 3f;
				hnsManager.LogicMusic.SetMusicCrossfadeSpeed(0.6f);
			}
			if (firstCrossfadeCountdown > 0f)
			{
				firstCrossfadeCountdown -= Time.deltaTime;
				if (firstCrossfadeCountdown <= 0f)
				{
					hnsManager.LogicMusic.SetMusicCrossfadeSpeed(5f);
				}
			}
			dangerLevel1 = Mathf.Clamp01((scaryMusicDistance - num) / (scaryMusicDistance - veryScaryMusicDistance));
			dangerLevel2 = Mathf.Clamp01((veryScaryMusicDistance - num) / veryScaryMusicDistance);
		}
		UpdateDangerMeter();
		UpdateDangerMusic();
	}

	private void UpdateDangerMusic()
	{
		PlayerControl localPlayer = PlayerControl.LocalPlayer;
		if (localPlayer != null && localPlayer.Data != null && localPlayer.Data.IsDead)
		{
			hnsManager.LogicMusic.SetTaskState(isDoingTask: false);
			hnsManager.LogicMusic.ResetMusic();
		}
		else
		{
			hnsManager.LogicMusic.SetMusicValues(dangerLevel1, dangerLevel2);
		}
	}

	private void UpdateDangerMeter()
	{
		if (!(dangerMeter == null))
		{
			dangerMeter.SetDangerValue(dangerLevel1, dangerLevel2);
		}
	}

	public override void OnDestroy()
	{
		impostors = null;
	}

	public override bool Serialize(MessageWriter writer)
	{
		return false;
	}

	public override void Deserialize(MessageReader reader)
	{
	}
}
