using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.GameOptions;
using AmongUs.Matchmaking;
using InnerNet;
using TMPro;
using UnityEngine;

public class FilterPopUp : MonoBehaviour
{
	public class FilterInfoUI
	{
		public enum FilterName
		{
			PlayerSpeed,
			KillCooldown,
			VotingTime,
			VisualTasks,
			AnonymousVotes,
			ConfirmEjects,
			Tag,
			CrossPlay,
			Roles,
			ImpostorNumber,
			FlashlightMode,
			HidingTime,
			FinalHideTime,
			MaxVentUses
		}

		public FilterName myFilterName;

		public int ButtonCount;

		public List<string> buttonTexts;

		public FilterOptionUI.ButtonSize ButtonSize;

		public StringNames stringName;

		public BoolOptionNames BoolOptionN;

		public Int32OptionNames Int32OptionN;

		public CategorizedOptionNames CatOptionName;

		public int tooltipIndex;

		public string ExtraInfoString = string.Empty;

		public string GetFilterType()
		{
			switch (myFilterName)
			{
			case FilterName.CrossPlay:
				return "crossplay";
			case FilterName.PlayerSpeed:
			case FilterName.KillCooldown:
			case FilterName.VotingTime:
			case FilterName.HidingTime:
			case FilterName.FinalHideTime:
				return "cat";
			case FilterName.ImpostorNumber:
				return "int";
			case FilterName.Tag:
			case FilterName.MaxVentUses:
				return "int0";
			default:
				return "bool";
			}
		}

