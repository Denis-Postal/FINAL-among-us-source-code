using System;
using System.Collections.Generic;
using AmongUs.Data;
using Rewired;
using TMPro;
using UnityEngine;

public class PlayerCustomizationMenu : MonoBehaviour
{
	public static PlayerCustomizationMenu Instance;

	public GameObject cosmicubeMenuTab;

	public CubesTab cubesTab;

	public TabButton[] Tabs;

	private int selectedTab;

	private bool cubeTabSelected;

	[SerializeField]
	private AudioClip swapTabSound;

	public PoolablePlayer PreviewArea;

	public GameObject equipButton;

	public GameObject equippedText;

	public GameObject nameplateMaskArea;

	public GameObject cubeArea;

	public GameObject cosmicubeMenu;

	public TextMeshPro itemName;

	public Action OnClose;

	public Action OnOpen;

	public GameObject AprilFoolText;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public GameObject glyphL;

	public GameObject glyphR;

	private readonly Logger logger = new Logger(Logger.Category.UI, "PlayerCustomizationMenu");

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		DataManager.Player.Save();
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.PlayerCam.OverrideScreenShakeEnabled = true;
		}
	}

	public void Start()
	{
		if ((bool)Instance && Instance != this)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
		OpenTab(Tabs[0].Tab);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.PlayerCam.OverrideScreenShakeEnabled = false;
		}
		if (DestroyableSingleton<GameStartManager>.InstanceExists)
		{
			DestroyableSingleton<GameStartManager>.Instance.CloseGameOptionsMenus();
		}
	}

	public void OpenTab(InventoryTab tab)
	{
		cosmicubeMenuTab.SetActive(value: false);
		TabButton tabButton = Tabs[0];
		for (int i = 0; i < Tabs.Length; i++)
		{
			tabButton = Tabs[i];
			if (tabButton.Tab == tab)
			{
				selectedTab = i;
				tabButton.Tab.gameObject.SetActive(value: true);
				tabButton.Button.enabled = true;
				if ((bool)tabButton.tabText)
				{
					tabButton.tabText.outlineColor = Palette.InventorySelectedTextColor;
				}
				List<ColorChip> list = new List<ColorChip>();
				ColorChip colorChip = null;
				PreviewArea.gameObject.SetActive(value: false);
				cubeArea.SetActive(value: false);
				cubeTabSelected = false;
				PlayerTab component = tabButton.Tab.GetComponent<PlayerTab>();
				if (component != null)
				{
					PreviewArea.gameObject.SetActive(value: true);
					list = component.ColorChips;
					colorChip = component.GetDefaultSelectable();
					HandleSelection(list, colorChip);
					nameplateMaskArea.gameObject.SetActive(value: false);
				}
				HatsTab component2 = tabButton.Tab.GetComponent<HatsTab>();
				if (component2 != null)
				{
					PreviewArea.gameObject.SetActive(value: true);
					list = component2.ColorChips;
					colorChip = component2.GetDefaultSelectable();
					HandleSelection(list, colorChip);
					nameplateMaskArea.gameObject.SetActive(value: false);
				}
				PetsTab component3 = tabButton.Tab.GetComponent<PetsTab>();
				if (component3 != null)
				{
					PreviewArea.gameObject.SetActive(value: true);
					list = component3.ColorChips;
					colorChip = component3.GetDefaultSelectable();
					HandleSelection(list, colorChip);
					nameplateMaskArea.gameObject.SetActive(value: false);
				}
				SkinsTab component4 = tabButton.Tab.GetComponent<SkinsTab>();
				if (component4 != null)
				{
					PreviewArea.gameObject.SetActive(value: true);
					list = component4.ColorChips;
					colorChip = component4.GetDefaultSelectable();
					HandleSelection(list, colorChip);
					nameplateMaskArea.gameObject.SetActive(value: false);
				}
				VisorsTab component5 = tabButton.Tab.GetComponent<VisorsTab>();
				if (component5 != null)
				{
					PreviewArea.gameObject.SetActive(value: true);
					list = component5.ColorChips;
					colorChip = component5.GetDefaultSelectable();
					HandleSelection(list, colorChip);
					nameplateMaskArea.gameObject.SetActive(value: false);
				}
				CubesTab component6 = tabButton.Tab.GetComponent<CubesTab>();
				if (component6 != null)
				{
					PreviewArea.gameObject.SetActive(value: false);
					list = component6.ColorChips;
					colorChip = component6.GetDefaultSelectable();
					HandleSelection(list, colorChip);
					cubeTabSelected = true;
					equipButton.SetActive(value: false);
					cubeArea.SetActive(value: true);
					nameplateMaskArea.gameObject.SetActive(value: false);
				}
				NameplatesTab component7 = tabButton.Tab.GetComponent<NameplatesTab>();
				if (component7 != null)
				{
					PreviewArea.gameObject.SetActive(value: false);
					list = component7.ColorChips;
					colorChip = component7.GetDefaultSelectable();
					HandleSelection(list, colorChip);
					nameplateMaskArea.gameObject.SetActive(value: true);
				}
				itemName.gameObject.SetActive(!cubeTabSelected);
				logger.Info("Opened tab " + tabButton.Tab.name);
			}
			else
			{
				tabButton.Tab.gameObject.SetActive(value: false);
				tabButton.Button.enabled = false;
				if ((bool)tabButton.tabText)
				{
					tabButton.tabText.outlineColor = Palette.InventoryTextColor;
				}
			}
		}
		bool flag = ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick;
		glyphL.SetActive(flag && selectedTab != 0);
		glyphR.SetActive(flag && selectedTab < Tabs.Length - 1);
		ControllerManager.Instance.ClearDestroyedSelectableUiElements();
		PreviewArea.UpdateFromDataManager(PlayerMaterial.MaskType.None, tabButton.Tab.CurrentColorId());
		PreviewArea.ToggleName(active: false);
	}

	private void CloseAllTabs()
	{
		TabButton[] tabs = Tabs;
		for (int i = 0; i < tabs.Length; i++)
		{
			tabs[i].Tab.gameObject.SetActive(value: false);
		}
	}

	private void HandleSelection(List<ColorChip> buttons, ColorChip selectedChip)
	{
		foreach (ColorChip button in buttons)
		{
			ControllerManager.Instance.AddSelectableUiElement(button.Button);
		}
		if ((bool)selectedChip)
		{
			ControllerManager.Instance.SetCurrentSelected(selectedChip.Button);
			selectedChip.Button.ReceiveMouseOver();
		}
		else if (buttons.Count > 0)
		{
			ControllerManager.Instance.SetCurrentSelected(buttons[0].Button);
		}
		else
		{
			ControllerManager.Instance.PickTopSelectable();
		}
	}

	public void Open()
	{
		DestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, gameObject, () =>
		{
			gameObject.SetActive(value: true);
			OnOpen?.Invoke();
		});
	}

	public void Close(bool canMove)
	{
		OnClose?.Invoke();
		logger.Info("Closing");
		CloseAllTabs();
		DestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(gameObject, null, DestroyObj);
	}

	private void DestroyObj()
	{
		OnClose?.Invoke();
		OnClose = null;
		UnityEngine.Object.Destroy(gameObject);
	}

	private void Update()
	{
		if ((bool)ShipStatus.Instance)
		{
			DestroyObj();
		}
		else if (!(ControllerManager.Instance.CurrentUiState.MenuName != name))
		{
			Player player = ReInput.players.GetPlayer(0);
			if (selectedTab > 0 && player.GetButtonDown(35))
			{
				selectedTab--;
				SoundManager.Instance.PlaySound(swapTabSound, loop: false);
				OpenTab(Tabs[selectedTab].Tab);
			}
			if (selectedTab < Tabs.Length - 1 && player.GetButtonDown(34))
			{
				selectedTab++;
				SoundManager.Instance.PlaySound(swapTabSound, loop: false);
				OpenTab(Tabs[selectedTab].Tab);
			}
			bool flag = !Tabs[selectedTab].Tab.IsSelectedItemEquipped();
			equipButton.SetActive(flag && !cubeTabSelected);
			equippedText.SetActive(!flag && !cubeTabSelected);
			AprilFoolText.SetActive(value: false);
			string currentProdID = Tabs[selectedTab].Tab.GetCurrentProdID();
			if (!string.IsNullOrEmpty(currentProdID) && (!DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(currentProdID, PreviewArea.GetIgnoreLongMode()) || (AprilFoolsMode.ShouldClassicMode() && selectedTab == 2)))
			{
				equipButton.SetActive(value: false);
				equippedText.SetActive(value: false);
				AprilFoolText.SetActive(value: true);
			}
			if ((flag || cubeTabSelected) && player.GetButtonDown(11))
			{
				ClickEquip();
			}
			if (player.GetButtonDown(29) && cubeTabSelected)
			{
				ViewCube();
			}
		}
	}

	public void ViewCube()
	{
		cosmicubeMenuTab.gameObject.SetActive(value: true);
		cubesTab.ViewCube();
	}

	public void SetItemName(string text)
	{
		itemName.text = text;
	}

	public void ClickEquip()
	{
		equipButton.SetActive(value: false);
		Tabs[selectedTab].Tab.GetComponent<InventoryTab>().ClickEquip();
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}
}
