using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class RoleOptionSetting : OptionBehaviour
{
	[SerializeField]
	private TextMeshPro titleText;

	[SerializeField]
	private TextMeshPro countText;

	[SerializeField]
	private TextMeshPro chanceText;

	[SerializeField]
	private SpriteRenderer labelSprite;

	[SerializeField]
	public List<PassiveButton> controllerSelectable;

	[SerializeField]
	private GameOptionButton CountPlusBtn;

	[SerializeField]
	private GameOptionButton CountMinusBtn;

	[SerializeField]
	private GameOptionButton ChancePlusBtn;

	[SerializeField]
	private GameOptionButton ChanceMinusBtn;

	private RoleBehaviour role;

	private int roleMaxCount;

	private int roleChance;

	public int RoleMaxCount => roleMaxCount;

	public int RoleChance => roleChance;

	public RoleBehaviour Role => role;

	public List<PassiveButton> ControllerSelectable => controllerSelectable;

	private int RoleMax => Mathf.Min(role.MaxCount, 15);

	public unsafe void SetRole(IRoleOptionsCollection options, RoleBehaviour role, int maskLayer)
	{
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		}
		TextMeshPro[] componentsInChildren2 = GetComponentsInChildren<TextMeshPro>(includeInactive: true);
		foreach (TextMeshPro obj in componentsInChildren2)
		{
			obj.fontMaterial.SetFloat("_StencilComp", 3f);
			obj.fontMaterial.SetFloat("_Stencil", maskLayer);
		}
		this.role = role;
		titleText.text = this.role.NiceName;
		if (role.TeamType == RoleTeamTypes.Crewmate)
		{
			labelSprite.color = Palette.CrewmateRoleBlue;
		}
		else
		{
			labelSprite.color = Palette.ImpostorRoleRed;
		}
		UpdateValuesAndText(options);
		AdjustCountButtonsActiveState();
		AdjustChanceButtonsActiveState();
	}

	public unsafe void UpdateValuesAndText(IRoleOptionsCollection options)
	{
		roleMaxCount = options.GetNumPerGame(role.Role);
		roleChance = options.GetChancePerGame(role.Role);
		countText.text = roleMaxCount.ToString();
		chanceText.text = roleChance.ToString();
		AdjustCountButtonsActiveState();
		AdjustChanceButtonsActiveState();
	}

	public void IncreaseCount()
	{
		if (roleMaxCount < role.MaxCount)
		{
			if (roleMaxCount == 0)
			{
				roleChance = 50;
				AdjustChanceButtonsActiveState();
			}
			roleMaxCount = Mathf.Clamp(roleMaxCount + 1, 0, RoleMax);
			OnValueChanged(this);
			AdjustCountButtonsActiveState();
		}
	}

	public void DecreaseCount()
	{
		if (roleMaxCount != 0)
		{
			roleMaxCount = Mathf.Clamp(roleMaxCount - 1, 0, RoleMax);
			if (roleMaxCount == 0)
			{
				roleChance = 0;
				AdjustChanceButtonsActiveState();
			}
			OnValueChanged(this);
			AdjustCountButtonsActiveState();
		}
	}

	public void IncreaseChance()
	{
		if (roleChance < 100)
		{
			if (roleChance == 0)
			{
				roleMaxCount = 1;
				AdjustCountButtonsActiveState();
			}
			roleChance = Mathf.Clamp(roleChance + 10, 10, 100);
			OnValueChanged(this);
			AdjustChanceButtonsActiveState();
		}
	}

	public void DecreaseChance()
	{
		if (roleChance != 0)
		{
			roleChance = Mathf.Clamp(roleChance - 10, 0, 100);
			if (roleChance == 0)
			{
				roleMaxCount = 0;
				AdjustCountButtonsActiveState();
			}
			OnValueChanged(this);
			AdjustChanceButtonsActiveState();
		}
	}

	private void AdjustCountButtonsActiveState()
	{
		if (roleMaxCount == 0)
		{
			CountMinusBtn.SetInteractable(interactable: false);
			CountPlusBtn.SetInteractable(interactable: true);
		}
		else if (roleMaxCount == RoleMax)
		{
			CountMinusBtn.SetInteractable(interactable: true);
			CountPlusBtn.SetInteractable(interactable: false);
		}
		else
		{
			CountMinusBtn.SetInteractable(interactable: true);
			CountPlusBtn.SetInteractable(interactable: true);
		}
	}

	private void AdjustChanceButtonsActiveState()
	{
		if (roleChance == 0)
		{
			ChanceMinusBtn.SetInteractable(interactable: false);
			ChancePlusBtn.SetInteractable(interactable: true);
		}
		else if (roleChance == 100)
		{
			ChanceMinusBtn.SetInteractable(interactable: true);
			ChancePlusBtn.SetInteractable(interactable: false);
		}
		else
		{
			ChanceMinusBtn.SetInteractable(interactable: true);
			ChancePlusBtn.SetInteractable(interactable: true);
		}
	}
}
