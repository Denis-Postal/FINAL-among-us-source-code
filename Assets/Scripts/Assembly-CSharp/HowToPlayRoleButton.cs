using System;
using TMPro;
using UnityEngine;

public class HowToPlayRoleButton : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer background;

	[SerializeField]
	private SpriteRenderer border;

	[SerializeField]
	private SpriteRenderer roleIcon;

	[SerializeField]
	private TextMeshPro roleName;

	[SerializeField]
	private PassiveButton passiveButton;

	[SerializeField]
	private GameObject newRoleAlert;

	[SerializeField]
	private int MaskLayer;

	[SerializeField]
	private Collider2D collider;

	private RoleBehaviour role;

	private readonly float offscreenYPos = 1.7f;

	private void OnEnable()
	{
		background.material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
		border.material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
		roleIcon.material.SetInt(PlayerMaterial.MaskLayer, MaskLayer);
	}

	public void SetRoleInfo(RoleBehaviour _role, Sprite roleIcon)
	{
		roleName.text = _role.NiceName;
		this.roleIcon.sprite = roleIcon;
		role = _role;
	}

	public RoleBehaviour GetRole()
	{
		return role;
	}

	public void SetButtonAction(Action action)
	{
		passiveButton.AddOnClickListeners(action);
	}

	private void Update()
	{
		if (transform.position.y > offscreenYPos)
		{
			collider.enabled = false;
		}
		else
		{
			collider.enabled = true;
		}
	}
}
