using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

namespace Innersloth.Assets
{
	public class AddressableSpriteBehavior : MonoBehaviour
	{
		public bool unloadOnDisable = true;

		private AddressableAsset<Sprite> asset;

		private SpriteRenderer spriteRenderer;

		public void ReplaceAsset(AddressableAsset<Sprite> newAsset)
		{
			Unload();
			asset?.Destroy();
			asset = newAsset;
			Load();
		}

		public static AddressableSpriteBehavior AddToSpriteRenderer(SpriteRenderer pSpriteRenderer, AddressableAsset<Sprite> asset)
		{
			AddressableSpriteBehavior addressableSpriteBehavior = pSpriteRenderer.gameObject.AddComponent<AddressableSpriteBehavior>();
			addressableSpriteBehavior.spriteRenderer = pSpriteRenderer;
			addressableSpriteBehavior.asset = asset;
			return addressableSpriteBehavior;
		}

		private void Start()
		{
			Load();
		}

		private void Awake()
		{
			Load();
		}

		private void OnDisable()
		{
			if (unloadOnDisable)
			{
				Unload();
			}
		}

		private void OnEnable()
		{
			Load();
		}

		private void OnDestroy()
		{
			Unload();
			asset = null;
		}

		private void Load()
		{
			if (asset != null)
			{
				asset.LoadAsync(null, null, OnAssetLoaded);
			}
		}

		private void Unload()
		{
			spriteRenderer.sprite = null;
			asset?.Unload();
		}

		private void OnAssetLoaded()
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.sprite = asset.GetAsset();
			}
		}
	}
}
