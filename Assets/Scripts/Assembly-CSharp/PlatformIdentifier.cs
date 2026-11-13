using Object = UnityEngine.Object;
using Cpp2ILInjected;
using InnerNet;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlatformIdentifier : MonoBehaviour
{
	public SpriteRenderer iconRenderer;

	public TextMeshPro platformIDText;

	public bool isOnHUD;

	public Sprite genericConsole;

	public Sprite mobile;

	public Sprite pc;

	public Sprite nSwitch;

	public Sprite ps4;

	public Sprite xbox;

	public bool Masked;

	private bool Visible
	{
		set
		{
			gameObject.SetActive(value);
		}
	}

	private bool UsernameVisible
	{
		set
		{
			platformIDText.gameObject.SetActive(value);
		}
	}

	public static bool SupportsPlatformUsername(Platforms platformTypeID)
	{
		if ((uint)(platformTypeID - 8) <= 2u)
		{
			return true;
		}
		return false;
	}

	public void Awake()
	{
		iconRenderer.enabled = false;
		if (Masked)
		{
			platformIDText.fontMaterial.SetFloat("_StencilComp", 3f);
			platformIDText.fontMaterial.SetFloat("_Stencil", 51f);
		}
	}

	public void SetInfo(ClientData clientData)
	{
		if (clientData == null)
		{
			Visible = false;
		}
		else
		{
			SetInfo(clientData.PlatformData.Platform, clientData.PlatformData.PlatformName);
		}
	}

	public void SetInfo(GameListing gameListingData)
	{
		SetInfo(gameListingData.Platform, gameListingData.HostPlatformName);
	}

	public void SetInfo(Platforms platform, string platformName)
	{
		Visible = true;
		bool flag = Constants.GetPlatformType() == platform;
		if ((SupportsPlatformUsername(platform) & flag) && !string.IsNullOrEmpty(platformName))
		{
			UsernameVisible = true;
			platformIDText.text = platformName;
		}
		else
		{
			UsernameVisible = false;
		}
		SetIcon(platform);
	}

	private void SetIcon(Platforms platform)
	{
		iconRenderer.enabled = true;
		Sprite sprite = ValidateIcon(GetPrevalidationIcon(platform));
		if ((bool)sprite)
		{
			iconRenderer.sprite = sprite;
		}
		else
		{
			iconRenderer.enabled = false;
		}
		if (!isOnHUD)
		{
			UpdatePosition();
		}
	}

	public unsafe void UpdatePosition()
	{
		Vector3 localPosition = iconRenderer.transform.localPosition;
		if (platformIDText.horizontalAlignment == HorizontalAlignmentOptions.Left)
		{
			localPosition.y = platformIDText.textBounds.center.y;
			localPosition.x = platformIDText.bounds.min.x * platformIDText.transform.localScale.x - 0.5f * iconRenderer.transform.localScale.x * iconRenderer.sprite.rect.width / iconRenderer.sprite.pixelsPerUnit;
		}
		else
		{
			Vector2 renderedValues = platformIDText.GetRenderedValues(onlyVisibleCharacters: true);
			localPosition.y = platformIDText.textBounds.center.y;
			localPosition.x = renderedValues.x * -0.5f - 0.5f * iconRenderer.transform.localScale.x * iconRenderer.sprite.rect.width / iconRenderer.sprite.pixelsPerUnit;
		}
		iconRenderer.transform.localPosition = localPosition;
	}

	private Sprite GetPrevalidationIcon(Platforms platform)
	{
		switch (platform)
		{
		default:
			return pc;
		case Platforms.IPhone:
		case Platforms.Android:
			return mobile;
		case Platforms.Xbox:
			return xbox;
		case Platforms.Switch:
			return nSwitch;
		case Platforms.Playstation:
			return ps4;
		}
	}

	private Sprite ValidateIcon(Sprite icon)
	{
		if (icon == ps4 || icon == xbox || icon == nSwitch)
		{
			return genericConsole;
		}
		return icon;
	}
}
