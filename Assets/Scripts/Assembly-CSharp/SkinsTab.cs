using AmongUs.Data;
using UnityEngine;

public class SkinsTab : InventoryTab
{
	private string skinId;

	private bool currentSkinIsEquipped;

	public override void OnEnable()
	{
		base.OnEnable();
		SkinData[] unlockedSkins = DestroyableSingleton<HatManager>.Instance.GetUnlockedSkins();
		int num = 0;
		foreach (SkinData skin in unlockedSkins)
		{
			float x = XRange.Lerp((float)(num % NumPerRow) / ((float)NumPerRow - 1f));
			float y = YStart - (float)(num / NumPerRow) * YOffset;
			ColorChip colorChip = Object.Instantiate(ColorTabPrefab, scroller.Inner);
			colorChip.transform.localPosition = new Vector3(x, y, -1f);
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
			{
				colorChip.Button.OnMouseOver.AddListener(() =>
				{
					SelectSkin(skin);
				});
				colorChip.Button.OnMouseOut.AddListener(() =>
				{
					SelectSkin(DestroyableSingleton<HatManager>.Instance.GetSkinById(DataManager.Player.Customization.Skin));
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
					SelectSkin(skin);
				});
			}
			colorChip.Button.ClickMask = scroller.Hitbox;
			colorChip.ProductId = skin.ProductId;
			colorChip.SelectionHighlight.gameObject.SetActive(value: false);
			UpdateMaterials(colorChip.Inner.FrontLayer, skin);
			skin.SetPreview(colorChip.Inner.FrontLayer, GetDisplayColor());
			ColorChips.Add(colorChip);
			num++;
			if (!DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(skin.ProdId, PlayerPreview.GetIgnoreLongMode()) || AprilFoolsMode.ShouldClassicMode())
			{
				colorChip.SetUnavailable();
			}
		}
		skinId = DataManager.Player.Customization.Skin;
		currentSkinIsEquipped = true;
		SetScrollerBounds();
	}

	public void Update()
	{
		SkinData skinById = DestroyableSingleton<HatManager>.Instance.GetSkinById(DataManager.Player.Customization.Skin);
		SkinData skinById2 = DestroyableSingleton<HatManager>.Instance.GetSkinById(skinId);
		currentSkinIsEquipped = DataManager.Player.Customization.Skin == skinId;
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			colorChip.PlayerEquippedForeground.SetActive(skinById.ProductId == colorChip.ProductId);
			colorChip.SelectionHighlight.gameObject.SetActive(skinById2.ProductId == colorChip.ProductId);
		}
	}

	public override void ClickEquip()
	{
		if (DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(skinId, PlayerPreview.GetIgnoreLongMode()))
		{
			base.ClickEquip();
			DataManager.Player.Customization.Skin = skinId;
			if (HasLocalPlayer())
			{
				PlayerControl.LocalPlayer.RpcSetSkin(skinId);
			}
			Logger.GlobalInstance.Info("PlayerCustomizationMenu::Equipped skin " + skinId);
		}
	}

	private void SelectSkin(SkinData skin)
	{
		skinId = skin.ProdId;
		PlayerPreview.SetSkin(skinId, GetDisplayColor());
		PlayerCustomizationMenu.Instance.SetItemName(skin.GetItemName());
	}

	public override ColorChip GetDefaultSelectable()
	{
		SkinData skinById = DestroyableSingleton<HatManager>.Instance.GetSkinById(DataManager.Player.Customization.Skin);
		PlayerCustomizationMenu.Instance.SetItemName(skinById.GetItemName());
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			if (skinById.ProductId == colorChip.ProductId)
			{
				return colorChip;
			}
		}
		return ColorChips[0];
	}

	public override bool IsSelectedItemEquipped()
	{
		return currentSkinIsEquipped;
	}

	public override string GetCurrentProdID()
	{
		return skinId;
	}
}
