using System;
using System.Collections;
using Innersloth.Assets;
using PowerTools;
using UnityEngine;

public class SkinLayer : MonoBehaviour
{
	private readonly int JUMP_ZIPLINE_FRAME_STOP = 5;

	public SpriteRenderer layer;

	public SpriteAnim animator;

	public SkinViewData skin;

	public SkinData data;

	private PlayerMaterial.Properties matProperties;

	public bool IsLoaded => skin != null;

	public bool Flipped
	{
		set
		{
			if (!skin)
			{
				layer.flipX = value;
			}
			else
			{
				layer.flipX = value && animator.Clip != skin.IdleLeftAnim && animator.Clip != skin.RunLeftAnim && animator.Clip != skin.EnterLeftVentAnim && animator.Clip != skin.ExitLeftVentAnim && animator.Clip != skin.SpawnLeftAnim;
			}
		}
	}

	public bool Visible
	{
		set
		{
			layer.enabled = value;
		}
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

	public void SetLocalPlayer(bool localPlayer)
	{
		matProperties.IsLocalPlayer = localPlayer;
		UpdateMaterial();
	}

	public void SetRun(bool isLeft)
	{
		if (!skin || !animator)
		{
			SetGhost();
			return;
		}
		if (isLeft && (bool)skin.RunLeftAnim)
		{
			if (!animator.IsPlaying(skin.RunLeftAnim))
			{
				animator.Play(skin.RunLeftAnim);
				animator.Time = 11f / 24f;
			}
		}
		else if (!animator.IsPlaying(skin.RunAnim))
		{
			animator.Play(skin.RunAnim);
			animator.Time = 11f / 24f;
		}
		Flipped = isLeft;
	}

	public void SetSpawn(bool isLeft, float time = 0f)
	{
		if (!skin || !animator)
		{
			SetGhost();
			return;
		}
		if (isLeft && (bool)skin.SpawnLeftAnim)
		{
			animator.Play(skin.SpawnLeftAnim);
		}
		else
		{
			animator.Play(skin.SpawnAnim);
		}
		Flipped = isLeft;
		animator.Time = time;
	}

	public void SetClimb(bool down)
	{
		if (!skin || !animator)
		{
			SetGhost();
			return;
		}
		animator.Play(down ? skin.ClimbDownAnim : skin.ClimbAnim);
		animator.Time = 0f;
	}

	public void SetExitVent(bool isLeft)
	{
		if (!skin || !animator)
		{
			SetGhost();
			return;
		}
		if (isLeft && (bool)skin.ExitLeftVentAnim)
		{
			animator.Play(skin.ExitLeftVentAnim);
		}
		else
		{
			animator.Play(skin.ExitVentAnim);
		}
		Flipped = isLeft;
		animator.Time = 0f;
	}

	public void SetJump(bool isLeft)
	{
		if (!skin || !animator)
		{
			SetGhost();
			return;
		}
		if (isLeft && (bool)skin.EnterLeftVentAnim)
		{
			animator.Play(skin.EnterLeftVentAnim);
		}
		else
		{
			animator.Play(skin.EnterVentAnim);
		}
		Flipped = isLeft;
		animator.Time = 0f;
		StartCoroutine(CoCheckJumpAnim());
	}

	private IEnumerator CoCheckJumpAnim()
	{
		while (animator.FrameTime < JUMP_ZIPLINE_FRAME_STOP)
		{
			yield return null;
		}
		animator.Stop();
	}

	public void SetEnterVent(bool isLeft)
	{
		if (!skin || !animator)
		{
			SetGhost();
			return;
		}
		if (isLeft && (bool)skin.EnterLeftVentAnim)
		{
			animator.Play(skin.EnterLeftVentAnim);
		}
		else
		{
			animator.Play(skin.EnterVentAnim);
		}
		Flipped = isLeft;
		animator.Time = 0f;
	}

	public void SetIdle(bool isLeft)
	{
		if (!skin || !animator)
		{
			SetGhost();
			return;
		}
		if (isLeft && (bool)skin.IdleLeftAnim)
		{
			if (!animator.IsPlaying(skin.IdleLeftAnim))
			{
				animator.Play(skin.IdleLeftAnim);
			}
		}
		else if (!animator.IsPlaying(skin.IdleAnim))
		{
			animator.Play(skin.IdleAnim);
		}
		Flipped = isLeft;
	}

	public void SetGhost()
	{
		if ((bool)animator)
		{
			animator.Stop();
			layer.sprite = null;
		}
	}

	public void SetSkin(string skinId, int color, bool isLeft, CosmeticsLayer cosmeticsLayer, Action onLoaded = null)
	{
		SetSkin(DestroyableSingleton<HatManager>.Instance.GetSkinById(skinId), color, isLeft, cosmeticsLayer, onLoaded);
	}

	public void SetSkin(SkinData skinData, int color, bool isLeft, CosmeticsLayer cosmeticsLayer, Action onLoaded = null)
	{
		((MonoBehaviour)this).LoadAssetAsync((IAddressableAssetProvider<SkinViewData>)skinData, (Action<SkinViewData>)((SkinViewData skinView) =>
		{
			if (!this.IsDestroyedOrNull() && !gameObject.IsDestroyedOrNull())
			{
				data = skinData;
				SetSkin(skinView, color, isLeft);
				cosmeticsLayer.ValidateSkin(skinData.ProdId, color);
				onLoaded?.Invoke();
			}
		}), (Action)null);
	}

	public void SetSkin(SkinViewData skin, int color, bool isLeft)
	{
		this.skin = skin;
		matProperties.ColorId = color;
		UpdateMaterial();
		SetIdle(isLeft);
	}

	public void SetColor(int color)
	{
		matProperties.ColorId = color;
		UpdateMaterial();
	}

	public bool IsPlayingRunAnim()
	{
		if (animator == null || skin == null)
		{
			return false;
		}
		if (!(animator.GetCurrentAnimation() == skin.RunAnim))
		{
			return animator.GetCurrentAnimation() == skin.RunLeftAnim;
		}
		return true;
	}

	private void UpdateMaterial()
	{
		PlayerMaterial.MaskType maskType = matProperties.MaskType;
		if ((bool)skin && skin.MatchPlayerColor)
		{
			if (maskType == PlayerMaterial.MaskType.ComplexUI || maskType == PlayerMaterial.MaskType.ScrollingUI)
			{
				layer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedPlayerMaterial;
			}
			else
			{
				layer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
			}
		}
		else if (maskType == PlayerMaterial.MaskType.ComplexUI || maskType == PlayerMaterial.MaskType.ScrollingUI)
		{
			layer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.MaskedMaterial;
		}
		else
		{
			layer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
		}
		switch (maskType)
		{
		case PlayerMaterial.MaskType.SimpleUI:
			layer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			break;
		case PlayerMaterial.MaskType.Exile:
			layer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			break;
		default:
			layer.maskInteraction = SpriteMaskInteraction.None;
			break;
		}
		layer.material.SetInt(PlayerMaterial.MaskLayer, matProperties.MaskLayer);
		if ((bool)skin && skin.MatchPlayerColor)
		{
			PlayerMaterial.SetColors(matProperties.ColorId, layer);
		}
		if (matProperties.MaskLayer <= 0)
		{
			PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(layer, matProperties.IsLocalPlayer);
		}
	}
}
