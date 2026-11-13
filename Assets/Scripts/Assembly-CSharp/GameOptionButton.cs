using System;
using System.Collections;
using UnityEngine;

public class GameOptionButton : PassiveButton
{
	private bool isInteractable = true;

	[Header("Game Option Button Stuff")]
	[SerializeField]
	private SpriteRenderer buttonSprite;

	[SerializeField]
	private Color interactableColor;

	[SerializeField]
	private Color interactableHoveredColor;

	[SerializeField]
	private Color interactableClickColor;

	[SerializeField]
	private Color uninteractableColor;

	[SerializeField]
	private Color uninteractableHoveredColor;

	[SerializeField]
	private Color uninteractableClickColor;

	public void SetInteractable(bool interactable)
	{
		if (interactable != isInteractable)
		{
			buttonSprite.color = (interactable ? interactableColor : uninteractableColor);
		}
		isInteractable = interactable;
	}

	public override void ReceiveMouseOver()
	{
		if (enabled)
		{
			AdjustHoverColors(isHovering: true);
			if ((bool)HoverSound)
			{
				SoundManager.Instance.PlaySound(HoverSound, loop: false);
			}
			OnMouseOver?.Invoke();
		}
	}

	public override void ReceiveMouseOut()
	{
		if (enabled)
		{
			AdjustHoverColors(isHovering: false);
			StopCoroutine("DelayControllerClickUp");
			OnMouseOut?.Invoke();
		}
	}

	public override void ReceiveClickUp()
	{
		if (enabled)
		{
			AdjustClickedColors(clickDown: false);
		}
	}

	public override void ReceiveClickDown()
	{
		if (!enabled)
		{
			return;
		}
		AdjustClickedColors(clickDown: true);
		if (isInteractable && !HoldToUse)
		{
			if ((bool)ClickSound)
			{
				SoundManager.Instance.PlaySound(ClickSound, loop: false);
			}
			OnClick.Invoke();
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
			{
				StopCoroutine("DelayControllerClickUp");
				StartCoroutine("DelayControllerClickUp");
			}
		}
	}

	private void AdjustHoverColors(bool isHovering)
	{
		if (ActiveInputManager.currentControlType != ActiveInputManager.InputType.Touch)
		{
			if (isInteractable)
			{
				buttonSprite.color = (isHovering ? interactableHoveredColor : interactableColor);
			}
			else
			{
				buttonSprite.color = (isHovering ? uninteractableHoveredColor : uninteractableColor);
			}
		}
	}

	private void AdjustClickedColors(bool clickDown)
	{
		if (ActiveInputManager.currentControlType != ActiveInputManager.InputType.Touch)
		{
			if (clickDown)
			{
				buttonSprite.color = (isInteractable ? interactableClickColor : uninteractableClickColor);
			}
			else
			{
				buttonSprite.color = (isInteractable ? interactableHoveredColor : uninteractableHoveredColor);
			}
		}
	}

	private IEnumerator DelayControllerClickUp()
	{
		yield return new WaitUntil((Func<bool>)DestroyableSingleton<ActiveInputManager>.Instance.CurrentController.GetAnyButtonUp);
		ReceiveClickUp();
	}
}
