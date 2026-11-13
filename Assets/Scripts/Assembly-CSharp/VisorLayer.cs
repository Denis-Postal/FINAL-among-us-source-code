using Object = UnityEngine.Object;
using System;
using System.Linq;
using Cpp2ILInjected;
using Innersloth.Assets;
using UnityEngine;

public class VisorLayer : MonoBehaviour
{
	private const float BackZLayer = -1.5f;

	private const float FrontZLayer = -3f;

	private const float ClimbZOffset = -0.01f;

	[SerializeField]
	private SpriteRenderer Image;

	public VisorData visorData;

	private AddressableAsset<VisorViewData> viewAsset;

	private PlayerMaterial.Properties matProperties;

	private VisorOptions options;

	public float ZIndexSpacing { get; set; } = 0.0001f;

	private float LocalZFrontLayer => ZIndexSpacing * -3f;

	private float LocalZBackLayer => ZIndexSpacing * -1.5f;

	public float DesiredLocalZPosition
	{
		get
		{
			if (visorData != null && visorData.BehindHats)
			{
				return LocalZBackLayer;
			}
			return LocalZFrontLayer;
		}
	}

	public bool IsLoaded => viewAsset?.IsLoaded() ?? false;

	public bool Visible
	{
		set
		{
			Image.enabled = value;
		}
	}

	public float Alpha
	{
		set
		{
			Image.color = Image.color.SetAlpha(value);
		}
	}

	public CosmeticData GetData()
	{
		return visorData;
	}

	public void SetVisor(string visorId, int colorId)
	{
		if (DestroyableSingleton<HatManager>.InstanceExists)
		{
			VisorData visorById = DestroyableSingleton<HatManager>.Instance.GetVisorById(visorId);
			SetVisor(visorById, colorId);
		}
	}

	public void SetVisor(VisorData data, int color)
	{
		if (data == null || data != visorData)
		{
			Image.sprite = null;
		}
		visorData = data;
		SetMaterialColor(color);
		UnloadAsset();
		viewAsset = visorData.CreateAddressableAsset();
		viewAsset.LoadAsync(() =>
		{
			PopulateFromViewData();
		});
	}

	private void PopulateFromViewData()
	{
		UpdateMaterial();
		if (viewAsset != null && (bool)viewAsset.GetAsset() && !this.IsDestroyedOrNull() && !gameObject.IsDestroyedOrNull())
		{
			transform.SetLocalZ(DesiredLocalZPosition);
			SetFlipX(Image.flipX);
		}
	}

	public void SetOptions(VisorOptions options)
	{
		this.options = options;
		transform.localScale = (options.Enabled ? Vector3.one : Vector3.zero);
	}

	public void SetFlipX(bool flipX)
	{
		Image.flipX = flipX;
		if (IsLoaded)
		{
			VisorViewData asset = viewAsset.GetAsset();
			if (flipX && (bool)asset.LeftIdleFrame)
			{
				Image.sprite = asset.LeftIdleFrame;
			}
			else
			{
				Image.sprite = asset.IdleFrame;
			}
		}
	}

	public void SetLocalPlayer(bool localPlayer)
	{
		matProperties.IsLocalPlayer = localPlayer;
		UpdateMaterial();
	}

	public void SetIdleAnim(int colorId)
	{
		if ((bool)visorData)
		{
			SetVisor(visorData, colorId);
		}
	}

	public void SetFloorAnim()
	{
		if (IsLoaded)
		{
			Image.sprite = viewAsset.GetAsset().FloorFrame;
		}
	}

	// TODO(verify): the climb z offset application is inferred (local z + ClimbZOffset)
	public void SetClimbAnim(PlayerBodyTypes bodyType)
	{
		if (!options.HideDuringClimb && bodyType != PlayerBodyTypes.Horse)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.01f);
			Image.sprite = viewAsset.GetAsset().ClimbFrame;
		}
	}

	public void SetMaskType(PlayerMaterial.MaskType maskType)
	{
		matProperties.MaskType = maskType;
		UpdateMaterial();
	}

	public void SetMaterialColor(int color)
	{
		matProperties.ColorId = color;
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

	private void UpdateMaterial()
	{
		PlayerMaterial.MaskType maskType = matProperties.MaskType;
		if (visorData != null && IsLoaded && viewAsset.GetAsset().MatchPlayerColor)
		{
			if (maskType == PlayerMaterial.MaskType.ComplexUI || maskType == PlayerMaterial.MaskType.ScrollingUI)
			{
				Image.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedPlayerMaterial;
			}
			else
			{
				Image.sharedMaterial = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
			}
		}
		else if (maskType == PlayerMaterial.MaskType.ComplexUI || maskType == PlayerMaterial.MaskType.ScrollingUI)
		{
			Image.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedMaterial;
		}
		else
		{
			Image.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
		}
		switch (maskType)
		{
		case PlayerMaterial.MaskType.SimpleUI:
			Image.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			break;
		case PlayerMaterial.MaskType.Exile:
			Image.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		default:
			Image.maskInteraction = SpriteMaskInteraction.None;
			break;
		}
		Image.material.SetInt(PlayerMaterial.MaskLayer, matProperties.MaskLayer);
		if (visorData != null && IsLoaded && viewAsset.GetAsset().MatchPlayerColor)
		{
			PlayerMaterial.SetColors(matProperties.ColorId, Image);
		}
		if (matProperties.MaskLayer <= 0)
		{
			PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(Image, matProperties.IsLocalPlayer);
		}
	}

	public void SetVisorColor(Color color)
	{
		Image.color = color;
	}

	private void OnDestroy()
	{
		UnloadAsset();
	}

	private void UnloadAsset()
	{
		viewAsset?.Unload();
		viewAsset = null;
	}

	public VisorLayer()
	{ }

	private void _003CSetVisor_003Eb__26_0()
	{
		PopulateFromViewData();
	}
}
