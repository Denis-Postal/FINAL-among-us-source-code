using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.UI;

public class ActionMapGlyphDisplay : MonoBehaviour
{
	public RewiredConstsEnum.Action actionToDisplayMappedGlyphFor;

	private SpriteRenderer spriteRenderer;

	private Image uiImage;

	private void Awake()
	{
		// TODO(verify): rebuilt from decomp9
		spriteRenderer = GetComponent<SpriteRenderer>();
		uiImage = GetComponent<Image>();
		UpdateGlyphDisplay();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateGlyphDisplay));
	}

	private void OnDestroy()
	{
		// TODO(verify): rebuilt from decomp9
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateGlyphDisplay));
	}

	public void UpdateGlyphDisplay()
	{
		if ((bool)spriteRenderer)
		{
			spriteRenderer.gameObject.SetActive(value: false);
		}
		if ((bool)uiImage)
		{
			uiImage.gameObject.SetActive(value: false);
		}
		if (ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick)
		{
			return;
		}
		Sprite sprite = GlyphCollection.FindGlyph((int)actionToDisplayMappedGlyphFor, out var error);
		if ((bool)spriteRenderer)
		{
			spriteRenderer.gameObject.SetActive(value: true);
			if (error != GlyphCollection.ErrorCode.NoController)
			{
				spriteRenderer.sprite = sprite;
			}
			else
			{
				spriteRenderer.gameObject.SetActive(value: false);
			}
		}
		if ((bool)uiImage)
		{
			uiImage.gameObject.SetActive(value: true);
			if (error != GlyphCollection.ErrorCode.NoController)
			{
				uiImage.sprite = sprite;
			}
			else
			{
				uiImage.gameObject.SetActive(value: false);
			}
		}
	}

	public unsafe void SetColor(Color color)
	{
		if ((bool)spriteRenderer)
		{
			spriteRenderer.color = color;
		}
		if ((bool)uiImage)
		{
			uiImage.color = color;
		}
	}

	public void SetSpriteVisibility(bool visible)
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.enabled = visible;
		}
		if (uiImage != null)
		{
			uiImage.enabled = visible;
		}
	}
}
