using UnityEngine;

public class AbilityButton : ActionButton
{
	[SerializeField]
	private GameObject commsDown;

	public override void Hide()
	{
		base.Hide();
	}

	public override void DoClick()
	{
		if (isActiveAndEnabled && !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed && (bool)PlayerControl.LocalPlayer)
		{
			RoleBehaviour role = PlayerControl.LocalPlayer.Data.Role;
			if ((bool)role.Ability)
			{
				logger.Debug($"Attempting to use role ability for role {role.Role}");
				role.UseAbility();
			}
		}
	}

	public void Refresh(AbilityButtonSettings ability)
	{
		if (ability != null)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	public void SetFromSettings(AbilityButtonSettings settings)
	{
		SetInfiniteUses();
		graphic.sprite = settings.Image;
		graphic.SetCooldownNormalizedUvs();
		buttonLabelText.fontSharedMaterial = settings.FontMaterial;
		buttonLabelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(settings.Text);
	}

	public void SetSecondImage(AbilityButtonSettings settings)
	{
		graphic.sprite = settings.SecondImage;
		graphic.SetCooldownNormalizedUvs();
	}

	private void Update()
	{
		if (PlayerControl.LocalPlayer.Data.Role.IsAffectedByComms)
		{
			commsDown.SetActive(value: true);
		}
		else
		{
			commsDown.SetActive(value: false);
		}
	}
}
