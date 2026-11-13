using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class AdminButton : ActionButton
{
	[SerializeField]
	private Animator MyAnimator;

	[SerializeField]
	private AudioClip RevealSFX;

	private bool useable;

	public override void DoClick()
	{
		if (useable)
		{
			DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(GameManager.Instance.GetMapOptions());
		}
	}

	public void Refresh()
	{
		if (GameManager.Instance == null || !(GameManager.Instance.LogicFlow is LogicGameFlowHnS logicGameFlowHnS))
		{
			ToggleVisible(visible: false);
			SetDisabled();
			return;
		}
		useable = logicGameFlowHnS.SeekerAdminMapEnabled(PlayerControl.LocalPlayer);
		if (!useable || ((bool)MapBehaviour.Instance && MapBehaviour.Instance.IsOpen))
		{
			ToggleVisible(visible: false);
			SetDisabled();
		}
		else
		{
			ToggleVisible(visible: true);
			SetEnabled();
		}
	}

	public void AnimEvent_TurnOffAnimator()
	{
		MyAnimator.enabled = false;
	}

	public void AnimEvent_PlayRevealSFX()
	{
		SoundManager.Instance.PlaySound(RevealSFX, loop: false);
	}
}
