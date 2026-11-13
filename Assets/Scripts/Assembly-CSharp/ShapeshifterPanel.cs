using System;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class ShapeshifterPanel : MonoBehaviour
{
	public PoolablePlayer PlayerIcon;

	public SpriteRenderer Background;

	public TextMeshPro NameText;

	public TextMeshPro LevelNumberText;

	public PassiveButton Button;

	private Action shapeshift;

	public TextMeshPro ColorBlindName;

	public void SetPlayer(int index, NetworkedPlayerInfo playerInfo, Action onShift)
	{
		shapeshift = onShift;
		PlayerIcon.SetFlipX(flipped: false);
		PlayerIcon.ToggleName(active: false);
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, index + 2);
		}
		PlayerIcon.SetMaskLayer(index + 2);
		PlayerIcon.UpdateFromEitherPlayerDataOrCache(playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ComplexUI, includePet: false);
		LevelNumberText.text = ProgressionManager.FormatVisualLevel(playerInfo.PlayerLevel);
		Background.sprite = ShipStatus.Instance.CosmeticsCache.GetNameplate(playerInfo.DefaultOutfit.NamePlateId).Image;
		NameText.text = playerInfo.PlayerName;
		DataManager.Settings.Accessibility.OnColorBlindModeChanged += SetColorblindText;
		SetColorblindText();
	}

	public void ShapeShift()
	{
		shapeshift();
	}

	private void OnDisable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged -= SetColorblindText;
	}

	private void SetColorblindText()
	{
		ColorBlindName.enabled = DataManager.Settings.Accessibility.ColorBlindMode;
		ColorBlindName.text = PlayerIcon.ColorBlindName;
	}
}
