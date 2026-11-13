using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using Hazel;
using InnerNet;
using Innersloth.Assets;
using TMPro;
using UnityEngine;

public class PlayerVoteArea : MonoBehaviour
{
	public const byte HasNotVoted = byte.MaxValue;

	public const byte MissedVote = 254;

	public const byte SkippedVote = 253;

	public const byte DeadVote = 252;

	public GameObject Buttons;

	public UiElement ConfirmButton;

	public UiElement CancelButton;

	public UiElement PlayerButton;

	public SpriteRenderer Background;

	public SpriteRenderer MaskArea;

	public SpriteRenderer Flag;

	public SpriteRenderer Megaphone;

	public SpriteRenderer Overlay;

	public SpriteRenderer XMark;

	public SpriteRenderer ThumbsDown;

	public SpriteRenderer HighlightedFX;

	public TextMeshPro NameText;

	public TextMeshPro LevelNumberText;

	public TextMeshPro ColorBlindName;

	[SerializeField]
	private bool AnimateButtonsFromLeft;

	[SerializeField]
	private PoolablePlayer PlayerIcon;

	[SerializeField]
	private UiElement JudgeOverruleButton;

	[SerializeField]
	private GameObject JudgeOverruleButtonCommsDisable;

	[SerializeField]
	private GameObject JudgeUsedLabel;

	[SerializeField]
	private SpriteRenderer JudgeHandMaterial;

	private bool resultsShowing;

	public MeetingHud Parent { get; set; }

	public bool AmDead { get; private set; }

	public bool DidReport { get; private set; }

	public bool DidVote { get; private set; }

	public PlayerId PlayerId { get; private set; }

	public PlayerId VotedForId { get; private set; } = byte.MaxValue;

	public bool VoteComplete { private get; set; }

	public int MaskLayer { get; private set; }

	private void Start()
	{
		Buttons.SetActive(value: false);
	}

	public void SetPlayerId(PlayerId playerId)
	{
		PlayerId = playerId;
		if ((bool)PlayerIcon)
		{
			SetMaskLayer((byte)playerId);
		}
	}

