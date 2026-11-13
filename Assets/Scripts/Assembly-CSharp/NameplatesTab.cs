using System;
using System.Collections;
using AmongUs.Data;
using Innersloth.Assets;
using UnityEngine;

public class NameplatesTab : InventoryTab
{
	public PlayerVoteArea previewArea;

	private string plateId;

	private bool currentNameplateIsEquipped;

	private void Awake()
	{
		NumRowsVisible = 4;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		PlayerPreview.gameObject.SetActive(value: false);
		StartCoroutine(CoLoadNameplatePreview());
		NamePlateData[] unlockedNamePlates = DestroyableSingleton<HatManager>.Instance.GetUnlockedNamePlates();
		for (int i = 0; i < unlockedNamePlates.Length; i++)
		{
			NamePlateData plate = unlockedNamePlates[i];
			float x = XRange.Lerp((float)(i % NumPerRow) / ((float)NumPerRow - 1f));
			float y = YStart - (float)(i / NumPerRow) * YOffset;
			ColorChip chip = UnityEngine.Object.Instantiate(ColorTabPrefab, scroller.Inner);
			chip.transform.localPosition = new Vector3(x, y, -1f);
			chip.Button.ClickMask = scroller.Hitbox;
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
			{
				chip.Button.OnMouseOver.AddListener(() =>
				{
					SelectNameplate(plate);
				});
				chip.Button.OnMouseOut.AddListener(() =>
				{
					SelectNameplate(DestroyableSingleton<HatManager>.Instance.GetNamePlateById(DataManager.Player.Customization.NamePlate));
				});
				chip.Button.OnClick.AddListener(() =>
				{
					ClickEquip();
				});
			}
			else
			{
				chip.Button.OnClick.AddListener(() =>
				{
					SelectNameplate(plate);
				});
			}
			StartCoroutine(((MonoBehaviour)this).CoLoadAssetAsync((IAddressableAssetProvider<NamePlateViewData>)plate, (Action<NamePlateViewData>)((NamePlateViewData viewData) =>
			{
				(chip as NameplateChip).image.sprite = viewData?.Image;
			})));
			(chip as NameplateChip).ProductId = plate.ProdId;
			ColorChips.Add(chip);
		}
		if (unlockedNamePlates.Length != 0)
		{
			GetDefaultSelectable().PlayerEquippedForeground.SetActive(value: true);
		}
		plateId = DataManager.Player.Customization.NamePlate;
		currentNameplateIsEquipped = true;
		SetScrollerBounds();
	}

	private IEnumerator CoLoadNameplatePreview()
	{
		yield return new WaitForEndOfFrame();
		previewArea.PreviewNameplate(DataManager.Player.Customization.NamePlate);
	}

	public void Update()
	{
		NamePlateData namePlateById = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(DataManager.Player.Customization.NamePlate);
		NamePlateData namePlateById2 = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(plateId);
		currentNameplateIsEquipped = DataManager.Player.Customization.NamePlate == plateId;
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			colorChip.PlayerEquippedForeground.SetActive(namePlateById.ProdId == (colorChip as NameplateChip).ProductId);
			colorChip.SelectionHighlight.gameObject.SetActive(namePlateById2.ProdId == (colorChip as NameplateChip).ProductId);
		}
	}

	public override void ClickEquip()
	{
		if (DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(plateId, PlayerPreview.GetIgnoreLongMode()))
		{
			base.ClickEquip();
			DataManager.Player.Customization.NamePlate = plateId;
			if (HasLocalPlayer())
			{
				PlayerControl.LocalPlayer.RpcSetNamePlate(plateId);
			}
			Logger.GlobalInstance.Info("PlayerCustomizationMenu::Equipped nameplate " + plateId);
		}
	}

	private void SelectNameplate(NamePlateData plate)
	{
		plateId = plate.ProdId;
		currentNameplateIsEquipped = DataManager.Player.Customization.NamePlate == plate.ProdId;
		previewArea.PreviewNameplate(plateId);
		PlayerCustomizationMenu.Instance.SetItemName(plate.GetItemName());
	}

	public override ColorChip GetDefaultSelectable()
	{
		if (ColorChips.Count == 0)
		{
			return null;
		}
		NamePlateData namePlateById = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(DataManager.Player.Customization.NamePlate);
		PlayerCustomizationMenu.Instance.SetItemName(namePlateById.GetItemName());
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			if (namePlateById.ProdId == (colorChip as NameplateChip).ProductId)
			{
				return colorChip;
			}
		}
		return ColorChips[0];
	}

	public override bool IsSelectedItemEquipped()
	{
		return currentNameplateIsEquipped;
	}
}
