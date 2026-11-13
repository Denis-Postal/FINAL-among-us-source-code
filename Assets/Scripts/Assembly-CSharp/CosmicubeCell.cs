using System.Collections;
using AmongUs.Data;
using Innersloth.Assets;
using PowerTools;
using TMPro;
using UnityEngine;

public class CosmicubeCell : MonoBehaviour
{
	private const float cellSpacing = 0.74f;

	private const float cellUnlockGlowDuration = 0.9f;

	public CosmicubeCell parent;

	public CosmicubeCell northChild;

	public CosmicubeCell southChild;

	public CosmicubeCell eastChild;

	public CosmicubeCell westChild;

	public SpriteRenderer icon;

	public TextMeshPro costText;

	public SpriteRenderer cellSprite;

	public SpriteRenderer qualitySprite;

	public SpriteRenderer glowSprite;

	public SpriteRenderer MaskArea;

	public SpriteRenderer SelectionHighlight;

	public GameObject data;

	public bool unlocked;

	public SpriteAnim Animator;

	public AnimationClip PurchaseAnimation;

	public UnlockableCosmetic cosmetic;

	[SerializeField]
	private Material iconMaterial;

	[SerializeField]
	private Material iconColoredMaterial;

	[SerializeField]
	private Material maskMaterial;

	private CosmicubeMenu menu;

	public bool HasEastCell
	{
		get
		{
			if (!(eastChild != null))
			{
				if (parent != null)
				{
					return parent.westChild == this;
				}
				return false;
			}
			return true;
		}
	}

	public bool HasWestCell
	{
		get
		{
			if (!(westChild != null))
			{
				if (parent != null)
				{
					return parent.eastChild == this;
				}
				return false;
			}
			return true;
		}
	}

	public bool HasNorthCell
	{
		get
		{
			if (!(northChild != null))
			{
				if (parent != null)
				{
					return parent.southChild == this;
				}
				return false;
			}
			return true;
		}
	}

	public bool HasSouthCell
	{
		get
		{
			if (!(southChild != null))
			{
				if (parent != null)
				{
					return parent.northChild == this;
				}
				return false;
			}
			return true;
		}
	}

	public UnlockableCosmetic CosmeticData => cosmetic;

	private void Start()
	{
		if (cosmetic != null)
		{
			SetData(cosmetic);
		}
		if (CosmeticData == null || DataManager.Player.Purchases.GetPurchase(CosmeticData.GetProdId(), CosmeticData.GetBundleId()))
		{
			SetCellPurchased();
		}
	}

	private void Update()
	{
		if ((bool)parent && !parent.unlocked)
		{
			SetUnavailable();
		}
		else if ((bool)parent && parent.unlocked)
		{
			SetAvailable();
		}
	}

	public void SetMenu(CosmicubeMenu menu)
	{
		this.menu = menu;
	}

	public void SetParent(CosmicubeCell parent)
	{
		this.parent = parent;
	}

	public void SetEastChild(CosmicubeCell c, UnlockableCosmetic data)
	{
		eastChild = c;
		eastChild.SetData(data);
		eastChild.transform.position = transform.position + new Vector3(0.74f, 0f, 0f);
		eastChild.parent = this;
	}

	public void SetWestChild(CosmicubeCell c, UnlockableCosmetic data)
	{
		westChild = c;
		westChild.SetData(data);
		westChild.transform.position = transform.position - new Vector3(0.74f, 0f, 0f);
		westChild.parent = this;
	}

	public void SetNorthChild(CosmicubeCell c, UnlockableCosmetic data)
	{
		northChild = c;
		northChild.SetData(data);
		northChild.transform.position = transform.position + new Vector3(0f, 0.74f, 0f);
		northChild.parent = this;
	}

	public void SetSouthChild(CosmicubeCell c, UnlockableCosmetic data)
	{
		southChild = c;
		southChild.SetData(data);
		southChild.transform.position = transform.position - new Vector3(0f, 0.74f, 0f);
		southChild.parent = this;
	}

	public void SetData(UnlockableCosmetic cosmetic)
	{
		this.cosmetic = cosmetic;
		SetPositionAndScale(cosmetic);
		SetMetadata(cosmetic);
		SetMask();
		StartCoroutine(CoSetSprites(cosmetic));
	}

	public IEnumerator CoEditorSetData(UnlockableCosmetic cosmetic)
	{
		this.cosmetic = cosmetic;
		SetMetadata(cosmetic);
		SetMask();
		SetPositionAndScale(cosmetic);
		yield return CoSetSprites(cosmetic);
	}