	public void SetMaskLayer(int maskLayer)
	{
		MaskLayer = maskLayer + 2;
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
		}
		PlayerIcon.SetMaskLayer(MaskLayer);
	}

	public void SetReporter()
	{
		DidReport = true;
		Megaphone.enabled = true;
	}

	public void SetDead(bool isDead)
	{
		AmDead = isDead;
		Overlay.gameObject.SetActive(isDead);
		XMark.gameObject.SetActive(isDead);
	}

	public void SetDisabled()
	{
		if (!AmDead)
		{
			if ((bool)Overlay)
			{
				Overlay.gameObject.SetActive(value: true);
				XMark.gameObject.SetActive(value: false);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	public void SetEnabled()
	{
		if (!AmDead)
		{
			if ((bool)Overlay)
			{
				Overlay.gameObject.SetActive(value: false);
			}
			else
			{
				gameObject.SetActive(value: true);
			}
		}
	}

	public void ConfigureOverlay()
	{
		if (AmDead)
		{
			Overlay.color = Color.white;
			XMark.transform.localScale = Vector3.one;
		}
		else if (DidReport)
		{
			Megaphone.transform.localEulerAngles = Vector3.zero;
			Megaphone.transform.localScale = Vector3.one;
		}
	}

	public IEnumerator CoAnimateOverlay()
	{
		if (DidReport)
		{
			float duration = 1f;
			for (float time = 0f; time < duration; time += Time.deltaTime)
			{
				float num = time / duration;
				float num2 = TriangleWave(num * 3f) * 2f - 1f;
				Megaphone.transform.localEulerAngles = new Vector3(0f, 0f, num2 * 30f);
				num2 = Mathf.Lerp(0.7f, 1.2f, TriangleWave(num * 2f));
				Megaphone.transform.localScale = new Vector3(num2, num2, num2);
				yield return null;
			}
			Megaphone.transform.localEulerAngles = Vector3.zero;
			Megaphone.transform.localScale = Vector3.one;
		}
	}

	public void SetVote(PlayerId suspectId)
	{
		VotedForId = suspectId;
		SetHasVoted();
	}

	public void SetHasVoted()
	{
		Flag.enabled = true;
		DidVote = true;
	}

	public void SetVoteMissed()
	{
		VotedForId = (byte)254;
		DidVote = true;
	}

	public void UnsetVote()
	{
		VotedForId = byte.MaxValue;
		UnsetHasVoted();
	}

	public void UnsetHasVoted()
	{
		Flag.enabled = false;
		DidVote = false;
	}

	public void ClearButtons()
	{
		StopAllCoroutines();
		Buttons.SetActive(value: false);
	}

	public void ClearForResults()
	{
		resultsShowing = true;
		Flag.enabled = false;
	}

	public void VoteForMe()
	{
		if (!VoteComplete)
		{
			Parent.Confirm(PlayerId);
			ControllerManager.Instance.CloseOverlayMenu(name);
			if ((bool)ThumbsDown)
			{
				ThumbsDown.enabled = true;
			}
		}
	}

	public void JudgeOverruleVote()
	{
		JudgeRole judgeRole = PlayerControl.LocalPlayer.Data.Role as JudgeRole;
		if (!(judgeRole == null) && !PlayerControl.LocalPlayer.Data.Role.IsAffectedByComms)
		{
			if (judgeRole.TryOverrule(PlayerId))
			{
				JudgeUsedLabel.SetActive(value: true);
				JudgeHandMaterial.material.SetColor("_BodyColor", PlayerControl.LocalPlayer.Data.Color);
				JudgeHandMaterial.material.SetInt(PlayerMaterial.MaskLayer, 20);
			}
			ControllerManager.Instance.CloseOverlayMenu(name);
			ClearButtons();
		}
	}

	public void RemoveJudgeUsedLabel()
	{
		JudgeUsedLabel.SetActive(value: false);
	}

	public void SetHighlighted(bool value)
	{
		if ((bool)HighlightedFX)
		{
			HighlightedFX.enabled = value && CanBeHighlighted();
		}
	}

	public void Select()
	{
		if (PlayerControl.LocalPlayer.Data.IsDead || AmDead || !Parent)
		{
			return;
		}
		JudgeRole judgeRole = PlayerControl.LocalPlayer.Data.Role as JudgeRole;
		bool flag = judgeRole != null && PlayerControl.LocalPlayer.PlayerId != (byte)PlayerId;
		if (VoteComplete || !Parent.Select((byte)PlayerId))
		{
			return;
		}
		Buttons.SetActive(value: true);
		float startPos = (AnimateButtonsFromLeft ? 0.2f : 1.95f);
		float duration = 0.35f;
		Vector2 judgeOverruleButtonEndPosition = Vector2.right * 0.65f;
		List<UiElement> list = new List<UiElement> { CancelButton };
		StartCoroutine(Effects.All(Effects.Lerp(0.25f, (float t) =>
		{
			CancelButton.transform.localPosition = Vector2.Lerp(Vector2.right * startPos, Vector2.right * 1.3f, Effects.ExpOut(t));
		})));
		if (!VoteComplete)
		{
			duration = 0.45f;
			judgeOverruleButtonEndPosition = Vector2.zero;
			ConfirmButton.gameObject.SetActive(value: true);
			list.Add(ConfirmButton);
			StartCoroutine(Effects.All(Effects.Lerp(0.35f, (float t) =>
			{
				ConfirmButton.transform.localPosition = Vector2.Lerp(Vector2.right * startPos, Vector2.right * 0.65f, Effects.ExpOut(t));
			})));
		}
		else
		{
			ConfirmButton.gameObject.SetActive(value: false);
		}
		if (JudgeOverruleButton != null)
		{
			if (flag && !judgeRole.IsBlockedByTasks() && !judgeRole.HasAlreadyOverruledThisMeeting && judgeRole.HasAnOverruleUse)
			{
				JudgeOverruleButton.gameObject.SetActive(value: true);
				if (PlayerControl.LocalPlayer.Data.Role.IsAffectedByComms)
				{
					JudgeOverruleButtonCommsDisable.SetActive(value: true);
					JudgeOverruleButton.enabled = false;
					JudgeOverruleButton.GetComponent<SpriteRenderer>().color = Palette.DisabledClear;
				}
				else
				{
					JudgeOverruleButtonCommsDisable.SetActive(value: false);
					JudgeOverruleButton.enabled = true;
					JudgeOverruleButton.GetComponent<SpriteRenderer>().color = Palette.White;
				}
				list.Add(JudgeOverruleButton);
				StartCoroutine(Effects.All(Effects.Lerp(duration, (float t) =>
				{
					JudgeOverruleButton.transform.localPosition = Vector2.Lerp(Vector2.right * startPos, judgeOverruleButtonEndPosition, Effects.ExpOut(t));
				})));
			}
			else
			{
				JudgeOverruleButton.gameObject.SetActive(value: false);
			}
		}
		ControllerManager.Instance.OpenOverlayMenu(name, CancelButton, list[1], list);
	}

	public void Cancel()
	{
		ClearButtons();
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Serialize(MessageWriter writer)
	{
		writer.Write(VotedForId);
		writer.Write(DidReport);
	}

	public void Deserialize(MessageReader reader)
	{
		VotedForId = reader.ReadByte();
		DidReport = reader.ReadBoolean();
	}

	public void SetCosmetics(NetworkedPlayerInfo playerInfo)
	{
		Background.sprite = ShipStatus.Instance.CosmeticsCache.GetNameplate(playerInfo.DefaultOutfit.NamePlateId).Image;
		PlayerIcon.UpdateFromEitherPlayerDataOrCache(playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ComplexUI, includePet: false);
		PlayerIcon.ToggleName(active: false);
		NameText.text = playerInfo.PlayerName;
		LevelNumberText.text = ProgressionManager.FormatVisualLevel(playerInfo.PlayerLevel);
		PlayerMaterial.SetColors(DataManager.Player.Customization.Color, ThumbsDown);
		DataManager.Settings.Accessibility.OnColorBlindModeChanged += SetColorblindText;
		SetColorblindText();
	}

	public void PreviewNameplate(string plateID)
	{
		gameObject.SetActive(value: true);
		NamePlateData namePlateById = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(plateID);
		StartCoroutine(((MonoBehaviour)this).CoLoadAssetAsync((IAddressableAssetProvider<NamePlateViewData>)namePlateById, (Action<NamePlateViewData>)((NamePlateViewData viewData) =>
		{
			Background.sprite = viewData.Image;
		})));
		PlayerIcon.gameObject.SetActive(value: false);
		NameText.text = DataManager.Player.Customization.Name;
		LevelNumberText.text = ProgressionManager.Instance.CurrentVisualLevel;
	}

	private void OnDestroy()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged -= SetColorblindText;
		if (Buttons.activeSelf)
		{
			ControllerManager.Instance.CloseOverlayMenu(name);
		}
	}

	private void SetColorblindText()
	{
		ColorBlindName.enabled = DataManager.Settings.Accessibility.ColorBlindMode;
		ColorBlindName.text = PlayerIcon.ColorBlindName;
	}

	private bool CanBeHighlighted()
	{
		if (AmDead)
		{
			return false;
		}
		if (PlayerControl.LocalPlayer.Data.IsDead)
		{
			return false;
		}
		if (VoteComplete)
		{
			return false;
		}
		if (Parent == null)
		{
			return false;
		}
		if (Parent.CurrentState == MeetingHud.MeetingStates.Discussion)
		{
			return false;
		}
		return true;
	}

	private static float TriangleWave(float t)
	{
		t -= (float)(int)t;
		if (t < 0.5f)
		{
			return t * 2f;
		}
		return 1f - (t - 0.5f) * 2f;
	}
}
