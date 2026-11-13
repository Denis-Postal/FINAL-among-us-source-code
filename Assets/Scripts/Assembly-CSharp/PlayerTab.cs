using System.Collections.Generic;
using AmongUs.Data;
using UnityEngine;

public class PlayerTab : InventoryTab
{
	private const int Columns = 4;

	public Transform ColorTabArea;

	private HashSet<int> AvailableColors = new HashSet<int>();

	private bool currentColorIsEquipped;

	private int currentColor;

	public override void OnEnable()
	{
		base.OnEnable();
		_ = (float)Palette.PlayerColors.Length / 4f;
		for (int i = 0; i < Palette.PlayerColors.Length; i++)
		{
			float x = XRange.Lerp((float)(i % 4) / 3f);
			float y = YStart - (float)(i / 4) * YOffset;
			ColorChip colorChip = Object.Instantiate(ColorTabPrefab);
			colorChip.transform.SetParent(ColorTabArea);
			colorChip.transform.localPosition = new Vector3(x, y, -1f);
			int j = i;
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard)
			{
				colorChip.Button.OnMouseOver.AddListener(() =>
				{
					SelectColor(j);
				});
				colorChip.Button.OnMouseOut.AddListener(() =>
				{
					SelectColor(GetDisplayColor());
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
					SelectColor(j);
				});
			}
			colorChip.Inner.SpriteColor = Palette.PlayerColors[i];
			colorChip.Tag = j;
			ColorChips.Add(colorChip);
		}
		currentColor = DataManager.Player.Customization.Color;
	}

	public void Update()
	{
		UpdateAvailableColors();
		for (int i = 0; i < ColorChips.Count; i++)
		{
			ColorChip colorChip = ColorChips[i];
			colorChip.InUseForeground.SetActive(!AvailableColors.Contains(i));
			colorChip.SelectionHighlight.enabled = currentColor == i;
			colorChip.PlayerEquippedForeground.SetActive(GetDisplayColor() == i);
		}
		currentColorIsEquipped = GetDisplayColor() == currentColor;
	}

	private void SelectColor(int colorId)
	{
		UpdateAvailableColors();
		string colorName = Palette.GetColorName(colorId);
		PlayerCustomizationMenu.Instance.SetItemName(colorName);
		currentColor = colorId;
		PlayerPreview.SetBodyColor(colorId);
		PlayerPreview.SetPetColor(colorId);
		PlayerPreview.SetSkin(DataManager.Player.Customization.Skin, colorId);
		PlayerPreview.SetHat(DataManager.Player.Customization.Hat, colorId);
		PlayerPreview.SetVisor(DataManager.Player.Customization.Visor, colorId);
	}

	public override ColorChip GetDefaultSelectable()
	{
		int displayColor = GetDisplayColor();
		PlayerCustomizationMenu.Instance.SetItemName(Palette.GetColorName(displayColor));
		return ColorChips[displayColor];
	}

	public override void ClickEquip()
	{
		if (AvailableColors.Remove(currentColor))
		{
			DataManager.Player.Customization.Color = (byte)currentColor;
			DataManager.Player.Save();
			if (HasLocalPlayer())
			{
				PlayerControl.LocalPlayer.CmdCheckColor((byte)currentColor);
			}
			string colorName = Palette.GetColorName(currentColor);
			Logger.GlobalInstance.Info("PlayerCustomizationMenu::Set player color " + colorName);
		}
	}

	public override bool IsSelectedItemEquipped()
	{
		return currentColorIsEquipped;
	}

	private void UpdateAvailableColors()
	{
		for (int i = 0; i < Palette.PlayerColors.Length; i++)
		{
			AvailableColors.Add(i);
		}
		if ((bool)GameData.Instance)
		{
			List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
			for (int j = 0; j < allPlayers.Count; j++)
			{
				NetworkedPlayerInfo networkedPlayerInfo = allPlayers[j];
				AvailableColors.Remove(networkedPlayerInfo.DefaultOutfit.ColorId);
			}
		}
	}
}
