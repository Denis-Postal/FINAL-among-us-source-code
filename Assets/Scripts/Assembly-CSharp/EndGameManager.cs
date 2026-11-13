using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.GameOptions;
using Assets.CoreScripts;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
	public TextMeshPro WinText;

	public MeshRenderer BackgroundBar;

	public MeshRenderer Foreground;

	public FloatRange ForegroundRadius;

	public SpriteRenderer FrontMost;

	public PoolablePlayer PlayerPrefab;

	public Sprite GhostSprite;

	public AudioClip DisconnectStinger;

	public AudioClip CrewStinger;

	public AudioClip ImpostorStinger;

	public EndGameNavigation Navigation;

	private const float ScaleAll = 0.9f;

	private const float OffsetWidth = 1f;

	private float stingerTime;

	public void Start()
	{
		SetEverythingUp();
		StartCoroutine(CoBegin());
		Invoke("ShowButtons", 1.1f);
		ConsoleJoystick.SetMode_Menu();
		DestroyableSingleton<AchievementManager>.Instance.OnMatchEnd(EndGameResult.CachedGameOverReason, EndGameResult.CachedWinners.Any((CachedPlayerData h) => h.IsYou));
	}

	private void ShowButtons()
	{
		FrontMost.gameObject.SetActive(value: false);
		if (EndGameResult.CachedXpGrantResult.GrantedXp != 0 || EndGameResult.CachedPodsGrantResult.GrantedPodsWithMultiplierApplied != 0 || EndGameResult.CachedBeansGrantResult.GrantedPodsWithMultiplierApplied != 0)
		{
			Navigation.ShowNavigationToProgressionScreen();
		}
		else
		{
			Navigation.ShowDefaultNavigation();
		}
	}

	private void SetEverythingUp()
	{
		DataManager.Player.Stats.IncrementStat(StatID.GamesFinished);
		Navigation.HideButtons();
		bool flag = GameManager.Instance.DidHumansWin(EndGameResult.CachedGameOverReason);
		if (EndGameResult.CachedGameOverReason == GameOverReason.ImpostorDisconnect)
		{
			DataManager.Player.Stats.IncrementGameResultStat(EndGameResult.CachedGameOverReason, GameResultStat.Draws);
			WinText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ImpostorDisconnected);
			SoundManager.Instance.PlaySound(DisconnectStinger, loop: false);
		}
		else
		{
			if (EndGameResult.CachedWinners.Any((CachedPlayerData h) => h.IsYou))
			{
				DataManager.Player.Stats.IncrementWinStats(EndGameResult.CachedGameOverReason, (MapNames)GameManager.Instance.LogicOptions.MapId, EndGameResult.CachedLocalPlayer.RoleWhenAlive);
				DestroyableSingleton<AchievementManager>.Instance.SetWinMap(GameManager.Instance.LogicOptions.MapId);
				WinText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Victory);
				BackgroundBar.material.SetColor("_Color", Palette.CrewmateBlue);
				CachedPlayerData cachedPlayerData = EndGameResult.CachedWinners.FirstOrDefault((CachedPlayerData h) => h.IsYou);
				if (cachedPlayerData != null)
				{
					DestroyableSingleton<UnityTelemetry>.Instance.WonGame(cachedPlayerData.ColorId, cachedPlayerData.HatId, cachedPlayerData.SkinId, cachedPlayerData.PetId, cachedPlayerData.VisorId, cachedPlayerData.NamePlateId);
				}
			}
			else
			{
				DataManager.Player.Stats.IncrementGameResultStat(EndGameResult.CachedGameOverReason, GameResultStat.Losses);
				WinText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Defeat);
				WinText.color = Color.red;
			}
			if (flag)
			{
				SoundManager.Instance.PlayDynamicSound("Stinger", CrewStinger, loop: false, GetStingerVol, SoundManager.Instance.MusicChannel);
			}
			else
			{
				SoundManager.Instance.PlayDynamicSound("Stinger", ImpostorStinger, loop: false, GetStingerVol, SoundManager.Instance.MusicChannel);
			}
		}
		int num = Mathf.CeilToInt(7.5f);
		List<CachedPlayerData> list = EndGameResult.CachedWinners.OrderBy((CachedPlayerData b) => b.IsYou ? (-1) : 0).ToList();
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			CachedPlayerData cachedPlayerData2 = list[num2];
			int num3 = ((num2 % 2 != 0) ? 1 : (-1));
			int num4 = (num2 + 1) / 2;
			float t = (float)num4 / (float)num;
			float num5 = Mathf.Lerp(1f, 0.75f, t);
			float num6 = ((num2 == 0) ? (-8) : (-1));
			PoolablePlayer poolablePlayer = UnityEngine.Object.Instantiate(PlayerPrefab, transform);
			poolablePlayer.transform.localPosition = new Vector3(1f * (float)num3 * (float)num4 * num5, FloatRange.SpreadToEdges(-1.125f, 0f, num4, num), num6 + (float)num4 * 0.01f) * 0.9f;
			float num7 = Mathf.Lerp(1f, 0.65f, t) * 0.9f;
			Vector3 vector = new Vector3(num7, num7, 1f);
			poolablePlayer.transform.localScale = vector;
			if (cachedPlayerData2.IsDead)
			{
				poolablePlayer.SetBodyAsGhost();
				poolablePlayer.SetDeadFlipX(num2 % 2 == 0);
			}
			else
			{
				poolablePlayer.SetFlipX(num2 % 2 == 0);
			}
			poolablePlayer.UpdateFromPlayerOutfit(cachedPlayerData2.Outfit, PlayerMaterial.MaskType.None, cachedPlayerData2.IsDead, includePet: true);
			if (flag)
			{
				poolablePlayer.ToggleName(active: false);
				continue;
			}
			Color color = (cachedPlayerData2.IsImpostor ? Palette.ImpostorRed : Palette.White);
			poolablePlayer.SetName(cachedPlayerData2.PlayerName, vector.Inv(), color, -15f);
			Vector3 namePosition = new Vector3(0f, -1.31f, -0.5f);
			poolablePlayer.SetNamePosition(namePosition);
			if (AprilFoolsMode.ShouldHorseAround() && GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek)
			{
				poolablePlayer.SetBodyType(PlayerBodyTypes.Normal);
				poolablePlayer.SetFlipX(flipped: true);
			}
		}
	}

	private void GetStingerVol(AudioSource source, float dt)
	{
		stingerTime += dt * 0.75f;
		source.volume = Mathf.Clamp(1f / stingerTime, 0f, 1f);
	}

	private IEnumerator CoBegin()
	{
		GameManager.Instance.EndGame();
		DataManager.Player.Stats.SaveStats();
		Color c = WinText.color;
		Color fade = Color.black;
		_ = Color.white;
		Vector3 titlePos = WinText.transform.localPosition;
		float timer = 0f;
		while (timer < 3f)
		{
			timer += Time.deltaTime;
			float num = Mathf.Min(1f, timer / 3f);
			Foreground.material.SetFloat("_Rad", ForegroundRadius.ExpOutLerp(num * 2f));
			fade.a = Mathf.Lerp(1f, 0f, num * 3f);
			FrontMost.color = fade;
			c.a = Mathf.Clamp(FloatRange.ExpOutLerp(num, 0f, 1f), 0f, 1f);
			WinText.color = c;
			titlePos.y = 2.7f - num * 0.3f;
			WinText.transform.localPosition = titlePos;
			yield return null;
		}
		FrontMost.gameObject.SetActive(value: false);
	}

	public static IEnumerator WaitWithTimeout(Func<bool> success)
	{
		for (float timer = 0f; timer < 5f; timer += Time.deltaTime)
		{
			if (success())
			{
				break;
			}
			yield return null;
		}
	}
}
