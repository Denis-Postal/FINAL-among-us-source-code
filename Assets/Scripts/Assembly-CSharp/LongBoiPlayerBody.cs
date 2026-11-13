using Object = UnityEngine.Object;
using AmongUs.GameOptions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using InnerNet;
using UnityEngine;

public class LongBoiPlayerBody : MonoBehaviour
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Predicate<PlayerControl> _003C_003E9__33_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CHideAndSeekUpdate_003Eb__33_0(PlayerControl p)
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}
	}

	[SerializeField]
	private SpriteRenderer neckSprite;

	[SerializeField]
	private SpriteRenderer foregroundNeckSprite;

	[SerializeField]
	private SpriteRenderer headSprite;

	[SerializeField]
	private PlayerControl myPlayerControl;

	[SerializeField]
	private PoolablePlayer myPoolablePlayer;

	[SerializeField]
	private CosmeticsLayer cosmeticLayer;

	[SerializeField]
	private float neckHeadZPos = -3.9f;

	[SerializeField]
	private float cosmeticYOffset = 6f;

	[SerializeField]
	private bool isPoolablePlayer;

	[SerializeField]
	private bool isExiledPlayer;

	[SerializeField]
	private float targetHeight = 3f;

	[SerializeField]
	private bool skipNeckAnim;

	[SerializeField]
	private bool isSeekerHorse;

	[SerializeField]
	private GameObject neckHeadParent;

	[SerializeField]
	private bool hideCosmeticsQC;

	[SerializeField]
	private float growSpeed = 0.2f;

	private PlayerControl hideAndSeekImpostor;

	private float calculatedNeckHeight;

	private bool ShouldLongAround;

	private float[] heightsPerColor = new float[18]
	{
		3f, 1.3f, 2.2f, 1.6f, 2f, 3.2f, 4.3f, 2.4f, 2.5f, 4f,
		1.9f, 3.5f, 3.1f, 2.8f, 3.4f, 3.7f, 0.9f, 4.6f
	};

	private const float BASE_NECK_MAX_HEIGHT = 1.1f;

	private const float GHOST_NECK_WIDTH = 3.17f;

	private const float GHOST_NECK_YPOS = 0.531f;

	private const float NAME_POS_Y = 0.45f;

	public const float HAT_Y_OFFSET = 0.1f;

	private const float Z_ROTATION_OFFSET = -17.75f;

	private void Awake()
	{
		if (AprilFoolsMode.ShouldLongAround())
		{
			cosmeticLayer.OnSetBodyAsGhost += SetPoolableGhost;
			cosmeticLayer.OnColorChange += SetHeightFromColor;
			cosmeticLayer.OnCosmeticSet += OnCosmeticSet;
		}
	}

	private void OnDestroy()
	{
		if (AprilFoolsMode.ShouldLongAround())
		{
			cosmeticLayer.OnSetBodyAsGhost -= SetPoolableGhost;
			cosmeticLayer.OnColorChange -= SetHeightFromColor;
			cosmeticLayer.OnCosmeticSet -= OnCosmeticSet;
		}
	}

	private void Start()
	{
		if (!AprilFoolsMode.ShouldLongAround())
		{
			ShouldLongAround = false;
			headSprite.gameObject.SetActive(value: false);
			neckSprite.gameObject.SetActive(value: false);
			foregroundNeckSprite.gameObject.SetActive(value: false);
			return;
		}
		ShouldLongAround = true;
		if (hideCosmeticsQC)
		{
			cosmeticLayer.SetHatVisorVisible(isVisible: false);
		}
		SetupNeckGrowth();
		if (isExiledPlayer)
		{
			ShipStatus instance = ShipStatus.Instance;
			if ((object)instance == null || instance.Type != ShipStatus.MapType.Fungle)
			{
				cosmeticLayer.AdjustCosmeticRotations(-17.75f);
			}
		}
		if (!isPoolablePlayer)
		{
			cosmeticLayer.ValidateCosmetics();
		}
	}

	public bool ValidateSkin(string prodId, int colorID)
	{
		return DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(prodId);
	}

	private void OnCosmeticSet(string prodId, int colorID, CosmeticsLayer.CosmeticKind cosmeticType)
	{
		if (DestroyableSingleton<HatManager>.Instance.CheckLongModeValidCosmetic(prodId))
		{
			return;
		}
		switch (cosmeticType)
		{
		case CosmeticsLayer.CosmeticKind.HAT:
			if (!isPoolablePlayer)
			{
				myPlayerControl.SetHat("hat_NoHat", colorID);
			}
			else
			{
				myPoolablePlayer.SetHat("hat_NoHat", colorID);
			}
			break;
		case CosmeticsLayer.CosmeticKind.SKIN:
			if (!isPoolablePlayer)
			{
				myPlayerControl.SetSkin("skin_None", colorID);
			}
			else
			{
				myPoolablePlayer.SetSkin("skin_None", colorID);
			}
			break;
		case CosmeticsLayer.CosmeticKind.VISOR:
			if (!isPoolablePlayer)
			{
				myPlayerControl.SetVisor("visor_EmptyVisor", colorID);
			}
			else
			{
				myPoolablePlayer.SetVisor("visor_EmptyVisor", colorID);
			}
			break;
		case CosmeticsLayer.CosmeticKind.PET:
			if (!isPoolablePlayer)
			{
				myPlayerControl.SetPet("pet_EmptyPet", colorID);
			}
			else
			{
				myPoolablePlayer.SetPetIdle("pet_EmptyPet", colorID);
			}
			break;
		}
	}

	public void SetHeightFromColor(int colorIndex)
	{
		if (!isPoolablePlayer && (!GameManager.Instance.IsHideAndSeek() || AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started || !(myPlayerControl.Data.Role != null) || myPlayerControl.Data.Role.TeamType != RoleTeamTypes.Impostor))
		{
			targetHeight = heightsPerColor[colorIndex];
			if ((bool)LobbyBehaviour.Instance)
			{
				SetupNeckGrowth(snapNeck: false, resetNeck: false);
			}
			else
			{
				SetupNeckGrowth(snapNeck: true, resetNeck: false);
			}
		}
	}

	public void SetHeighFromDistanceHnS(float distance)
	{
		if (!isPoolablePlayer)
		{
			targetHeight = Mathf.Clamp(distance / 10f + 0.5f, 1.1f, 5.5f);
			SetupNeckGrowth(snapNeck: true);
		}
	}

	private unsafe void HideAndSeekUpdate()
	{
		if (!myPlayerControl.AmOwner)
		{
			return;
		}
		if (hideAndSeekImpostor == null)
		{
			hideAndSeekImpostor = PlayerControl.AllPlayerControls.Find((PlayerControl p) => p.Data.RoleType == RoleTypes.Impostor);
		}
		if (hideAndSeekImpostor == null)
		{
			return;
		}
		foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
		{
			if (allPlayerControl.Data?.Role != null && !allPlayerControl.Data.Role.IsImpostor)
			{
				allPlayerControl.cosmetics.GetLongBoi().SetHeighFromDistanceHnS((hideAndSeekImpostor.transform.position - allPlayerControl.transform.position).sqrMagnitude);
			}
		}
	}

	private void SetupNeckGrowth(bool snapNeck = false, bool resetNeck = true)
	{
		if ((bool)LobbyBehaviour.Instance && !isPoolablePlayer)
		{
			skipNeckAnim = false;
		}
		if (skipNeckAnim | snapNeck)
		{
			neckSprite.size = new Vector2(neckSprite.size.x, Mathf.Clamp(targetHeight, 0f, 1.1f));
			foregroundNeckSprite.size = new Vector2(foregroundNeckSprite.size.x, Mathf.Clamp(targetHeight - 1.1f, 0f, 100f));
		}
		else if (resetNeck)
		{
			ResetNeck();
		}
	}

	private unsafe void LateUpdate()
	{
		if (!ShouldLongAround)
		{
			return;
		}
		foregroundNeckSprite.transform.localPosition = new Vector3(foregroundNeckSprite.transform.localPosition.x, neckSprite.size.y, foregroundNeckSprite.transform.localPosition.z);
		headSprite.transform.localPosition = new Vector3(headSprite.transform.localPosition.x, neckSprite.transform.localPosition.y + (foregroundNeckSprite.size.y + neckSprite.size.y) - 0.01f, headSprite.transform.localPosition.z);
		cosmeticLayer.NormalCosmeticOffset = new Vector3(cosmeticLayer.NormalCosmeticOffset.x, cosmeticYOffset + neckSprite.transform.localPosition.y + (foregroundNeckSprite.size.y + neckSprite.size.y), cosmeticLayer.NormalCosmeticOffset.z);
		cosmeticLayer.FlippedCosmeticOffset = new Vector3(cosmeticLayer.FlippedCosmeticOffset.x, cosmeticYOffset + neckSprite.transform.localPosition.y + (foregroundNeckSprite.size.y + neckSprite.size.y), cosmeticLayer.FlippedCosmeticOffset.z);
		foregroundNeckSprite.sprite = neckSprite.sprite;
		foregroundNeckSprite.size = new Vector2(neckSprite.size.x, foregroundNeckSprite.size.y);
		foregroundNeckSprite.flipX = neckSprite.flipX;
		if (!isPoolablePlayer)
		{
			if ((bool)GameManager.Instance && GameManager.Instance.IsHideAndSeek() && GameManager.Instance.GameHasStarted)
			{
				HideAndSeekUpdate();
			}
			myPlayerControl.cosmetics.SetNamePosition(new Vector3(0f, 0.45f, neckHeadZPos - 1E-05f));
			if (!myPlayerControl.MyPhysics.Animations.IsPlayingSpawnAnimation())
			{
				if (!skipNeckAnim)
				{
					GrowNeck();
				}
			}
			else
			{
				neckSprite.size = new Vector2(neckSprite.size.x, 0f);
				foregroundNeckSprite.size = new Vector2(foregroundNeckSprite.size.x, 0f);
			}
			if (isSeekerHorse)
			{
				if (cosmeticLayer.FlipX)
				{
					neckSprite.transform.localScale = new Vector3(0f - Math.Abs(neckSprite.transform.localScale.x), neckSprite.transform.localScale.y, neckSprite.transform.localScale.z);
					headSprite.transform.localScale = new Vector3(0f - Math.Abs(headSprite.transform.localScale.x), headSprite.transform.localScale.y, headSprite.transform.localScale.z);
					neckHeadParent.transform.localScale = new Vector3(0f - Math.Abs(neckHeadParent.transform.localScale.x), neckHeadParent.transform.localScale.y, neckHeadParent.transform.localScale.z);
				}
				else
				{
					neckSprite.transform.localScale = new Vector3(Math.Abs(neckSprite.transform.localScale.x), neckSprite.transform.localScale.y, neckSprite.transform.localScale.z);
					headSprite.transform.localScale = new Vector3(Math.Abs(headSprite.transform.localScale.x), headSprite.transform.localScale.y, headSprite.transform.localScale.z);
					neckHeadParent.transform.localScale = new Vector3(Math.Abs(neckHeadParent.transform.localScale.x), neckHeadParent.transform.localScale.y, neckHeadParent.transform.localScale.z);
				}
			}
			float num = 0f;
			if (neckSprite.size.y >= 1.1f && foregroundNeckSprite.size.y >= 0.712f)
			{
				num = neckHeadZPos;
			}
			foregroundNeckSprite.gameObject.transform.localPosition = new Vector3(foregroundNeckSprite.gameObject.transform.localPosition.x, foregroundNeckSprite.gameObject.transform.localPosition.y, num);
			headSprite.gameObject.transform.localPosition = new Vector3(headSprite.gameObject.transform.localPosition.x, headSprite.gameObject.transform.localPosition.y, num);
			myPlayerControl.MyPhysics.Animations.UpdateCosmeticOffset(new Vector3(cosmeticLayer.FlippedCosmeticOffset.x, cosmeticLayer.FlippedCosmeticOffset.y, cosmeticLayer.FlippedCosmeticOffset.z), new Vector3(cosmeticLayer.NormalCosmeticOffset.x, cosmeticLayer.NormalCosmeticOffset.y, cosmeticLayer.NormalCosmeticOffset.z));
			cosmeticLayer.UpdateCosmeticOffset(num, isExiledPlayer);
		}
		else
		{
			if (!skipNeckAnim)
			{
				GrowNeck();
			}
			else
			{
				neckSprite.size = new Vector2(neckSprite.size.x, Mathf.Clamp(targetHeight, 0f, 1.1f));
				foregroundNeckSprite.size = new Vector2(foregroundNeckSprite.size.x, Mathf.Clamp(targetHeight - 1.1f, 0f, 100f));
				headSprite.transform.localPosition = new Vector3(headSprite.transform.localPosition.x, neckSprite.transform.localPosition.y + (foregroundNeckSprite.size.y + neckSprite.size.y) - 0.01f, headSprite.transform.localPosition.z);
			}
			cosmeticLayer.UpdateCosmeticOffset(0f, isExiledPlayer);
		}
	}

	private void GrowNeck()
	{
		if ((neckSprite.size.y < 1.1f && targetHeight > neckSprite.size.y) || (foregroundNeckSprite.size.y <= 0f && targetHeight < 1.1f))
		{
			calculatedNeckHeight = Mathf.MoveTowards(neckSprite.size.y, targetHeight, growSpeed * Time.deltaTime);
			neckSprite.size = new Vector2(neckSprite.size.x, Mathf.Clamp(calculatedNeckHeight, 0f, 1.1f));
			foregroundNeckSprite.size = new Vector2(foregroundNeckSprite.size.x, 0f);
		}
		else
		{
			calculatedNeckHeight = Mathf.MoveTowards(foregroundNeckSprite.size.y, targetHeight - 1.1f, growSpeed * Time.deltaTime);
			foregroundNeckSprite.size = new Vector2(foregroundNeckSprite.size.x, calculatedNeckHeight);
		}
	}

	private void ResetNeck()
	{
		neckSprite.size = new Vector2(neckSprite.size.x, 0f);
		foregroundNeckSprite.size = new Vector2(foregroundNeckSprite.size.x, 0f);
	}

	public unsafe void SetPoolableGhost()
	{
		neckSprite.size = new Vector2(3.17f, targetHeight);
		neckSprite.transform.localPosition = new Vector3(neckSprite.transform.localPosition.x, 0.531f, neckSprite.transform.localPosition.z);
	}
}
