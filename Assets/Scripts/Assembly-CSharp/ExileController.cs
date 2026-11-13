using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class ExileController : MonoBehaviour
{
	public class InitProperties
	{
		public NetworkedPlayerInfo.PlayerOutfit outfit;

		public NetworkedPlayerInfo networkedPlayer;

		public bool voteTie;

		public bool wasOverruled;

		public bool isImpostor;

		public bool confirmImpostor;

		public int totalImpostorCount;

		public int remainingImpostorCount;
	}

	public static ExileController Instance;

	public TextMeshPro ImpostorText;

	public TextMeshPro Text;

	public PoolablePlayer Player;

	public AnimationCurve LerpCurve;

	public float Duration = 7f;

	public AudioClip TextSound;

	public AudioClip EjectSound;

	[SerializeField]
	private bool useIdleAnim;

	[SerializeField]
	private Vector3 exileHatPosition = new Vector3(-0.216f, 0.528f);

	[SerializeField]
	private Vector3 exileVisorPosition = new Vector3(-0.148f, 0.647f, -0.002f);

	protected string completeString = "TestPlayer was not The Impostor";

	protected bool wasOverruled;

	[SerializeField]
	private TextMeshPro judgeText;

	protected InitProperties initData;

	private SpecialInputHandler specialInputHandler;

	public event Action OnCompleteEvent;

	private void Awake()
	{
		specialInputHandler = GetComponent<SpecialInputHandler>();
	}

	protected abstract IEnumerator Animate();

	protected virtual IEnumerator HandleText(float firstWaitTime, float animDuration)
	{
		yield return Effects.Wait(firstWaitTime);
		for (float t = 0f; t <= animDuration; t += Time.deltaTime)
		{
			int num = (int)(t / animDuration * (float)completeString.Length);
			if (num > Text.text.Length)
			{
				Text.text = completeString.Substring(0, num);
				Text.gameObject.SetActive(value: true);
				if (completeString[num - 1] != ' ')
				{
					SoundManager.Instance.PlaySoundImmediate(TextSound, loop: false, 0.8f);
				}
			}
			yield return null;
		}
		Text.text = completeString;
	}

	public void BeginForTesting(NetworkedPlayerInfo.PlayerOutfit outfit, bool voteTie = false, bool isImpostor = true, bool confirmImpostor = true, int totalImpostorCount = 1, int remainingImpostorCount = 0)
	{
		InitProperties initProperties = new InitProperties();
		initProperties.outfit = outfit;
		initProperties.isImpostor = isImpostor;
		initProperties.confirmImpostor = confirmImpostor;
		initProperties.voteTie = voteTie;
		initProperties.totalImpostorCount = totalImpostorCount;
		initProperties.remainingImpostorCount = remainingImpostorCount;
		Begin(initProperties);
	}

	public void BeginForGameplay(NetworkedPlayerInfo player, bool voteTie, bool wasOverruled)
	{
		InitProperties initProperties = new InitProperties();
		if (player != null)
		{
			initProperties.outfit = player.Outfits[PlayerOutfitType.Default];
			initProperties.networkedPlayer = player;
			initProperties.isImpostor = player.Role.IsImpostor;
			initProperties.wasOverruled = wasOverruled;
		}
		initProperties.voteTie = voteTie;
		initProperties.confirmImpostor = GameManager.Instance.LogicOptions.GetConfirmImpostor();
		initProperties.totalImpostorCount = GameData.Instance.AllPlayers.Count((NetworkedPlayerInfo p) => p.Role.IsImpostor);
		initProperties.remainingImpostorCount = GameData.Instance.AllPlayers.Count((NetworkedPlayerInfo p) => p.Role.IsImpostor && !p.IsDead && !p.Disconnected);
		if (player != null && player.Role.IsImpostor && !player.Disconnected)
		{
			initProperties.remainingImpostorCount--;
		}
		Begin(initProperties);
	}

	private void Begin(InitProperties init)
	{
		if (specialInputHandler != null)
		{
			specialInputHandler.disableVirtualCursor = true;
		}
		Instance = this;
		ControllerManager.Instance.CloseAndResetAll();
		initData = init;
		Text.gameObject.SetActive(value: false);
		Text.text = string.Empty;
		wasOverruled = init.wasOverruled;
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.SetMapAndInfoButtonsEnabled(enabled: false);
		}
		if (init != null && init.outfit != null)
		{
			if (!init.confirmImpostor)
			{
				completeString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ExileTextNonConfirm, init.outfit.PlayerName);
			}
			else if (init.isImpostor)
			{
				if (init.totalImpostorCount > 1)
				{
					completeString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ExileTextPP, init.outfit.PlayerName);
				}
				else
				{
					completeString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ExileTextSP, init.outfit.PlayerName);
				}
			}
			else if (init.totalImpostorCount > 1)
			{
				completeString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ExileTextPN, init.outfit.PlayerName);
			}
			else
			{
				completeString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ExileTextSN, init.outfit.PlayerName);
			}
			Player.UpdateFromPlayerOutfit(init.outfit, PlayerMaterial.MaskType.Exile, isDead: false, includePet: false, () =>
			{
				SkinViewData skinViewData = null;
				skinViewData = ((!(GameManager.Instance != null)) ? Player.GetSkinView() : ShipStatus.Instance.CosmeticsCache.GetSkin(initData.outfit.SkinId));
				if (GameManager.Instance != null && !DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(init.outfit.SkinId, Player.GetIgnoreLongMode()))
				{
					skinViewData = ShipStatus.Instance.CosmeticsCache.GetSkin("skin_None");
				}
				if (useIdleAnim)
				{
					Player.FixSkinSprite(skinViewData.IdleFrame);
				}
				else
				{
					Player.FixSkinSprite(skinViewData.EjectFrame);
				}
			});
			Player.ToggleName(active: false);
			if (!useIdleAnim)
			{
				Player.SetCustomHatPosition(exileHatPosition);
				Player.SetCustomVisorPosition(exileVisorPosition);
			}
		}
		else
		{
			if (init.voteTie)
			{
				completeString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NoExileTie);
			}
			else
			{
				completeString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NoExileSkip);
			}
			Player.gameObject.SetActive(value: false);
		}
		if (init.wasOverruled)
		{
			judgeText.gameObject.SetActive(value: true);
		}
		if (init.remainingImpostorCount == 1)
		{
			ImpostorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ImpostorsRemainS, init.remainingImpostorCount);
		}
		else
		{
			ImpostorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ImpostorsRemainP, init.remainingImpostorCount);
		}
		StartCoroutine(Animate());
	}

	protected void WrapUp()
	{
		if (initData.networkedPlayer != null)
		{
			PlayerControl playerControl = initData.networkedPlayer.Object;
			if ((bool)playerControl)
			{
				playerControl.Exiled();
			}
			initData.networkedPlayer.IsDead = true;
		}
		if (DestroyableSingleton<TutorialManager>.InstanceExists || (GameManager.Instance != null && !GameManager.Instance.LogicFlow.IsGameOverDueToDeath()))
		{
			ReEnableGameplay();
		}
		UnityEngine.Object.Destroy(gameObject);
	}

	protected void ReEnableGameplay()
	{
		DestroyableSingleton<HudManager>.Instance.StartCoroutine(DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear));
		PlayerControl localPlayer = PlayerControl.LocalPlayer;
		localPlayer.SetKillTimer(GameManager.Instance.LogicOptions.GetKillCooldown());
		ShipStatus.Instance.EmergencyCooldown = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
		Camera.main.GetComponent<FollowerCamera>().Locked = false;
		DestroyableSingleton<HudManager>.Instance.SetMapAndInfoButtonsEnabled(enabled: true);
		DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: true);
		ControllerManager.Instance.CloseAndResetAll();
		localPlayer.Data.Role.OnExileComplete();
	}
}