	public void SetPositionAndScale(UnlockableCosmetic cosmetic)
	{
		if ((bool)cosmetic.unlockableCosmetic)
		{
			if (!(cosmetic.unlockableCosmetic is NamePlateData))
			{
				icon.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
				icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, -3f);
			}
			else
			{
				icon.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
				icon.transform.localPosition = new Vector3(-0.65f, -0.14f, -3f);
			}
		}
	}

	public void SetMetadata(UnlockableCosmetic cosmetic)
	{
		costText.text = cosmetic.cost.ToString();
		qualitySprite.color = CosmicubeManager.qualityColorMap[cosmetic.quality];
		glowSprite.color = CosmicubeManager.qualityColorMap[cosmetic.quality];
	}

	public void SetMask()
	{
		CosmicubeMenu.CellMaskLayer++;
		if (!Application.isPlaying)
		{
			EditMaterialInEditorMode(MaskArea, maskMaterial, CosmicubeMenu.CellMaskLayer);
			return;
		}
		MaskArea.material = new Material(maskMaterial);
		MaskArea.material.SetInt(PlayerMaterial.MaskLayer, CosmicubeMenu.CellMaskLayer);
	}

	public IEnumerator CoSetSprites(UnlockableCosmetic cosmetic)
	{
		int cellMaskLayer = CosmicubeMenu.CellMaskLayer;
		if ((bool)cosmetic.unlockableCosmetic)
		{
			if (cosmetic.unlockableCosmetic is NamePlateData)
			{
				yield return this.cosmetic.CoLoadIcon((Sprite sprite, AddressableAsset asset) =>
				{
					icon.sprite = sprite;
					if (!Application.isPlaying)
					{
						EditMaterialInEditorMode(icon, iconMaterial, cellMaskLayer);
					}
					else
					{
						icon.material = new Material(iconMaterial);
						icon.material.SetInt(PlayerMaterial.MaskLayer, cellMaskLayer);
						AddressableAssetHandler.AddToGameObject(icon.gameObject, asset);
					}
				});
			}
			else
			{
				cosmetic.unlockableCosmetic.SetPreview(icon, 0);
				if (!Application.isPlaying)
				{
					EditMaterialInEditorMode(icon, GetIconMaterial(), cellMaskLayer);
				}
				else
				{
					icon.material = new Material(GetIconMaterial());
					icon.material.SetInt(PlayerMaterial.MaskLayer, cellMaskLayer);
					PlayerMaterial.SetColors(DataManager.Player.Customization.Color, icon.material);
				}
			}
		}
		yield return null;
	}

	private void EditMaterialInEditorMode(SpriteRenderer sprite, Material material, int cellMaskLayer)
	{
		Material material2 = new Material(material);
		material2.SetInt(PlayerMaterial.MaskLayer, cellMaskLayer);
		sprite.sharedMaterial = material2;
	}

	private Material GetIconMaterial()
	{
		if (cosmetic != null && cosmetic.unlockableCosmetic != null && cosmetic.unlockableCosmetic.PreviewCrewmateColor)
		{
			return iconColoredMaterial;
		}
		return iconMaterial;
	}

	public void ChooseCell()
	{
		menu.SelectItem(this);
	}

	public void SetSelected()
	{
		SelectionHighlight.enabled = true;
	}

	public void SetDeselected()
	{
		SelectionHighlight.enabled = false;
	}

	public void SetCellPurchased()
	{
		unlocked = true;
		cellSprite.color = Color.white;
		costText.gameObject.SetActive(value: false);
	}

	public void SetUnavailable()
	{
		cellSprite.color = Palette.DisabledGrey;
		icon.color = Color.gray;
		qualitySprite.color = CosmicubeManager.qualityColorMap[cosmetic.quality] * 0.5f;
		costText.color = Color.gray;
	}

	public void SetAvailable()
	{
		cellSprite.color = (unlocked ? Color.white : Palette.CosmicubeCellLockedColor);
		icon.color = Color.white;
		qualitySprite.color = CosmicubeManager.qualityColorMap[cosmetic.quality];
		costText.color = Color.white;
	}

	public IEnumerator CoPurchase()
	{
		if ((bool)eastChild)
		{
			eastChild.enabled = false;
		}
		if ((bool)westChild)
		{
			westChild.enabled = false;
		}
		if ((bool)northChild)
		{
			northChild.enabled = false;
		}
		if ((bool)southChild)
		{
			southChild.enabled = false;
		}
		data.GetComponentsInChildren<CosmicubeBuildHelper>().ForEach((CosmicubeBuildHelper h) =>
		{
			h.AnimateUnlock(0.9f);
		});
		SetCellPurchased();
		yield return CoPurchaseAnimation();
	}

	public bool CanBePurchased()
	{
		if (parent.unlocked && !unlocked)
		{
			return cosmetic != null;
		}
		return false;
	}

	private IEnumerator CoPurchaseAnimation()
	{
		StartCoroutine(new WaitForAnimationFinish(Animator, PurchaseAnimation));
		yield return Effects.Wait(0.9f);
		if ((bool)eastChild)
		{
			eastChild.enabled = true;
		}
		if ((bool)westChild)
		{
			westChild.enabled = true;
		}
		if ((bool)northChild)
		{
			northChild.enabled = true;
		}
		if ((bool)southChild)
		{
			southChild.enabled = true;
		}
	}
}
