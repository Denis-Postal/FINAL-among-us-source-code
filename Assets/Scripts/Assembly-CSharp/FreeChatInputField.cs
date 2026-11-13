using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FreeChatInputField : AbstractChatInputField
{
	[SerializeField]
	public TextBoxTMP textArea;

	[SerializeField]
	private PassiveButton fieldButton;

	[SerializeField]
	private TextMeshPro charCountText;

	private Action m_OnChangedEvent;

	public string Text
	{
		get
		{
			TextBoxTMP textBoxTMP = textArea;
			if ((object)textArea != null)
			{
				return textBoxTMP.text;
			}
			return (string)(object)new NullReferenceException();
		}
	}

	public event Action OnChangedEvent
	{
		add
		{
			m_OnChangedEvent = (Action)Delegate.Combine(m_OnChangedEvent, value);
		}
		remove
		{
			m_OnChangedEvent = (Action)Delegate.Remove(m_OnChangedEvent, value);
		}
	}

	protected unsafe override void Awake()
	{
		base.Awake();
		fieldButton.AddOnClickListeners(new Action[1] { OnFieldPressed });
		textArea.OnChange.AddListener(OnFieldChanged);
		textArea.OnEnter.AddListener(OnFieldSubmit);
		textArea.characterLimit = 100;
		UpdateState();
	}

	public override void SetVisible(bool visible)
	{
		base.visible = visible;
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(visible);
		textArea.enabled = visible;
	}

	public override void Clear()
	{
		textArea.Clear();
		UpdateState();
	}

	private void OnFieldChanged()
	{
		UpdateCharCount();
		bool flag = textArea.text.Length > 0;
		canSubmit = flag;
		submitButton.enabled = flag;
		SetButtonsEnabled(flag);
		m_OnChangedEvent?.Invoke();
	}

	protected unsafe override void OnTextFieldTransformChanged(Vector3 position, Vector2 size)
	{
		// TODO(verify): exact local position of the text output was unrecoverable.
		textArea.outputText.rectTransform.localPosition = position;
		textArea.outputText.rectTransform.sizeDelta = size;
	}

	protected override void Submit()
	{
		base.Submit();
		UpdateState();
	}

	public void Focus()
	{
		textArea.GiveFocus();
		UpdateState();
	}

	public void Unfocus()
	{
		textArea.LoseFocus();
		UpdateState();
	}

	public void ForceKeyboardClose()
	{
		textArea.ForceKeyboardClose();
	}

	private void OnFieldPressed()
	{
		Focus();
	}

	private void OnFieldSubmit()
	{
		Submit();
	}

	public unsafe void UpdateCharCount()
	{
		// TODO(verify): sizing/position constants inferred from the 2019 ChatController.UpdateCharCount.
		Vector2 size = background.size;
		size.y = Math.Max(0.62f, textArea.TextHeight + 0.2f);
		background.size = size;
		Vector3 localPosition = background.transform.localPosition;
		localPosition.y = (0.62f - size.y) / 2f;
		background.transform.localPosition = localPosition;
		Vector3 localPosition2 = base.transform.localPosition;
		localPosition2.y = -2.08f - localPosition.y * 2f;
		base.transform.localPosition = localPosition2;
		int length = textArea.text.Length;
		charCountText.text = $"{length}/100";
		if (length < 75)
		{
			charCountText.color = Color.black;
		}
		else if (length < 100)
		{
			charCountText.color = new Color(1f, 1f, 0f, 1f);
		}
		else
		{
			charCountText.color = Color.red;
		}
	}

	private void UpdateState()
	{
		bool flag = textArea.text.Length > 0;
		canSubmit = flag;
		submitButton.enabled = flag;
		SetButtonsEnabled(flag);
	}

	public FreeChatInputField()
	{
		visible = true;
	}
}
