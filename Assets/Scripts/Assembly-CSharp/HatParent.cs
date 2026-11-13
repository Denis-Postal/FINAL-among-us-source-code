using Object = UnityEngine.Object;
using System;
using System.Linq;
using Cpp2ILInjected;
using Innersloth.Assets;
using PowerTools;
using UnityEngine;

public class HatParent : MonoBehaviour
{
	public SpriteRenderer BackLayer;

	public SpriteRenderer FrontLayer;

	public SpriteRenderer Parent;

	public SpriteAnimNodeSync SpriteSyncNode;

	private AddressableAsset<HatViewData> viewAsset;

	private PlayerMaterial.Properties matProperties;

	private HatOptions options;

	public bool shouldFaceLeft;

	private const float ClimbZOffset = -0.02f;

	public bool IsLoaded => viewAsset?.IsLoaded() ?? false;

	public HatData Hat { get; set; }

	public bool Visible
	{
		set
		{
			FrontLayer.gameObject.SetActive(value);
			BackLayer.gameObject.SetActive(value);
		}
	}

	public Color SpriteColor
	{
		set
		{
			BackLayer.color = value;
			FrontLayer.color = value;
		}
	}

	public bool FlipX
	{
		set
		{
			BackLayer.flipX = value;
			FrontLayer.flipX = value;
		}
	}

	private void OnDestroy()
	{
		UnloadAsset();
	}

	public bool HasHat()
	{
		if (Hat != null)
		{
			return !Hat.IsEmpty;
		}
		return false;
	}

	public void SetHat(string hatId, int color)
	{
		if (DestroyableSingleton<HatManager>.InstanceExists)
		{
			HatData hatById = DestroyableSingleton<HatManager>.Instance.GetHatById(hatId);
			SetHat(hatById, color);
		}
	}

	public void SetHat(HatData hat, int color)
	{
		if (hat == null || hat != Hat)
		{
			BackLayer.sprite = null;
			FrontLayer.sprite = null;
		}
		Hat = hat;
		SetHat(color);
	}

	private void SetHat(int color)
	{
		if ((bool)Hat)
		{
			SetMaterialColor(color);
			UnloadAsset();
			viewAsset = Hat.CreateAddressableAsset();
			viewAsset.LoadAsync(() =>
			{
				PopulateFromViewData();
			});
		}
	}

	public void SetIdleAnim(int colorId)
	{
		if ((bool)Hat)
		{
			SetHat(colorId);
			transform.SetLocalZ(0f);
		}
	}

	public void SetShouldFaceLeft(bool leftFacingVictim)
	{
		shouldFaceLeft = leftFacingVictim;
	}

	public void SetFloorAnim()
	{
		BackLayer.enabled = false;
		FrontLayer.enabled = true;
		FrontLayer.flipX = false;
		FrontLayer.sprite = viewAsset.GetAsset().FloorImage;
	}

