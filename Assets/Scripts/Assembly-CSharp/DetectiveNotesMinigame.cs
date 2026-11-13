using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.GameOptions;
using Rewired;
using TMPro;
using UnityEngine;

public class DetectiveNotesMinigame : Minigame
{
	[SerializeField]
	private AudioClip[] scribbleSFXs;

	[SerializeField]
	private Transform tabsParent;

	[SerializeField]
	private GameObject tabPrefab;

	[SerializeField]
	private TMP_Text victimName;

	[SerializeField]
	private TMP_Text victimColorBlindText;

	[SerializeField]
	private TMP_Text victimPreposition;

	[SerializeField]
	private TMP_Text victimLocation;

	[SerializeField]
	private DetectiveNotesSuspectContainer[] suspectContainers;

	[SerializeField]
	private GameObject InterrogationsParent;

	[SerializeField]
	private GameObject NoPagesParent;

	[SerializeField]
	private GameObject noSuspectsPostIt;

	[SerializeField]
	private SpriteRenderer bodyMaterial;

	[SerializeField]
	private PassiveButton prepositionPopup;

	[SerializeField]
	private PassiveButton impostorTypePopup;

	[SerializeField]
	private Transform impostorTypeParent;

	[SerializeField]
	private Sprite defaultBodySprite;

	[SerializeField]
	private TMP_Text impostorTypeText;

	[SerializeField]
	private GameObject impostorTypePrefab;

	[SerializeField]
	private GameObject mapFadeBackground;

	[SerializeField]
	private PassiveButton closeButton;

	[SerializeField]
	[Header("Console Controller Navigation")]
	private List<UiElement> ControllerSelectable;

	[SerializeField]
	private List<UiElement> PrepositionSelectable;

	private List<UiElement> ImpostorIconControllerSelectables = new List<UiElement>();

	[SerializeField]
	private SpriteRenderer glyphL;

	[SerializeField]
	private SpriteRenderer glyphR;

	private readonly string MainOverlay = "DetectiveNotes";

	private readonly string PrepositionOverlay = "PrepositionMenu";

	private readonly string ImpostorOverlay = "ImpostorMenu";

	private List<DetectiveNotesTab> tabs = new List<DetectiveNotesTab>();

	private int currentPageIndex;

	private float suspectsWidth = 4f;

	private DetectiveRole associatedDetective;

	private DeadBody inspectedBody;

	private List<GameObject> impostorButton;

	public Dictionary<Color, Material> bodyColorMaterials = new Dictionary<Color, Material>();

	private bool notesSetup;

	private void Update()
	{
		if (ReInput.players.GetPlayer(0).GetButtonDown(49))
		{
			Close();
		}
		if (!(ControllerManager.Instance.CurrentUiState.MenuName == MainOverlay))
		{
			return;
		}
		Player player = ReInput.players.GetPlayer(0);
		bool flag = false;
		int num = currentPageIndex;
		if (player.GetButtonDown(35))
		{
			num = currentPageIndex - 1;
			if (num < 0)
			{
				num = tabs.Count - 1;
			}
			flag = true;
		}
		if (player.GetButtonDown(34))
		{
			num = currentPageIndex + 1;
			if (num >= tabs.Count)
			{
				num = 0;
			}
			flag = true;
		}
		if (flag)
		{
			SetPage(num);
		}
	}

