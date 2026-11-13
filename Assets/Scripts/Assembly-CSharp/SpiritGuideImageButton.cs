using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SpiritGuideImageButton : MonoBehaviour
{
	private SpiritGuideRole spiritGuideRole;

	public Vector3 defaultPosition;

	public Image spiritGuideImage;

	public int myIndex;

	[SerializeField]
	private Color selectedColor;

	[SerializeField]
	private Color unselectedColor;

	[SerializeField]
	private AudioClip selectAudio;

	[SerializeField]
	private AudioClip deselectAudio;

	[SerializeField]
	public GameObject highlight;

	private void Start()
	{
		if (spiritGuideRole == null)
		{
			spiritGuideRole = transform.GetComponentInParent<SpiritGuideRole>();
		}
		defaultPosition = transform.localPosition;
	}

	public void SetSprite(Sprite sprite)
	{
		spiritGuideImage.sprite = sprite;
	}

	public void ToggleHighlight(bool isHighlighted)
	{
		highlight.SetActive(isHighlighted);
	}

	public void ToggleSelection()
	{
		if (!spiritGuideRole.SendingImage)
		{
			if (spiritGuideRole.selectedImageButtons.Contains(this))
			{
				spiritGuideRole.selectedImageButtons.Remove(this);
				spiritGuideRole.ResetSelectedImage(this);
				SoundManager.Instance.PlaySoundImmediate(deselectAudio, loop: false);
			}
			else if (spiritGuideRole.selectedImageButtons.Count != 3)
			{
				transform.localScale = Vector3.one * 1.2f;
				spiritGuideImage.color = selectedColor;
				spiritGuideRole.SelectImage(this);
				SoundManager.Instance.PlaySoundImmediate(selectAudio, loop: false);
			}
		}
	}

	public void ResetImage()
	{
		if (defaultPosition != Vector3.zero)
		{
			transform.localPosition = defaultPosition;
		}
		transform.localScale = Vector2.one;
		spiritGuideImage.color = unselectedColor;
	}

	public void Hide()
	{
		spiritGuideImage.color = Color.clear;
	}
}
