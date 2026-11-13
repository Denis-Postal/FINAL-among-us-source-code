using System;
using UnityEngine;

public class RoleSettingsTabButton : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer background;

	[SerializeField]
	private SpriteRenderer icon;

	[SerializeField]
	private PassiveButton button;

	public PassiveButton Button => button;

	public void SetButton(RoleBehaviour role, Action onClick)
	{
		icon.sprite = role.RoleIconWhite;
		button.OnClick.AddListener(() =>
		{
			onClick();
		});
	}
}
