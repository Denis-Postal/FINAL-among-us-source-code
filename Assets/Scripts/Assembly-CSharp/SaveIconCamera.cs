using Object = UnityEngine.Object;
using System;
using System.Threading;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SaveIconCamera : DestroyableSingleton<SaveIconCamera>
{
	private Camera cam;

	public ConditionalRenderTexture platformRenderTextures;

	private RenderTexture targetTexture;

	public PlayerControl saveIconDummy;

	public LayerMask layerToRenderWith;

	private int layerIndex = -1;

	private static object lockObject= 1;

	private static bool needsRender= false;

	private static byte[] renderedPNG;

	private new void Awake()
	{
		targetTexture = platformRenderTextures;
	}

	private void Start()
	{
		cam = GetComponent<Camera>();
		if ((bool)cam)
		{
			cam.enabled = false;
		}
		saveIconDummy.gameObject.SetActive(value: false);
	}

	private void LateUpdate()
	{
		// TODO(verify): reimplemented (decomp stub)
		// TODO(verify): rebuilt from decomp9 shape (locked flag check, render, clear flag)
		lock (lockObject)
		{
			if (needsRender)
			{
				renderedPNG = RenderSaveIconLocal();
				needsRender = false;
			}
		}
	}

	private void TestIcon()
	{
		RenderSaveIconLocal();
	}

	public unsafe static byte[] RenderSaveIcon()
	{
		lock (lockObject)
		{
			needsRender = true;
		}
		while (needsRender)
		{
			Debug.Log("Waiting for render");
		}
		return renderedPNG;
	}

	private unsafe byte[] RenderSaveIconLocal()
	{
		saveIconDummy.gameObject.SetActive(value: true);
		cam.targetTexture = targetTexture;
		GameData instance = GameData.Instance;
		GameData.Instance = null;
		GameData.Instance = instance;
		if (layerIndex == -1)
		{
			for (int i = 0; i < 32; i++)
			{
				if (((1 << i) & (int)layerToRenderWith) != 0)
				{
					layerIndex = i;
					break;
				}
			}
		}
		if ((bool)saveIconDummy.GetPet())
		{
			cam.cullingMask = layerToRenderWith;
			PetBehaviour pet = saveIconDummy.GetPet();
			pet.SetCrewmateColor(DataManager.Player.Customization.Color);
			pet.enabled = false;
			pet.transform.SetParent(saveIconDummy.transform);
			pet.transform.position = saveIconDummy.transform.position + new Vector3(-0.5f, 0f, -5f);
			SpriteRenderer[] componentsInChildren = saveIconDummy.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].gameObject.layer = layerIndex;
			}
			componentsInChildren = pet.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].gameObject.layer = layerIndex;
			}
		}
		saveIconDummy.cosmetics.SetNameMask(alive: false);
		cam.Render();
		saveIconDummy.gameObject.SetActive(value: false);
		RenderTexture.active = targetTexture;
		Texture2D texture2D = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.RGB24, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, targetTexture.width, targetTexture.height), 0, 0);
		RenderTexture.active = null;
		return texture2D.EncodeToPNG();
	}

	public SaveIconCamera()
	{
		//IL_0014: Expected I4, but got I8
	}

	static SaveIconCamera()
	{ }
}