	public override void Begin(PlayerTask task = null)
	{
		base.Begin(task);
		CreateBodyMaterials();
		SetPage(currentPageIndex);
		ControllerManager.Instance.OpenOverlayMenu(MainOverlay, closeButton, ControllerSelectable[0], ControllerSelectable);
		bool flag = ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick && tabs.Count > 1;
		glyphL.enabled = flag;
		glyphR.enabled = flag;
		DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: false);
		DestroyableSingleton<HudManager>.Instance.ToggleMapButton(isActive: false);
	}

	public override void Close()
	{
		if (amClosing == CloseState.Closing)
		{
			return;
		}
		amClosing = CloseState.Closing;
		foreach (Material value in bodyColorMaterials.Values)
		{
			Object.Destroy(value);
		}
		bodyColorMaterials.Clear();
		ControllerManager.Instance.CloseOverlayMenu(MainOverlay);
		ControllerManager.Instance.CloseOverlayMenu(PrepositionOverlay);
		ControllerManager.Instance.CloseOverlayMenu(ImpostorOverlay);
		if ((bool)MapBehaviour.Instance)
		{
			MapBehaviour.Instance.Close();
		}
		if (!MeetingHud.Instance)
		{
			DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: true);
		}
		DestroyableSingleton<HudManager>.Instance.ToggleMapButton(isActive: true);
		if ((bool)CloseSound && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(CloseSound, loop: false);
		}
		if ((bool)PlayerControl.LocalPlayer)
		{
			PlayerControl.HideCursorTemporarily();
		}
		StartCoroutine(CoDestroySelf());
	}

	private void SetUpCurrentPage()
	{
		if (associatedDetective.notesPageInfos.Count == 0)
		{
			NoPagesParent.SetActive(value: true);
			InterrogationsParent.SetActive(value: false);
			return;
		}
		NoPagesParent.SetActive(value: false);
		InterrogationsParent.SetActive(value: true);
		DetectiveNotesSuspectContainer[] array = suspectContainers;
		foreach (DetectiveNotesSuspectContainer obj in array)
		{
			obj.ClearSuspect();
			obj.SetMaskLayer(30);
		}
		float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.DetectiveSuspectLimit);
		for (int j = 1; (float)j < 5f - roleFloat; j++)
		{
			suspectContainers[^j].Disable();
		}
		DetectiveNotesPageInfo detectiveNotesPageInfo = associatedDetective.notesPageInfos[currentPageIndex];
		victimName.text = detectiveNotesPageInfo.victimPlayer.PlayerName;
		victimColorBlindText.gameObject.SetActive(DataManager.Settings.Accessibility.ColorBlindMode);
		victimColorBlindText.text = detectiveNotesPageInfo.victimPlayer.GetPlayerColorString();
		bodyMaterial.material = bodyColorMaterials[detectiveNotesPageInfo.victimPlayer.Color];
		if (detectiveNotesPageInfo.prepositionSet)
		{
			victimPreposition.text = DestroyableSingleton<TranslationController>.Instance.GetString(detectiveNotesPageInfo.prepositionStringName);
		}
		else
		{
			victimPreposition.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNotesPromptPreposition);
		}
		if (detectiveNotesPageInfo.locationSet)
		{
			victimLocation.text = detectiveNotesPageInfo.locationName;
		}
		else
		{
			victimLocation.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNotesPromptLocation);
		}
		if (detectiveNotesPageInfo.impostorSet)
		{
			impostorTypeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNotesKiller, detectiveNotesPageInfo.impostorString);
		}
		else
		{
			impostorTypeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNotesPromptImpostor);
		}
		int count = detectiveNotesPageInfo.suspects.Count;
		if (count > 0)
		{
			noSuspectsPostIt.SetActive(value: false);
		}
		else
		{
			noSuspectsPostIt.SetActive(value: true);
		}
		for (int k = 0; k < count && k < suspectContainers.Length; k++)
		{
			DetectiveSuspect detectiveSuspect = detectiveNotesPageInfo.suspects[k];
			suspectContainers[k].SetSuspect(detectiveSuspect, detectiveNotesPageInfo.victimPlayer, associatedDetective.PlayerWasDead(detectiveSuspect), k + 1);
		}
		tabs[currentPageIndex].SetHighlighted(this, highlighted: true);
		notesSetup = true;
	}

	public void OpenVictimLocationMap()
	{
		if (!MapBehaviour.Instance.IsOpen && amClosing != CloseState.Closing)
		{
			DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(new MapOptions
			{
				Mode = MapOptions.Modes.Detective
			});
			mapFadeBackground.SetActive(value: true);
			ControllerManager.Instance.SetBackButton(mapFadeBackground.GetComponent<PassiveButton>());
		}
	}

	public void CloseVictimLocationMap()
	{
		MapBehaviour.Instance.Close();
	}

	public void SetVictimLocation(string _locationName)
	{
		associatedDetective.notesPageInfos[currentPageIndex].SetLocation(_locationName);
		victimLocation.text = _locationName;
		mapFadeBackground.SetActive(value: false);
	}

	public void SetPrepositionPopup(bool active)
	{
		prepositionPopup.gameObject.SetActive(active);
		if (active)
		{
			ControllerManager.Instance.OpenOverlayMenu(PrepositionOverlay, prepositionPopup, PrepositionSelectable[0], PrepositionSelectable);
			return;
		}
		ControllerManager.Instance.CloseOverlayMenu(PrepositionOverlay);
		ControllerManager.Instance.SetUpSelectables(ControllerManager.Instance.CurrentUiState, ControllerSelectable[0], ControllerSelectable);
		ControllerManager.Instance.SetCurrentSelected(ControllerSelectable[0]);
	}

	public void SetImpostorPopup(bool active)
	{
		impostorTypePopup.gameObject.SetActive(active);
		if (active)
		{
			if (impostorButton != null)
			{
				ControllerManager.Instance.OpenOverlayMenu(ImpostorOverlay, impostorTypePopup, ImpostorIconControllerSelectables[0], ImpostorIconControllerSelectables);
				return;
			}
			impostorButton = new List<GameObject>();
			int num = 0;
			foreach (RoleBehaviour item in DestroyableSingleton<RoleManager>.Instance.AllRoles.FindAll((RoleBehaviour r) => r.TeamType == RoleTeamTypes.Impostor && r.Role != RoleTypes.ImpostorGhost))
			{
				GameObject gameObject = Object.Instantiate(impostorTypePrefab, impostorTypeParent);
				impostorButton.Add(gameObject);
				gameObject.GetComponent<DetectiveImpostorType>().Initialize(this, item);
				num++;
				ImpostorIconControllerSelectables.Add(gameObject.GetComponent<PassiveButton>());
			}
			ControllerManager.Instance.OpenOverlayMenu(ImpostorOverlay, impostorTypePopup, ImpostorIconControllerSelectables[0], ImpostorIconControllerSelectables);
		}
		else
		{
			ControllerManager.Instance.CloseOverlayMenu(ImpostorOverlay);
			ControllerManager.Instance.SetUpSelectables(ControllerManager.Instance.CurrentUiState, ControllerSelectable[0], ControllerSelectable);
			ControllerManager.Instance.SetCurrentSelected(ControllerSelectable[2]);
		}
	}

	public void SetImpostorType(string impostorString)
	{
		SoundManager.Instance.PlaySound(scribbleSFXs.Random(), loop: false);
		impostorTypeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DetectiveNotesKiller, impostorString);
		associatedDetective.notesPageInfos[currentPageIndex].SetImpostorType(impostorString);
		SetImpostorPopup(active: false);
	}

	public void SetVictimPreposition(string _preposition)
	{
		SoundManager.Instance.PlaySound(scribbleSFXs.Random(), loop: false);
		StringNames stringNames = StringNames.None;
		switch (_preposition)
		{
		case "in":
			stringNames = StringNames.DetectiveNotesPrepositionIn;
			break;
		case "by":
			stringNames = StringNames.DetectiveNotesPrepositionBy;
			break;
		case "maybe":
			stringNames = StringNames.DetectiveNotesPrepositionMaybe;
			break;
		}
		associatedDetective.notesPageInfos[currentPageIndex].SetPreposition(stringNames);
		victimPreposition.text = DestroyableSingleton<TranslationController>.Instance.GetString(stringNames);
		SetPrepositionPopup(active: false);
	}

	public void ChangePage(int dir)
	{
		int page = Mathf.Clamp(currentPageIndex + dir, 0, associatedDetective.notesPageInfos.Count - 1);
		SetPage(page);
	}

	public void SetPage(int index)
	{
		if (!notesSetup || index != currentPageIndex)
		{
			currentPageIndex = index;
			associatedDetective.SetCurrentIndex(currentPageIndex);
			SetUpCurrentPage();
		}
	}

	public void GenerateNewPage(DetectiveRole detective, NetworkedPlayerInfo victimPlayer)
	{
		SetAssociatedDetective(detective);
		DetectiveNotesPageInfo item = new DetectiveNotesPageInfo(victimPlayer);
		associatedDetective.notesPageInfos.Add(item);
		currentPageIndex = associatedDetective.notesPageInfos.Count - 1;
		associatedDetective.SetCurrentIndex(currentPageIndex);
		SetupTabs();
	}

	public void OpenExistingPage(DetectiveRole detective, DetectiveNotesPageInfo pageInfo)
	{
		SetAssociatedDetective(detective);
		currentPageIndex = ((associatedDetective.notesPageInfos.Count > 0) ? associatedDetective.notesPageInfos.IndexOf(pageInfo) : 0);
		associatedDetective.SetCurrentIndex(currentPageIndex);
		SetupTabs();
	}

	private void SetupTabs()
	{
		if (associatedDetective.notesPageInfos.Count > 6)
		{
			tabsParent.transform.localPosition = new Vector3(-4.972f, tabsParent.localPosition.y, tabsParent.localPosition.z);
		}
		for (int i = 0; i < associatedDetective.notesPageInfos.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(tabPrefab, tabsParent);
			gameObject.GetComponent<DetectiveNotesTab>().Setup(this, i, associatedDetective.notesPageInfos[i].victimPlayer, associatedDetective.notesPageInfos.Count);
			tabs.Add(gameObject.GetComponent<DetectiveNotesTab>());
		}
		Transform transform = glyphR.transform;
		List<DetectiveNotesTab> list = tabs;
		transform.position = new Vector3((list[list.Count - 1].transform.position + Vector3.right * 0.8f).x, glyphR.transform.position.y, glyphR.transform.position.z);
	}

	public void SetAssociatedDetective(DetectiveRole detective)
	{
		associatedDetective = detective;
	}

	public void ClearTabHighlights()
	{
		foreach (DetectiveNotesTab tab in tabs)
		{
			tab.SetHighlighted(this, highlighted: false);
		}
	}

	public void MapClosed()
	{
		mapFadeBackground.SetActive(value: false);
		ControllerManager.Instance.SetDefaultSelection(ControllerSelectable[1], ControllerSelectable);
		ControllerManager.Instance.SetBackButton(closeButton);
		DestroyableSingleton<HudManager>.Instance.SetHudActive(isActive: false);
	}

	private void CreateBodyMaterials()
	{
		Color32[] playerColors = Palette.PlayerColors;
		foreach (Color color in playerColors)
		{
			Material material = new Material(bodyMaterial.material);
			PlayerMaterial.SetColors(color, material);
			PlayerMaterial.SetMaskLayer(material, 20);
			bodyColorMaterials.Add(color, material);
		}
	}
}
