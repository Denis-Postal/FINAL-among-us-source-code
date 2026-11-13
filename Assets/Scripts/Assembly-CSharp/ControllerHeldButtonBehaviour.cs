using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class ControllerHeldButtonBehaviour : MonoBehaviour
{
	public RewiredConstsEnum.Action Action;

	public SpriteRenderer targetCooldownSprite;

	public float holdDuration;

	public ActionButton TargetActionButton;

	private Player player;

	private ButtonBehavior buttonBehavior;

	private PassiveButton passiveButton;

	private float holdTimer;

	private bool alreadyPressed;

	private void Start()
	{
		player = ReInput.players.GetPlayer(0);
		buttonBehavior = GetComponent<ButtonBehavior>();
		passiveButton = GetComponent<PassiveButton>();
	}

	private void Update()
	{
		if (player.GetButton((int)Action))
		{
			if (alreadyPressed || (TargetActionButton != null && !TargetActionButton.CanInteract()))
			{
				return;
			}
			holdTimer += Time.unscaledDeltaTime;
			if ((bool)targetCooldownSprite)
			{
				float num = Mathf.Clamp01(holdTimer / 0.5f);
				targetCooldownSprite.SetCooldownNormalizedUvs();
				targetCooldownSprite.material.SetFloat("_Percent", 1f - num);
			}
			if (holdTimer >= holdDuration)
			{
				alreadyPressed = true;
				if ((bool)buttonBehavior)
				{
					buttonBehavior.OnClick?.Invoke();
				}
				else if ((bool)passiveButton)
				{
					passiveButton.OnClick?.Invoke();
				}
				targetCooldownSprite.SetCooldownNormalizedUvs();
			}
		}
		else
		{
			if (holdTimer > 0f)
			{
				targetCooldownSprite.SetCooldownNormalizedUvs();
				targetCooldownSprite.material.SetFloat("_Percent", 0f);
			}
			alreadyPressed = false;
			holdTimer = 0f;
		}
	}
}
