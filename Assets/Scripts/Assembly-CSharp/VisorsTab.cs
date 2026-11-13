using AmongUs.Data;
using UnityEngine;

public class VisorsTab : InventoryTab
{
	private string visorId;

	private bool currentVisorIsEquipped;

	public override void OnEnable()
	{
		base.OnEnable();
		VisorData[] unlockedVisors = DestroyableSingleton<HatManager>.Instance.GetUnlockedVisors();
		int num = 0;
		foreach (VisorData visor in unlockedVisors)
		{
			float x = XRange.Lerp((float)(num % NumPerRow) / ((float)NumPerRow - 1f));
			float y = YStart - (float)(num / NumPerRow) * YOffset;
			ColorChip colorChip = Object.Instantiate(ColorTabPrefab, scroller.Inner);
			colorChip.transform.localPosition = new Vector3(x, y, -1f);
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
			{
				colorChip.Button.OnMouseOver.AddListener(() =>
				{
					SelectVisor(visor);
				});
				colorChip.Button.OnMouseOut.AddListener(() =>
				{
					SelectVisor(DestroyableSingleton<HatManager>.Instance.GetVisorById(DataManager.Player.Customization.Visor));
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
					SelectVisor(visor);
				});
			}
			colorChip.Button.ClickMask = scroller.Hitbox;
			colorChip.ProductId = visor.ProductId;
			UpdateMaterials(colorChip.Inner.FrontLayer, visor);
			visor.SetPreview(colorChip.Inner.FrontLayer, GetDisplayColor());
			colorChip.Tag = visor.ProdId;
			colorChip.SelectionHighlight.gameObject.SetActive(value: false);
			ColorChips.Add(colorChip);
			num++;
			if (!DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(visor.ProdId, PlayerPreview.GetIgnoreLongMode()))
			{
				colorChip.SetUnavailable();
			}
		}
		if (unlockedVisors.Length != 0)
		{
			GetDefaultSelectable().PlayerEquippedForeground.SetActive(value: true);
		}
		visorId = DataManager.Player.Customization.Visor;
		currentVisorIsEquipped = true;
		SetScrollerBounds();
	}

	public void Update()
	{
		VisorData visorById = DestroyableSingleton<HatManager>.Instance.GetVisorById(DataManager.Player.Customization.Visor);
		VisorData visorById2 = DestroyableSingleton<HatManager>.Instance.GetVisorById(visorId);
		currentVisorIsEquipped = DataManager.Player.Customization.Visor == visorId;
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			colorChip.PlayerEquippedForeground.SetActive(visorById.ProductId == colorChip.ProductId);
			colorChip.SelectionHighlight.gameObject.SetActive(visorById2.ProductId == colorChip.ProductId);
		}
	}

	public override void ClickEquip()
	{
		if (!DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(visorId, PlayerPreview.GetIgnoreLongMode()))
		{
			return;
		}
		base.ClickEquip();
		DataManager.Player.Customization.Visor = visorId;
		if (HasLocalPlayer())
		{
			PlayerControl.LocalPlayer.RpcSetVisor(visorId);
		}
		if (DestroyableSingleton<HatManager>.Instance.GetHatById(HasLocalPlayer() ? PlayerControl.LocalPlayer.Data.DefaultOutfit.HatId : DataManager.Player.Customization.Hat).BlocksVisors)
		{
			DataManager.Player.Customization.Hat = "";
			if (HasLocalPlayer())
			{
				PlayerControl.LocalPlayer.RpcSetHat("");
			}
		}
		Logger.GlobalInstance.Info("PlayerCustomizationMenu::Equipped visor " + visorId);
	}

	public override ColorChip GetDefaultSelectable()
	{
		if (ColorChips.Count == 0)
		{
			return null;
		}
		VisorData visorById = DestroyableSingleton<HatManager>.Instance.GetVisorById(DataManager.Player.Customization.Visor);
		PlayerCustomizationMenu.Instance.SetItemName(visorById.GetItemName());
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			if (visorById.ProductId == colorChip.ProductId)
			{
				return colorChip;
			}
		}
		return ColorChips[0];
	}

	public override bool IsSelectedItemEquipped()
	{
		return currentVisorIsEquipped;
	}

	public override string GetCurrentProdID()
	{
		return visorId;
	}

	private void SelectVisor(VisorData visor)
	{
		int displayColor = GetDisplayColor();
		visorId = visor.ProdId;
		PlayerPreview.SetVisor(visor.ProdId, displayColor);
		PlayerCustomizationMenu.Instance.SetItemName(visor.GetItemName());
		HatData hatById = DestroyableSingleton<HatManager>.Instance.GetHatById(HasLocalPlayer() ? PlayerControl.LocalPlayer.Data.DefaultOutfit.HatId : DataManager.Player.Customization.Hat);
		if (hatById.BlocksVisors && !visor.IsEmpty)
		{
			PlayerPreview.SetHat("", displayColor);
		}
		else
		{
			PlayerPreview.SetHat(hatById.ProdId, displayColor);
		}
	}
}
