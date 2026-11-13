using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritGuideReceivedMessage : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup group;

	[SerializeField]
	private List<SpiritGuideReceivedImage> receivedImages = new List<SpiritGuideReceivedImage>();

	[SerializeField]
	private AudioClip popupSFX;

	private float uptimeSeconds;

	private float flashLength = 0.15f;

	private bool isRunning;

	private static readonly float ANIM_FRAMERATE = 1f / 24f;

	private static readonly float SIZE_DELTA_SMALL = 150f;

	private static readonly float SIZE_DELTA_REGULAR = 200f;

	private static readonly float SIZE_DELTA_BIG = 250f;

	private static readonly float FADE_RATE = 0.01f;

	private static readonly int STEPS_GROW = 23;

	private static readonly string MAT_FLASH = "_FlashAmount";

	public float TimeElapsedSeconds => uptimeSeconds;

	private void OnDestroy()
	{
		StopCoroutine("CoFlashIn");
	}

	public void FixedUpdate()
	{
		if (isRunning)
		{
			if (uptimeSeconds < 1f)
			{
				group.alpha = 1f;
			}
			if (uptimeSeconds < 3f)
			{
				uptimeSeconds += Time.deltaTime;
			}
		}
	}

	public void SetImages(List<Sprite> receivedSprites)
	{
		group.alpha = 0f;
		for (int i = 0; i < receivedSprites.Count; i++)
		{
			receivedImages[i].image.sprite = receivedSprites[i];
		}
	}

	public void Run()
	{
		isRunning = true;
		int count = receivedImages.Count;
		SoundManager.Instance.PlaySoundImmediate(popupSFX, loop: false);
		for (int i = 0; i < count; i++)
		{
			Image image = receivedImages[i].image;
			if (!(image == null) && !(image.sprite == null))
			{
				StartCoroutine("CoFlashIn", receivedImages[i]);
			}
		}
	}

	private IEnumerator CoFlashIn(SpiritGuideReceivedImage receivedImage)
	{
		receivedImage.image.rectTransform.sizeDelta = new Vector2(SIZE_DELTA_SMALL, SIZE_DELTA_SMALL);
		receivedImage.image.color = new Color(1f, 1f, 1f, 0.2f);
		receivedImage.image.material.SetFloat(MAT_FLASH, 1f);
		yield return new WaitForSeconds(ANIM_FRAMERATE);
		receivedImage.image.rectTransform.sizeDelta = new Vector2(SIZE_DELTA_REGULAR, SIZE_DELTA_REGULAR);
		receivedImage.image.color = Color.white;
		receivedImage.outerImage.gameObject.SetActive(value: true);
		receivedImage.outerImage.sprite = receivedImage.image.sprite;
		receivedImage.outerImage.rectTransform.sizeDelta = new Vector3(SIZE_DELTA_BIG, SIZE_DELTA_BIG);
		receivedImage.outerImage.rectTransform.rotation = receivedImage.image.rectTransform.rotation;
		receivedImage.outerImage.color = new Color(1f, 1f, 1f, 0.3f);
		yield return new WaitForSeconds(ANIM_FRAMERATE);
		float outerImageCurrentSize = receivedImage.outerImage.rectTransform.sizeDelta.x;
		Color outerCurrentColor = receivedImage.outerImage.color;
		Color initialAlpha = new Color(1f, 1f, 1f, 0.75f);
		for (int i = 0; i < STEPS_GROW; i++)
		{
			outerImageCurrentSize += (450f - outerImageCurrentSize) / (float)STEPS_GROW;
			receivedImage.outerImage.rectTransform.sizeDelta = outerImageCurrentSize * Vector2.one;
			outerCurrentColor.a -= 0.3f / (float)STEPS_GROW;
			receivedImage.outerImage.color = outerCurrentColor;
			if (i > 1 && i < 13)
			{
				receivedImage.image.material.SetFloat(MAT_FLASH, 0f);
				receivedImage.image.color = initialAlpha;
			}
			yield return new WaitForSeconds(ANIM_FRAMERATE);
		}
		float newAlpha = receivedImage.image.color.a;
		Color fadeOutColor = new Color(1f, 1f, 1f, newAlpha);
		while (newAlpha > 0f)
		{
			newAlpha = (fadeOutColor.a = newAlpha - FADE_RATE);
			receivedImage.image.color = fadeOutColor;
			yield return new WaitForSeconds(ANIM_FRAMERATE);
		}
	}
}
