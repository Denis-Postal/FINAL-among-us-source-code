using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class GamePresetsTab : MonoBehaviour
{
	[SerializeField]
	private List<SpriteRenderer> SpritesToDesaturate;

	[SerializeField]
	private PassiveButton StandardPresetButton;

	[SerializeField]
	private PassiveButton SecondPresetButton;

	[SerializeField]
	private GameOptionsMenu GameOptionsMenu;

	[SerializeField]
	private SpriteRenderer[] StandardRulesSprites;

	[SerializeField]
	private SpriteRenderer[] AlternateRulesSprites;

	[SerializeField]
	private TextMeshPro StandardRulesText;

	[SerializeField]
	private TextMeshPro AlternateRulesText;

	[SerializeField]
	private TextMeshPro PresetDescriptionText;

	[SerializeField]
	private TransitionOpen ConfirmPresetPopUp;

	[SerializeField]
	private PassiveButton PresetConfirmButton;

	[SerializeField]
	private PassiveButton[] PresetCancelButtons;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private void OnEnable()
	{
		if (GameOptionsManager.Instance.CurrentGameOptions.RulesPreset == RulesPresets.Standard)
		{
			SecondPresetButton.SelectButton(isSelected: false);
			StandardPresetButton.SelectButton(isSelected: true);
			StandardPresetButton.ReceiveMouseOut();
			SecondPresetButton.ReceiveMouseOut();
		}
		else if (GameOptionsManager.Instance.CurrentGameOptions.RulesPreset != RulesPresets.Custom)
		{
			SecondPresetButton.SelectButton(isSelected: true);
			StandardPresetButton.SelectButton(isSelected: false);
			StandardPresetButton.ReceiveMouseOut();
			SecondPresetButton.ReceiveMouseOut();
		}
		else
		{
			SecondPresetButton.SelectButton(isSelected: false);
			StandardPresetButton.SelectButton(isSelected: false);
		}
		SetSelectedText();
	}

	private void OnDisable()
	{
		CloseMenu();
	}

	public void OpenMenu()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
	}

	public void CloseMenu()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void Start()
	{
		StandardPresetButton.OnClick.AddListener(() =>
		{
			ClickPresetButton(RulesPresets.Standard, standardButtonSelected: true);
		});
		StandardPresetButton.OnMouseOver.AddListener(() =>
		{
			PresetDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameManager.Instance.GameSettingsList.StandardPresetDescription);
		});
		StandardPresetButton.OnMouseOut.AddListener(() =>
		{
			SetSelectedText();
		});
		SecondPresetButton.OnClick.AddListener(() =>
		{
			ClickPresetButton(GameManager.Instance.GameSettingsList.AlternateRulesType, standardButtonSelected: false);
		});
		SecondPresetButton.OnMouseOver.AddListener(() =>
		{
			PresetDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameManager.Instance.GameSettingsList.AlternatePresetDescription);
		});
		SecondPresetButton.OnMouseOut.AddListener(() =>
		{
			SetSelectedText();
		});
		StandardRulesText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameManager.Instance.GameSettingsList.StandardRulesName);
		AlternateRulesText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameManager.Instance.GameSettingsList.AlternateRulesName);
		StandardRulesSprites.ForEach((SpriteRenderer s) =>
		{
			s.sprite = GameManager.Instance.GameSettingsList.StandardRulesImage;
		});
		AlternateRulesSprites.ForEach((SpriteRenderer s) =>
		{
			s.sprite = GameManager.Instance.GameSettingsList.AlternateRulesImage;
		});
		SpritesToDesaturate.ForEach((SpriteRenderer s) =>
		{
			s.material.SetFloat("_Desat", 1f);
		});
	}

	private void SetSelectedText()
	{
		if (GameOptionsManager.Instance.CurrentGameOptions.RulesPreset == RulesPresets.Standard)
		{
			PresetDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameManager.Instance.GameSettingsList.StandardPresetDescription);
		}
		else if (GameOptionsManager.Instance.CurrentGameOptions.RulesPreset != RulesPresets.Custom)
		{
			PresetDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameManager.Instance.GameSettingsList.AlternatePresetDescription);
		}
		else
		{
			PresetDescriptionText.text = string.Empty;
		}
	}

	private void ClickPresetButton(RulesPresets preset, bool standardButtonSelected)
	{
		if (GameOptionsManager.Instance.CurrentGameOptions.RulesPreset == preset)
		{
			return;
		}
		ConfirmPresetPopUp.gameObject.SetActive(value: true);
		PresetConfirmButton.OnClick.RemoveAllListeners();
		PresetCancelButtons.ForEach((PassiveButton x) =>
		{
			x.OnClick.RemoveAllListeners();
		});
		PresetConfirmButton.OnClick.AddListener(() =>
		{
			GameOptionsMenu.ClickPresetButton(preset);
			SetSelectedText();
			SecondPresetButton.SelectButton(!standardButtonSelected);
			StandardPresetButton.SelectButton(standardButtonSelected);
			ControllerManager.Instance.CloseOverlayMenu(ConfirmPresetPopUp.name);
			ConfirmPresetPopUp.Close();
		});
		PresetCancelButtons.ForEach((PassiveButton x) =>
		{
			x.OnClick.AddListener(() =>
			{
				ControllerManager.Instance.CloseOverlayMenu(ConfirmPresetPopUp.name);
				ConfirmPresetPopUp.Close();
			});
		});
		ControllerManager.Instance.OpenOverlayMenu(ConfirmPresetPopUp.name, PresetCancelButtons.First(), PresetConfirmButton);
	}
}
