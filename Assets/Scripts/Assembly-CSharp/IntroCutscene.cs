using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PowerTools;
using TMPro;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
	private const float BaseY = -1f;

	private const float ScaleAll = 1f;

	private const float OffsetWidth = 0.9f;

	private const float OffsetHeight = 0.15f;

	public static IntroCutscene Instance;

	public TextMeshPro TeamTitle;

	public TextMeshPro ImpostorText;

	public TextMeshPro YouAreText;

	public TextMeshPro RoleText;

	public TextMeshPro RoleBlurbText;

	public TextMeshPro HideAndSeekTimerText;

	public PoolablePlayer PlayerPrefab;

	public PoolablePlayer OurCrewmate;

	public GameObject HideAndSeekPanels;

	public GameObject CrewmateRules;

	public GameObject ImpostorRules;

	public MeshRenderer BackgroundBar;

	public MeshRenderer Foreground;

	public FloatRange ForegroundRadius;

	public SpriteRenderer FrontMost;

	public AudioClip IntroStinger;

	[Header("HnS Temp")]
	public TextMeshPro ImpostorName;

	public TextMeshPro ImpostorTitle;

	private Vector3 impostorPos = new Vector3(-1.23f, -1.46f, -24f);

	private float impostorScale = 1f;

	[SerializeField]
	private AnimationClip HnSSeekerSpawnAnim;

	[SerializeField]
	private AnimationClip HnSSeekerSpawnHorseAnim;

	[SerializeField]
	private AnimationClip HnSSeekerSpawnHorseInGameAnim;

	[SerializeField]
	private AnimationClip HnSSeekerSpawnLongAnim;

	[SerializeField]
	private AnimationClip HnSSeekerSpawnLongInGameAnim;

	public PoolablePlayer HideAndSeekPlayerVisual;

	public PoolablePlayer HorseWrangleVisualSuit;

	public PoolablePlayer HorseWrangleVisualPlayer;

	private PoolablePlayer ourCrewmate;

	private DualshockLightManager.LightOverlayHandle overlayHandle;

	public IEnumerator CoBegin()
	{
		Logger.GlobalInstance.Info("IntroCutscene :: CoBegin() :: Starting intro cutscene");
		SoundManager.Instance.PlaySound(IntroStinger, loop: false);
		if (GameManager.Instance.IsNormal())
		{
			Logger.GlobalInstance.Info("IntroCutscene :: CoBegin() :: Game Mode: Normal");
			LogPlayerRoleData();
			HideAndSeekPanels.SetActive(value: false);
			CrewmateRules.SetActive(value: false);
			ImpostorRules.SetActive(value: false);
			ImpostorName.gameObject.SetActive(value: false);
			ImpostorTitle.gameObject.SetActive(value: false);
			List<PlayerControl> list = SelectTeamToShow((NetworkedPlayerInfo pcd) => !PlayerControl.LocalPlayer.Data.Role.IsImpostor || pcd.Role.TeamType == PlayerControl.LocalPlayer.Data.Role.TeamType);
			if (list == null || list.Count < 1)
			{
				Logger.GlobalInstance.Error("IntroCutscene :: CoBegin() :: teamToShow is EMPTY or NULL");
			}
			if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
			{
				ImpostorText.gameObject.SetActive(value: false);
			}
			else
			{
				int adjustedNumImpostors = GameManager.Instance.LogicOptions.GetAdjustedNumImpostors(GameData.Instance.PlayerCount);
				if (adjustedNumImpostors == 1)
				{
					ImpostorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NumImpostorsS);
				}
				else
				{
					ImpostorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NumImpostorsP, adjustedNumImpostors);
				}
				ImpostorText.text = ImpostorText.text.Replace("[FF1919FF]", "<color=#FF1919FF>");
				ImpostorText.text = ImpostorText.text.Replace("[]", "</color>");
			}
			yield return ShowTeam(list, 3f);
			yield return ShowRole();
		}
		else
		{
			Logger.GlobalInstance.Info("IntroCutscene :: CoBegin() :: Game Mode: Hide and Seek");
			LogPlayerRoleData();
			HideAndSeekPanels.SetActive(value: true);
			if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
			{
				CrewmateRules.SetActive(value: false);
				ImpostorRules.SetActive(value: true);
			}
			else
			{
				CrewmateRules.SetActive(value: true);
				ImpostorRules.SetActive(value: false);
			}
			List<PlayerControl> list2 = SelectTeamToShow((NetworkedPlayerInfo pcd) => PlayerControl.LocalPlayer.Data.Role.IsImpostor != pcd.Role.IsImpostor);
			if (list2 == null || list2.Count < 1)
			{
				Logger.GlobalInstance.Error("IntroCutscene :: CoBegin() :: teamToShow is EMPTY or NULL");
			}
			PlayerControl impostor = PlayerControl.AllPlayerControls.Find((PlayerControl pc) => pc.Data.Role.IsImpostor);
			if (impostor == null)
			{
				Logger.GlobalInstance.Error("IntroCutscene :: CoBegin() :: impostor is NULL");
			}
			if (impostor != null)
			{
				GameManager.Instance.SetSpecialCosmetics(impostor);
			}
			ImpostorName.gameObject.SetActive(value: true);
			ImpostorTitle.gameObject.SetActive(value: true);
			BackgroundBar.enabled = false;
			TeamTitle.gameObject.SetActive(value: false);
			if (impostor != null)
			{
				ImpostorName.text = impostor.Data.PlayerName;
			}
			else
			{
				ImpostorName.text = "???";
			}
			yield return new WaitForSecondsRealtime(0.1f);
			PoolablePlayer playerSlot = null;
			if (impostor != null)
			{
				playerSlot = CreatePlayer(1, 1, impostor.Data, impostorPositioning: false);
				playerSlot.SetBodyType(PlayerBodyTypes.Normal);
				playerSlot.SetFlipX(flipped: false);
				playerSlot.transform.localPosition = impostorPos;
				playerSlot.transform.localScale = Vector3.one * impostorScale;
			}
			yield return ShipStatus.Instance.CosmeticsCache.PopulateFromPlayers();
			yield return new WaitForSecondsRealtime(6f);
			if (playerSlot != null)
			{
				playerSlot.gameObject.SetActive(value: false);
			}
			HideAndSeekPanels.SetActive(value: false);
			CrewmateRules.SetActive(value: false);
			ImpostorRules.SetActive(value: false);
			LogicOptionsHnS logicOptionsHnS = GameManager.Instance.LogicOptions as LogicOptionsHnS;
			(GameManager.Instance.GetLogicComponent<LogicHnSMusic>() as LogicHnSMusic)?.StartMusicWithIntro();
			if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
			{
				float crewmateLeadTime = logicOptionsHnS.GetCrewmateLeadTime();
				HideAndSeekTimerText.gameObject.SetActive(value: true);
				PoolablePlayer poolablePlayer;
				AnimationClip anim;
				if (AprilFoolsMode.ShouldHorseAround())
				{
					poolablePlayer = HorseWrangleVisualSuit;
					poolablePlayer.gameObject.SetActive(value: true);
					poolablePlayer.SetBodyType(PlayerBodyTypes.Seeker);
					anim = HnSSeekerSpawnHorseAnim;
					HorseWrangleVisualPlayer.SetBodyType(PlayerBodyTypes.Normal);
					HorseWrangleVisualPlayer.UpdateFromPlayerData(PlayerControl.LocalPlayer.Data, PlayerControl.LocalPlayer.CurrentOutfitType, PlayerMaterial.MaskType.None, includePet: false);
				}
				else if (AprilFoolsMode.ShouldLongAround())
				{
					poolablePlayer = HideAndSeekPlayerVisual;
					poolablePlayer.gameObject.SetActive(value: true);
					poolablePlayer.SetBodyType(PlayerBodyTypes.LongSeeker);
					anim = HnSSeekerSpawnLongAnim;
				}
				else
				{
					poolablePlayer = HideAndSeekPlayerVisual;
					poolablePlayer.gameObject.SetActive(value: true);
					poolablePlayer.SetBodyType(PlayerBodyTypes.Seeker);
					anim = HnSSeekerSpawnAnim;
				}
				poolablePlayer.SetBodyCosmeticsVisible(b: false);
				poolablePlayer.UpdateFromPlayerData(PlayerControl.LocalPlayer.Data, PlayerControl.LocalPlayer.CurrentOutfitType, PlayerMaterial.MaskType.None, includePet: false);
				SpriteAnim component = poolablePlayer.GetComponent<SpriteAnim>();
				poolablePlayer.gameObject.SetActive(value: true);
				poolablePlayer.ToggleName(active: false);
				component.Play(anim);
				while (crewmateLeadTime > 0f)
				{
					HideAndSeekTimerText.text = Mathf.RoundToInt(crewmateLeadTime).ToString();
					crewmateLeadTime -= Time.deltaTime;
					yield return null;
				}
			}
			else
			{
				ShipStatus.Instance.HideCountdown = logicOptionsHnS.GetCrewmateLeadTime();
				if (AprilFoolsMode.ShouldHorseAround())
				{
					if (impostor != null)
					{
						impostor.AnimateCustom(HnSSeekerSpawnHorseInGameAnim);
					}
				}
				else if (AprilFoolsMode.ShouldLongAround())
				{
					if (impostor != null)
					{
						impostor.AnimateCustom(HnSSeekerSpawnLongInGameAnim);
					}
				}
				else if (impostor != null)
				{
					impostor.AnimateCustom(HnSSeekerSpawnAnim);
					impostor.cosmetics.SetBodyCosmeticsVisible(b: false);
				}
			}
		}
		ShipStatus.Instance.StartSFX();
		UnityEngine.Object.Destroy(gameObject);
	}

	public void CheckSeekerHand_AnimEvent()
	{
		if (HideAndSeekPlayerVisual.HasHat())
		{
			HideAndSeekPlayerVisual.GetHandHat().gameObject.SetActive(value: true);
			HideAndSeekPlayerVisual.ToggleHat(b: true);
		}
	}

	private static List<PlayerControl> SelectTeamToShow(Func<NetworkedPlayerInfo, bool> roleFilter)
	{
		return (from pcd in GameData.Instance.AllPlayers
			where !pcd.Disconnected && roleFilter(pcd)
			select pcd.Object into pc
			orderby (!(pc == PlayerControl.LocalPlayer)) ? 1 : 0
			select pc).ToList();
	}

	private IEnumerator ShowTeam(List<PlayerControl> teamToShow, float duration)
	{
		if (overlayHandle == null)
		{
			overlayHandle = DestroyableSingleton<DualshockLightManager>.Instance.AllocateLight();
		}
		yield return ShipStatus.Instance.CosmeticsCache.PopulateFromPlayers();
		if (!teamToShow.TrueForAll((PlayerControl p) => p.Data.Role.IsImpostor))
		{
			BeginCrewmate(teamToShow);
			overlayHandle.color = Palette.CrewmateBlue;
		}
		else
		{
			BeginImpostor(teamToShow);
			overlayHandle.color = Palette.ImpostorRed;
		}
		Color c = TeamTitle.color;
		Color fade = Color.black;
		Color impColor = Color.white;
		Vector3 titlePos = TeamTitle.transform.localPosition;
		float timer = 0f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			float num = Mathf.Min(1f, timer / duration);
			Foreground.material.SetFloat("_Rad", ForegroundRadius.ExpOutLerp(num * 2f));
			fade.a = Mathf.Lerp(1f, 0f, num * 3f);
			FrontMost.color = fade;
			c.a = Mathf.Clamp(FloatRange.ExpOutLerp(num, 0f, 1f), 0f, 1f);
			TeamTitle.color = c;
			RoleText.color = c;
			impColor.a = Mathf.Lerp(0f, 1f, (num - 0.3f) * 3f);
			ImpostorText.color = impColor;
			titlePos.y = 2.7f - num * 0.3f;
			TeamTitle.transform.localPosition = titlePos;
			overlayHandle.color.a = Mathf.Min(1f, timer * 2f);
			yield return null;
		}
		timer = 0f;
		while (timer < 1f)
		{
			timer += Time.deltaTime;
			float num2 = timer / 1f;
			fade.a = Mathf.Lerp(0f, 1f, num2 * 3f);
			FrontMost.color = fade;
			overlayHandle.color.a = 1f - fade.a;
			yield return null;
		}
	}

	private void BeginCrewmate(List<PlayerControl> teamToDisplay)
	{
		Vector3 position = BackgroundBar.transform.position;
		position.y -= 0.25f;
		BackgroundBar.transform.position = position;
		BackgroundBar.material.SetColor("_Color", Palette.CrewmateBlue);
		TeamTitle.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Crewmate);
		TeamTitle.color = Palette.CrewmateBlue;
		int maxDepth = Mathf.CeilToInt(7.5f);
		for (int i = 0; i < teamToDisplay.Count; i++)
		{
			PlayerControl playerControl = teamToDisplay[i];
			if (!playerControl)
			{
				continue;
			}
			NetworkedPlayerInfo data = playerControl.Data;
			if (!(data == null))
			{
				PoolablePlayer poolablePlayer = CreatePlayer(i, maxDepth, data, impostorPositioning: false);
				if (i == 0 && data.PlayerId == PlayerControl.LocalPlayer.PlayerId)
				{
					ourCrewmate = poolablePlayer;
				}
			}
		}
	}

	private void BeginImpostor(List<PlayerControl> yourTeam)
	{
		TeamTitle.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Impostor);
		TeamTitle.color = Palette.ImpostorRed;
		for (int i = 0; i < yourTeam.Count; i++)
		{
			PlayerControl playerControl = yourTeam[i];
			if (!playerControl)
			{
				continue;
			}
			NetworkedPlayerInfo data = playerControl.Data;
			if (!(data == null))
			{
				PoolablePlayer poolablePlayer = CreatePlayer(i, 1, data, impostorPositioning: true);
				if (i == 0 && data.PlayerId == PlayerControl.LocalPlayer.PlayerId)
				{
					ourCrewmate = poolablePlayer;
				}
			}
		}
	}

	private IEnumerator ShowRole()
	{
		RoleBehaviour role = PlayerControl.LocalPlayer.Data.Role;
		RoleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(role.StringName);
		RoleBlurbText.text = role.Blurb;
		RoleText.color = role.TeamColor;
		YouAreText.color = role.TeamColor;
		RoleBlurbText.color = role.TeamColor;
		SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.Data.Role.IntroSound, loop: false);
		YouAreText.gameObject.SetActive(value: true);
		RoleText.gameObject.SetActive(value: true);
		RoleBlurbText.gameObject.SetActive(value: true);
		if (ourCrewmate == null)
		{
			ourCrewmate = CreatePlayer(0, 1, PlayerControl.LocalPlayer.Data, impostorPositioning: false);
			ourCrewmate.gameObject.SetActive(value: false);
		}
		ourCrewmate.gameObject.SetActive(value: true);
		ourCrewmate.transform.localPosition = new Vector3(0f, -1.05f, -18f);
		ourCrewmate.transform.localScale = new Vector3(1f, 1f, 1f);
		ourCrewmate.ToggleName(active: false);
		yield return new WaitForSeconds(2.5f);
		YouAreText.gameObject.SetActive(value: false);
		RoleText.gameObject.SetActive(value: false);
		RoleBlurbText.gameObject.SetActive(value: false);
		ourCrewmate.gameObject.SetActive(value: false);
	}

	private void OnDestroy()
	{
		if (overlayHandle != null)
		{
			overlayHandle.Dispose();
			overlayHandle = null;
		}
	}

	private PoolablePlayer CreatePlayer(int i, int maxDepth, NetworkedPlayerInfo pData, bool impostorPositioning)
	{
		int num = ((i % 2 != 0) ? 1 : (-1));
		int num2 = (i + 1) / 2;
		float num3 = ((i == 0) ? (-8) : (-1));
		PoolablePlayer poolablePlayer = UnityEngine.Object.Instantiate(PlayerPrefab, transform);
		poolablePlayer.name = pData.PlayerName + "Dummy";
		poolablePlayer.SetFlipX(i % 2 == 0);
		if (impostorPositioning)
		{
			poolablePlayer.transform.localPosition = new Vector3((float)(num * num2) * 1.5f, -1f + (float)num2 * 0.15f, num3 + (float)num2 * 0.01f) * 1f;
			float num4 = (1f - (float)num2 * 0.075f) * 1f;
			Vector3 vector = new Vector3(num4, num4, 1f);
			poolablePlayer.transform.localScale = vector;
			poolablePlayer.SetNameScale(vector.Inv());
		}
		else
		{
			float t = (float)num2 / (float)maxDepth;
			float num5 = Mathf.Lerp(1f, 0.75f, t);
			poolablePlayer.transform.localPosition = new Vector3(0.9f * (float)num * (float)num2 * num5, FloatRange.SpreadToEdges(-1.125f, 0f, num2, maxDepth), num3 + (float)num2 * 0.01f) * 1f;
			float num6 = Mathf.Lerp((i == 0) ? 1.2f : 1f, 0.65f, t) * 1f;
			poolablePlayer.transform.localScale = new Vector3(num6, num6, 1f);
		}
		poolablePlayer.UpdateFromEitherPlayerDataOrCache(pData, PlayerOutfitType.Default, PlayerMaterial.MaskType.None, includePet: true);
		if (impostorPositioning)
		{
			Vector3 namePosition = new Vector3(0f, -1.31f, -0.5f);
			poolablePlayer.SetNamePosition(namePosition);
			poolablePlayer.SetName(pData.PlayerName);
			poolablePlayer.SetNameColor(pData.Role.NameColor);
		}
		poolablePlayer.ToggleName(impostorPositioning);
		return poolablePlayer;
	}

	private void LogPlayerRoleData()
	{
		string text = "IntroCutscene :: CoBegin() :: PlayerControl.LocalPlayer";
		if (PlayerControl.LocalPlayer == null)
		{
			Logger.GlobalInstance.Error(text + " is NULL");
			return;
		}
		text += ".Data";
		if (PlayerControl.LocalPlayer.Data == null)
		{
			Logger.GlobalInstance.Error(text + " is NULL");
			return;
		}
		text += ".Role";
		if (PlayerControl.LocalPlayer.Data.Role == null)
		{
			Logger.GlobalInstance.Error(text + " is NULL");
		}
		else
		{
			Logger.GlobalInstance.Info(text + " data is valid");
		}
	}
}
