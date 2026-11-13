using AmongUs.Data;
using UnityEngine;

public class HatsTab : InventoryTab
{
	private HatData currentHat;

	private bool currentHatIsEquipped;

	public override void OnEnable()
	{
		base.OnEnable();
		HatData[] unlockedHats = DestroyableSingleton<HatManager>.Instance.GetUnlockedHats();
		currentHat = DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat);
		int num = 0;
		foreach (HatData hat in unlockedHats)
		{
			float x = XRange.Lerp((float)(num % NumPerRow) / ((float)NumPerRow - 1f));
			float y = YStart - (float)(num / NumPerRow) * YOffset;
			ColorChip colorChip = Object.Instantiate(ColorTabPrefab, scroller.Inner);
			colorChip.transform.localPosition = new Vector3(x, y, -1f);
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
			{
				colorChip.Button.OnMouseOver.AddListener(() =>
				{
					SelectHat(hat);
				});
				colorChip.Button.OnMouseOut.AddListener(() =>
				{
					SelectHat(DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat));
				});
				colorChip.Button.OnClick.AddListener(() =>
				{
					ClickEquip();
				});
			}
			else
			{
				colorChip.Button.OnClick.AddListener(() =>
				{
					SelectHat(hat);
				});
			}
			colorChip.Button.ClickMask = scroller.Hitbox;
			colorChip.Inner.SetMaskType(PlayerMaterial.MaskType.SimpleUI);
			UpdateMaterials(colorChip.Inner.FrontLayer, hat);
			hat.SetPreview(colorChip.Inner.FrontLayer, GetDisplayColor());
			colorChip.Tag = hat;
			colorChip.SelectionHighlight.gameObject.SetActive(value: false);
			ColorChips.Add(colorChip);
			num++;
			if (!DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(hat.ProdId, PlayerPreview.GetIgnoreLongMode()))
			{
				colorChip.SetUnavailable();
			}
		}
		currentHatIsEquipped = true;
		SetScrollerBounds();
	}

	public void Update()
	{
		HatData hatById = DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat);
		currentHatIsEquipped = DataManager.Player.Customization.Hat == currentHat.ProdId;
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			colorChip.PlayerEquippedForeground.SetActive(hatById == (HatData)colorChip.Tag);
			colorChip.SelectionHighlight.gameObject.SetActive(currentHat == (HatData)colorChip.Tag);
		}
	}

	private void SelectHat(HatData hat)
	{
		currentHat = hat;
		int displayColor = GetDisplayColor();
		PlayerPreview.SetHat(currentHat.ProdId, displayColor);
		PlayerCustomizationMenu.Instance.SetItemName(hat.GetItemName());
		if (hat.BlocksVisors)
		{
			PlayerPreview.SetVisor(string.Empty, 0);
		}
		else
		{
			PlayerPreview.SetVisor(HasLocalPlayer() ? PlayerControl.LocalPlayer.Data.DefaultOutfit.VisorId : DataManager.Player.Customization.Visor, displayColor);
		}
	}

	public override bool IsSelectedItemEquipped()
	{
		return currentHatIsEquipped;
	}

	public override string GetCurrentProdID()
	{
		if (currentHat != null)
		{
			return currentHat.ProdId;
		}
		return string.Empty;
	}

	public override void ClickEquip()
	{
		if (!DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(currentHat.ProdId, PlayerPreview.GetIgnoreLongMode()))
		{
			return;
		}
		base.ClickEquip();
		DataManager.Player.Customization.Hat = currentHat.ProdId;
		if (HasLocalPlayer())
		{
			PlayerControl.LocalPlayer.RpcSetHat(currentHat.ProdId);
		}
		if (currentHat.BlocksVisors)
		{
			DataManager.Player.Customization.Visor = "";
			if (HasLocalPlayer())
			{
				PlayerControl.LocalPlayer.RpcSetVisor("");
			}
		}
		Logger.GlobalInstance.Info("PlayerCustomizationMenu::Equipped hat " + currentHat.ProdId);
	}

	public override ColorChip GetDefaultSelectable()
	{
		HatData hatById = DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat);
		PlayerCustomizationMenu.Instance.SetItemName(hatById.GetItemName());
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			if (hatById == (HatData)colorChip.Tag)
			{
				return colorChip;
			}
		}
		return ColorChips[0];
	}
}
