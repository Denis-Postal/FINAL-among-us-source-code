using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PassiveButton : PassiveUiElement
{
	public Button.ButtonClickedEvent OnClick= new Button.ButtonClickedEvent();

	public AudioClip ClickSound;

	public AudioClip HoverSound;

	public bool OnUp= true;

	public bool OnDown;

	public bool OnRepeat;

	public bool OnUpGraphic;

	public bool OnDownGraphic;

	public float RepeatDuration= 0.3f;

	public SpriteRenderer HeldButtonSprite;

	public ActionButton TargetActionButton;

	public bool HoldToUse;

	private bool beingHeldDown;

	private float repeatTimer;

	private float totalHeldTime;

	private bool checkedClickEvent;

	public bool selected;

	public bool Disabled;

	[SerializeField]
	private GameObject selectedSprites;

	[SerializeField]
	private GameObject activeSprites;

	[SerializeField]
	private GameObject inactiveSprites;

	[SerializeField]
	private GameObject disabledSprites;

	[SerializeField]
	private GameObject selectedInactiveSprites;

	[SerializeField]
	private GameObject onClickSprites;

	[SerializeField]
	private bool forceInactiveSpritesMobile;

	[SerializeField]
	private bool forceClickGraphicMobile;

	[SerializeField]
	private TextMeshPro buttonText;

	[SerializeField]
	private Color selectedTextColor;

	[SerializeField]
	private Color activeTextColor;

	[SerializeField]
	private Color inactiveTextColor;

	[SerializeField]
	private Color disabledTextColor;

	[SerializeField]
	private Color selectedInactiveTextColor= Color.black;

	private static readonly int STENCIL_COMP= Shader.PropertyToID("_StencilComp");

	private static readonly int STENCIL= Shader.PropertyToID("_Stencil");

	public override bool HandleUp => OnUp;

	public override bool HandleDown => OnDown;

	public override bool HandleRepeat => OnRepeat;

	public override bool HandleUpClickGraphic => OnUpGraphic;

	public override bool HandleDownClickGraphic => OnDownGraphic;

	private void Awake()
	{
		if (Disabled)
		{
			SetButtonEnableState(enabled: false);
		}
		else
		{
			SetPassiveButtonHoverStateInactive();
		}
	}

	private void OnValidate()
	{
		if (Application.isPlaying)
		{
			SetButtonEnableState(!Disabled);
		}
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
		if (HoldToUse && beingHeldDown)
		{
			if (totalHeldTime > 0f)
			{
				float num = Mathf.Clamp01(totalHeldTime / 0.5f);
				HeldButtonSprite.SetCooldownNormalizedUvs();
				HeldButtonSprite.material.SetFloat("_Percent", 1f - num);
			}
			else
			{
				beingHeldDown = false;
				HeldButtonSprite.material.SetFloat("_Percent", 0f);
			}
		}
	}

	private new void OnDisable()
	{
		SetPassiveButtonHoverStateInactive();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (checkedClickEvent)
		{
			return;
		}
		checkedClickEvent = true;
		int persistentEventCount = OnClick.GetPersistentEventCount();
		CloseButtonConsoleBehaviour component = GetComponent<CloseButtonConsoleBehaviour>();
		string text = gameObject.name.ToLower();
		if ((bool)component)
		{
			return;
		}
		for (int i = 0; i < persistentEventCount; i++)
		{
			string persistentMethodName = OnClick.GetPersistentMethodName(i);
			if (text.Contains("close") && persistentMethodName.ToLower().Contains("close"))
			{
				gameObject.AddComponent<CloseButtonConsoleBehaviour>();
				break;
			}
		}
	}

	public override void ReceiveClickDown()
	{
		if (enabled && !HoldToUse)
		{
			if ((bool)ClickSound)
			{
				SoundManager.Instance.PlaySound(ClickSound, loop: false);
			}
			repeatTimer = 0f;
			OnClick.Invoke();
		}
	}

	public override void ReceiveRepeatDown()
	{
		if (!enabled)
		{
			return;
		}
		if (HoldToUse)
		{
			if (TargetActionButton != null && !TargetActionButton.CanInteract())
			{
				return;
			}
			totalHeldTime += Time.deltaTime;
			beingHeldDown = true;
			if (totalHeldTime < 0.5f)
			{
				return;
			}
		}
		else
		{
			repeatTimer += Time.deltaTime;
			if (repeatTimer < RepeatDuration)
			{
				return;
			}
			repeatTimer = 0f;
		}
		if ((bool)ClickSound)
		{
			SoundManager.Instance.PlaySound(ClickSound, loop: false);
		}
		OnClick.Invoke();
	}

	public override void ReceiveClickUp()
	{
		if (enabled)
		{
			ReceiveClickDown();
		}
	}

	public unsafe void SetButtonEnableState(bool enabled)
	{
		Disabled = !enabled;
		base.enabled = enabled;
		if (enabled)
		{
			SetPassiveButtonHoverStateInactive();
			GetComponent<ButtonRolloverHandler>()?.SetEnabledColors();
		}
		else
		{
			SetPassiveButtonDisabledState();
			GetComponent<ButtonRolloverHandler>()?.SetDisabledColors();
		}
		GetComponentInChildren<ActionMapGlyphDisplay>(includeInactive: true)?.SetSpriteVisibility(base.enabled);
	}

	public void AddOnClickListeners(params Action[] callbacks)
	{
		OnClick.RemoveAllListeners();
		foreach (Action action in callbacks)
		{
			Action cbCopy = action;
			OnClick.AddListener(() =>
			{
				cbCopy();
			});
		}
	}

	public override void ReleaseButton()
	{
		totalHeldTime = 0f;
		repeatTimer = 0f;
	}

	public override void ReceiveMouseOut()
	{
		if (enabled)
		{
			base.ReceiveMouseOut();
			SetPassiveButtonHoverStateInactive();
		}
	}

	public override void ReceiveMouseOver()
	{
		if (enabled)
		{
			base.ReceiveMouseOver();
			SetPassiveButtonHoverStateActive();
			if ((bool)HoverSound)
			{
				SoundManager.Instance.PlaySound(HoverSound, loop: false);
			}
		}
	}

	public override void ReceiveClickUpGraphic()
	{
		SetPassiveButtonClickState(clicked: false);
	}

	public override void ReceiveClickDownGraphic()
	{
		SetPassiveButtonClickState(clicked: true);
	}

	private void SetPassiveButtonClickState(bool clicked)
	{
		if (!enabled || !(onClickSprites != null))
		{
			return;
		}
		onClickSprites.SetActive(clicked);
		if (forceClickGraphicMobile & clicked)
		{
			if (activeSprites != null)
			{
				activeSprites.SetActive(value: false);
			}
			if (inactiveSprites != null)
			{
				inactiveSprites.SetActive(value: false);
			}
			if (disabledSprites != null)
			{
				disabledSprites.SetActive(value: false);
			}
			if (selectedSprites != null)
			{
				selectedSprites.SetActive(value: false);
			}
		}
		else if (forceClickGraphicMobile && !clicked && inactiveSprites != null)
		{
			inactiveSprites.SetActive(value: true);
		}
	}

	public unsafe void SetPassiveButtonHoverStateActive()
	{
		if (!enabled)
		{
			return;
		}
		if (selectedInactiveSprites != null)
		{
			selectedInactiveSprites.SetActive(value: false);
			if (buttonText != null)
			{
				buttonText.color = selectedInactiveTextColor;
			}
			if (selectedSprites != null && selected)
			{
				selectedSprites.SetActive(value: true);
			}
		}
		if (!selected)
		{
			if (activeSprites != null)
			{
				activeSprites.SetActive(value: true);
			}
			if (inactiveSprites != null)
			{
				inactiveSprites.SetActive(value: false);
			}
			if (disabledSprites != null)
			{
				disabledSprites.SetActive(value: false);
			}
			if (buttonText != null)
			{
				buttonText.color = activeTextColor;
			}
		}
	}

	public unsafe void SetPassiveButtonHoverStateInactive()
	{
		if (!enabled)
		{
			return;
		}
		if (selectedInactiveSprites != null && selected)
		{
			if (selectedSprites != null)
			{
				selectedSprites.SetActive(value: false);
			}
			if (inactiveSprites != null)
			{
				inactiveSprites.SetActive(value: false);
			}
			selectedInactiveSprites.SetActive(value: true);
		}
		else if (!selected)
		{
			if (activeSprites != null)
			{
				activeSprites.SetActive(value: false);
			}
			if (selectedSprites != null)
			{
				selectedSprites.SetActive(value: false);
			}
			if (inactiveSprites != null)
			{
				inactiveSprites.SetActive(value: true);
			}
			if (disabledSprites != null)
			{
				disabledSprites.SetActive(value: false);
			}
			if (buttonText != null)
			{
				buttonText.color = inactiveTextColor;
			}
		}
	}

	private unsafe void SetPassiveButtonDisabledState()
	{
		if (activeSprites != null)
		{
			activeSprites.SetActive(value: false);
		}
		if (inactiveSprites != null)
		{
			inactiveSprites.SetActive(value: false);
		}
		if (disabledSprites != null)
		{
			disabledSprites.SetActive(value: true);
		}
		if (selectedSprites != null)
		{
			selectedSprites.SetActive(value: false);
		}
		if (selectedInactiveSprites != null)
		{
			selectedInactiveSprites.SetActive(value: false);
		}
		if (buttonText != null)
		{
			buttonText.color = disabledTextColor;
		}
	}

	public unsafe void SelectButton(bool isSelected)
	{
		if (Disabled)
		{
			SetPassiveButtonDisabledState();
			return;
		}
		selected = isSelected;
		if (selectedSprites != null)
		{
			selectedSprites.SetActive(isSelected);
		}
		if (activeSprites != null)
		{
			activeSprites.SetActive(value: false);
		}
		if (inactiveSprites != null)
		{
			inactiveSprites.SetActive(!isSelected);
		}
		if (disabledSprites != null)
		{
			disabledSprites.SetActive(value: false);
		}
		if (selectedInactiveSprites != null)
		{
			selectedInactiveSprites.SetActive(value: false);
		}
		if (buttonText != null)
		{
			if (isSelected)
			{
				buttonText.color = selectedTextColor;
			}
			else
			{
				buttonText.color = inactiveTextColor;
			}
		}
	}

	public void ChangeButtonText(string s)
	{
		if (buttonText != null)
		{
			buttonText.text = s;
		}
	}

	private void SetButtonTextColor(Color c)
	{
		// TODO(verify): colour fields are only filled when the original ctor/serialized data provides them; ignore fully transparent values
		if (buttonText != null && c.a > 0f) buttonText.color = c;
	}

	public bool IsSelected()
	{
		return selected;
	}

	public void SetMaskLayer(int maskID)
	{
		TextMeshPro[] componentsInChildren = GetComponentsInChildren<TextMeshPro>(includeInactive: true);
		foreach (TextMeshPro obj in componentsInChildren)
		{
			obj.fontMaterial.SetFloat(STENCIL_COMP, 3f);
			obj.fontMaterial.SetFloat(STENCIL, maskID);
		}
	}

	public PassiveButton()
	{ }

	static PassiveButton()
	{ }
}
