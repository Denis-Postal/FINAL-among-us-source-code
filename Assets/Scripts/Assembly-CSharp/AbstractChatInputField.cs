using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public abstract class AbstractChatInputField : MonoBehaviour
{
	protected const float BUTTON_MARGIN = 0.1f;

	private const float TEXT_FIELD_MARGIN = 0.12f;

	[SerializeField]
	public SpriteRenderer background;

	[SerializeField]
	public ChatInputFieldButton submitButton;

	public bool visible = true;

	public bool canSubmit= true;

	private Action m_OnSubmitEvent;

	public SpriteRenderer Background => background;

	public bool Visible => visible;

	public bool CanSubmit => canSubmit;

	public event Action OnSubmitEvent
	{
		add
		{
			m_OnSubmitEvent = (Action)Delegate.Combine(m_OnSubmitEvent, value);
		}
		remove
		{
			m_OnSubmitEvent = (Action)Delegate.Remove(m_OnSubmitEvent, value);
		}
	}

	protected virtual void Awake()
	{
		submitButton.OnPressed += OnSubmitButtonPressed;
		submitButton.OnSizeChanged += OnSubmitButtonSizeChanged;
	}

	protected virtual void Start()
	{
		UpdateElements();
		Clear();
	}

	private void OnSubmitButtonPressed()
	{
		Submit();
	}

	private void OnSubmitButtonSizeChanged()
	{
		UpdateElements();
	}

	private unsafe void UpdateElements()
	{
		Bounds bounds = GetBounds();
		Vector3 localPosition = submitButton.transform.localPosition;
		localPosition.x = bounds.max.x - submitButton.Size.x - 0.1f;
		submitButton.transform.localPosition = localPosition;
		Vector3 position = new Vector3(0f, 0f, -0.1f);
		position.x = bounds.min.x + 0.12f;
		Vector2 size = bounds.size;
		size.x = bounds.size.x - submitButton.Size.x - 0.1f - 0.24f;
		size.y = 0.5f;
		OnTextFieldTransformChanged(position, size);
	}

	protected virtual void OnTextFieldTransformChanged(Vector3 position, Vector2 size)
	{
	}

	protected unsafe Bounds GetBounds()
	{
		return new Bounds(Vector3.zero, background.size);
	}

	public virtual void SetVisible(bool visible)
	{
		this.visible = visible;
		gameObject.SetActive(visible);
	}

	public void SetCanSubmit(bool canSubmit)
	{
		this.canSubmit = canSubmit;
		submitButton.enabled = canSubmit;
		SetButtonsEnabled(canSubmit);
	}

	protected virtual void SetButtonsEnabled(bool enabled)
	{
		submitButton.SetButtonEnabled(enabled);
	}

	public virtual void Clear()
	{
	}

	protected virtual void Submit()
	{
		m_OnSubmitEvent?.Invoke();
	}
}
