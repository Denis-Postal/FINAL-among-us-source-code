using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.GameOptions;
using Hazel;
using InnerNet;
using TMPro;
using UnityEngine;

public class CreateGameOptions : MonoBehaviour, IConnectButton
{
	[SerializeField]
	private GameObject AprilFoolsToggle;

	[SerializeField]
	private TextMeshPro AprilFoolsToggleText;

	[SerializeField]
	private MainMenuManager mainMenuManager;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private SpriteRenderer foreground;

	[SerializeField]
	private TextMeshPro tooltip;

	[SerializeField]
	private Transform crewmatesContainer;

	[SerializeField]
	private GameObject[] skeldCrewSprites;

	[SerializeField]
	private GameObject[] polusCrewSprites;

	[SerializeField]
	private GameObject[] miraCrewSprites;

	[SerializeField]
	private GameObject[] airshipCrewSprites;

	[SerializeField]
	private GameObject[] fungleCrewSprites;

	[SerializeField]
	private Sprite[] bgCrewmates;

	[SerializeField]
	private SpriteRenderer rendererBGCrewmates;

	[SerializeField]
	private SpriteRenderer mapBanner;

	[SerializeField]
	private Sprite[] mapBanners;

	[SerializeField]
	private ServerDropdown serverDropdown;

	[SerializeField]
	private TextMeshPro[] serverTexts;

	[SerializeField]
	private GameObject serverButton;

	[SerializeField]
	private IntGameSetting capacitySetting;

	[SerializeField]
	private BaseGameSetting mapSetting;

	[SerializeField]
	private CreateGameMapPicker mapPicker;

	[SerializeField]
	private NumberOption capacityOption;

	[SerializeField]
	private StringNames[] tooltipStrings;

	[SerializeField]
	private StringNames[] mapTooltips;

	[SerializeField]
	private PassiveButton[] tabButtons;

	[SerializeField]
	private GameObject[] contentObjects;

	[SerializeField]
	private PassiveButton[] modeButtons;

	[SerializeField]
	private PassiveButton[] levelButtons;

	[SerializeField]
	private PassiveButton[] crossPlayButtons;

	[SerializeField]
	private PassiveButton[] aprilFoolButtons;

	[SerializeField]
	private PassiveButton[] chatTypeButtons;

	[SerializeField]
	private PassiveButton minusButtonCapacity;

	[SerializeField]
	private PassiveButton plusButtonCapacity;

	[SerializeField]
	private GameObject confirmPopUp;

	[SerializeField]
	private Transform containerConfirm;

	[SerializeField]
	private LanguagePicker langPicker;

	[SerializeField]
	private TextMeshPro chatDescText;

	[SerializeField]
	private TextMeshPro chatWarningText;

	[SerializeField]
	private GameObject loadAnim;

	[SerializeField]
	private FilterOptionUI chatOptionUI;

	[SerializeField]
	private Scroller scrollBar;

	private int currentTag;

	private Coroutine changeTooltipCo;

	private bool animating;

	private int currentDefaultTooltip;

	private const float Y_POS_LANGPICKER_NORMAL = 0.84f;

	private const float Y_POS_LANGPICKER_WARNING = 0.14f;

	private const float SCROLLER_HEIGHT = 3f;

	private const float Y_POS_APRILFOOLS = -3.4f;

	private const float WAIT_TOOLTIP_TIME = 0.3f;

	private const float CREWMATE_SLIDE_TIME = 0.3f;

	private const float POPUP_SLIDE_TIME = 0.3f;

	private const float X_OFFSET_LEFT = -7f;

	private const float X_CREWMATES_POS = 0f;

	private const float Y_BOTTOM_POS = -14f;

	private GameObject[] currentCrewSprites;

	private static readonly int STENCIL_COMP = Shader.PropertyToID("_StencilComp");

	private static readonly int STENCIL = Shader.PropertyToID("_Stencil");