	// TODO(verify): the climb z offset application is inferred (local z + ClimbZOffset)
	public void SetClimbAnim()
	{
		if (options.ShowForClimb)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.02f);
			BackLayer.enabled = false;
			FrontLayer.enabled = true;
			FrontLayer.sprite = viewAsset.GetAsset().ClimbImage;
		}
	}

	public void SetLocalPlayer(bool localPlayer)
	{
		matProperties.IsLocalPlayer = localPlayer;
		UpdateMaterial();
	}

	public void SetMaterialColor(int color)
	{
		matProperties.ColorId = color;
		UpdateMaterial();
	}

	public void SetMaskType(PlayerMaterial.MaskType maskType)
	{
		matProperties.MaskType = maskType;
		UpdateMaterial();
	}

	public void SetMaskLayer(int layer)
	{
		if (matProperties.MaskLayer != layer)
		{
			matProperties.MaskLayer = layer;
			UpdateMaterial();
		}
	}

	private void UnloadAsset()
	{
		viewAsset?.Unload();
		viewAsset = null;
	}

	public void SetOptions(HatOptions b)
	{
		options = b;
		options.Initialized = true;
	}

	private void UpdateMaterial()
	{
		PlayerMaterial.MaskType maskType = matProperties.MaskType;
		if (IsLoaded && viewAsset.GetAsset().MatchPlayerColor)
		{
			if (maskType == PlayerMaterial.MaskType.ComplexUI || maskType == PlayerMaterial.MaskType.ScrollingUI)
			{
				BackLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedPlayerMaterial;
				FrontLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedPlayerMaterial;
			}
			else
			{
				BackLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
				FrontLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
			}
		}
		else if (maskType == PlayerMaterial.MaskType.ComplexUI || maskType == PlayerMaterial.MaskType.ScrollingUI)
		{
			BackLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedMaterial;
			FrontLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedMaterial;
		}
		else
		{
			BackLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
			FrontLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
		}
		switch (maskType)
		{
		case PlayerMaterial.MaskType.SimpleUI:
			BackLayer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			FrontLayer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			break;
		case PlayerMaterial.MaskType.Exile:
			BackLayer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			FrontLayer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		default:
			BackLayer.maskInteraction = SpriteMaskInteraction.None;
			FrontLayer.maskInteraction = SpriteMaskInteraction.None;
			break;
		}
		BackLayer.material.SetInt(PlayerMaterial.MaskLayer, matProperties.MaskLayer);
		FrontLayer.material.SetInt(PlayerMaterial.MaskLayer, matProperties.MaskLayer);
		if (IsLoaded && viewAsset.GetAsset().MatchPlayerColor)
		{
			PlayerMaterial.SetColors(matProperties.ColorId, BackLayer);
			PlayerMaterial.SetColors(matProperties.ColorId, FrontLayer);
		}
		if (matProperties.MaskLayer <= 0)
		{
			PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(BackLayer, matProperties.IsLocalPlayer);
			PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(FrontLayer, matProperties.IsLocalPlayer);
		}
	}

	private void PopulateFromViewData()
	{
		UpdateMaterial();
		if (viewAsset == null)
		{
			return;
		}
		HatViewData asset = viewAsset.GetAsset();
		if ((bool)asset)
		{
			SpriteAnimNodeSync spriteAnimNodeSync = SpriteSyncNode ?? GetComponent<SpriteAnimNodeSync>();
			if ((bool)spriteAnimNodeSync)
			{
				spriteAnimNodeSync.NodeId = (Hat.NoBounce ? 1 : 0);
			}
			if (Hat.InFront)
			{
				BackLayer.enabled = false;
				FrontLayer.enabled = true;
				FrontLayer.sprite = asset.MainImage;
			}
			else if ((bool)asset.BackImage)
			{
				BackLayer.enabled = true;
				FrontLayer.enabled = true;
				BackLayer.sprite = asset.BackImage;
				FrontLayer.sprite = asset.MainImage;
			}
			else
			{
				BackLayer.enabled = true;
				FrontLayer.enabled = false;
				FrontLayer.sprite = null;
				BackLayer.sprite = asset.MainImage;
			}
			if (options.Initialized && HideHat())
			{
				FrontLayer.enabled = false;
				BackLayer.enabled = false;
			}
		}
	}

	public void UpdateBounceHatZipline()
	{
		SpriteAnimNodeSync spriteAnimNodeSync = SpriteSyncNode ?? GetComponent<SpriteAnimNodeSync>();
		if ((bool)spriteAnimNodeSync)
		{
			spriteAnimNodeSync.NodeId = 1;
		}
	}

	public bool HideHat()
	{
		if (!options.VisorBlockingHatsAllowed)
		{
			return Hat.BlocksVisors;
		}
		return false;
	}

	// TODO(verify): reconstructed from a heavily damaged decompile; swaps between left/right hat sprites when the body flips
	private void LateUpdate()
	{
		if (!Parent || !HasHat() || !IsLoaded)
		{
			return;
		}
		HatViewData hatViewData = viewAsset?.GetAsset();
		if (!Hat || !(hatViewData != null))
		{
			return;
		}
		if (FrontLayer.sprite != hatViewData.ClimbImage && FrontLayer.sprite != hatViewData.FloorImage)
		{
			if ((Hat.InFront || (bool)hatViewData.BackImage) && (bool)hatViewData.LeftMainImage)
			{
				FrontLayer.sprite = ((Parent.flipX || shouldFaceLeft) ? hatViewData.LeftMainImage : hatViewData.MainImage);
			}
			if ((bool)hatViewData.BackImage && (bool)hatViewData.LeftBackImage)
			{
				BackLayer.sprite = ((Parent.flipX || shouldFaceLeft) ? hatViewData.LeftBackImage : hatViewData.BackImage);
			}
			else if (!hatViewData.BackImage && !Hat.InFront && (bool)hatViewData.LeftMainImage)
			{
				BackLayer.sprite = ((Parent.flipX || shouldFaceLeft) ? hatViewData.LeftMainImage : hatViewData.MainImage);
			}
		}
		else if (FrontLayer.sprite == hatViewData.ClimbImage || FrontLayer.sprite == hatViewData.LeftClimbImage)
		{
			SpriteAnimNodeSync spriteAnimNodeSync = SpriteSyncNode ?? GetComponent<SpriteAnimNodeSync>();
			if ((bool)spriteAnimNodeSync)
			{
				spriteAnimNodeSync.NodeId = 0;
			}
		}
	}

	private void _003CSetHat_003Eb__25_0()
	{
		PopulateFromViewData();
	}
}
