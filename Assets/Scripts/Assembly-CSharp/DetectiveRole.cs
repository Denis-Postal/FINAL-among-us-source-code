using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using UnityEngine;

public class DetectiveRole : CrewmateRole
{
	[SerializeField]
	private GameObject notesPrefab;

	[SerializeField]
	private GameObject inspectionGraphic;

	[SerializeField]
	private GameObject abilityInfoPrefab;

	[SerializeField]
	private AudioClip interrogateSFX;

	private int currentNotesIndex;

	public List<DetectiveNotesPageInfo> notesPageInfos = new List<DetectiveNotesPageInfo>();

	private DetectiveNotesMinigame notesMinigame;

	private List<NetworkedPlayerInfo> deadPlayers = new List<NetworkedPlayerInfo>();

	private DetectiveAbilityInfo abilityInfo;

	private DetectiveAbilityInfo meetingAbilityInfo;

	[SerializeField]
	private PlayerControl currentTarget;

	[Header("Ship Locations")]
	[SerializeField]
	private GameObject skeldShipLocationsPrefab;

	[SerializeField]
	private GameObject miraShipLocationsPrefab;

	[SerializeField]
	private GameObject polusShipLocationsPrefab;

	[SerializeField]
	private GameObject airshipShipLocationsPrefab;

	[SerializeField]
	private GameObject fungleShipLocationsPrefab;

	private float cooldownSecondsRemaining;

	private float interrogateCooldown = 10f;

	private bool IsCoolingDown => cooldownSecondsRemaining > 0f;

	public override void Initialize(PlayerControl player)
	{
		base.Initialize(player);
		GameObject gameObject = Object.Instantiate(abilityInfoPrefab, DestroyableSingleton<HudManager>.Instance.AbilityButton.transform, worldPositionStays: true);
		gameObject.transform.localPosition = new Vector3(-0.1f, -0.15f, -1f);
		gameObject.transform.localScale = Vector3.one;
		abilityInfo = gameObject.GetComponent<DetectiveAbilityInfo>();
		abilityInfo.gameObject.SetActive(value: false);
		if (Player.AmOwner)
		{
			CreateMapLocations();
		}
		if (Object.FindObjectsOfType<DeadBody>().Any())
		{
			DeadBody[] array = Object.FindObjectsOfType<DeadBody>();
			for (int i = 0; i < array.Length; i++)
			{
				DetectiveDeadBodyInfo detectiveDeadBodyInfo = new DetectiveDeadBodyInfo(array[i].ParentId);
				CreatePlayerLocations(detectiveDeadBodyInfo);
				DetectiveLocationsController.Instance.AddBodyInfo(detectiveDeadBodyInfo);
			}
		}
	}

	public override void SetCooldown()
	{
		cooldownSecondsRemaining = interrogateCooldown;
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetCoolDown(cooldownSecondsRemaining, interrogateCooldown);
	}

	public override void UseAbility()
	{
		if (Player.AmOwner && !Player.Data.IsDead && Player.moveable && !IsAffectedByComms && !Minigame.Instance)
		{
			OpenNotes();
		}
	}

	public override void UseSecondaryAbility()
	{
		if (Player.AmOwner && !IsAffectedByComms && !Player.Data.IsDead && Player.moveable && !Minigame.Instance && !IsCoolingDown && !(currentTarget == null))
		{
			Interrogate();
			SetPlayerTarget(null);
		}
	}

	private void Interrogate()
	{
		if (Player.AmOwner && DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.CanInteract() && !(currentTarget == null) && !DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen)
		{
			notesPageInfos[currentNotesIndex].AddSuspect(currentTarget.Data);
			SoundManager.Instance.PlaySound(interrogateSFX, loop: false);
			OpenNotes();
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetDisabled();
		}
	}

	private void AddVictimToNotes(NetworkedPlayerInfo playerInfo)
	{
		if (!notesPageInfos.Any((DetectiveNotesPageInfo pageInfo) => pageInfo.victimPlayer == playerInfo) && !playerInfo.WasEjected)
		{
			SetAbilityInfo(playerInfo);
			OpenNotes(playerInfo, openInterface: false);
		}
	}

