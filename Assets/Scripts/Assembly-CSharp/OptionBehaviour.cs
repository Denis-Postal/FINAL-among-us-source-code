using System;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public abstract class OptionBehaviour : MonoBehaviour
{
	public SpriteRenderer LabelBackground;

	public StringNames Title;

	public Action<OptionBehaviour> OnValueChanged;

	public RoleTypes AssociatedRole;

	public BaseGameSetting data;

	private PassiveButton[] buttons;

	public BaseGameSetting Data => data;

	public virtual float GetFloat()
	{
		throw new NotImplementedException();
	}

	public virtual int GetInt()
	{
		throw new NotImplementedException();
	}

	public virtual bool GetBool()
	{
		throw new NotImplementedException();
	}

	public void SetAsPlayer()
	{
		if (buttons == null || buttons.Length == 0)
		{
			buttons = GetComponentsInChildren<PassiveButton>();
		}
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].gameObject.SetActive(value: false);
		}
	}

	public void SetClickMask(Collider2D clickMask)
	{
		if (buttons == null || buttons.Length == 0)
		{
			buttons = GetComponentsInChildren<PassiveButton>();
		}
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].ClickMask = clickMask;
		}
	}

	public virtual void SetUpFromData(BaseGameSetting data, int maskLayer)
	{
		this.data = data;
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
	}

	public virtual void Initialize()
	{
	}

	public string GetValueString(float value)
	{
		return data.GetValueString(value);
	}
}
