using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChatInputFieldButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro text;

	[SerializeField]
	private TextTranslatorTMP translator;

	[SerializeField]
	public PassiveButton button;

	[SerializeField]
	public BoxCollider2D collider;

	[SerializeField]
	private Transform inputGlyph;

	[SerializeField]
	private SpriteRenderer[] backgroundSprites;

	[SerializeField]
	private SpriteRenderer[] iconSprites;

	private Action m_OnPressed;

	private Action m_OnSizeChanged;

	public Vector2 Size => collider.size;

	public event Action OnPressed
	{
		add
		{
			m_OnPressed = (Action)Delegate.Combine(m_OnPressed, value);
		}
		remove
		{
			m_OnPressed = (Action)Delegate.Remove(m_OnPressed, value);
		}
	}

	public event Action OnSizeChanged
	{
		add
		{
			m_OnSizeChanged = (Action)Delegate.Combine(m_OnSizeChanged, value);
		}
		remove
		{
			m_OnSizeChanged = (Action)Delegate.Remove(m_OnSizeChanged, value);
		}
	}

	private void Awake()
	{
		button.OnClick.AddListener(OnButtonClicked);
		translator.OnTranslate.AddListener(OnTextTranslationChanged);
		UpdateSize();
	}

	public void SetButtonEnabled(bool enabled)
	{
		button.SetButtonEnableState(enabled);
	}

	private void OnButtonClicked()
	{
		m_OnPressed?.Invoke();
	}

	private void OnTextTranslationChanged()
	{
		UpdateSize();
	}

	private unsafe void UpdateSize()
	{
		text.autoSizeTextContainer = true;
		text.Rebuild(CanvasUpdate.Prelayout);
		float x = text.rectTransform.sizeDelta.x;
		Vector2 size = Size;
		Vector2 vector = new Vector2(size.x * 0.5f, (0f - size.y) * 0.5f);
		float[] array = new float[backgroundSprites.Length];
		for (int i = 0; i < backgroundSprites.Length; i++)
		{
			SpriteRenderer spriteRenderer = backgroundSprites[i];
			array[i] = Mathf.Abs(spriteRenderer.size.x * 0.5f - vector.x);
		}
		size.x = x;
		vector.x = size.x * 0.5f;
		if (backgroundSprites != null && backgroundSprites.Length != 0)
		{
			for (int j = 0; j < backgroundSprites.Length; j++)
			{
				SpriteRenderer obj = backgroundSprites[j];
				Vector3 localPosition = obj.transform.localPosition;
				float num = array[j];
				localPosition.x = vector.x;
				obj.transform.localPosition = localPosition;
				Vector2 size2 = obj.size;
				size2.x = size.x + num * 2f;
				obj.size = size2;
			}
		}
		collider.size = size;
		collider.offset = new Vector2(vector.x, 0f);
		Vector3 localPosition2 = inputGlyph.localPosition;
		localPosition2.x = Size.x;
		inputGlyph.localPosition = localPosition2;
		m_OnSizeChanged?.Invoke();
	}
}