	private const int MASK_LAYER = 51;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private readonly Logger logger = new Logger(Logger.Category.Gameplay, "CreateGameOptions");

	private void Start()
	{
		langPicker.callbackLang = SetLanguage;
		langPicker.callbackTooltip = SetLangTooltip;
		chatOptionUI.SetupButtonMaskLayer();
		scrollBar.SetYBoundsMax(3f);
		scrollBar.ScrollPercentY(0f);
		chatDescText.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		chatDescText.fontMaterial.SetFloat(STENCIL, 51f);
		chatWarningText.fontMaterial.SetFloat(STENCIL_COMP, 3f);
		chatWarningText.fontMaterial.SetFloat(STENCIL, 51f);
	}

	public void Show()
	{
		if (!DestroyableSingleton<AccountManager>.Instance.CanPlayOnline())
		{
			AmongUsClient.Instance.LastDisconnectReason = DisconnectReasons.NotAuthorized;
			DestroyableSingleton<DisconnectPopup>.Instance.Show();
		}
		else if (DataManager.Player.Ban.IsBanned)
		{
			AmongUsClient.Instance.LastDisconnectReason = DisconnectReasons.IntentionalLeaving;
			DestroyableSingleton<DisconnectPopup>.Instance.Show();
		}
		else
		{
			gameObject.SetActive(value: true);
			foreground.gameObject.SetActive(value: true);
			StartCoroutine(CoShow());
		}
	}

	private IEnumerator CoShow()
	{
		animating = true;
		GameOptionsManager optionsManager = GameOptionsManager.Instance;
		if (optionsManager == null)
		{
			optionsManager = new GameOptionsManager(new UnityLogger());
		}
		if (optionsManager.currentGameOptions == null || optionsManager.currentHostOptions == null)
		{
			optionsManager.Initialize();
		}
		scrollBar.ScrollPercentY(0f);
		content.SetActive(value: true);
		crewmatesContainer.localPosition = new Vector3(-7f, crewmatesContainer.localPosition.y, crewmatesContainer.localPosition.z);
		capacityOption.SetUpFromData(capacitySetting, 1);
		capacityOption.OnValueChanged = ValueChanged;
		capacityOption.Initialize();
		capacityOption.gameObject.SetActive(value: true);
		SetCurrentServer();
		serverDropdown.Initialize(UpdateServerText, CloseServerDropdown);
		if (DataManager.Settings.Multiplayer.LastPlayedGameMode == GameModes.HideNSeek)
		{
			SelectMode(1, saveSetting: false);
		}
		else
		{
			SelectMode(0, saveSetting: false);
		}
		SetCrossPlay(DataManager.Settings.Multiplayer.CrossPlay);
		if (AprilFoolsMode.ShouldShowAprilFoolsToggle())
		{
			SetAprilFool(isOn: true);
			AprilFoolsToggleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(AprilFoolsMode.GetOptionsToggleStringName());
		}
		AprilFoolsToggle.SetActive(AprilFoolsMode.ShouldShowAprilFoolsToggle());
		Vector3 localPosition = AprilFoolsToggle.transform.localPosition;
		localPosition = new Vector3(localPosition.x, -3.4f, localPosition.z);
		AprilFoolsToggle.transform.localPosition = localPosition;
		GameOptionsManager.Instance.GameHostOptions.TryGetInt(Int32OptionNames.Tag, out var value);
		currentTag = value;
		SetTag(currentTag);
		CheckChatType();
		OpenTab(isGeneral: true);
		langPicker.SetupLanguageButtons((uint)GameOptionsManager.Instance.GameHostOptions.Keywords);
		langPicker.SetupMaskLayer();
		ControllerManager.Instance.SetCurrentSelected(null);
		mapPicker.SetUpFromData(mapSetting, 1);
		mapPicker.Initialize(1);
		mapPicker.OnValueChanged = MapChanged;
		mapPicker.SetupMouseOver(SetMapTooltip);
		SetUpMapButtonsNav();
		SetUpLangButtonsNav();
		ResetHoverStateButtons();
		yield return Effects.ColorFade(foreground, Color.black, Color.clear, 0.1f);
		MapChanged(null);
		animating = false;
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
		Vector2 source = new Vector2(-7f, crewmatesContainer.localPosition.y);
		Vector2 dest = new Vector2(0f, crewmatesContainer.localPosition.y);
		foreground.gameObject.SetActive(value: false);
		yield return Effects.Slide2D(crewmatesContainer, source, dest, 0.3f);
	}

