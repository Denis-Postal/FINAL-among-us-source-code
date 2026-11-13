using System;
using Cpp2ILInjected;
using UnityEngine;

public static class PlayerMaterial
{
	public struct Properties
	{
		public bool IsLocalPlayer;

		public MaskType MaskType;

		public int MaskLayer;

		public int ColorId;
	}

	public enum MaskType
	{
		None,
		SimpleUI,
		ComplexUI,
		Exile,
		ScrollingUI
	}

	public static readonly int MaskLayer= Shader.PropertyToID("_MaskLayer");

	private static readonly int BackColor= Shader.PropertyToID("_BackColor");

	private static readonly int BodyColor= Shader.PropertyToID("_BodyColor");

	private static readonly int VisorColor= Shader.PropertyToID("_VisorColor");

	public static void SetMaskLayer(Material material, int maskLayer)
	{
		material.SetInt(MaskLayer, maskLayer);
	}

	public static void SetMaskLayerBasedOnLocalPlayer(SpriteRenderer sprite, bool isLocalPlayer)
	{
		sprite.material.SetInt(MaskLayer, (!isLocalPlayer) ? 1 : 2);
	}

	public static void SetColors(int colorId, Renderer rend)
	{
		if (rend == null)
		{
			Logger.GlobalInstance.Error("[PlayerMaterial] SetColors() :: Aborting! Renderer rend is NULL");
		}
		else
		{
			SetColors(colorId, rend.material);
		}
	}

	public unsafe static void SetColors(Color color, Renderer rend)
	{
		if (rend == null)
		{
			Logger.GlobalInstance.Error("[PlayerMaterial] SetColors() :: Aborting! Renderer rend is NULL");
		}
		else
		{
			SetColors(color, rend.material);
		}
	}

	public unsafe static void SetColors(int colorId, Material material)
	{
		if (material == null)
		{
			Logger.GlobalInstance.Error("[PlayerMaterial] SetColors() :: Aborting! Material is NULL");
		}
		else if (colorId < 0 || colorId >= Palette.PlayerColors.Length)
		{
			Logger.GlobalInstance.Error(string.Format("[{0}] {1}() :: Aborting! {2} is invalid! ({3})", "PlayerMaterial", "SetColors", "colorId", colorId));
		}
		else
		{
			material.SetColor(BackColor, Palette.ShadowColors[colorId]);
			material.SetColor(BodyColor, Palette.PlayerColors[colorId]);
			material.SetColor(VisorColor, Palette.VisorColor);
		}
	}

	public unsafe static void SetColors(Color color, Material material)
	{
		if (material == null)
		{
			Logger.GlobalInstance.Error("[PlayerMaterial] SetColors() :: Aborting! Material is NULL");
			return;
		}
		material.SetColor(BackColor, color);
		material.SetColor(BodyColor, color);
		material.SetColor(VisorColor, Palette.VisorColor);
	}

	static PlayerMaterial()
	{ }
}