	private void OpenNotes(NetworkedPlayerInfo victimPlayer = null, bool openInterface = true)
	{
		if (((bool)notesMinigame & openInterface) || DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening || DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen || ((bool)MatchInfoGuide.Instance && MatchInfoGuide.Instance.IsActive))
		{
			return;
		}
		GameObject gameObject = Object.Instantiate(notesPrefab);
		notesMinigame = gameObject.GetComponent<DetectiveNotesMinigame>();
		notesMinigame.transform.SetParent(Camera.main.transform, worldPositionStays: false);
		notesMinigame.transform.localPosition = new Vector3(0f, 0f, -34f);
		if (victimPlayer != null)
		{
			if (notesPageInfos.All((DetectiveNotesPageInfo info) => info.victimPlayer != victimPlayer))
			{
				notesMinigame.GenerateNewPage(this, victimPlayer);
			}
			else
			{
				DetectiveNotesPageInfo pageInfo = notesPageInfos.First((DetectiveNotesPageInfo info) => info.victimPlayer == victimPlayer);
				notesMinigame.OpenExistingPage(this, pageInfo);
			}
		}
		else if (notesPageInfos.Count > 0)
		{
			DetectiveNotesPageInfo pageInfo2 = notesPageInfos[currentNotesIndex];
			notesMinigame.OpenExistingPage(this, pageInfo2);
		}
		else
		{
			notesMinigame.SetAssociatedDetective(this);
		}
		if (openInterface)
		{
			notesMinigame.Begin(null);
		}
		else
		{
			notesMinigame.Close();
		}
	}