	private void ResetHoverStateButtons()
	{
		PassiveButton[] array = modeButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		array = levelButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		array = crossPlayButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		array = aprilFoolButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		array = chatTypeButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		array = tabButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPassiveButtonHoverStateInactive();
		}
		foreach (MapSelectButton mapButton in mapPicker.GetMapButtons())
		{
			mapButton.Button.SetPassiveButtonHoverStateInactive();
		}
	}

	private void SetUpMapButtonsNav()
	{
		List<MapSelectButton> mapButtons = mapPicker.GetMapButtons();
		foreach (MapSelectButton item in mapButtons)
		{
			ControllerSelectable.Add(item.Button);
			item.Button.ControllerNav.mode = ControllerNavigation.Mode.Explicit;
			item.Button.ControllerNav.selectOnDown = minusButtonCapacity;
			item.Button.ControllerNav.selectOnUp = modeButtons[0];
		}
		modeButtons[0].ControllerNav.selectOnDown = mapButtons[0].Button;
		modeButtons[1].ControllerNav.selectOnDown = mapButtons[0].Button;
		minusButtonCapacity.ControllerNav.selectOnUp = mapButtons[0].Button;
		plusButtonCapacity.ControllerNav.selectOnUp = mapButtons[0].Button;
	}

	private void SetUpLangButtonsNav()
	{
		List<LanguageOption> langButtons = langPicker.GetLangButtons();
		foreach (LanguageOption item in langButtons)
		{
			ControllerSelectable.Add(item.Button);
			item.Button.ClickMask = scrollBar.Hitbox;
		}
		chatTypeButtons[0].ControllerNav.selectOnDown = langButtons[0].Button;
		chatTypeButtons[1].ControllerNav.selectOnDown = langButtons[0].Button;
	}

	private void MapChanged(OptionBehaviour behaviour)
	{
		if (mapPicker.GetSelectedID() == 0 && AprilFoolsMode.ShouldFlipSkeld())
		{
			mapBanner.flipX = true;
		}
		else
		{
			mapBanner.flipX = false;
		}
		rendererBGCrewmates.sprite = bgCrewmates[mapPicker.GetSelectedID()];
		mapBanner.sprite = mapBanners[mapPicker.GetSelectedID()];
		TurnOffCrewmates();
		switch (mapPicker.GetSelectedID())
		{
		case 0:
			currentCrewSprites = skeldCrewSprites;
			break;
		case 1:
			currentCrewSprites = miraCrewSprites;
			break;
		case 2:
			currentCrewSprites = polusCrewSprites;
			break;
		case 4:
			currentCrewSprites = airshipCrewSprites;
			break;
		case 5:
			currentCrewSprites = fungleCrewSprites;
			break;
		}
		SetCrewmateGraphic(capacityOption.Value - 1f);
	}

	private void TurnOffCrewmates()
	{
		GameObject[] array = skeldCrewSprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		array = miraCrewSprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		array = polusCrewSprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		array = airshipCrewSprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		array = fungleCrewSprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	public void OpenTab(bool isGeneral)
	{
		if (isGeneral)
		{
			tabButtons[0].SelectButton(isSelected: true);
			tabButtons[1].SelectButton(isSelected: false);
			contentObjects[0].SetActive(value: true);
			contentObjects[1].SetActive(value: false);
			currentDefaultTooltip = 0;
			ForceTooltipChange(currentDefaultTooltip);
			ControllerManager.Instance.SetCurrentSelected(modeButtons[0]);
			return;
		}
		tabButtons[0].SelectButton(isSelected: false);
		tabButtons[1].SelectButton(isSelected: true);
		contentObjects[0].SetActive(value: false);
		contentObjects[1].SetActive(value: true);
		currentDefaultTooltip = 10;
		ForceTooltipChange(currentDefaultTooltip);
		if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
		{
			ControllerManager.Instance.SetCurrentSelected(chatTypeButtons[1]);
		}
		else
		{
			ControllerManager.Instance.SetCurrentSelected(chatTypeButtons[0]);
		}
	}

	public void SetLanguage(uint lang)
	{
		IGameOptions gameHostOptions = GameOptionsManager.Instance.GameHostOptions;
		gameHostOptions.SetUInt(UInt32OptionNames.Keywords, lang);
		GameOptionsManager.Instance.GameHostOptions = gameHostOptions;
	}

	private void SetCurrentServer()
	{
		IRegionInfo currentRegion = DestroyableSingleton<ServerManager>.Instance.CurrentRegion;
		UpdateServerText(DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(currentRegion.TranslateName, currentRegion.Name));
	}

	private void SetLangTooltip(bool isActive)
	{
		if (isActive)
		{
			SetTooltip(9);
		}
		else
		{
			SetTooltip(10);
		}
	}

	private void ForceTooltipChange(int index)
	{
		if (changeTooltipCo != null)
		{
			StopCoroutine(changeTooltipCo);
		}
		tooltip.text = DestroyableSingleton<TranslationController>.Instance.GetString(tooltipStrings[index]);
	}

	public void SetTooltip(int index)
	{
		// Controller mouse-out events also fire while scene changes disable this screen.
		if (!gameObject.activeInHierarchy) return;
		if (changeTooltipCo != null)
		{
			StopCoroutine(changeTooltipCo);
		}
		if (index == 0 || index == 10)
		{
			if (index == currentDefaultTooltip)
			{
				changeTooltipCo = StartCoroutine(SetTooltipCo(index));
			}
		}
		else
		{
			tooltip.text = DestroyableSingleton<TranslationController>.Instance.GetString(tooltipStrings[index]);
		}
	}

	private IEnumerator SetTooltipCo(int index)
	{
		yield return new WaitForSeconds(0.3f);
		tooltip.text = DestroyableSingleton<TranslationController>.Instance.GetString(tooltipStrings[index]);
	}

	private void SetMapTooltip(int index)
	{
		if (index < 0)
		{
			SetTooltip(0);
			return;
		}
		if (changeTooltipCo != null)
		{
			StopCoroutine(changeTooltipCo);
		}
		tooltip.text = DestroyableSingleton<TranslationController>.Instance.GetString(mapTooltips[index]);
	}

	public float GetCapacity()
	{
		return capacityOption.Value;
	}

	private void UpdateServerText(string text)
	{
		TextMeshPro[] array = serverTexts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = text;
		}
	}

	public void OpenServerDropdown()
	{
		serverDropdown.gameObject.SetActive(value: true);
		serverButton.SetActive(value: false);
	}

	public void CloseServerDropdown()
	{
		serverButton.SetActive(value: true);
	}

	private void ValueChanged(OptionBehaviour option)
	{
		GameOptionsManager.Instance.CurrentGameOptions.SetInt(Int32OptionNames.MaxPlayers, (int)capacityOption.Value);
		GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
		SetCrewmateGraphic(capacityOption.Value - 1f);
	}

	private void SetCrewmateGraphic(float capacity)
	{
		int num = capacity.ClampToInt(0, 15);
		for (int i = 0; i < currentCrewSprites.Length; i++)
		{
			if (num - 4 >= i)
			{
				currentCrewSprites[i].gameObject.SetActive(value: true);
			}
			else
			{
				currentCrewSprites[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void SetCrossPlay(bool crossplay)
	{
		if (crossplay)
		{
			crossPlayButtons[0].SelectButton(isSelected: false);
			crossPlayButtons[1].SelectButton(isSelected: true);
		}
		else
		{
			crossPlayButtons[0].SelectButton(isSelected: true);
			crossPlayButtons[1].SelectButton(isSelected: false);
		}
		DataManager.Settings.Multiplayer.CrossPlay = crossplay;
		DataManager.Settings.Save();
	}

	private void CheckChatType()
	{
		if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
		{
			SetChatType(isFreeChat: false);
			chatTypeButtons[0].SetButtonEnableState(enabled: false);
			chatWarningText.gameObject.SetActive(value: true);
			langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 0.14f, langPicker.transform.localPosition.z);
			chatDescText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QCOnlyInfo);
			return;
		}
		langPicker.transform.localPosition = new Vector3(langPicker.transform.localPosition.x, 0.84f, langPicker.transform.localPosition.z);
		chatWarningText.gameObject.SetActive(value: false);
		chatTypeButtons[0].SetButtonEnableState(enabled: true);
		if (DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.FreeChatOrQuickChat)
		{
			chatTypeButtons[0].SelectButton(isSelected: true);
			chatTypeButtons[1].SelectButton(isSelected: false);
			chatDescText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeChatInfo);
		}
		else
		{
			chatTypeButtons[0].SelectButton(isSelected: false);
			chatTypeButtons[1].SelectButton(isSelected: true);
			chatDescText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QCOnlyInfo);
		}
	}

	public void SetChatType(bool isFreeChat)
	{
		if (isFreeChat)
		{
			if (DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
			{
				chatTypeButtons[0].SelectButton(isSelected: true);
				chatTypeButtons[1].SelectButton(isSelected: false);
				DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
				DataManager.Settings.Save();
				chatDescText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeChatInfo);
			}
		}
		else
		{
			if (chatTypeButtons[0].enabled)
			{
				chatTypeButtons[0].SelectButton(isSelected: false);
			}
			chatTypeButtons[1].SelectButton(isSelected: true);
			DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
			DataManager.Settings.Save();
			chatDescText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QCOnlyInfo);
		}
	}

	public void SetAprilFool(bool isOn)
	{
		if (isOn)
		{
			aprilFoolButtons[0].SelectButton(isSelected: true);
			aprilFoolButtons[1].SelectButton(isSelected: false);
		}
		else
		{
			aprilFoolButtons[0].SelectButton(isSelected: false);
			aprilFoolButtons[1].SelectButton(isSelected: true);
		}
		AprilFoolsMode.IsAprilFoolsModeToggledOn = isOn;
	}

	public void SetModeButton(int i)
	{
		SelectMode(i);
	}

	public void SetTag(int i)
	{
		PassiveButton[] array = levelButtons;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SelectButton(isSelected: false);
		}
		levelButtons[i].SelectButton(isSelected: true);
		currentTag = i;
		GameOptionsManager.Instance.CurrentGameOptions.SetInt(Int32OptionNames.Tag, currentTag);
		GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
	}

	public string GetLevelString()
	{
		return currentTag switch
		{
			1 => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterBeginner), 
			2 => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterIntermediate), 
			3 => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterExpert), 
			_ => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterNone), 
		};
	}

	private void SelectMode(int i, bool saveSetting = true)
	{
		if (i == 0)
		{
			modeButtons[0].SelectButton(isSelected: true);
			modeButtons[1].SelectButton(isSelected: false);
			if (saveSetting)
			{
				SetGameMode(GameModes.Normal);
			}
			SwitchOptions(GameModes.Normal);
		}
		else
		{
			modeButtons[0].SelectButton(isSelected: false);
			modeButtons[1].SelectButton(isSelected: true);
			if (saveSetting)
			{
				SetGameMode(GameModes.HideNSeek);
			}
			SwitchOptions(GameModes.HideNSeek);
		}
	}

	private void SwitchOptions(GameModes gameMode)
	{
		byte mapId = GameOptionsManager.Instance.GameHostOptions.MapId;
		uint keywords = (uint)GameOptionsManager.Instance.GameHostOptions.Keywords;
		GameOptionsManager.Instance.SwitchGameMode(gameMode);
		GameOptionsManager.Instance.GameHostOptions.SetUInt(UInt32OptionNames.Keywords, keywords);
		GameOptionsManager.Instance.GameHostOptions.SetByte(ByteOptionNames.MapId, mapId);
		GameOptionsManager.Instance.GameHostOptions.SetInt(Int32OptionNames.MaxPlayers, (int)capacityOption.Value);
		GameOptionsManager.Instance.GameHostOptions.SetInt(Int32OptionNames.Tag, currentTag);
	}

	private void SetGameMode(GameModes mode)
	{
		DataManager.Settings.Multiplayer.LastPlayedGameMode = mode;
		DataManager.Settings.Save();
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void StartIcon()
	{
		loadAnim.SetActive(value: true);
	}

	public void StopIcon()
	{
		loadAnim.SetActive(value: false);
	}

	public void Close()
	{
		if (!animating)
		{
			StartCoroutine(CoHide());
		}
	}

	private IEnumerator CoHide()
	{
		animating = true;
		confirmPopUp.SetActive(value: false);
		foreground.gameObject.SetActive(value: true);
		foreground.color = Color.clear;
		yield return Effects.ColorFade(foreground, Color.clear, Color.black, 0.1f);
		content.SetActive(value: false);
		animating = false;
		mainMenuManager.GoBackCreateGame();
	}

	public void OpenConfirmPopup()
	{
		if (!animating)
		{
			confirmPopUp.SetActive(value: true);
			StartCoroutine(OpenConfirmPopupCo());
		}
	}

	public void CloseConfirmPopup()
	{
		if (!animating)
		{
			confirmPopUp.gameObject.SetActive(value: false);
		}
	}

	private IEnumerator OpenConfirmPopupCo()
	{
		animating = true;
		containerConfirm.localPosition = new Vector3(containerConfirm.localPosition.x, -14f, containerConfirm.localPosition.z);
		yield return Effects.Slide2D(source: new Vector2(containerConfirm.localPosition.x, -14f), dest: new Vector2(containerConfirm.localPosition.x, 0f), target: containerConfirm, duration: 0.3f);
		animating = false;
	}

	public void Confirm()
	{
		if (DestroyableSingleton<MatchMaker>.Instance.Connecting(this))
		{
			logger.Info("Hosting an online game");
			GameOptionsManager.Instance.GameHostOptions.TryGetInt(Int32OptionNames.MaxPlayers, out var value);
			GameOptionsManager.Instance.GameHostOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var value2);
			GameOptionsManager.Instance.GameHostOptions.TryGetInt(Int32OptionNames.NumImpostors, out var value3);
			if (value3 > value2[value])
			{
				GameOptionsManager.Instance.GameHostOptions.SetInt(Int32OptionNames.NumImpostors, value2[value]);
			}
			if (value3 == 0)
			{
				GameOptionsManager.Instance.GameHostOptions.SetInt(Int32OptionNames.NumImpostors, 1);
			}
			CoStartGame();
		}
	}

	private void CoStartGame()
	{
		AmongUsClient.Instance.CheckOnlinePermissions(ContinueStart, NotOnlinePermissions, StartIcon, checkOnline: false);
	}

	private void NotOnlinePermissions()
	{
	}

	private void ContinueStart()
	{
		StartCoroutine(ContinueCoStart());
	}

	private IEnumerator ContinueCoStart()
	{
		StopIcon();
		DataManager.Settings.Save();
		SoundManager.Instance.CrossFadeSound("MainBG", null, 0.5f);
		foreground.gameObject.SetActive(value: true);
		yield return Effects.ColorFade(foreground, Color.clear, Color.black, 0.2f);
		yield return AmongUsClient.Instance.CoCreateOnlineGame();
	}
}
