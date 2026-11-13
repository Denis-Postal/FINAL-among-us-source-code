using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Player;
using UnityEngine;

public class InventoryTab : MonoBehaviour
{
	public ColorChip ColorTabPrefab;

	public PoolablePlayer PlayerPreview;

	public FloatRange XRange= new FloatRange(1.5f, 3f);

	public float YStart= 1.1f;

	public float YOffset= 0.8f;

	public int NumPerRow= 4;

	public Scroller scroller;

	protected int NumRowsVisible= 3;

	[HideInInspector]
	public List<ColorChip> ColorChips= new List<ColorChip>();

	public bool ignoreLongMode;

	protected bool HasLocalPlayer()
	{
		return PlayerControl.LocalPlayer != null;
	}

	public int GetDisplayColor()
	{
		if (!HasLocalPlayer())
		{
			return DataManager.Player.Customization.Color;
		}
		return PlayerControl.LocalPlayer.Data.DefaultOutfit.ColorId;
	}

	public int CurrentColorId()
	{
		return GetDisplayColor();
	}

	public virtual void ClickEquip()
	{
	}

	public virtual void OnEnable()
	{
		PlayerPreview.gameObject.SetActive(value: true);
		if (HasLocalPlayer())
		{
			PlayerPreview.UpdateFromLocalPlayer(PlayerMaterial.MaskType.None);
		}
		else
		{
			PlayerPreview.UpdateFromDataManager(PlayerMaterial.MaskType.None);
		}
	}

	public virtual void OnDisable()
	{
		for (int i = 0; i < ColorChips.Count; i++)
		{
			Object.Destroy(ColorChips[i].gameObject);
		}
		ColorChips.Clear();
	}

	public virtual ColorChip GetDefaultSelectable()
	{
		return ColorChips[0];
	}

	public virtual bool IsSelectedItemEquipped()
	{
		return false;
	}

	public virtual string GetCurrentProdID()
	{
		return string.Empty;
	}

	protected void UpdateMaterials(SpriteRenderer spriteRenderer, CosmeticData data)
	{
		if (!data.PreviewCrewmateColor)
		{
			spriteRenderer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
		}
		else
		{
			spriteRenderer.sharedMaterial = new Material(DestroyableSingleton<HatManager>.Instance.PlayerMaterial);
		}
	}

	protected void SetScrollerBounds()
	{
		scroller.CalculateAndSetYBounds(ColorChips.Count, NumPerRow, NumRowsVisible, YOffset);
	}

	public InventoryTab()
	{ }
}
