using System;
using Rewired;
using UnityEngine;

public class InfectedOverlay : MonoBehaviour
{
	public MapRoom[] rooms;

	private IActivatable doors;

	private SabotageSystemType sabSystem;

	public ButtonBehavior[] allButtons;

	public ButtonBehavior selectedButton;

	private const float selectCooldown = 0.5f;

	private float currentCooldown;

	public bool CanUseDoors
	{
		get
		{
			if (ShipStatus.Instance.Type != ShipStatus.MapType.Pb)
			{
				return !sabSystem.AnyActive;
			}
			return true;
		}
	}

	public bool CanUseSabotage
	{
		get
		{
			if (GameManager.Instance.SabotagesEnabled() && sabSystem.Timer <= 0f && !sabSystem.AnyActive)
			{
				return !DoorsPreventingSabotage;
			}
			return false;
		}
	}

	private bool DoorsPreventingSabotage
	{
		get
		{
			if (doors != null)
			{
				return doors.IsActive;
			}
			return false;
		}
	}

	public void Start()
	{
		for (int i = 0; i < rooms.Length; i++)
		{
			rooms[i].Parent = this;
		}
		sabSystem = (SabotageSystemType)ShipStatus.Instance.Systems[SystemTypes.Sabotage];
		if (ShipStatus.Instance.Systems.TryGetValue(SystemTypes.Doors, out var value))
		{
			doors = (IActivatable)value;
		}
	}

	private void FixedUpdate()
	{
		float specialActive = (DoorsPreventingSabotage ? 1f : sabSystem.PercentCool);
		for (int i = 0; i < rooms.Length; i++)
		{
			rooms[i].SetSpecialActive(specialActive);
			rooms[i].DoorsUpdate();
		}
	}

	private void OnEnable()
	{
		allButtons = GetComponentsInChildren<ButtonBehavior>(includeInactive: true);
		if (!selectedButton && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			SelectClosestButton(transform.position);
		}
		else if ((bool)selectedButton && ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick)
		{
			DeselectCurrent();
		}
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnControlTypeChanged));
	}

	private void OnDisable()
	{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnControlTypeChanged));
	}

	public void OnControlTypeChanged()
	{
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			SelectClosestButton(transform.position);
		}
		else
		{
			DeselectCurrent();
		}
	}

	private void DeselectCurrent()
	{
		if ((bool)selectedButton && (bool)selectedButton.spriteRenderer)
		{
			selectedButton.spriteRenderer.color = Color.white;
			selectedButton = null;
		}
	}

	private void Select(ButtonBehavior newSelected)
	{
		if ((bool)selectedButton)
		{
			DeselectCurrent();
		}
		selectedButton = newSelected;
		if ((bool)selectedButton && (bool)selectedButton.spriteRenderer)
		{
			selectedButton.spriteRenderer.color = Color.green;
		}
	}

	private void SelectClosestButton(Vector2 anchorSpot)
	{
		ButtonBehavior buttonBehavior = null;
		float num = float.PositiveInfinity;
		ButtonBehavior[] array = allButtons;
		foreach (ButtonBehavior buttonBehavior2 in array)
		{
			Vector2 vector = buttonBehavior2.transform.position;
			float sqrMagnitude = (anchorSpot - vector).sqrMagnitude;
			if (sqrMagnitude < num || buttonBehavior == null)
			{
				num = sqrMagnitude;
				buttonBehavior = buttonBehavior2;
			}
		}
		if (buttonBehavior != null)
		{
			if (!buttonBehavior.spriteRenderer)
			{
				buttonBehavior.spriteRenderer = buttonBehavior.GetComponent<SpriteRenderer>();
			}
			Select(buttonBehavior);
		}
	}

	private void Update()
	{
		Player player = ReInput.players.GetPlayer(0);
		Vector2 vector = new Vector2(player.GetAxis(16), player.GetAxis(17));
		float magnitude = vector.magnitude;
		if (magnitude > 0.9f)
		{
			if (currentCooldown > 0f)
			{
				currentCooldown -= Time.deltaTime;
			}
			else
			{
				ButtonBehavior buttonBehavior = null;
				float num = float.PositiveInfinity;
				Vector2 vector2 = selectedButton.transform.position;
				vector /= magnitude;
				Debug.DrawRay(vector2, vector, Color.green, 5f);
				ButtonBehavior[] array = allButtons;
				foreach (ButtonBehavior buttonBehavior2 in array)
				{
					if (buttonBehavior2 != selectedButton)
					{
						Vector2 vector3 = buttonBehavior2.transform.position;
						Vector2 rhs = vector3 - vector2;
						float magnitude2 = rhs.magnitude;
						rhs /= magnitude2;
						float num2 = Vector2.Dot(vector, rhs);
						float num3 = num2 / magnitude2;
						if (num2 > 0.7f && (buttonBehavior == null || num3 > num))
						{
							num = num3;
							buttonBehavior = buttonBehavior2;
							Debug.DrawLine(vector2, vector3, Color.cyan, 5f);
						}
						else
						{
							Debug.DrawLine(vector2, vector3, Color.red, 5f);
						}
					}
				}
				if (buttonBehavior != null)
				{
					DeselectCurrent();
					Select(buttonBehavior);
					currentCooldown = 0.5f;
				}
			}
		}
		else
		{
			currentCooldown = 0f;
		}
		if ((bool)selectedButton && player.GetButtonDown(11))
		{
			selectedButton.OnClick.Invoke();
		}
	}
}
