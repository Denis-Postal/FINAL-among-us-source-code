using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableShell : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer shellSprite;

	[SerializeField]
	private PassiveButton[] sands;

	[SerializeField]
	private AudioClip[] shellSounds;

	private Action onSandCleared;

	private SpriteRenderer[] sandSprites;

	private bool isCollected;

	public IReadOnlyList<PassiveButton> SandButtons => sands;

	private void Awake()
	{
	}

	private void Start()
	{
		sandSprites = new SpriteRenderer[sands.Length];
		for (int i = 0; i < sands.Length; i++)
		{
			sandSprites[i] = sands[i].GetComponent<SpriteRenderer>();
		}
	}

	public SpriteRenderer GetSandSprite(int index)
	{
		return sandSprites[index];
	}

	public void SetOnSandClearedAction(Action pOnSandCleared)
	{
		onSandCleared = pOnSandCleared;
	}

	public void CheckIfAllSandCleared()
	{
		if (isCollected)
		{
			return;
		}
		PassiveButton[] array = sands;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.activeSelf)
			{
				return;
			}
		}
		isCollected = true;
		StartCoroutine(CoCollectShellAnimation());
	}

	private IEnumerator CoCollectShellAnimation()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(shellSounds.Random(), loop: false);
		}
		yield return Effects.ScaleIn(shellSprite.transform, 1f, 1.2f, 0.5f);
		yield return Effects.ColorFade(shellSprite, Color.white, Palette.ClearWhite, 0.5f);
		onSandCleared();
	}
}
