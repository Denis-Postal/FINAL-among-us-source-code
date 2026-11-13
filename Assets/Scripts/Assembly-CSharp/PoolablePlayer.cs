using System;
using AmongUs.Data;
using PowerTools;
using UnityEngine;

public class PoolablePlayer : MonoBehaviour
{
	public SpriteRenderer[] Hands;

	public SpriteRenderer[] OtherBodySprites;

	[SerializeField]
	private CosmeticsLayer cosmetics;

	private PlayerBodyTypes bodyType;

	[SerializeField]
	public bool ignoreLongMode;

	public CosmeticsLayer Cosmetics => cosmetics;

	public string ColorBlindName => cosmetics.GetColorBlindText();

	public PlayerBodyTypes CurrentBodyType => cosmetics.CurrentBodyType;

	public int ColorId => cosmetics.ColorId;

	public bool GetIgnoreLongMode()
	{
		return ignoreLongMode;
	}

	public void SetBodyCosmeticsVisible(bool b)
	{
		cosmetics.SetBodyCosmeticsVisible(b);
	}

	public void ToggleHat(bool b)
	{
		cosmetics.ToggleHat(b);
	}

	public void SetCustomHatPosition(Vector3 pos)
	{
		cosmetics.SetCustomHatPosition(pos);
	}

	public void SetCustomVisorPosition(Vector3 pos)
	{
		cosmetics.SetCustomVisorPosition(pos);
	}

	public bool HasHat()
	{
		return cosmetics.HasHat();
	}

	public SpriteRenderer GetHandHat()
	{
		return cosmetics.HatHand;
	}

