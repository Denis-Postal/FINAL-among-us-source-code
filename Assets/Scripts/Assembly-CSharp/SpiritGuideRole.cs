using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.GameOptions;
using PowerTools;
using Rewired;
using UnityEngine;

public class SpiritGuideRole : CrewmateGhostRole
{
	private const float MINIMUM_COOLDOWN_SECONDS = 10f;

	[SerializeField]
	private GameObject spiritGuidePanel;

	[SerializeField]
	private Animator spiritGuidePanelAnimator;

	[SerializeField]
	private SpiritGuideCamera spiritGuideCamera;

	[SerializeField]
	private SpriteRenderer cameraHandSpriteRenderer;

	[SerializeField]
	private SpriteRenderer[] cameraPartsRenderer;

	[SerializeField]
	private AnimationClip cameraFlashLeftAnim;

	[SerializeField]
	private AnimationClip cameraFlashAnim;

	[SerializeField]
	private AnimationClip cameraIdleAnim;

	[SerializeField]
	private AnimationClip cameraIdleLeftAnim;

	[SerializeField]
	private AudioClip refreshAudio;

	[SerializeField]
	private AudioClip openMessageSFX;

	[SerializeField]
	private AudioClip closeMessageSFX;

	[SerializeField]
	private AudioClip wooshSFX;

	private PlayerControl currentTarget;

	private PlayerControl hauntTarget;

	private List<Sprite> possibleSprites = new List<Sprite>();

	private float cooldownSecondsRemaining;

	private bool sendingImage;

	private const float minVelMultiplier = 0.5f;

	private const float maxVelMultiplier = 4f;

	private const float minDistance = 0.1f;

	private const float minOffset = 0.2f;

	private const float maxOffset = 0.5f;

	private Vector2 offset;

	private const float damping = 0.9f;

	private const float easing = 0.4f;

	private bool imagesGenerated;

	private Vector2 horseCameraOffsetPos = new Vector2(-0.65f, -0.1f);

	private Vector2 horseCameraDefaultPos = new Vector2(0f, -0.1f);

	private int[] previouslyGeneratedImageIndices = new int[6] { -1, -1, -1, -1, -1, -1 };

	public List<SpiritGuideImageButton> imageButtons;

	public List<SpiritGuideImageButton> selectedImageButtons = new List<SpiritGuideImageButton>();

	private readonly string PANEL_ANIM_REFRESH = "SpiritGuidePanelRefresh";

	public static readonly int MaskLayer = Shader.PropertyToID("_MaskLayer");

	private Controller myController = new Controller();

	private Player player;

	private SpiritGuideImageButton currentImageButtonJoystick;

	private bool selectionCooldown;

	private Coroutine CoSelectionCooldown;

	private bool IsCoolingDown => cooldownSecondsRemaining > 0f;

	public bool SendingImage => sendingImage;

	public override bool IsAffectedByComms => CommsSabotaged;

