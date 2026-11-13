using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using Innersloth.Assets;
using PowerTools;
using TMPro;
using UnityEngine;

public class CosmeticsLayer : MonoBehaviour
{
	public enum CosmeticKind
	{
		SKIN,
		HAT,
		VISOR,
		PET
	}

	[Header("Options")]
	[SerializeField]
	private bool alwaysDrawNormalPlayer;

	[SerializeField]
	private bool uiPet = true;

	[SerializeField]
	private float zIndexSpacing = 1E-08f;

	[SerializeField]
	private bool showColorBlindText;

	[SerializeField]
	[Header("Cosmetics")]
	private List<PlayerBodySprite> bodySprites;

	[SerializeField]
	public TextMeshPro colorBlindText;

	[SerializeField]
	public HatParent hat;

	[SerializeField]
	public TextMeshPro nameText;

	[SerializeField]
	public GameObject nameTextContainer;

	[SerializeField]
	private Transform petParent;

	[SerializeField]
	public SkinLayer skin;

	[SerializeField]
	public VisorLayer visor;

	[SerializeField]
	private Vector3 defaultHatVisorPosition;

	private PlayerBodyTypes bodyType;

	private PlayerMaterial.Properties bodyMatProperties;

	public PlayerBodySprite currentBodySprite;

	public PetBehaviour currentPet;

	private PlayerBodySprite normalBodySprite;

	private bool initialized;

	private AddressableAsset<PetBehaviour> petAsset;

	private LongBoiPlayerBody longboi;

	private const string horseWranglerSkinID = "skin_MilitaryDesert";

	public bool petHiddenByViper;

	public const string EMPTY_SKIN_ID = "skin_None";

	public const string EMPTY_VISOR_ID = "visor_EmptyVisor";

	public const string EMPTY_HAT_ID = "hat_NoHat";

	public const string EMPTY_PET_ID = "pet_EmptyPet";

	private bool visible = true;

	private bool isNameVisible = true;

	private bool lockVisible;

	private bool localPlayer;

	private bool loggedBodyMaterialDiagnostics;

	public PetBehaviour CurrentPet => currentPet;

	public SkinLayer CurrentSkin => skin;

	public VisorLayer CurrentVisor => visor;

	public PlayerPettingHand PettingHand => currentBodySprite.PettingHand;

	public SpriteRenderer HatHand => currentBodySprite.HandHat;

	public PlayerBodyTypes CurrentBodyType => currentBodySprite.Type;

	public Vector3 FlippedCosmeticOffset
	{
		get
		{
			return currentBodySprite.flippedCosmeticOffset;
		}
		set
		{
			currentBodySprite.flippedCosmeticOffset = value;
		}
	}

	public Vector3 NormalCosmeticOffset
	{
		get
		{
			return currentBodySprite.normalCosmeticOffset;
		}
		set
		{
			currentBodySprite.normalCosmeticOffset = value;
		}
	}

	public bool AlwaysDrawNormalPlayer => alwaysDrawNormalPlayer;

	public bool FlipX => currentBodySprite.BodySprite.flipX;