	private void OnEnable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged += SetColorBlindTag;
	}

	private void OnDisable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged -= SetColorBlindTag;
	}

	private void SetColorBlindTag()
	{
		cosmetics.SetEnabledColorblind(DataManager.Settings.Accessibility.ColorBlindMode);
	}

	public void ResetCosmetics(string ignoreType = "")
	{
		cosmetics.ResetCosmetics(ignoreType);
	}

	public SpriteAnim GetSkinSpriteAnim()
	{
		return cosmetics.GetSkinSpriteAnim();
	}

	public void SetMaskLayer(int layer)
	{
		cosmetics.SetMaskLayer(layer);
	}

	public void SetHatLeftFacingVictim(bool leftFacingVictim)
	{
		cosmetics.SetHatLeftFacingVictim(leftFacingVictim);
	}

	public void SetHatOnFloor()
	{
		cosmetics.SetHatOnFloor();
	}

	public void SetVisorOnFloor()
	{
		cosmetics.SetVisorOnFloor();
	}

	public void FlipHatAndVisor()
	{
		cosmetics.SetHatOnFloor();
	}

	public void SetHatAndVisorMaskType(PlayerMaterial.MaskType type)
	{
		cosmetics.SetHatAndVisorMaskType(type);
	}

	public void SetVisor(string visorId, int color)
	{
		cosmetics.SetVisor(visorId, color);
	}

	public void SetVisor(VisorData visorData, int color)
	{
		cosmetics.SetVisor(visorData, color);
	}

	public void SetHatWithoutChangingColor(HatData hatData)
	{
		cosmetics.SetHatWithoutChangingColor(hatData);
	}

	public void SetHat(string hatId, int color)
	{
		cosmetics.SetHat(hatId, color);
	}

	public void SetHat(HatData hatData, int color)
	{
		cosmetics.SetHat(hatData, color);
	}

	public void SetNameScale(Vector3 scale)
	{
		cosmetics.SetNameScale(scale);
	}

	public void SetName(string nameStr, Vector3 scale, Color color, float zPosition)
	{
		cosmetics.SetName(nameStr, scale, color, zPosition);
	}

	public void SetName(string nameStr)
	{
		cosmetics.SetName(nameStr);
	}

	public void SetNameColor(Color color)
	{
		cosmetics.SetNameColor(color);
	}

	public void SetNamePosition(Vector3 newPos)
	{
		cosmetics.SetNamePosition(newPos);
	}

	public void SetBodyAsGhost()
	{
		cosmetics.SetBodyAsGhost();
	}

	public void SetBodyColor(int color)
	{
		cosmetics.SetBodyColor(color);
	}

	public void FixSkinSprite(Sprite skinSprite)
	{
		cosmetics.FixSkinSprite(skinSprite);
	}

	public void SetSkin(string skinId, int color)
	{
		cosmetics.SetSkin(skinId, color);
	}

	public void SetSkin(SkinData skin, int color)
	{
		cosmetics.SetSkin(skin, color);
	}

	public void ReplacePetParent(Transform newParent, Vector3 newPosition)
	{
		cosmetics.ReplacePetParent(newParent, newPosition);
	}

	public void ToggleName(bool active)
	{
		cosmetics.ToggleName(active);
	}

	public void TogglePet(bool active)
	{
		cosmetics.TogglePet(active);
	}

	public void SetPetColor(int color)
	{
		cosmetics.GetPet()?.SetCrewmateColor(color);
	}

	public void SetPetIdle(string petId, int colorId)
	{
		SetPetIdle(DestroyableSingleton<HatManager>.Instance.GetPetById(petId), colorId);
	}

	public void SetPetIdle(PetData petData, int colorId)
	{
		cosmetics.SetPetIdle(petData, colorId);
	}

	public void SetPetIdle(PetBehaviour petBehaviour, int colorId)
	{
		cosmetics.SetPetIdle(petBehaviour, colorId);
	}

	public void SetPetScared(PetBehaviour petBehaviour, int colorId)
	{
		cosmetics.SetPetScared(petBehaviour, colorId);
	}

	public void SetFlipX(bool flipped)
	{
		cosmetics.SetFlipX(flipped);
	}

	public void SetDeadFlipX(bool flipped)
	{
		cosmetics.SetDeadFlipX(flipped);
	}

	public void SetPetFlipX(bool flipped)
	{
		cosmetics.SetPetFlipX(flipped);
	}

	public void SetBodyType(PlayerBodyTypes bodyType)
	{
		cosmetics.EnsureInitialized(bodyType);
	}

	public void UpdateFromLocalPlayer(PlayerMaterial.MaskType maskType)
	{
		UpdateFromEitherPlayerDataOrCache(PlayerControl.LocalPlayer.Data, PlayerOutfitType.Default, maskType, includePet: true);
	}

	public void UpdateFromDataManager(PlayerMaterial.MaskType maskType)
	{
		InitBody();
		cosmetics.SetMaskType(maskType);
		cosmetics.SetBodyColor(DataManager.Player.Customization.Color);
		cosmetics.SetSkin(DataManager.Player.Customization.Skin, DataManager.Player.Customization.Color);
		cosmetics.SetPetIdle(DestroyableSingleton<HatManager>.Instance.GetPetById(DataManager.Player.Customization.Pet), DataManager.Player.Customization.Color);
		cosmetics.SetHat(DataManager.Player.Customization.Hat, DataManager.Player.Customization.Color);
		if (DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat).BlocksVisors)
		{
			DataManager.Player.Customization.Visor = "visor_EmptyVisor";
		}
		cosmetics.SetVisor(DataManager.Player.Customization.Visor, DataManager.Player.Customization.Color);
		SetColorBlindTag();
	}

	public void UpdateFromDataManager(PlayerMaterial.MaskType maskType, int colorId)
	{
		InitBody();
		cosmetics.SetMaskType(maskType);
		cosmetics.SetBodyColor(colorId);
		cosmetics.SetSkin(DataManager.Player.Customization.Skin, colorId);
		cosmetics.SetPetIdle(DestroyableSingleton<HatManager>.Instance.GetPetById(DataManager.Player.Customization.Pet), colorId);
		cosmetics.SetHat(DataManager.Player.Customization.Hat, colorId);
		if (DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat).BlocksVisors)
		{
			DataManager.Player.Customization.Visor = "visor_EmptyVisor";
		}
		cosmetics.SetVisor(DataManager.Player.Customization.Visor, colorId);
		SetColorBlindTag();
	}

	public void UpdateFromEitherPlayerDataOrCache(NetworkedPlayerInfo pData, PlayerOutfitType outfitType, PlayerMaterial.MaskType maskType, bool includePet, Action OnSkinLoaded = null)
	{
		UpdateFromPlayerData(pData, outfitType, maskType, includePet, OnSkinLoaded);
	}

	public void UpdateFromPlayerData(NetworkedPlayerInfo pData, PlayerOutfitType outfitType, PlayerMaterial.MaskType maskType, bool includePet, Action OnSkinLoaded = null, bool forceAlive = false)
	{
		NetworkedPlayerInfo.PlayerOutfit outfit = pData.Outfits[outfitType];
		UpdateFromPlayerOutfit(outfit, maskType, pData.IsDead, includePet, OnSkinLoaded, forceAlive);
	}

	public void UpdateFromPlayerOutfit(NetworkedPlayerInfo.PlayerOutfit outfit, PlayerMaterial.MaskType maskType, bool isDead, bool includePet, Action OnSkinLoaded = null, bool forceAlive = false)
	{
		cosmetics.SetMaskType(maskType);
		cosmetics.SetBodyColor(outfit.ColorId);
		Hands.ForEach((SpriteRenderer b) =>
		{
			b.sharedMaterial = CosmeticsLayer.GetBodyMaterial(maskType);
			PlayerMaterial.SetColors(outfit.ColorId, b);
		});
		OtherBodySprites.ForEach((SpriteRenderer b) =>
		{
			b.sharedMaterial = CosmeticsLayer.GetBodyMaterial(maskType);
			PlayerMaterial.SetColors(outfit.ColorId, b);
		});
		if (isDead && !forceAlive)
		{
			cosmetics.SetSkin(string.Empty, outfit.ColorId, OnSkinLoaded);
			cosmetics.SetHatColor(Palette.HalfWhite);
			cosmetics.SetVisorAlpha(Palette.HalfWhite.a);
		}
		else
		{
			cosmetics.SetSkin(outfit.SkinId, outfit.ColorId, OnSkinLoaded);
			cosmetics.SetHatColor(Palette.White);
			cosmetics.SetVisorAlpha(Palette.White.a);
		}
		cosmetics.SetHat(outfit.HatId, outfit.ColorId);
		cosmetics.SetVisor(outfit.VisorId, outfit.ColorId);
		if (includePet)
		{
			cosmetics.SetPetIdle(DestroyableSingleton<HatManager>.Instance.GetPetById(outfit.PetId), outfit.ColorId);
		}
		SetColorBlindTag();
	}

	private void Awake()
	{
		InitBody();
		if (bodyType != PlayerBodyTypes.Normal && bodyType != PlayerBodyTypes.Classic && !cosmetics.AlwaysDrawNormalPlayer)
		{
			Hands.ForEach((SpriteRenderer h) =>
			{
				h.enabled = false;
			});
		}
	}

	private void InitBody()
	{
		if (AprilFoolsMode.ShouldHorseAround())
		{
			bodyType = PlayerBodyTypes.Horse;
		}
		else if (AprilFoolsMode.ShouldLongAround() && !ignoreLongMode)
		{
			bodyType = PlayerBodyTypes.Long;
		}
		else if (AprilFoolsMode.ShouldClassicMode())
		{
			bodyType = PlayerBodyTypes.Classic;
		}
		else
		{
			bodyType = PlayerBodyTypes.Normal;
		}
		cosmetics?.EnsureInitialized(bodyType);
	}

	public void HideHatAndVisor_AnimEvent()
	{
		cosmetics.SetHatVisorVisible(isVisible: false);
	}

	public void FadeBlackAll(float duration)
	{
		cosmetics.FadeBlackCosmetics(duration);
	}

	public SkinViewData GetSkinView()
	{
		return cosmetics.GetSkinView();
	}
}