	public override void SetPlayerTarget(PlayerControl target)
	{
		if (Player.AmOwner && !Player.Data.IsDead && notesPageInfos.Count != 0)
		{
			RoleTeamTypes teamType = Player.Data.Role.TeamType;
			if (target != currentTarget && currentTarget != null)
			{
				currentTarget.ToggleHighlight(active: false, teamType);
			}
			currentTarget = target;
			if ((float)notesPageInfos[currentNotesIndex].suspects.Count >= GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.DetectiveSuspectLimit))
			{
				ClearPlayerTarget();
			}
			else if ((bool)currentTarget && !target.Data.IsDead && !IsAffectedByComms)
			{
				currentTarget.ToggleHighlight(active: true, teamType);
				DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetEnabled();
			}
		}
	}

	private void ClearPlayerTarget()
	{
		if (Player.AmOwner)
		{
			if (currentTarget != null)
			{
				currentTarget.ToggleHighlight(active: false, Player.Data.Role.TeamType);
			}
			currentTarget = null;
		}
	}

	public override PlayerControl FindClosestTarget()
	{
		if (!Player.AmOwner)
		{
			return null;
		}
		List<PlayerControl> playersInAbilityRangeSorted = GetPlayersInAbilityRangeSorted(RoleBehaviour.GetTempPlayerList());
		if (playersInAbilityRangeSorted.Count <= 0)
		{
			return null;
		}
		return playersInAbilityRangeSorted[0];
	}

	protected override float GetAbilityDistance()
	{
		return 0.8f;
	}

	private void FixedUpdate()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance || (bool)ExileController.Instance)
		{
			return;
		}
		if (cooldownSecondsRemaining > 0f)
		{
			cooldownSecondsRemaining -= Time.deltaTime;
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetCoolDown(cooldownSecondsRemaining, interrogateCooldown);
		}
		if (!Player.moveable || Player.Data.IsDead || IsAffectedByComms)
		{
			if ((bool)notesMinigame)
			{
				notesMinigame.Close();
			}
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
			abilityInfo.SetDisabled();
		}
		else if (!DestroyableSingleton<HudManager>.Instance.AbilityButton.CanInteract())
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			abilityInfo.SetEnabled();
		}
		if (!Player.moveable || Player.Data.IsDead || IsAffectedByComms || currentTarget == null)
		{
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetDisabled();
		}
	}

	public override void OnMeetingStart()
	{
		if (!Player.AmOwner)
		{
			return;
		}
		base.OnMeetingStart();
		if ((bool)notesMinigame)
		{
			notesMinigame.Close();
		}
		UpdateDeadPlayers();
		GameObject gameObject = Object.Instantiate(abilityInfoPrefab, MeetingHud.Instance.MeetingAbilityButton.transform, worldPositionStays: true);
		gameObject.transform.localPosition = new Vector3(-0.1f, -0.15f, -1f);
		gameObject.transform.localScale = Vector3.one;
		meetingAbilityInfo = gameObject.GetComponent<DetectiveAbilityInfo>();
		meetingAbilityInfo.gameObject.SetActive(value: false);
		if (abilityInfo.CurrentPlayerInfo != null)
		{
			meetingAbilityInfo.SetPlayerInfo(abilityInfo.CurrentPlayerInfo, abilityInfo.SuspectCount, isMeeting: true);
			meetingAbilityInfo.gameObject.SetActive(value: true);
		}
		foreach (NetworkedPlayerInfo deadPlayer in deadPlayers)
		{
			AddVictimToNotes(deadPlayer);
		}
	}

	private void SetAbilityInfo(NetworkedPlayerInfo playerInfo)
	{
		int suspectCount = 0;
		if (notesPageInfos.Count > currentNotesIndex)
		{
			suspectCount = notesPageInfos[currentNotesIndex].suspects.Count;
		}
		if (abilityInfo != null)
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetSecondImage(Ability);
			abilityInfo.gameObject.SetActive(value: true);
			abilityInfo.SetPlayerInfo(playerInfo, suspectCount);
		}
		if (meetingAbilityInfo != null)
		{
			MeetingHud.Instance.MeetingAbilityButton.SetSecondImage(Ability);
			meetingAbilityInfo.gameObject.SetActive(value: true);
			meetingAbilityInfo.SetPlayerInfo(playerInfo, suspectCount, isMeeting: true);
		}
	}

	public override void OnVotingComplete()
	{
		UpdateDeadPlayers();
		if (Player.AmOwner)
		{
			SetCooldown();
		}
	}

	private void UpdateDeadPlayers()
	{
		deadPlayers.Clear();
		foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
		{
			if (allPlayer.IsDead || allPlayer.WasEjected)
			{
				deadPlayers.Add(allPlayer);
			}
		}
	}

	public bool PlayerWasDead(DetectiveSuspect playerInfo)
	{
		return deadPlayers.Contains(playerInfo.playerInfo);
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}

	public void SetCurrentIndex(int _index)
	{
		currentNotesIndex = _index;
		if (currentNotesIndex >= 0 && notesPageInfos.Count > 0)
		{
			SetAbilityInfo(notesPageInfos[currentNotesIndex].victimPlayer);
		}
	}

	public override void KillAnimSpecialSetup(DeadBody deadBody, PlayerControl killer, PlayerControl victim)
	{
		if (Player.AmOwner)
		{
			DetectiveDeadBodyInfo detectiveDeadBodyInfo = new DetectiveDeadBodyInfo(victim.PlayerId);
			CreatePlayerLocations(detectiveDeadBodyInfo);
			DetectiveLocationsController.Instance.AddBodyInfo(detectiveDeadBodyInfo);
		}
	}

	private void CreatePlayerLocations(DetectiveDeadBodyInfo deadBodyInfo)
	{
		if (deadBodyInfo.PlayerLocationsAtDeath.Any())
		{
			return;
		}
		new List<KeyValuePair<byte, int>>();
		foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
		{
			if (!allPlayer.IsDead && !allPlayer.Disconnected)
			{
				deadBodyInfo.PlayerLocationsAtDeath.Add(allPlayer.PlayerId, RoomTracker.Instance.GetRoomForPlayer(allPlayer));
			}
		}
	}

	public string GetPlayerLocation(byte playerID, byte victimID)
	{
		if (!DetectiveLocationsController.Instance.DeadBodyInfos.Exists((DetectiveDeadBodyInfo info) => info.victimID == victimID))
		{
			return DetectiveLocationsController.Instance.DefaultLocation.ToString();
		}
		return DetectiveLocationsController.Instance.DeadBodyInfos.First((DetectiveDeadBodyInfo info) => info.victimID == victimID).GetPlayerLocation(playerID);
	}

	private void CreateMapLocations()
	{
		if ((bool)DetectiveLocationsController.Instance)
		{
			return;
		}
		ShipStatus instance = ShipStatus.Instance;
		if (!(instance is SkeldShipStatus))
		{
			if (!(instance is MiraShipStatus))
			{
				if (!(instance is PolusShipStatus))
				{
					if (!(instance is AirshipStatus))
					{
						if (instance is FungleShipStatus)
						{
							Object.Instantiate(fungleShipLocationsPrefab);
						}
					}
					else
					{
						Object.Instantiate(airshipShipLocationsPrefab);
					}
				}
				else
				{
					Object.Instantiate(polusShipLocationsPrefab);
				}
			}
			else
			{
				Object.Instantiate(miraShipLocationsPrefab);
			}
		}
		else
		{
			Object.Instantiate(skeldShipLocationsPrefab);
		}
	}

	public override void OnDeath(DeathReason reason)
	{
		base.OnDeath(reason);
		ClearPlayerTarget();
		Object.Destroy(abilityInfo.gameObject);
	}

	public override void OnRoleSet()
	{
		base.OnRoleSet();
		Object.Destroy(abilityInfo.gameObject);
	}
}
