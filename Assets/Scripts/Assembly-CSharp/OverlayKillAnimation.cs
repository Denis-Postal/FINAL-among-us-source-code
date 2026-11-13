using System.Collections;
using System.Collections.Generic;
using AmongUs.GameOptions;
using PowerTools;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class OverlayKillAnimation : OverlayAnimation
{
	[SerializeField]
	private KillAnimType KillType;

	[SerializeField]
	protected PoolablePlayer killerParts;

	[SerializeField]
	private Vector3 KillerPetPosition = new Vector3(-2.068f, -0.37f, 0.2f);

	[SerializeField]
	protected PoolablePlayer victimParts;

	[SerializeField]
	private Vector3 VictimPetPosition = new Vector3(0.282f, -0.37f, 0.1f);

	[SerializeField]
	private AudioClip Stinger;

	[SerializeField]
	private AudioClip Sfx;

	[SerializeField]
	private float StingerVolume = 0.6f;

	protected KillOverlayInitData initData;

	[SerializeField]
	private bool leftFacingVictim;

	[SerializeField]
	private bool isClassicAnimation;

	private string victimHat;

	private HashSet<GameObject> petObjects;

	public bool LeftFacingVictim => leftFacingVictim;

	public bool IsClassicAnimation => isClassicAnimation;

	public virtual void Initialize(KillOverlayInitData initData)
	{
		this.initData = initData;
		if ((bool)killerParts)
		{
			if (KillType == KillAnimType.ViperKill)
			{
				killerParts.SetBodyType(PlayerBodyTypes.Normal);
			}
			else
			{
				killerParts.SetBodyType(initData.killerBodyType);
			}
			killerParts.UpdateFromPlayerOutfit(initData.killerOutfit, PlayerMaterial.MaskType.None, isDead: false, includePet: false, () =>
			{
				LoadKillerSkin(initData.killerOutfit);
			});
			killerParts.ToggleName(active: false);
			LoadKillerPet(initData.killerOutfit);
		}
		if (initData.victimOutfit != null && (bool)victimParts)
		{
			victimHat = initData.victimOutfit.HatId;
			if (KillType == KillAnimType.ViperKill || (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek && AprilFoolsMode.ShouldClassicMode()))
			{
				victimParts.SetBodyType(PlayerBodyTypes.Normal);
			}
			else
			{
				victimParts.SetBodyType(initData.victimBodyType);
			}
			victimParts.UpdateFromPlayerOutfit(initData.victimOutfit, PlayerMaterial.MaskType.None, isDead: false, includePet: false, () =>
			{
				LoadVictimSkin(initData.victimOutfit);
			});
			victimParts.SetHatLeftFacingVictim(leftFacingVictim);
			victimParts.ToggleName(active: false);
			LoadVictimPet(initData.victimOutfit);
		}
	}

	public void SetHatFloor()
	{
		HatData hatById = DestroyableSingleton<HatManager>.Instance.GetHatById(victimHat);
		if ((bool)hatById)
		{
			victimParts.SetHatWithoutChangingColor(hatById);
			victimParts.SetHatOnFloor();
		}
	}

	public void SetVisorFloor()
	{
		victimParts.SetVisorOnFloor();
	}

	public void PlayKillSound()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(Sfx, loop: false).volume = 0.8f;
			VibrationManager.Vibrate(3f, 3f, 0f, VibrationManager.VibrationFalloff.None, Sfx);
			DualshockLightManager.Flash(Color.red, 2f, Sfx);
		}
	}

	public override IEnumerator CoShow(KillOverlay parent)
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(Stinger, loop: false).volume = StingerVolume;
		}
		petObjects = new HashSet<GameObject>();
		SpriteAnim[] componentsInChildren = GetComponentsInChildren<SpriteAnim>();
		foreach (SpriteAnim spriteAnim in componentsInChildren)
		{
			if (spriteAnim.GetComponent<PetBehaviour>() != null)
			{
				petObjects.Add(spriteAnim.gameObject);
			}
		}
		parent.background.enabled = true;
		yield return Effects.Wait(1f / 12f);
		parent.background.enabled = false;
		parent.flameParent.SetActive(value: true);
		parent.flameParent.transform.localScale = new Vector3(1f, 0.3f, 1f);
		parent.flameParent.transform.localEulerAngles = new Vector3(0f, 0f, 25f);
		yield return Effects.Wait(1f / 12f);
		parent.flameParent.transform.localScale = new Vector3(1f, 0.5f, 1f);
		parent.flameParent.transform.localEulerAngles = new Vector3(0f, 0f, -15f);
		yield return Effects.Wait(1f / 12f);
		parent.flameParent.transform.localScale = new Vector3(1f, 1f, 1f);
		parent.flameParent.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		gameObject.SetActive(value: true);
		yield return WaitForFinish();
		gameObject.SetActive(value: false);
		yield return new WaitForLerp(1f / 6f, (float t) =>
		{
			parent.flameParent.transform.localScale = new Vector3(1f, 1f - t, 1f);
		});
		parent.flameParent.SetActive(value: false);
	}

	private IEnumerator WaitForFinish()
	{
		SpriteAnim[] anims = GetComponentsInChildren<SpriteAnim>();
		if (anims.Length == 0)
		{
			yield return new WaitForSeconds(1f);
			yield break;
		}
		while (true)
		{
			bool flag = false;
			for (int i = 0; i < anims.Length; i++)
			{
				if (!petObjects.Contains(anims[i].gameObject) && anims[i].IsPlaying())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				yield return null;
				continue;
			}
			break;
		}
	}

	private void LoadVictimSkin(NetworkedPlayerInfo.PlayerOutfit outfit)
	{
		if (killerParts.CurrentBodyType == PlayerBodyTypes.Classic && KillType != KillAnimType.ViperKill)
		{
			return;
		}
		SkinData skinById = DestroyableSingleton<HatManager>.Instance.GetSkinById(outfit.SkinId);
		if ((bool)skinById && !skinById.IsEmpty)
		{
			SkinViewData skinView = victimParts.GetSkinView();
			SpriteAnim skinSpriteAnim = victimParts.GetSkinSpriteAnim();
			switch (KillType)
			{
			case KillAnimType.RHM:
				skinSpriteAnim.Play(skinView.KillRHMVictim);
				break;
			case KillAnimType.Tongue:
				skinSpriteAnim.Play(skinView.KillTongueVictim);
				break;
			case KillAnimType.Stab:
				skinSpriteAnim.Play(skinView.KillStabVictim);
				break;
			case KillAnimType.Neck:
				skinSpriteAnim.Play(skinView.KillNeckVictim);
				break;
			case KillAnimType.Shoot:
				skinSpriteAnim.Play(skinView.KillShootVictim);
				break;
			case KillAnimType.Werewolf_Slash:
				skinSpriteAnim.Play(skinView.IdleLeftAnim);
				break;
			case KillAnimType.ViperKill:
				skinSpriteAnim.Play(skinView.IdleLeftAnim);
				break;
			}
		}
	}

	private void LoadKillerSkin(NetworkedPlayerInfo.PlayerOutfit outfit)
	{
		if (killerParts.CurrentBodyType == PlayerBodyTypes.Classic && KillType != KillAnimType.ViperKill)
		{
			return;
		}
		SkinData skinById = DestroyableSingleton<HatManager>.Instance.GetSkinById(outfit.SkinId);
		if ((bool)skinById && !skinById.IsEmpty)
		{
			SkinViewData skinView = killerParts.GetSkinView();
			SpriteAnim skinSpriteAnim = killerParts.GetSkinSpriteAnim();
			switch (KillType)
			{
			case KillAnimType.Tongue:
			case KillAnimType.ViperKill:
				skinSpriteAnim.Play(skinView.KillTongueImpostor);
				break;
			case KillAnimType.Shoot:
				skinSpriteAnim.Play(skinView.KillShootImpostor);
				break;
			case KillAnimType.Neck:
				skinSpriteAnim.Play(skinView.KillNeckImpostor);
				break;
			case KillAnimType.Stab:
				skinSpriteAnim.Play(skinView.KillStabImpostor);
				break;
			case KillAnimType.RHM:
			case KillAnimType.Werewolf_Slash:
				break;
			}
		}
	}

	private void LoadKillerPet(NetworkedPlayerInfo.PlayerOutfit outfit)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		PetData petById = DestroyableSingleton<HatManager>.Instance.GetPetById(outfit.PetId);
		if (!petById || petById.IsEmpty)
		{
			return;
		}
		PetBehaviour petBehaviour = null;
		if (ShipStatus.Instance != null)
		{
			petBehaviour = ShipStatus.Instance.CosmeticsCache.GetPet(outfit.PetId);
		}
		else
		{
			if (petById.PetPrefabRef == null)
			{
				return;
			}
			petBehaviour = Addressables.LoadAssetAsync<GameObject>((object)petById.PetPrefabRef).WaitForCompletion().GetComponent<PetBehaviour>();
		}
		killerParts.SetPetIdle(petBehaviour, outfit.ColorId);
		killerParts.ReplacePetParent(transform, KillerPetPosition);
	}

	private void LoadVictimPet(NetworkedPlayerInfo.PlayerOutfit outfit)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		PetData petById = DestroyableSingleton<HatManager>.Instance.GetPetById(outfit.PetId);
		if (!petById || petById.IsEmpty)
		{
			return;
		}
		PetBehaviour petBehaviour = null;
		if (ShipStatus.Instance != null)
		{
			petBehaviour = ShipStatus.Instance.CosmeticsCache.GetPet(outfit.PetId);
		}
		else
		{
			if (petById.PetPrefabRef == null)
			{
				return;
			}
			petBehaviour = Addressables.LoadAssetAsync<GameObject>((object)petById.PetPrefabRef).WaitForCompletion().GetComponent<PetBehaviour>();
		}
		victimParts.SetPetScared(petBehaviour, outfit.ColorId);
		victimParts.SetPetFlipX(flipped: true);
		Vector3 victimPetPosition = VictimPetPosition;
		Debug.Log("pet kill test: " + victimPetPosition);
		victimParts.ReplacePetParent(transform, VictimPetPosition);
	}

	private void OnDestroy()
	{
		petObjects = null;
	}
}