		public void Init(FilterName filterName)
		{
			myFilterName = filterName;
			buttonTexts = new List<string>();
			switch (myFilterName)
			{
			case FilterName.PlayerSpeed:
				ButtonCount = 4;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterLow));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterMed));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterHigh));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterVeryHigh));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.GamePlayerSpeed;
				CatOptionName = CategorizedOptionNames.PlayerSpeed;
				tooltipIndex = 5;
				ExtraInfoString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.TooltipSpeedInfo);
				break;
			case FilterName.KillCooldown:
				ButtonCount = 4;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterLow));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterMed));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterHigh));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterVeryHigh));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.GameKillCooldown;
				CatOptionName = CategorizedOptionNames.KillCooldown;
				ExtraInfoString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.TooltipKillCDInfo);
				tooltipIndex = 6;
				break;
			case FilterName.VotingTime:
				ButtonCount = 4;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterLow));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterMed));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterHigh));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterVeryHigh));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.GameVotingTime;
				CatOptionName = CategorizedOptionNames.VotingTime;
				ExtraInfoString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.TooltipVotingInfo);
				tooltipIndex = 7;
				break;
			case FilterName.VisualTasks:
				ButtonCount = 2;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.GameVisualTasks;
				BoolOptionN = BoolOptionNames.VisualTasks;
				tooltipIndex = 8;
				break;
			case FilterName.AnonymousVotes:
				ButtonCount = 2;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.GameAnonymousVotes;
				BoolOptionN = BoolOptionNames.AnonymousVotes;
				tooltipIndex = 9;
				break;
			case FilterName.ConfirmEjects:
				ButtonCount = 2;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.GameConfirmImpostor;
				BoolOptionN = BoolOptionNames.ConfirmImpostor;
				tooltipIndex = 10;
				break;
			case FilterName.Tag:
				ButtonCount = 4;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterNone));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterBeginner));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterIntermediate));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterExpert));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.FilterTag;
				Int32OptionN = Int32OptionNames.Tag;
				tooltipIndex = 11;
				break;
			case FilterName.CrossPlay:
				ButtonCount = 2;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CrossPlaySamePlatform));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CrossPlayAllPlatforms));
				ButtonSize = FilterOptionUI.ButtonSize.Long;
				stringName = StringNames.CrossPlayTitle;
				tooltipIndex = 12;
				break;
			case FilterName.ImpostorNumber:
				ButtonCount = 3;
				buttonTexts.Add("1");
				buttonTexts.Add("2");
				buttonTexts.Add("3");
				ButtonSize = FilterOptionUI.ButtonSize.Short;
				stringName = StringNames.GameNumImpostors;
				Int32OptionN = Int32OptionNames.NumImpostors;
				tooltipIndex = 3;
				break;
			case FilterName.Roles:
				ButtonCount = 2;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.Roles;
				BoolOptionN = BoolOptionNames.Roles;
				tooltipIndex = 4;
				break;
			case FilterName.FlashlightMode:
				ButtonCount = 2;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				stringName = StringNames.UseFlashlight;
				BoolOptionN = BoolOptionNames.UseFlashlight;
				tooltipIndex = 13;
				break;
			case FilterName.HidingTime:
				ButtonCount = 3;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterLow));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterMed));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterHigh));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				ExtraInfoString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.TooptipHidingTimeInfo);
				stringName = StringNames.EscapeTime;
				CatOptionName = CategorizedOptionNames.HidingTime;
				tooltipIndex = 14;
				break;
			case FilterName.FinalHideTime:
				ButtonCount = 4;
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterLow));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterMed));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterHigh));
				buttonTexts.Add(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterVeryHigh));
				ButtonSize = FilterOptionUI.ButtonSize.Med;
				ExtraInfoString = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.TooltipFinalTimeInfo);
				stringName = StringNames.FinalEscapeTime;
				CatOptionName = CategorizedOptionNames.FinalHideTime;
				tooltipIndex = 15;
				break;
			case FilterName.MaxVentUses:
				ButtonCount = 6;
				buttonTexts.Add("0");
				buttonTexts.Add("1");
				buttonTexts.Add("2");
				buttonTexts.Add("3");
				buttonTexts.Add("4");
				buttonTexts.Add("5");
				ButtonSize = FilterOptionUI.ButtonSize.Short;
				stringName = StringNames.MaxVentUses;
				Int32OptionN = Int32OptionNames.CrewmateVentUses;
				tooltipIndex = 16;
				break;
			}
		}
	}

	[SerializeField]
	private FilterMapPicker mapPicker;

	[SerializeField]
	private BaseGameSetting mapSetting;

	[SerializeField]
	private Scroller scrollBar;

	[SerializeField]
	private Scroller scrollBarChat;

	[SerializeField]
	private FilterOptionUI filterButtonPrefab;

	[SerializeField]
	private Transform container;

	[SerializeField]
	private PassiveButton[] modeButtons;

	[SerializeField]
	private PassiveButton[] crossPlayButtons;

	[SerializeField]
	private PassiveButton[] tabButtons;

	[SerializeField]
	private PassiveButton[] chatTypeButtons;

	[SerializeField]
	private PassiveButton[] afmButtons;

	[SerializeField]
	private GameObject afmToggle;

	[SerializeField]
	private TextMeshPro afmToggleText;

	[SerializeField]
	private FilterOptionUI modeFilterOptionUI;

	[SerializeField]
	private FilterOptionUI crossPlayOptionUI;

	[SerializeField]
	private FilterOptionUI chatOptionUI;

	[SerializeField]
	private FilterOptionUI aprilFoolsOptionUI;

	[SerializeField]
	private GameObject generalContainer;

	[SerializeField]
	private GameObject chatContainer;

	[SerializeField]
	private LanguagePicker langPicker;

	[SerializeField]
	private TextMeshPro tooltip;

	[SerializeField]
	private TextMeshPro tooltipExtraInfo;

	[SerializeField]
	private GameObject tooltipExtraBox;

	[SerializeField]
	private StringNames[] tooltipStrings;

	[SerializeField]
	private AspectPosition aspectPosition;

	[SerializeField]
	private TextMeshPro mapNames;

	[SerializeField]
	private TextMeshPro mapNameTitle;

	[SerializeField]
	private List<SpriteRenderer> blackSquares;

	[SerializeField]
	private Color boxColor;

	[SerializeField]
	private Color boxhighlightColor;

	[SerializeField]
	private TextMeshPro chatWarningText;

	[SerializeField]
	private GameObject filtersActiveText;

	[SerializeField]
	private GameObject maxFiltersText;

	[SerializeField]
	private GameObject numFiltersText;

	private List<FilterOptionUI> classicChildren;

	private List<FilterOptionUI> hnsChildren;

	private float start_Pos_Y = -0.1f;

	private const float START_POS_X = 0.44f;

	private const float SPACING_Y = 0.45f;

	private const float HEADER_HEIGHT = 0.65f;

	private const float WAIT_TOOLTIP_TIME = 0.3f;

	private const float ANCHOR_POSX = 0.1f;

	private const float ANCHOR_OFFSET_X = 0.89f;

	private const float WAIT_TIME_SLIDE = 0.3f;

	private float scrollerHeightClassic;

	private float scrollerHeightHnS;

	private List<FilterInfoUI> filtersClassic;

	private List<FilterInfoUI> filtersHnS;

	private Coroutine changeTooltipCo;

	private bool animating;

	private static readonly int STENCIL_COMP = Shader.PropertyToID("_StencilComp");

	private static readonly int STENCIL = Shader.PropertyToID("_Stencil");

	private const int MASK_LAYER = 51;

	private const float Y_POS_LANGPICKER_NORMAL = 1.38f;

	private const float Y_POS_LANGPICKER_WARNING = 0.58f;

	private const int FILTER_LIMIT = 8;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private void Awake()
	{
		if (AprilFoolsMode.ShouldShowAprilFoolsToggle())
		{
			afmToggleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(AprilFoolsMode.GetOptionsToggleStringName());
			afmToggle.SetActive(value: true);
			start_Pos_Y -= 0.55f;
			afmToggle.transform.localPosition = new Vector3(afmToggle.transform.localPosition.x, afmToggle.transform.localPosition.y + 0.35f, afmToggle.transform.localPosition.z);
		}
		else
		{
			afmToggle.SetActive(value: false);
		}
		langPicker.callbackLang = SetLanguage;
		langPicker.callbackTooltip = SetLangTooltip;
		mapNames.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		mapNames.fontMaterial.SetFloat(STENCIL, 51f);
		mapNameTitle.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		mapNameTitle.fontMaterial.SetFloat(STENCIL, 51f);
		chatWarningText.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		chatWarningText.fontMaterial.SetFloat(STENCIL, 51f);
		chatOptionUI.SetupButtonMaskLayer();
	}

	private void OnEnable()
	{
		mapPicker.SetUpFromData(mapSetting, 1);
		mapPicker.Initialize(51);
		mapPicker.SetupClickAction();
		mapPicker.SetupMouseOver(SetMapTooltip);
		mapPicker.SetupSelectedIcons(DestroyableSingleton<FindAGameManager>.Instance.GetMapFilter());
		switch (DataManager.Settings.Multiplayer.LastPlayedGameMode)
		{
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			SetMode(1);
			break;
		case GameModes.Normal:
		case GameModes.NormalFools:
			SetMode(0);
			break;
		default:
			SetMode(0);
			break;
		}
		if (AprilFoolsMode.IsAprilFoolsModeToggledOn)
		{
			afmButtons[0].SelectButton(isSelected: true);
			afmButtons[1].SelectButton(isSelected: false);
		}
		else
		{
			afmButtons[0].SelectButton(isSelected: false);
			afmButtons[1].SelectButton(isSelected: true);
		}
		if (DataManager.Settings.Multiplayer.CrossPlay)
		{
			crossPlayButtons[0].SelectButton(isSelected: true);
			crossPlayButtons[1].SelectButton(isSelected: false);
		}
		else
		{
			crossPlayButtons[0].SelectButton(isSelected: false);
			crossPlayButtons[1].SelectButton(isSelected: true);
		}
		crossPlayButtons[0].SetPassiveButtonHoverStateInactive();
		crossPlayButtons[1].SetPassiveButtonHoverStateInactive();
		modeFilterOptionUI.SetupButtonMaskLayer();
		crossPlayOptionUI.SetupButtonMaskLayer();
		aprilFoolsOptionUI.SetupButtonMaskLayer();
		SelectTab(isGeneral: true);
		uint langFilter = DestroyableSingleton<FindAGameManager>.Instance.GetLangFilter();
		langPicker.SetupLanguageButtons(langFilter);
		langPicker.SetupMaskLayer();
		CheckChatType();
		SetUpLangButtonsNav();
		ControllerManager.Instance.SetCurrentSelected(null);
		ForceDefaultTooltip();
		StartCoroutine(ShowCo());
		ResetHoverStateButtons();
	}

	private IEnumerator ShowCo()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, null, null, ControllerSelectable);
		animating = true;
		Vector2 vector = new Vector2(0.89f, aspectPosition.anchorPoint.y);
		Vector2 dest = new Vector2(0.1f, aspectPosition.anchorPoint.y);
		aspectPosition.anchorPoint = vector;
		yield return Effects.SlideAnchor(aspectPosition, vector, dest, 0.3f);
		animating = false;
		ControllerManager.Instance.SetBackButton(BackButton);
		ControllerManager.Instance.SetCurrentSelected(DefaultButtonSelected);
	}

	public void CloseFilter()
	{
		if (!animating)
		{
			StartCoroutine(CloseCo());
		}
	}

	private IEnumerator CloseCo()
	{
		animating = true;
		Vector2 vector = new Vector2(0.1f, aspectPosition.anchorPoint.y);
		Vector2 dest = new Vector2(0.89f, aspectPosition.anchorPoint.y);
		aspectPosition.anchorPoint = vector;
		yield return Effects.SlideAnchor(aspectPosition, vector, dest, 0.3f);
		animating = false;
		gameObject.SetActive(value: false);
	}

	private void ResetHoverStateButtons()
	{
		foreach (MapSelectButton mapButton in mapPicker.GetMapButtons())
		{
			mapButton.Button.SetPassiveButtonHoverStateInactive();
		}
		PassiveButton[] array = modeButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		array = tabButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		array = chatTypeButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		if (classicChildren != null)
		{
			foreach (FilterOptionUI classicChild in classicChildren)
			{
				foreach (PassiveButton button in classicChild.GetButtons())
				{
					button.SetPassiveButtonHoverStateInactive();
				}
			}
		}
		if (hnsChildren == null)
		{
			return;
		}
		foreach (FilterOptionUI hnsChild in hnsChildren)
		{
			foreach (PassiveButton button2 in hnsChild.GetButtons())
			{
				button2.SetPassiveButtonHoverStateInactive();
			}
		}
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		DataManager.Settings.Save();
	}

	private void SetLanguage(uint lang)
	{
		GameOptionsManager.Instance.GameHostOptions.SetUInt(UInt32OptionNames.Keywords, lang);
		DestroyableSingleton<FindAGameManager>.Instance.AddLangFilterValue(lang);
	}

	private void SetLangTooltip(bool isActive)
	{
		if (isActive)
		{
			SetTooltip(17, 4);
		}
		else
		{
			SetTooltip(0, -1);
		}
	}

	private IEnumerator SetTooltipCo(int index, int highlightIndex, string extraInfo = "")
	{
		yield return new WaitForSeconds(0.3f);
		tooltip.text = DestroyableSingleton<TranslationController>.Instance.GetString(tooltipStrings[index]);
		HighlightBox(highlightIndex);
		if (extraInfo != string.Empty)
		{
			tooltipExtraBox.SetActive(value: true);
			tooltipExtraInfo.text = extraInfo;
		}
		else
		{
			tooltipExtraBox.SetActive(value: false);
		}
	}

	private void ForceDefaultTooltip()
	{
		tooltip.text = DestroyableSingleton<TranslationController>.Instance.GetString(tooltipStrings[0]);
		tooltipExtraBox.SetActive(value: false);
		HighlightBox(-1);
	}

	private void SetMapTooltip(int index)
	{
		if (index == 0)
		{
			SetTooltip(0, -1);
		}
		else
		{
			SetTooltip(2, 1);
		}
	}

	public void SetModePublic(int i)
	{
		SetMode(i);
		DestroyableSingleton<FindAGameManager>.Instance.FilterModified();
	}

	public void SetAprilFools(bool isOn)
	{
		DestroyableSingleton<FindAGameManager>.Instance.ToggleAprilFools(isOn);
		DestroyableSingleton<FindAGameManager>.Instance.FilterModified();
		if (isOn)
		{
			afmButtons[0].SelectButton(isSelected: true);
			afmButtons[1].SelectButton(isSelected: false);
		}
		else
		{
			afmButtons[0].SelectButton(isSelected: false);
			afmButtons[1].SelectButton(isSelected: true);
		}
	}

	private void SetMode(int i)
	{
		switch (i)
		{
		case 0:
			GameOptionsManager.Instance.SwitchGameMode(GameModes.Normal);
			DataManager.Settings.Multiplayer.LastPlayedGameMode = GameModes.Normal;
			modeButtons[0].SelectButton(isSelected: true);
			modeButtons[1].SelectButton(isSelected: false);
			CreateButtons(isClassic: true);
			DestroyableSingleton<FindAGameManager>.Instance.UpdateGameModeText();
			break;
		case 1:
			GameOptionsManager.Instance.SwitchGameMode(GameModes.HideNSeek);
			DataManager.Settings.Multiplayer.LastPlayedGameMode = GameModes.HideNSeek;
			modeButtons[0].SelectButton(isSelected: false);
			modeButtons[1].SelectButton(isSelected: true);
			CreateButtons(isClassic: false);
			DestroyableSingleton<FindAGameManager>.Instance.UpdateGameModeText();
			break;
		}
		DataManager.Settings.Save();
		DestroyableSingleton<FindAGameManager>.Instance.UpdateFiltersTexts();
		SetUpMapButtonsNav();
	}

	public void SetTooltipModes(int index)
	{
		SetTooltip(index, 0);
	}

	public void SetTooltipApril(int index)
	{
		SetTooltip(index, 5);
	}

	public void SetTooltipChat(int index)
	{
		SetTooltip(index, 3);
	}

	public void SetTooltipCrossplay(int index)
	{
		SetTooltip(index, 2);
	}

	public void SetTooltip(int index, int highlightIndex, string extraInfo = "")
	{
		if (changeTooltipCo != null)
		{
			StopCoroutine(changeTooltipCo);
		}
		if (index == 0)
		{
			changeTooltipCo = StartCoroutine(SetTooltipCo(index, -1, extraInfo));
			return;
		}
		HighlightBox(highlightIndex);
		tooltip.text = DestroyableSingleton<TranslationController>.Instance.GetString(tooltipStrings[index]);
		if (extraInfo != string.Empty)
		{
			tooltipExtraBox.SetActive(value: true);
			tooltipExtraInfo.text = extraInfo;
		}
		else
		{
			tooltipExtraBox.SetActive(value: false);
		}
	}

	public void HighlightBox(int index)
	{
		foreach (SpriteRenderer blackSquare in blackSquares)
		{
			blackSquare.color = boxColor;
		}
		if (index >= 0)
		{
			blackSquares[index].color = boxhighlightColor;
		}
	}

	private void SetExtraInfoTooltip(string infoString)
	{
		tooltipExtraBox.SetActive(value: true);
		tooltipExtraInfo.text = infoString;
	}

	public void SelectTab(bool isGeneral)
	{
		if (isGeneral)
		{
			tabButtons[0].SelectButton(isSelected: true);
			tabButtons[1].SelectButton(isSelected: false);
			generalContainer.SetActive(value: true);
			chatContainer.SetActive(value: false);
		}
		else
		{
			tabButtons[0].SelectButton(isSelected: false);
			tabButtons[1].SelectButton(isSelected: true);
			generalContainer.SetActive(value: false);
			chatContainer.SetActive(value: true);
		}
	}

	public void SetCrossplayFilter(bool allPlatforms)
	{
		if (allPlatforms)
		{
			crossPlayButtons[0].SelectButton(isSelected: true);
			crossPlayButtons[1].SelectButton(isSelected: false);
			DataManager.Settings.Multiplayer.CrossPlay = true;
		}
		else
		{
			crossPlayButtons[0].SelectButton(isSelected: false);
			crossPlayButtons[1].SelectButton(isSelected: true);
			DataManager.Settings.Multiplayer.CrossPlay = false;
		}
		DataManager.Settings.Save();
		DestroyableSingleton<FindAGameManager>.Instance.AddCrossplayFilterValue((uint)CrossplayMode.GetCrossplayFlags());
	}

	private void SetUpMapButtonsNav()
	{
		foreach (MapSelectButton mapButton in mapPicker.GetMapButtons())
		{
			ControllerSelectable.Add(mapButton.Button);
			mapButton.Button.ControllerNav.selectOnUp = modeButtons[0];
			mapButton.Button.ClickMask = scrollBar.Hitbox;
		}
		modeButtons[0].ControllerNav.selectOnDown = mapPicker.GetMapButtons()[0].Button;
		modeButtons[1].ControllerNav.selectOnDown = mapPicker.GetMapButtons()[0].Button;
	}

	private void SetUpLangButtonsNav()
	{
		List<LanguageOption> langButtons = langPicker.GetLangButtons();
		foreach (LanguageOption item in langButtons)
		{
			item.Button.ClickMask = scrollBarChat.Hitbox;
			ControllerSelectable.Add(item.Button);
		}
		chatTypeButtons[0].ControllerNav.selectOnDown = langButtons[0].Button;
		chatTypeButtons[1].ControllerNav.selectOnDown = langButtons[0].Button;
	}

	private void CheckChatType()
	{
		switch (DestroyableSingleton<FindAGameManager>.Instance.GetChatFilter())
		{
		case -1:
			if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
			{
				chatTypeButtons[0].SetButtonEnableState(enabled: false);
				chatTypeButtons[1].SelectButton(isSelected: true);
				chatWarningText.gameObject.SetActive(value: true);
				langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 0.58f, langPicker.transform.localPosition.z);
			}
			else
			{
				langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 1.38f, langPicker.transform.localPosition.z);
				chatWarningText.gameObject.SetActive(value: false);
				chatTypeButtons[0].SelectButton(isSelected: false);
				chatTypeButtons[1].SelectButton(isSelected: false);
			}
			break;
		case 1:
			if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
			{
				langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 0.58f, langPicker.transform.localPosition.z);
				chatTypeButtons[0].SetButtonEnableState(enabled: false);
				chatTypeButtons[1].SelectButton(isSelected: true);
				chatWarningText.gameObject.SetActive(value: true);
			}
			else
			{
				langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 1.38f, langPicker.transform.localPosition.z);
				chatWarningText.gameObject.SetActive(value: false);
				chatTypeButtons[0].SelectButton(isSelected: true);
				chatTypeButtons[1].SelectButton(isSelected: false);
			}
			break;
		case 2:
			if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
			{
				langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 0.58f, langPicker.transform.localPosition.z);
				chatTypeButtons[0].SetButtonEnableState(enabled: false);
				chatWarningText.gameObject.SetActive(value: true);
			}
			else
			{
				langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 1.38f, langPicker.transform.localPosition.z);
				chatWarningText.gameObject.SetActive(value: false);
				chatTypeButtons[0].SelectButton(isSelected: false);
			}
			chatTypeButtons[1].SelectButton(isSelected: true);
			break;
		case 0:
			break;
		}
	}

	public void SetChatType(bool isFreeChat)
	{
		if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
		{
			chatTypeButtons[0].SetButtonEnableState(enabled: false);
			chatTypeButtons[1].SelectButton(isSelected: true);
			chatWarningText.gameObject.SetActive(value: true);
			langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 0.58f, langPicker.transform.localPosition.z);
			DestroyableSingleton<FindAGameManager>.Instance.AddChatFilterValue(QuickChatModes.QuickChatOnly);
		}
		else if (isFreeChat)
		{
			chatTypeButtons[0].SelectButton(isSelected: true);
			chatTypeButtons[1].SelectButton(isSelected: false);
			DestroyableSingleton<FindAGameManager>.Instance.AddChatFilterValue(QuickChatModes.FreeChatOrQuickChat);
		}
		else
		{
			if (chatTypeButtons[0].enabled)
			{
				chatTypeButtons[0].SelectButton(isSelected: false);
			}
			chatTypeButtons[1].SelectButton(isSelected: true);
			DestroyableSingleton<FindAGameManager>.Instance.AddChatFilterValue(QuickChatModes.QuickChatOnly);
		}
	}

	private void CreateButtons(bool isClassic)
	{
		if (isClassic)
		{
			if (classicChildren != null && classicChildren.Count > 0)
			{
				foreach (FilterOptionUI classicChild in classicChildren)
				{
					classicChild.gameObject.SetActive(value: true);
				}
				if (hnsChildren != null && hnsChildren.Count > 0)
				{
					foreach (FilterOptionUI hnsChild in hnsChildren)
					{
						hnsChild.gameObject.SetActive(value: false);
					}
				}
				InitializeControllerNavigation(isClassic: true);
				scrollBar.SetYBoundsMax(scrollerHeightClassic);
				scrollBar.ScrollPercentY(0f);
				return;
			}
			if (hnsChildren != null && hnsChildren.Count > 0)
			{
				foreach (FilterOptionUI hnsChild2 in hnsChildren)
				{
					hnsChild2.gameObject.SetActive(value: false);
				}
			}
			classicChildren = new List<FilterOptionUI>();
			float num = start_Pos_Y;
			ControllerSelectable.AddRange(scrollBar.GetComponentsInChildren<UiElement>());
			if (filtersClassic != null)
			{
				for (int i = 0; i < filtersClassic.Count; i++)
				{
					FilterOptionUI filterOptionUI = Object.Instantiate(filterButtonPrefab, Vector3.zero, Quaternion.identity, container);
					filterOptionUI.toolTipCallback = SetTooltip;
					filterOptionUI.transform.localScale = Vector3.one;
					filterOptionUI.FilterInfo = filtersClassic[i];
					filterOptionUI.transform.localPosition = new Vector3(0.44f, num, -2f);
					blackSquares.Add(filterOptionUI.GetBlackSquare());
					filterOptionUI.HighlightIndex = blackSquares.Count - 1;
					filterOptionUI.SetupButtons(ClickFilterOption, scrollBar.Hitbox);
					classicChildren.Add(filterOptionUI);
					num -= 0.45f;
				}
				scrollerHeightClassic = 0f - num - 0.65f;
				scrollBar.SetYBoundsMax(scrollerHeightClassic);
				scrollBar.ScrollPercentY(0f);
				InitializeControllerNavigation(isClassic: true);
			}
			return;
		}
		if (hnsChildren != null && hnsChildren.Count > 0)
		{
			foreach (FilterOptionUI hnsChild3 in hnsChildren)
			{
				hnsChild3.gameObject.SetActive(value: true);
			}
			if (classicChildren != null && classicChildren.Count > 0)
			{
				foreach (FilterOptionUI classicChild2 in classicChildren)
				{
					classicChild2.gameObject.SetActive(value: false);
				}
			}
			InitializeControllerNavigation(isClassic: false);
			scrollBar.SetYBoundsMax(scrollerHeightHnS);
			scrollBar.ScrollPercentY(0f);
			return;
		}
		if (classicChildren != null && classicChildren.Count > 0)
		{
			foreach (FilterOptionUI classicChild3 in classicChildren)
			{
				classicChild3.gameObject.SetActive(value: false);
			}
		}
		hnsChildren = new List<FilterOptionUI>();
		float num2 = start_Pos_Y;
		ControllerSelectable.AddRange(scrollBar.GetComponentsInChildren<UiElement>());
		if (filtersHnS != null)
		{
			for (int j = 0; j < filtersHnS.Count; j++)
			{
				FilterOptionUI filterOptionUI2 = Object.Instantiate(filterButtonPrefab, Vector3.zero, Quaternion.identity, container);
				filterOptionUI2.toolTipCallback = SetTooltip;
				filterOptionUI2.transform.localScale = Vector3.one;
				filterOptionUI2.FilterInfo = filtersHnS[j];
				filterOptionUI2.transform.localPosition = new Vector3(0.44f, num2, -2f);
				blackSquares.Add(filterOptionUI2.GetBlackSquare());
				filterOptionUI2.HighlightIndex = blackSquares.Count - 1;
				filterOptionUI2.SetupButtons(ClickFilterOption, scrollBar.Hitbox);
				hnsChildren.Add(filterOptionUI2);
				num2 -= 0.45f;
			}
			scrollerHeightHnS = 0f - num2 - 0.65f;
			scrollBar.SetYBoundsMax(scrollerHeightHnS);
			scrollBar.ScrollPercentY(0f);
			InitializeControllerNavigation(isClassic: false);
		}
	}

	public void ResetFilterSelection()
	{
		if (classicChildren != null)
		{
			foreach (FilterOptionUI classicChild in classicChildren)
			{
				classicChild.ResetFilterSelection();
			}
		}
		if (hnsChildren == null)
		{
			return;
		}
		foreach (FilterOptionUI hnsChild in hnsChildren)
		{
			hnsChild.ResetFilterSelection();
		}
	}

	private void InitializeControllerNavigation(bool isClassic)
	{
		List<GameObject> list = new List<GameObject>();
		if (isClassic)
		{
			list.Add(mapPicker.gameObject);
			foreach (FilterOptionUI classicChild in classicChildren)
			{
				list.Add(classicChild.gameObject);
			}
		}
		else
		{
			list.Add(mapPicker.gameObject);
			foreach (FilterOptionUI hnsChild in hnsChildren)
			{
				list.Add(hnsChild.gameObject);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i];
			if (!gameObject.gameObject.activeSelf)
			{
				continue;
			}
			UiElement[] array = null;
			UiElement[] array2 = null;
			if (i - 1 >= 0)
			{
				array = list[i - 1].GetComponentsInChildren<UiElement>(includeInactive: true);
			}
			if (i + 1 < list.Count)
			{
				array2 = list[i + 1].GetComponentsInChildren<UiElement>(includeInactive: true);
			}
			UiElement[] componentsInChildren = gameObject.GetComponentsInChildren<UiElement>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				UiElement uiElement = componentsInChildren[j];
				uiElement.ControllerNav.mode = ControllerNavigation.Mode.Explicit;
				if (array != null && array.Length != 0)
				{
					if (i == 1)
					{
						uiElement.ControllerNav.selectOnUp = array[0];
					}
					else if (j < array.Length)
					{
						uiElement.ControllerNav.selectOnUp = array[j];
					}
					else
					{
						uiElement.ControllerNav.selectOnUp = array[0];
					}
				}
				else
				{
					uiElement.ControllerNav.selectOnUp = null;
				}
				if (array2 != null && array2.Length != 0)
				{
					if (i == 0)
					{
						if (AprilFoolsMode.ShouldShowAprilFoolsToggle())
						{
							afmButtons[0].ControllerNav.selectOnDown = array2[0];
							afmButtons[1].ControllerNav.selectOnDown = array2[0];
							uiElement.ControllerNav.selectOnDown = afmButtons[0];
							afmButtons[0].ControllerNav.selectOnUp = mapPicker.GetMapButtons()[0].Button;
							afmButtons[1].ControllerNav.selectOnUp = mapPicker.GetMapButtons()[0].Button;
						}
						else
						{
							uiElement.ControllerNav.selectOnDown = array2[0];
						}
					}
					else if (j < array2.Length)
					{
						uiElement.ControllerNav.selectOnDown = array2[j];
					}
					else
					{
						uiElement.ControllerNav.selectOnDown = array2[0];
					}
				}
				else
				{
					uiElement.ControllerNav.selectOnDown = null;
				}
			}
			if (isClassic)
			{
				foreach (PassiveButton button in classicChildren[0].GetButtons())
				{
					if (AprilFoolsMode.ShouldShowAprilFoolsToggle())
					{
						button.ControllerNav.selectOnUp = afmButtons[0];
					}
					else
					{
						button.ControllerNav.selectOnUp = mapPicker.GetMapButtons()[0].Button;
					}
				}
				continue;
			}
			foreach (PassiveButton button2 in hnsChildren[0].GetButtons())
			{
				if (AprilFoolsMode.ShouldShowAprilFoolsToggle())
				{
					button2.ControllerNav.selectOnUp = afmButtons[0];
				}
				else
				{
					button2.ControllerNav.selectOnUp = mapPicker.GetMapButtons()[0].Button;
				}
			}
		}
	}

	private void ClickFilterOption(bool isSelected, int index, FilterInfoUI filterInfo)
	{
		switch (filterInfo.GetFilterType())
		{
		case "cat":
			if (isSelected)
			{
				DestroyableSingleton<FindAGameManager>.Instance.AddCatFilterValue(index, filterInfo.myFilterName.ToString(), filterInfo.CatOptionName);
			}
			else
			{
				DestroyableSingleton<FindAGameManager>.Instance.RemoveCatFilterValue(index, filterInfo.myFilterName.ToString(), filterInfo.CatOptionName);
			}
			break;
		case "int":
			if (isSelected)
			{
				DestroyableSingleton<FindAGameManager>.Instance.AddIntFilterValue(index + 1, filterInfo.myFilterName.ToString(), filterInfo.Int32OptionN);
			}
			else
			{
				DestroyableSingleton<FindAGameManager>.Instance.RemoveIntFilterValue(index + 1, filterInfo.myFilterName.ToString(), filterInfo.Int32OptionN);
			}
			break;
		case "int0":
			if (isSelected)
			{
				DestroyableSingleton<FindAGameManager>.Instance.AddIntFilterValue(index, filterInfo.myFilterName.ToString(), filterInfo.Int32OptionN);
			}
			else
			{
				DestroyableSingleton<FindAGameManager>.Instance.RemoveIntFilterValue(index, filterInfo.myFilterName.ToString(), filterInfo.Int32OptionN);
			}
			break;
		case "bool":
			if (isSelected)
			{
				DestroyableSingleton<FindAGameManager>.Instance.AddBoolFilterValue(index == 0, filterInfo.myFilterName.ToString(), filterInfo.BoolOptionN);
			}
			else
			{
				DestroyableSingleton<FindAGameManager>.Instance.RemoveBoolFilterValue(index == 0, filterInfo.myFilterName.ToString(), filterInfo.BoolOptionN);
			}
			break;
		}
	}

	private void CheckFilterLimit()
	{
		bool flag = DestroyableSingleton<FindAGameManager>.Instance.GetRealFilterCount() >= 8;
		switch (DestroyableSingleton<FindAGameManager>.Instance.GetGameMode())
		{
		default:
			if (classicChildren == null || classicChildren.Count <= 0)
			{
				break;
			}
			foreach (FilterOptionUI classicChild in classicChildren)
			{
				classicChild.SetInactiveButton(!flag);
			}
			break;
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			if (hnsChildren == null || hnsChildren.Count <= 0)
			{
				break;
			}
			foreach (FilterOptionUI hnsChild in hnsChildren)
			{
				hnsChild.SetInactiveButton(!flag);
			}
			break;
		}
		mapPicker.SetInactiveButton(!flag);
		if (flag)
		{
			if (mapPicker.IsFilterEnabled())
			{
				modeButtons[0].ControllerNav.selectOnDown = mapPicker.GetMapButtons()[0].Button;
				modeButtons[1].ControllerNav.selectOnDown = mapPicker.GetMapButtons()[0].Button;
			}
			else
			{
				UiElement selectOnDown = FindNextFilterAvailable(DestroyableSingleton<FindAGameManager>.Instance.GetGameMode());
				modeButtons[0].ControllerNav.selectOnDown = selectOnDown;
				modeButtons[1].ControllerNav.selectOnDown = selectOnDown;
			}
			maxFiltersText.SetActive(value: true);
			filtersActiveText.SetActive(value: false);
			numFiltersText.SetActive(value: false);
		}
		else
		{
			modeButtons[0].ControllerNav.selectOnDown = mapPicker.GetMapButtons()[0].Button;
			modeButtons[1].ControllerNav.selectOnDown = mapPicker.GetMapButtons()[0].Button;
			maxFiltersText.SetActive(value: false);
			filtersActiveText.SetActive(value: true);
			numFiltersText.SetActive(value: true);
		}
	}

	private UiElement FindNextFilterAvailable(GameModes gameMode)
	{
		switch (gameMode)
		{
		default:
		{
			for (int j = 0; j < classicChildren.Count; j++)
			{
				if (classicChildren[j].isFilterActive())
				{
					return classicChildren[j].GetButtons()[0];
				}
			}
			break;
		}
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
		{
			for (int i = 0; i < hnsChildren.Count; i++)
			{
				if (hnsChildren[i].isFilterActive())
				{
					return hnsChildren[i].GetButtons()[0];
				}
			}
			break;
		}
		}
		return null;
	}

	public void SetupFilters(PermittedFilters filters)
	{
		if (filters == null)
		{
			return;
		}
		if (classicChildren != null && classicChildren.Count > 0)
		{
			filtersClassic.Clear();
			foreach (FilterOptionUI classicChild in classicChildren)
			{
				Object.Destroy(classicChild.gameObject);
			}
		}
		if (hnsChildren != null && hnsChildren.Count > 0)
		{
			filtersHnS.Clear();
			foreach (FilterOptionUI hnsChild in hnsChildren)
			{
				Object.Destroy(hnsChild.gameObject);
			}
		}
		filtersClassic = new List<FilterInfoUI>();
		filtersHnS = new List<FilterInfoUI>();
		filtersClassic = new List<FilterInfoUI>();
		FilterInfoUI filterInfoUI;
		if (filters.Filters.Contains(Filters.NumImposters))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.ImpostorNumber);
			filtersClassic.Add(filterInfoUI);
		}
		if (filters.Filters.Contains(Filters.Roles))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.Roles);
			filtersClassic.Add(filterInfoUI);
		}
		if (filters.Filters.Contains(Filters.PlayerSpeed))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.PlayerSpeed);
			filtersClassic.Add(filterInfoUI);
			filtersHnS.Add(filterInfoUI);
		}
		if (filters.Filters.Contains(Filters.KillCooldown))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.KillCooldown);
			filtersClassic.Add(filterInfoUI);
		}
		if (filters.Filters.Contains(Filters.VotingTime))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.VotingTime);
			filtersClassic.Add(filterInfoUI);
		}
		if (filters.Filters.Contains(Filters.VisualTasks))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.VisualTasks);
			filtersClassic.Add(filterInfoUI);
		}
		if (filters.Filters.Contains(Filters.AnonymousVotes))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.AnonymousVotes);
			filtersClassic.Add(filterInfoUI);
		}
		if (filters.Filters.Contains(Filters.ConfirmEjects))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.ConfirmEjects);
			filtersClassic.Add(filterInfoUI);
		}
		filterInfoUI = new FilterInfoUI();
		filterInfoUI.Init(FilterInfoUI.FilterName.FlashlightMode);
		filtersHnS.Add(filterInfoUI);
		filterInfoUI = new FilterInfoUI();
		filterInfoUI.Init(FilterInfoUI.FilterName.HidingTime);
		filtersHnS.Add(filterInfoUI);
		filterInfoUI = new FilterInfoUI();
		filterInfoUI.Init(FilterInfoUI.FilterName.FinalHideTime);
		filtersHnS.Add(filterInfoUI);
		filterInfoUI = new FilterInfoUI();
		filterInfoUI.Init(FilterInfoUI.FilterName.MaxVentUses);
		filtersHnS.Add(filterInfoUI);
		if (filters.Filters.Contains(Filters.Tags))
		{
			filterInfoUI = new FilterInfoUI();
			filterInfoUI.Init(FilterInfoUI.FilterName.Tag);
			filtersClassic.Add(filterInfoUI);
			filtersHnS.Add(filterInfoUI);
		}
	}
}