	public int ColorId => bodyMatProperties.ColorId;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (!lockVisible)
			{
				visible = value;
			}
			UpdateVisibility();
		}
	}

	public event Action<int> OnColorChange;

	public event Action OnSetBodyAsGhost;

	public event Action<string, int, CosmeticKind> OnCosmeticSet;

	public void SetBodyCosmeticsVisible(bool b)
	{
		skin.Visible = b;
		hat.Visible = b;
		visor.Visible = b;
		colorBlindText.enabled = b;
	}

	public void UpdateVisibility()
	{
		currentBodySprite.Visible = visible;
		skin.Visible = visible;
		hat.Visible = visible;
		visor.Visible = visible;
		colorBlindText.enabled = visible;
		if ((bool)currentPet)
		{
			currentPet.Visible = visible;
		}
		UpdateNameVisibility();
	}

	public void SetPhantomRoleAlpha(float alphaValue)
	{
		currentBodySprite.BodySprite.color = currentBodySprite.BodySprite.color.SetAlpha(alphaValue);
		if (currentBodySprite.Type != PlayerBodyTypes.Classic && currentBodySprite.Type != PlayerBodyTypes.Horse)
		{
			skin.layer.color = skin.layer.color.SetAlpha(alphaValue);
		}
		hat.FrontLayer.color = hat.FrontLayer.color.SetAlpha(alphaValue);
		hat.BackLayer.color = hat.BackLayer.color.SetAlpha(alphaValue);
		visor.Alpha = alphaValue;
		if (currentPet != null)
		{
			currentPet.SetAlpha(alphaValue);
			PettingHand.SetAlpha(alphaValue);
		}
	}

	public float GetPhantomRoleAlpha()
	{
		return currentBodySprite.BodySprite.color.a;
	}

	public void SetPetVisible(bool v)
	{
		if ((bool)currentPet)
		{
			currentPet.Visible = v;
		}
	}

	public bool HasPetEquipped()
	{
		if ((bool)CurrentPet)
		{
			return CurrentPet.Data.ProductId != "pet_EmptyPet";
		}
		return false;
	}

	public void ToggleHat(bool b)
	{
		hat.Visible = b;
	}

	public void SetForcedVisible(bool isVisible)
	{
		if (isVisible)
		{
			lockVisible = false;
			Visible = isVisible;
		}
		else
		{
			Visible = isVisible;
			lockVisible = true;
		}
	}

	public PlayerBodySprite GetCurrentBodySprite()
	{
		return currentBodySprite;
	}

	public static Material GetBodyMaterial(PlayerMaterial.MaskType type)
	{
		Material result = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
		if (type == PlayerMaterial.MaskType.ComplexUI || type == PlayerMaterial.MaskType.ScrollingUI)
		{
			result = DestroyableSingleton<HatManager>.Instance.MaskedPlayerMaterial;
		}
		return result;
	}

	public void EnsureInitialized(PlayerBodyTypes bt)
	{
		if (initialized && bt == bodyType)
		{
			return;
		}
		if (alwaysDrawNormalPlayer && bt != PlayerBodyTypes.Normal)
		{
			Logger.GlobalInstance.Info($"Overriding body type {bt} with Normal because {name} always draws normal player.");
			bt = PlayerBodyTypes.Normal;
		}
		bodyType = bt;
		bodySprites.ForEach((PlayerBodySprite s) =>
		{
			if (s.Type == PlayerBodyTypes.Normal)
			{
				normalBodySprite = s;
			}
			if (s.Type == bodyType)
			{
				currentBodySprite = s;
				s.Visible = true;
				s.BodySprite.gameObject.SetActive(value: true);
			}
			else
			{
				s.BodySprite.gameObject.SetActive(value: false);
			}
		});
		if ((bool)hat)
		{
			hat.transform.localPosition = defaultHatVisorPosition;
			hat.Parent = currentBodySprite.BodySprite;
			hat.SetOptions(bodyType.GetHatOptions());
		}
		if (skin != null && skin.layer != null)
		{
			skin.layer.color = (bodyType.GetSkinOptions().Enabled ? Color.white : Color.clear);
		}
		if ((bool)visor)
		{
			visor.transform.localPosition = defaultHatVisorPosition;
			visor.SetOptions(bodyType.GetVisorOptions());
		}
		SetCosmeticZIndices();
		if (initialized)
		{
			SetColor(bodyMatProperties.ColorId);
		}
		initialized = true;
	}

	public void SetCustomHatPosition(Vector3 pos)
	{
		if ((bool)hat)
		{
			hat.transform.localPosition = pos;
		}
	}

	public void SetCustomVisorPosition(Vector3 pos)
	{
		if ((bool)visor)
		{
			visor.transform.localPosition = pos;
		}
	}

	public SkinViewData GetSkinView()
	{
		return skin.skin;
	}

	public void FixSkinSprite(Sprite skinSprite)
	{
		skin.animator.Stop();
		skin.layer.sprite = skinSprite;
	}

	public PetBehaviour GetPet()
	{
		return currentPet;
	}

	public SpriteAnim GetSkinSpriteAnim()
	{
		return skin.GetComponent<SpriteAnim>();
	}

	public void ReplacePetParent(Transform newParent, Vector3 newPosition)
	{
		if (petParent == null)
		{
			Debug.LogError("Trying to replace pet parent when none is set.");
			return;
		}
		petParent.SetParent(newParent);
		petParent.transform.localPosition = newPosition;
	}

	public void ResetCosmetics(string ignoreType = "")
	{
		bool isLeft = false;
		if (currentBodySprite != null && currentBodySprite.BodySprite != null)
		{
			isLeft = currentBodySprite.BodySprite.flipX;
		}
		if (ignoreType != "skin")
		{
			skin?.SetSkin("", 0, isLeft, this);
		}
		if (ignoreType != "hat")
		{
			hat?.SetHat("", 0);
		}
		if (ignoreType != "visor")
		{
			visor?.SetVisor("", 0);
		}
		if ((bool)currentPet && ignoreType != "pet")
		{
			UnityEngine.Object.Destroy(currentPet.gameObject);
			currentPet = null;
		}
	}

	public void SetBodyAsGhost()
	{
		currentBodySprite.BodySprite.sprite = currentBodySprite.GhostSprite;
		for (int i = 0; i < currentBodySprite.ExtraGhostSprites.Length; i++)
		{
			if (i < currentBodySprite.LongModeParts.Length)
			{
				currentBodySprite.LongModeParts[i].sprite = currentBodySprite.ExtraGhostSprites[i];
			}
		}
		if (OnSetBodyAsGhost != null)
		{
			OnSetBodyAsGhost();
		}
	}

	public void SetBodyColor(int bodyColor)
	{
		bodyMatProperties.ColorId = bodyColor;
		UpdateBodyMaterial();
		SetColorBlindColor(bodyColor);
		if (OnColorChange != null)
		{
			OnColorChange(bodyColor);
		}
	}

	public void SetColor(int color)
	{
		SetBodyColor(color);
		SetHatColor(color);
		SetSkinColor(color);
		SetPetColor(color);
		SetVisorColor(color);
	}

	public void SetDeadFlipX(bool flipped)
	{
		currentBodySprite.SetFlipX(flipped);
		hat.FlipX = flipped;
		Vector3 localPosition = defaultHatVisorPosition;
		Vector3 localPosition2 = visor.transform.localPosition;
		if (currentBodySprite.Type == PlayerBodyTypes.Horse)
		{
			localPosition2.x = (flipped ? (-1.3f) : 0f);
		}
		else
		{
			localPosition.x = (flipped ? (-0.2f) : 0.2f);
			localPosition.y = 0.75f;
			localPosition2.x = (flipped ? (-0.17f) : 0.17f);
			localPosition2.y = 0.75f;
		}
		hat.transform.localPosition = localPosition;
		if (flipped)
		{
			hat.transform.localPosition += currentBodySprite.flippedCosmeticOffset;
			visor.transform.localPosition += currentBodySprite.flippedCosmeticOffset;
		}
		else
		{
			hat.transform.localPosition += currentBodySprite.normalCosmeticOffset;
			visor.transform.localPosition += currentBodySprite.normalCosmeticOffset;
		}
		visor.SetFlipX(flipped);
		visor.transform.localPosition = localPosition2;
		SetPetFlipX(flipped);
	}

	public void SetEnabledColorblind(bool isEnabled)
	{
		if (showColorBlindText)
		{
			colorBlindText.gameObject.SetActive(isEnabled);
		}
	}

	public void SetFlipX(bool flipped)
	{
		SetFlipXWithoutPet(flipped);
		SetPetFlipX(flipped);
	}

	public void SetFlipXWithoutPet(bool flipped)
	{
		currentBodySprite.SetFlipX(flipped);
		hat.FlipX = flipped;
		Vector3 localPosition = defaultHatVisorPosition;
		localPosition.x = (flipped ? Mathf.Abs(localPosition.x) : (0f - Mathf.Abs(localPosition.x)));
		hat.transform.localPosition = localPosition;
		visor.transform.localPosition = new Vector3(localPosition.x, localPosition.y, visor.transform.localPosition.z);
		if (flipped)
		{
			hat.transform.localPosition += currentBodySprite.flippedCosmeticOffset;
			visor.transform.localPosition += currentBodySprite.flippedCosmeticOffset;
		}
		else
		{
			hat.transform.localPosition += currentBodySprite.normalCosmeticOffset;
			visor.transform.localPosition += currentBodySprite.normalCosmeticOffset;
		}
		visor.SetFlipX(flipped);
		skin.Flipped = flipped;
	}

	public void SetGhost()
	{
		skin.SetGhost();
	}

	public bool HasHat()
	{
		if (hat.HasHat())
		{
			return !hat.HideHat();
		}
		return false;
	}

	public void SetHat(string hatId, int color)
	{
		hat?.SetHat(hatId, color);
		OnCosmeticSet?.Invoke(hatId, color, CosmeticKind.HAT);
	}

	public void SetHat(HatData hatData, int color)
	{
		hat?.SetHat(hatData, color);
		OnCosmeticSet?.Invoke(hatData.ProdId, color, CosmeticKind.HAT);
	}

	public void SetHatColor(Color color)
	{
		if ((bool)hat)
		{
			hat.SpriteColor = color;
		}
	}

	public void SetHatColor(int color)
	{
		hat?.SetMaterialColor(color);
	}

	public void SetHatWithoutChangingColor(HatData hatData)
	{
		if ((bool)hat)
		{
			hat.Hat = hatData;
		}
	}

	public void SetVisor(string visorId, int color)
	{
		visor?.SetVisor(visorId, color);
		OnCosmeticSet?.Invoke(visorId, color, CosmeticKind.VISOR);
	}

	public void SetVisor(VisorData visorData, int color)
	{
		visor?.SetVisor(visorData, color);
		OnCosmeticSet?.Invoke(visorData.ProdId, color, CosmeticKind.VISOR);
	}

	public void ValidateCosmetics()
	{
		if ((bool)hat.Hat)
		{
			OnCosmeticSet?.Invoke(hat.Hat.ProdId, 0, CosmeticKind.HAT);
		}
		if ((bool)visor.GetData())
		{
			OnCosmeticSet?.Invoke(visor.GetData().ProdId, 0, CosmeticKind.VISOR);
		}
		if ((bool)skin.data)
		{
			OnCosmeticSet?.Invoke(skin.data.ProdId, 0, CosmeticKind.SKIN);
		}
		if ((bool)currentPet && (bool)currentPet.Data)
		{
			OnCosmeticSet?.Invoke(currentPet.Data.ProdId, 0, CosmeticKind.PET);
		}
	}

	public void SetVisorAlpha(float alpha)
	{
		if ((bool)visor)
		{
			visor.Alpha = alpha;
		}
	}

	public void SetVisorColor(int color)
	{
		visor?.SetMaterialColor(color);
	}

	public void ToggleVisor(bool active)
	{
		visor.gameObject.SetActive(active);
	}

	public void SetHatAndVisorIdle(int colorId)
	{
		hat.SetIdleAnim(colorId);
		if (visor.isActiveAndEnabled)
		{
			visor.SetIdleAnim(colorId);
		}
		else
		{
			ToggleVisor(active: true);
		}
	}

	public void SetHatLeftFacingVictim(bool leftFacingVictim)
	{
		hat.SetShouldFaceLeft(leftFacingVictim);
	}

	public void SetHatOnFloor()
	{
		hat.SetFloorAnim();
	}

	public void SetVisorOnFloor()
	{
		visor.SetFloorAnim();
	}

	public void SetHatAndVisorMaskType(PlayerMaterial.MaskType type)
	{
		hat.SetMaskType(type);
		visor.SetMaskType(type);
	}

	public void SetMaskLayer(int layer)
	{
		if (bodyMatProperties.MaskLayer != layer)
		{
			bodyMatProperties.MaskLayer = layer;
			UpdateBodyMaterial();
		}
		skin.SetMaskLayer(layer);
		hat.SetMaskLayer(layer);
		visor.SetMaskLayer(layer);
	}

	public void SetMaskType(PlayerMaterial.MaskType type)
	{
		bodyMatProperties.MaskType = type;
		UpdateBodyMaterial();
		hat?.SetMaskType(type);
		visor?.SetMaskType(type);
		skin?.SetMaskType(type);
	}

	public void SetName(string nameStr, Vector3 scale, Color color, float zPosition)
	{
		SetNameColor(color);
		SetNameScale(scale);
		nameText.transform.SetLocalZ(zPosition);
		SetName(nameStr);
	}

	public void SetName(string nameStr)
	{
		if (string.IsNullOrWhiteSpace(nameStr))
		{
			nameText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PlayerName);
		}
		else
		{
			nameText.text = nameStr;
		}
	}

	public void SetNameColor(Color color)
	{
		nameText.color = color;
	}

	public void SetNameMask(bool alive)
	{
		int value = (alive ? 4 : 0);
		nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", value);
	}

	public void SetNamePosition(Vector3 newPosition)
	{
		nameTextContainer.transform.localPosition = newPosition;
	}

	public void SetNamePositionZ(float zPos)
	{
		nameTextContainer.transform.localPosition = new Vector3(nameTextContainer.transform.localPosition.x, nameTextContainer.transform.localPosition.y, zPos);
	}

	public void SetNameScale(Vector3 scale)
	{
		nameTextContainer.transform.localScale = scale;
	}

	public void SetOutline(bool active, Color? color = null)
	{
		currentBodySprite.BodySprite.material.SetFloat("_Outline", active ? 1 : 0);
		SpriteRenderer[] longModeParts = currentBodySprite.LongModeParts;
		for (int i = 0; i < longModeParts.Length; i++)
		{
			longModeParts[i].material.SetFloat("_Outline", active ? 1 : 0);
		}
		if (color.HasValue)
		{
			currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color.Value);
			longModeParts = currentBodySprite.LongModeParts;
			for (int i = 0; i < longModeParts.Length; i++)
			{
				longModeParts[i].material.SetColor("_OutlineColor", color.Value);
			}
		}
	}

	public void SetPetFlipX(bool flipped)
	{
		if ((bool)currentPet)
		{
			currentPet.FlipX = flipped;
			if ((bool)petParent)
			{
				Vector3 localPosition = petParent.transform.localPosition;
				localPosition.x = (flipped ? (0f - Mathf.Abs(localPosition.x)) : Mathf.Abs(localPosition.x));
				petParent.transform.localPosition = localPosition;
			}
			else
			{
				Vector3 localPosition2 = currentPet.transform.localPosition;
				localPosition2.x = (flipped ? (0f - Mathf.Abs(localPosition2.x)) : Mathf.Abs(localPosition2.x));
				currentPet.transform.localPosition = localPosition2;
			}
		}
	}

	public void SetPetIdle(string petId, int colorId, Action onComplete = null)
	{
		SetPetIdle(DestroyableSingleton<HatManager>.Instance.GetPetById(petId), colorId, onComplete);
	}

	public void SetPetIdle(PetData petData, int colorId, Action onComplete = null)
	{
		StopAllCoroutines();
		if ((bool)currentPet && currentPet.Data.ProdId == petData.ProdId)
		{
			currentPet.SetIdle();
			onComplete?.Invoke();
		}
		else
		{
			StartCoroutine(CoLoadAndSetPetIdle(petData, colorId, onComplete));
		}
	}

	public void SetPetIdle(PetBehaviour petBehaviour, int colorId, Action onComplete = null)
	{
		StopAllCoroutines();
		InstantiatePetCopy(petBehaviour, colorId);
		currentPet.SetIdle();
		currentPet.Visible = visible;
		onComplete?.Invoke();
		OnCosmeticSet?.Invoke(petBehaviour.Data.ProdId, colorId, CosmeticKind.PET);
	}

	public void SetPetPosition(Vector3 newPosition)
	{
		if ((bool)currentPet)
		{
			currentPet.transform.position = newPosition;
		}
	}

	public IEnumerator WalkPetBehindPlayer(float speed)
	{
		yield return currentPet.MoveBehindSource(speed);
	}

	public void SetPetScared(PetBehaviour petBehaviour, int colorId)
	{
		StopAllCoroutines();
		InstantiatePetCopy(petBehaviour, colorId);
		currentPet.SetScared();
	}

	public void SetPetSource(PlayerControl playerControl)
	{
		if ((bool)currentPet)
		{
			currentPet.SetTargetPlayer(playerControl);
		}
	}

	public void SetScale(Vector3 playerScale, Vector3 cosmeticsScale)
	{
		currentBodySprite.BodySprite.transform.localScale = playerScale;
		normalBodySprite.BodySprite.transform.localScale = playerScale;
		transform.localScale = cosmeticsScale;
	}

	public void SetSkin(string skinId, int color, Action onLoaded = null)
	{
		SetSkin(DestroyableSingleton<HatManager>.Instance.GetSkinById(skinId), color, onLoaded);
	}

	public void SetSkin(SkinData skin, int color, Action onLoaded = null)
	{
		if (!this.skin)
		{
			return;
		}
		if (AprilFoolsMode.ShouldLongAround() && GetLongBoi() != null)
		{
			if (!GetLongBoi().ValidateSkin(skin.ProdId, color))
			{
				skin = DestroyableSingleton<HatManager>.Instance.GetSkinById("skin_None");
			}
		}
		else if (AprilFoolsMode.ShouldHorseAround() && bodyType == PlayerBodyTypes.Horse && skin.ProductId != "skin_MilitaryDesert")
		{
			skin = DestroyableSingleton<HatManager>.Instance.GetSkinById("skin_None");
		}
		this.skin.SetSkin(skin, color, currentBodySprite.BodySprite.flipX, this, onLoaded);
		this.skin.Flipped = currentBodySprite.BodySprite.flipX;
	}

	public void ValidateSkin(string prodId, int color)
	{
		OnCosmeticSet?.Invoke(prodId, color, CosmeticKind.SKIN);
	}

	public void SetSkin(SkinViewData skin, int color)
	{
		if ((bool)this.skin)
		{
			this.skin.SetSkin(skin, color, currentBodySprite.BodySprite.flipX);
			this.skin.Flipped = currentBodySprite.BodySprite.flipX;
		}
	}

	public void SetSkinColor(int color)
	{
		skin?.SetColor(color);
	}

	public void ToggleName(bool active)
	{
		nameText.gameObject.SetActive(active);
	}

	public void TogglePet(bool active)
	{
		if ((bool)currentPet)
		{
			currentPet.gameObject.SetActive(active);
		}
	}

	public void HidePetViper()
	{
		if ((bool)currentPet)
		{
			petHiddenByViper = true;
			TogglePet(active: false);
		}
	}

	public void TogglePetVisible(bool visible)
	{
		if ((bool)currentPet)
		{
			currentPet.Visible = visible;
		}
	}

	public void ToggleNameVisible(bool visible)
	{
		isNameVisible = visible;
		UpdateNameVisibility();
	}

	public bool IsSkinPlayingRunAnim()
	{
		return skin.IsPlayingRunAnim();
	}

	private void UpdateNameVisibility()
	{
		nameText.gameObject.SetActive(visible && isNameVisible);
		colorBlindText.gameObject.SetActive(visible && isNameVisible && DataManager.Settings.Accessibility.ColorBlindMode);
	}

	private static SpriteMaskInteraction GetBodySpriteMaskInteraction(PlayerMaterial.MaskType type)
	{
		SpriteMaskInteraction result = SpriteMaskInteraction.None;
		switch (type)
		{
		case PlayerMaterial.MaskType.SimpleUI:
			result = SpriteMaskInteraction.VisibleInsideMask;
			break;
		case PlayerMaterial.MaskType.Exile:
			result = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		}
		return result;
	}

	private IEnumerator CoLoadAndSetPetIdle(PetData petData, int colorId, Action onComplete = null)
	{
		this.UnloadAddressableAsset(petAsset);
		yield return ((MonoBehaviour)this).CoLoadAssetAsync((IAddressableAssetProvider<PetBehaviour>)petData, (Action<PetBehaviour>)((PetBehaviour pet) =>
		{
			SetPetIdle(pet, colorId, onComplete);
		}));
	}

	private void InstantiatePetCopy(PetBehaviour petBehaviour, int colorId)
	{
		if ((bool)currentPet)
		{
			UnityEngine.Object.Destroy(currentPet.gameObject);
			currentPet = null;
		}
		currentPet = UnityEngine.Object.Instantiate(petBehaviour, petParent);
		if (uiPet)
		{
			currentPet.PrepareForUI();
		}
		currentPet.SetCrewmateColor(colorId);
		currentPet.transform.localPosition = Vector3.zero;
		currentPet.SetDefaultMaterial();
		SetPetFlipX(FlipX);
		if (localPlayer)
		{
			currentPet.SetAsLocalPlayer();
		}
	}

	private void OnDestroy()
	{
		if ((bool)currentPet && currentPet.transform.parent == null)
		{
			UnityEngine.Object.Destroy(currentPet.gameObject);
		}
	}

	private void SetColorBlindColor(int color)
	{
		if (!(colorBlindText == null) && showColorBlindText)
		{
			colorBlindText.text = GetColorBlindText();
		}
	}

	public string GetColorBlindText()
	{
		char[] array = Palette.GetColorName(bodyMatProperties.ColorId).ToCharArray();
		if (array.Length != 0)
		{
			array[0] = char.ToUpper(array[0]);
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = char.ToLower(array[i]);
			}
		}
		return new string(array);
	}

	private void SetCosmeticZIndices()
	{
		if ((bool)hat)
		{
			hat.BackLayer?.transform.SetLocalZ(1f * zIndexSpacing);
			hat.FrontLayer?.transform.SetLocalZ(-2f * zIndexSpacing);
		}
		if ((bool)visor)
		{
			visor.ZIndexSpacing = zIndexSpacing;
			visor.transform.SetLocalZ(-3f * zIndexSpacing);
		}
		if ((bool)petParent)
		{
			petParent.transform.SetLocalZ(-4f * zIndexSpacing);
		}
		if ((bool)skin)
		{
			skin.transform.SetLocalZ(-1f * zIndexSpacing);
		}
	}

	private void SetPetColor(int color)
	{
		if ((bool)currentPet)
		{
			currentPet.SetCrewmateColor(color);
		}
	}

	private void UpdateBodyMaterial()
	{
		PlayerMaterial.MaskType maskType = bodyMatProperties.MaskType;
		Material bodyMaterial = GetBodyMaterial(maskType);
		SpriteMaskInteraction bodySpriteMaskInteraction = GetBodySpriteMaskInteraction(maskType);
		SpriteRenderer[] longModeParts;
		foreach (PlayerBodySprite bodySprite in bodySprites)
		{
			bodySprite.BodySprite.sharedMaterial = bodyMaterial;
			bodySprite.BodySprite.maskInteraction = bodySpriteMaskInteraction;
			longModeParts = bodySprite.LongModeParts;
			foreach (SpriteRenderer obj in longModeParts)
			{
				obj.sharedMaterial = bodyMaterial;
				obj.maskInteraction = bodySpriteMaskInteraction;
			}
		}
		if (currentBodySprite == null)
		{
			return;
		}
		PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(currentBodySprite.BodySprite, localPlayer);
		longModeParts = currentBodySprite.LongModeParts;
		for (int i = 0; i < longModeParts.Length; i++)
		{
			PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(longModeParts[i], localPlayer);
		}
		if (bodyMatProperties.MaskLayer > 0)
		{
			currentBodySprite.BodySprite.material.SetInt(PlayerMaterial.MaskLayer, bodyMatProperties.MaskLayer);
			longModeParts = currentBodySprite.LongModeParts;
			for (int i = 0; i < longModeParts.Length; i++)
			{
				longModeParts[i].material.SetInt(PlayerMaterial.MaskLayer, bodyMatProperties.MaskLayer);
			}
		}
		PlayerMaterial.SetColors(bodyMatProperties.ColorId, currentBodySprite.BodySprite);
		if (!loggedBodyMaterialDiagnostics)
		{
			Material assignedMaterial = currentBodySprite.BodySprite.sharedMaterial;
			string materialName = assignedMaterial != null ? assignedMaterial.name : "<null>";
			string shaderName = assignedMaterial != null && assignedMaterial.shader != null ? assignedMaterial.shader.name : "<null>";
			string spriteName = currentBodySprite.BodySprite.sprite != null ? currentBodySprite.BodySprite.sprite.name : "<null>";
			Logger.GlobalInstance.Error($"[PlayerRenderDiag] Body material assigned bodyType={currentBodySprite.Type}, local={localPlayer}, colorId={bodyMatProperties.ColorId}, maskType={bodyMatProperties.MaskType}, maskLayer={bodyMatProperties.MaskLayer}, material={materialName}, shader={shaderName}, sprite={spriteName}", this);
			loggedBodyMaterialDiagnostics = true;
		}
		longModeParts = currentBodySprite.LongModeParts;
		foreach (SpriteRenderer rend in longModeParts)
		{
			PlayerMaterial.SetColors(bodyMatProperties.ColorId, rend);
		}
		if (currentBodySprite != null && currentBodySprite.PettingHand != null)
		{
			PlayerMaterial.SetColors(bodyMatProperties.ColorId, currentBodySprite.PettingHand.HandSprite);
		}
		if (currentBodySprite != null && currentBodySprite.HandHat != null)
		{
			PlayerMaterial.SetColors(bodyMatProperties.ColorId, currentBodySprite.HandHat);
		}
	}

	public void ShowLongModeParts(bool show)
	{
		SpriteRenderer[] longModeParts = currentBodySprite.LongModeParts;
		for (int i = 0; i < longModeParts.Length; i++)
		{
			longModeParts[i].enabled = show;
		}
	}

	public void SetHatVisorVisible(bool isVisible)
	{
		hat.Visible = isVisible;
		visor.Visible = isVisible;
	}

	public void FadeBlackCosmetics(float duration)
	{
		StartCoroutine(Effects.Lerp(duration, (float t) =>
		{
			if ((bool)hat)
			{
				SetHatColor(Color.Lerp(Color.white, Color.black, t));
			}
			if ((bool)visor)
			{
				visor.SetVisorColor(Color.Lerp(Color.white, Color.black, t));
			}
			if ((bool)skin && bodyType.GetSkinOptions().Enabled)
			{
				skin.layer.color = Color.Lerp(Color.white, Color.black, t);
			}
			if ((bool)currentPet)
			{
				currentPet.SetRawColor(Color.Lerp(Color.white, Color.black, t));
			}
			currentBodySprite.BodySprite.color = Color.Lerp(Color.white, Color.black, t);
			SpriteRenderer[] longModeParts = currentBodySprite.LongModeParts;
			for (int i = 0; i < longModeParts.Length; i++)
			{
				longModeParts[i].color = Color.Lerp(Color.white, Color.black, t);
			}
		}));
	}

	public LongBoiPlayerBody GetLongBoi()
	{
		if (longboi == null)
		{
			longboi = currentBodySprite.BodySprite.GetComponent<LongBoiPlayerBody>();
		}
		return longboi;
	}

	public void UpdateCosmeticOffset(float headPosZ, bool isExiled)
	{
		hat.transform.localPosition = (isExiled ? (defaultHatVisorPosition - new Vector3(0f, 0.1f)) : defaultHatVisorPosition);
		visor.transform.localPosition = defaultHatVisorPosition;
		SetCosmeticZIndices();
		if (currentBodySprite.BodySprite.flipX)
		{
			hat.transform.localPosition += currentBodySprite.flippedCosmeticOffset;
			visor.transform.localPosition += currentBodySprite.flippedCosmeticOffset;
		}
		else
		{
			hat.transform.localPosition += currentBodySprite.normalCosmeticOffset;
			visor.transform.localPosition += currentBodySprite.normalCosmeticOffset;
		}
		hat.transform.localPosition = new Vector3(hat.transform.localPosition.x, hat.transform.localPosition.y, headPosZ);
		visor.transform.localPosition = new Vector3(visor.transform.localPosition.x, visor.transform.localPosition.y, headPosZ + visor.DesiredLocalZPosition);
	}

	public void AdjustCosmeticRotations(float rotOffset)
	{
		hat.transform.localEulerAngles += new Vector3(0f, 0f, rotOffset);
		visor.transform.localEulerAngles += new Vector3(0f, 0f, rotOffset);
		skin.transform.localEulerAngles += new Vector3(0f, 0f, rotOffset);
	}

	public List<AddressableAsset> GetAllLoadedAssets()
	{
		List<AddressableAsset> list = new List<AddressableAsset>();
		list.AddRange(this.GetAssetGroup().GetAssets());
		list.AddRange(visor.GetAssetGroup().GetAssets());
		list.AddRange(skin.GetAssetGroup().GetAssets());
		return list;
	}

	public void UnloadPet(AddressableAsset asset)
	{
		this.UnloadAddressableAsset(asset);
	}

	public void UnloadVisor(AddressableAsset asset)
	{
		visor.UnloadAddressableAsset(asset);
	}

	public void UnloadSkin(AddressableAsset asset)
	{
		skin.UnloadAddressableAsset(asset);
	}

	public void AnimateClimb(bool down)
	{
		skin.SetClimb(down);
		hat.SetClimbAnim();
		visor.SetClimbAnim(bodyType);
	}

	public void AnimatePetMourning()
	{
		if ((bool)currentPet)
		{
			currentPet.SetMourning();
		}
	}

	public void AnimateSkinJump()
	{
		skin.SetJump(FlipX);
	}

	public void AnimateSkinEnterVent()
	{
		skin.SetEnterVent(FlipX);
	}

	public void AnimateSkinExitVent()
	{
		skin.SetExitVent(FlipX);
	}

	public void AnimateSkinIdle()
	{
		skin.SetIdle(FlipX);
	}

	public void AnimateSkinRun()
	{
		skin.SetRun(FlipX);
	}

	public void AnimateSkinSpawn(float time = 0f)
	{
		skin.SetSpawn(FlipX, time);
	}

	public bool HasSkinLoaded()
	{
		return skin.IsLoaded;
	}

	public bool HasVisorLoaded()
	{
		return visor.IsLoaded;
	}

	public bool HaveAllCosmeticsLoaded()
	{
		if (visor.IsLoaded && skin.IsLoaded)
		{
			return hat.IsLoaded;
		}
		return false;
	}

	public void SetAsLocalPlayer()
	{
		localPlayer = true;
		PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(currentBodySprite.BodySprite, isLocalPlayer: true);
		hat.SetLocalPlayer(localPlayer: true);
		skin.SetLocalPlayer(localPlayer: true);
		visor.SetLocalPlayer(localPlayer: true);
		if (currentPet != null)
		{
			currentPet.SetAsLocalPlayer();
		}
		nameText.fontMaterial.SetFloat("_StencilComp", 7f);
		nameText.fontMaterial.SetFloat("_Stencil", 2f);
		colorBlindText.fontMaterial.SetFloat("_StencilComp", 7f);
		colorBlindText.fontMaterial.SetFloat("_Stencil", 2f);
	}

	public void UpdateBounceHatZipline()
	{
		hat.UpdateBounceHatZipline();
	}
}