	public override void Initialize(PlayerControl player)
	{
		base.Initialize(player);
		if (Player.AmOwner)
		{
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.Hide();
		}
		Player.SetPlayerMaterialColors(cameraHandSpriteRenderer);
		SpriteRenderer[] array = cameraPartsRenderer;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material.SetInt(MaskLayer, (!player.AmOwner) ? 1 : 2);
		}
		Player.RegisterVisibilityItem(spiritGuideCamera);
		spiritGuideCamera.transform.SetParent(player.transform);
		spiritGuideCamera.transform.localPosition = Vector3.zero;
		spiritGuideCamera.transform.localScale = Vector3.one;
		if (!AprilFoolsMode.ShouldHorseAround())
		{
			spiritGuideCamera.SetNodesParent(player.cosmetics.GetCurrentBodySprite().BodySprite.GetComponent<SpriteAnimNodes>());
		}
		if (Player.AmOwner)
		{
			SetInitialCooldown();
			this.player = ReInput.players.GetPlayer(0);
		}
		possibleSprites = ShipStatus.Instance.GetSocialMediumSpriteList();
	}

	public override void Deinitialize(PlayerControl targetPlayer)
	{
		Player.UnregisterVisibilityItem(spiritGuideCamera);
		UnityEngine.Object.Destroy(spiritGuideCamera.gameObject);
		base.Deinitialize(targetPlayer);
	}

	public override void UseAbility()
	{
		if (Player.AmOwner && (!MatchInfoGuide.Instance || !MatchInfoGuide.Instance.IsActive) && !DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen && isActiveAndEnabled && ((bool)currentTarget || (bool)hauntTarget) && Player.Data.IsDead && !IsAffectedByComms && !sendingImage)
		{
			if (spiritGuidePanel.activeSelf)
			{
				CloseSpiritGuidePanel();
			}
			else if (!IsCoolingDown)
			{
				OpenSpiritGuidePanel();
			}
		}
	}

	public override void UseSecondaryAbility()
	{
		if (Player.AmOwner && spiritGuidePanel.activeSelf)
		{
			if (selectedImageButtons.Count > 0)
			{
				SendMessageToSelectedPlayer();
			}
			else
			{
				RefreshImages();
			}
		}
	}

	public override void SetPlayerTarget(PlayerControl target)
	{
		if (spiritGuidePanel.activeSelf)
		{
			return;
		}
		RoleTeamTypes teamType = Player.Data.Role.TeamType;
		if (target != currentTarget && currentTarget != null)
		{
			currentTarget.ToggleHighlight(active: false, teamType);
		}
		currentTarget = target;
		if ((bool)currentTarget && !target.Data.IsDead && !IsAffectedByComms)
		{
			currentTarget.ToggleHighlight(active: true, teamType);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			return;
		}
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		if (spiritGuidePanel.activeSelf)
		{
			CloseSpiritGuidePanel();
		}
	}

	public override void OnExileComplete()
	{
		if (Player.AmOwner)
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.Hide();
			cooldownSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
		}
	}

	public override void SetCooldown()
	{
		if (Player.AmOwner)
		{
			cooldownSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
		}
	}

	private void SetInitialCooldown()
	{
		if (Player.AmOwner)
		{
			cooldownSecondsRemaining = 10f;
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
		}
	}

	private void SetRefreshCooldown()
	{
		if (Player.AmOwner)
		{
			cooldownSecondsRemaining = 0.5f * GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds);
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetDisabled();
		}
	}

	private void OpenSpiritGuidePanel()
	{
		GenerateRandomImages();
		ConsoleJoystick.SetMode_Task();
		VirtualCursor.instance.gameObject.SetActive(value: false);
		hauntTarget = currentTarget;
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetSecondImage(Ability);
		DestroyableSingleton<HudManager>.Instance.AbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SpiritGuideAbilityCancel));
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetFromSettings(SecondaryAbility);
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SpiritGuideAbilityRefresh));
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.Show();
		if (IsCoolingDown)
		{
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetDisabled();
		}
		offset = UnityEngine.Random.insideUnitCircle;
		if (offset.magnitude < 0.2f)
		{
			offset = offset.normalized * 0.2f;
		}
		else if (offset.magnitude > 0.5f)
		{
			offset = offset.normalized * 0.5f;
		}
		SoundManager.Instance.PlaySound(openMessageSFX, loop: false);
		Player.moveable = false;
		spiritGuidePanel.SetActive(value: true);
		spiritGuidePanel.GetComponent<SpiritGuidePanelMinigame>().Begin(null);
		HighlightControllerButton(0);
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCustomGlyph(RewiredConstsEnum.Action.MenuCancel);
	}

	private void ClearControllerHighlights()
	{
		foreach (SpiritGuideImageButton imageButton in imageButtons)
		{
			imageButton.ToggleHighlight(isHighlighted: false);
		}
	}

	private void HighlightControllerButton(int index)
	{
		ClearControllerHighlights();
		currentImageButtonJoystick = imageButtons[index];
		currentImageButtonJoystick.ToggleHighlight(isHighlighted: true);
	}

	private IEnumerator CoSendPhotoAnim(PlayerControl playerTarget)
	{
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetDisabled();
		foreach (SpiritGuideImageButton imageButton in imageButtons)
		{
			if (!selectedImageButtons.Contains(imageButton))
			{
				imageButton.Hide();
			}
		}
		while (selectedImageButtons.Count > 0)
		{
			foreach (SpiritGuideImageButton selectedImageButton in selectedImageButtons)
			{
				if (selectedImageButtons.IndexOf(selectedImageButton) == selectedImageButtons.Count - 1)
				{
					spiritGuideCamera.animator.Play(Player.cosmetics.FlipX ? cameraFlashLeftAnim : cameraFlashAnim);
				}
				StartCoroutine(CoAnimateSendImage(selectedImageButton, playerTarget));
				yield return new WaitForSeconds(0.2f);
			}
			selectedImageButtons.Clear();
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		sendingImage = false;
		CloseSpiritGuidePanel(playSFX: false);
		SetCooldown();
	}

	private IEnumerator CoAnimateSendImage(SpiritGuideImageButton imageToSend, PlayerControl playerTarget)
	{
		SoundManager.Instance.PlaySoundImmediate(wooshSFX, loop: false);
		float t = 0f;
		while (t < 0.7f)
		{
			imageToSend.transform.position = Vector3.Lerp(imageToSend.transform.position, Camera.main.WorldToScreenPoint(playerTarget.transform.position), t / 0.7f);
			imageToSend.transform.localScale = Vector3.Lerp(imageToSend.transform.localScale, Vector3.zero, t / 0.7f);
			t += Time.deltaTime;
			yield return null;
		}
		imageToSend.Hide();
	}

	public void CloseSpiritGuidePanel(bool playSFX = true)
	{
		ConsoleJoystick.SetMode_Gameplay();
		DestroyableSingleton<HudManager>.Instance.AbilityButton.RestoreGlyph();
		spiritGuidePanel.transform.localScale = Vector3.one;
		spiritGuidePanel.transform.localRotation = Quaternion.identity;
		spiritGuidePanel.GetComponent<SpiritGuidePanelMinigame>().RemoveInstance();
		spiritGuidePanel.SetActive(value: false);
		selectionCooldown = false;
		if (CoSelectionCooldown != null)
		{
			StopCoroutine(CoSelectionCooldown);
			CoSelectionCooldown = null;
		}
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.Hide();
		if (IsCoolingDown)
		{
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
		}
		if (playSFX)
		{
			SoundManager.Instance.PlaySound(closeMessageSFX, loop: false);
		}
		ResetPanelImages();
		hauntTarget = null;
		Player.moveable = true;
	}

	private void ResetPanelImages()
	{
		foreach (SpiritGuideImageButton imageButton in imageButtons)
		{
			ResetSelectedImage(imageButton);
		}
		selectedImageButtons.Clear();
	}

	private void GenerateRandomImages()
	{
		if (imagesGenerated)
		{
			return;
		}
		UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
		int[] array = new int[6] { -1, -1, -1, -1, -1, -1 };
		List<int> list = new List<int>();
		list = Enumerable.Range(0, possibleSprites.Count).ToList();
		int[] array2 = previouslyGeneratedImageIndices;
		foreach (int item in array2)
		{
			list.Remove(item);
		}
		for (int j = 0; j < 6; j++)
		{
			int num = list[UnityEngine.Random.Range(0, list.Count)];
			if (array.Contains(num))
			{
				j--;
			}
			else
			{
				array[j] = num;
			}
		}
		previouslyGeneratedImageIndices = array;
		for (int k = 0; k < imageButtons.Count; k++)
		{
			imageButtons[k].SetSprite(possibleSprites[array[k]]);
		}
		imagesGenerated = true;
	}

	public void SelectImage(SpiritGuideImageButton i)
	{
		selectedImageButtons.Add(i);
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetSecondImage(SecondaryAbility);
		DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SpiritGuideAbilitySend));
	}

	public void ResetSelectedImage(SpiritGuideImageButton i)
	{
		i.ResetImage();
		if (selectedImageButtons.Count == 0)
		{
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetFromSettings(SecondaryAbility);
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SpiritGuideAbilityRefresh));
		}
	}

	public void SendMessageToSelectedPlayer()
	{
		if (!IsCoolingDown && !sendingImage)
		{
			sendingImage = true;
			byte[] array = new byte[selectedImageButtons.Count];
			for (int i = 0; i < selectedImageButtons.Count; i++)
			{
				array[i] = (byte)possibleSprites.IndexOf(selectedImageButtons[i].spiritGuideImage.sprite);
			}
			ShipStatus.Instance.SendSpiritGuideMessage(array, currentTarget.Data.ClientId);
			DataManager.Player.Stats.IncrementStat(StatID.Role_SpiritGuide_SentMessage);
			imagesGenerated = false;
			ClearControllerHighlights();
			StartCoroutine("CoSendPhotoAnim", hauntTarget);
		}
	}

	public void RefreshImages()
	{
		if (!IsCoolingDown)
		{
			SetRefreshCooldown();
			ResetPanelImages();
			spiritGuidePanelAnimator.Play(PANEL_ANIM_REFRESH, 0, 0f);
			imagesGenerated = false;
			GenerateRandomImages();
			SoundManager.Instance.PlaySoundImmediate(refreshAudio, loop: false);
		}
	}

	public override PlayerControl FindClosestTarget()
	{
		List<PlayerControl> playersInAbilityRangeSorted = GetPlayersInAbilityRangeSorted(RoleBehaviour.GetTempPlayerList(), ignoreColliders: true);
		if (playersInAbilityRangeSorted.Count <= 0)
		{
			return null;
		}
		playersInAbilityRangeSorted.Sort((PlayerControl a, PlayerControl b) =>
		{
			if (a.Data == null || b.Data == null || a.Data.Role == null || b.Data.Role == null)
			{
				return 0;
			}
			if (a.Data.Role.IsImpostor && !b.Data.Role.IsImpostor)
			{
				return 1;
			}
			return (!a.Data.Role.IsImpostor && b.Data.Role.IsImpostor) ? (-1) : 0;
		});
		return playersInAbilityRangeSorted[0];
	}

	private void FixedUpdate()
	{
		if (spiritGuideCamera.animator.IsPlaying(cameraFlashAnim))
		{
			if (Player.cosmetics.FlipX)
			{
				float time = spiritGuideCamera.animator.Time;
				spiritGuideCamera.animator.Play(cameraFlashLeftAnim);
				spiritGuideCamera.animator.Time = time;
			}
		}
		else if (spiritGuideCamera.animator.IsPlaying(cameraFlashLeftAnim))
		{
			if (!Player.cosmetics.FlipX)
			{
				float time2 = spiritGuideCamera.animator.Time;
				spiritGuideCamera.animator.Play(cameraFlashAnim);
				spiritGuideCamera.animator.Time = time2;
			}
		}
		else
		{
			spiritGuideCamera.animator.Play(Player.cosmetics.FlipX ? cameraIdleLeftAnim : cameraIdleAnim);
		}
		if (AprilFoolsMode.ShouldHorseAround())
		{
			spiritGuideCamera.transform.localPosition = (Player.cosmetics.FlipX ? horseCameraOffsetPos : horseCameraDefaultPos);
		}
		if (!Player || !Player.AmOwner)
		{
			return;
		}
		if (IsCoolingDown)
		{
			cooldownSecondsRemaining -= Time.deltaTime;
			if (!spiritGuidePanel.activeSelf)
			{
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
			}
			DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
		}
		if (!spiritGuidePanel.activeSelf)
		{
			return;
		}
		if (((bool)hauntTarget && hauntTarget.Data.IsDead) || IsAffectedByComms)
		{
			CloseSpiritGuidePanel();
			return;
		}
		Vector2 normalizedVelocity = Vector2.zero;
		if ((bool)hauntTarget)
		{
			Vector2 vector = hauntTarget.GetTruePosition() + offset;
			Vector2 truePosition = Player.GetTruePosition();
			Vector2 vector2 = Player.MyPhysics.GetVelocity() / Player.MyPhysics.TrueSpeed;
			Vector2 vector3 = vector - truePosition;
			float magnitude = vector3.magnitude;
			if (magnitude > 0.1f)
			{
				vector3 = vector3.normalized * Mathf.Clamp(magnitude, 0.5f, 4f);
				normalizedVelocity = vector2 * 0.6f + vector3 * 0.4f;
			}
			else
			{
				normalizedVelocity *= 0.9f;
			}
		}
		Player.MyPhysics.SetNormalizedVelocity(normalizedVelocity);
		if (!IsCoolingDown && !sendingImage)
		{
			GenerateRandomImages();
			if (!DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.CanInteract())
			{
				DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.SetEnabled();
			}
		}
	}

	private void Update()
	{
		if (!Player || !Player.AmOwner || !spiritGuidePanel.activeSelf || ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick || sendingImage)
		{
			return;
		}
		VirtualCursor.instance.gameObject.SetActive(value: false);
		float axis = player.GetAxis(13);
		float axis2 = player.GetAxis(14);
		if (!selectionCooldown)
		{
			float num = Mathf.Sqrt(axis * axis + axis2 * axis2);
			float num2 = 0.2f;
			if (num >= num2)
			{
				float num3 = Mathf.Atan2(axis2, axis) * 57.29578f;
				if (num3 < 0f)
				{
					num3 += 360f;
				}
				if (num3 >= 30f && num3 < 90f)
				{
					HighlightControllerButton(1);
					CoSelectionCooldown = StartCoroutine(CoCooldownStick());
				}
				else if (num3 >= 90f && num3 < 150f)
				{
					HighlightControllerButton(0);
					CoSelectionCooldown = StartCoroutine(CoCooldownStick());
				}
				else if (num3 >= 150f && num3 < 210f)
				{
					HighlightControllerButton(5);
					CoSelectionCooldown = StartCoroutine(CoCooldownStick());
				}
				else if (num3 >= 210f && num3 < 270f)
				{
					HighlightControllerButton(3);
					CoSelectionCooldown = StartCoroutine(CoCooldownStick());
				}
				else if (num3 >= 270f && num3 < 330f)
				{
					HighlightControllerButton(2);
					CoSelectionCooldown = StartCoroutine(CoCooldownStick());
				}
				else if (num3 >= 330f || num3 < 30f)
				{
					HighlightControllerButton(4);
					CoSelectionCooldown = StartCoroutine(CoCooldownStick());
				}
			}
		}
		if (player.GetButtonDown(11))
		{
			currentImageButtonJoystick.ToggleSelection();
		}
		if (player.GetButtonDown(12))
		{
			CloseSpiritGuidePanel();
		}
		if (player.GetButtonDown(29))
		{
			UseSecondaryAbility();
		}
	}

	private IEnumerator CoCooldownStick()
	{
		selectionCooldown = true;
		yield return new WaitForSeconds(0.15f);
		selectionCooldown = false;
	}

	public override void OnMeetingStart()
	{
		if (Player.AmOwner)
		{
			base.OnMeetingStart();
			sendingImage = false;
			CloseSpiritGuidePanel();
		}
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}
}
