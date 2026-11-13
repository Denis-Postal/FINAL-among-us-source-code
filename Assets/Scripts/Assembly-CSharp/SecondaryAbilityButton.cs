using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class SecondaryAbilityButton : ActionButton
{
	[SerializeField]
	private GameObject commsDown;

	public override void DoClick()
	{
		if (isActiveAndEnabled && !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed && (bool)PlayerControl.LocalPlayer)
		{
			RoleBehaviour role = PlayerControl.LocalPlayer.Data.Role;
			if ((bool)role.SecondaryAbility)
			{
				logger.Debug($"Attempting to use secondary role ability for role {role.Role}");
				role.UseSecondaryAbility();
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
