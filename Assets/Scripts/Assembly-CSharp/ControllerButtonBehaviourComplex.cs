using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class ControllerButtonBehaviourComplex : MonoBehaviour
{
	public enum ActionTriggerType
	{
		Any,
		All
	}

	[Serializable]
	public struct ActionTrigger
	{
		public enum ActionType
		{
			Axis_GEqual,
			Axis_LEqual,
			Button_Down
		}

		public RewiredConstsEnum.Action action;

		public ActionType actionType;

		public float axisComparisonValue;

		public bool ConditionMet(Player player)
		{
			return actionType switch
			{
				ActionType.Button_Down => player.GetButtonDown((int)action), 
				ActionType.Axis_GEqual => player.GetAxisRaw((int)action) >= axisComparisonValue, 
				ActionType.Axis_LEqual => player.GetAxisRaw((int)action) <= axisComparisonValue, 
				_ => false, 
			};
		}
	}

	public ActionTriggerType actionTriggerType;

	public ActionTrigger[] actionTriggers;

	public GameObject requiredMenuObject;

	private Player player;

	private void Start()
	{
		player = ReInput.players.GetPlayer(0);
	}

	private bool ConditionMet()
	{
		ActionTrigger[] array;
		if (actionTriggerType == ActionTriggerType.All)
		{
			array = actionTriggers;
			foreach (ActionTrigger actionTrigger in array)
			{
				if (!actionTrigger.ConditionMet(player))
				{
					return false;
				}
			}
			return true;
		}
		array = actionTriggers;
		foreach (ActionTrigger actionTrigger2 in array)
		{
			if (actionTrigger2.ConditionMet(player))
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		if (!ConditionMet() || ((bool)requiredMenuObject && ControllerManager.Instance.CurrentUiState.MenuName != requiredMenuObject.name))
		{
			return;
		}
		ButtonBehavior component = GetComponent<ButtonBehavior>();
		if ((bool)component)
		{
			component.OnClick?.Invoke();
			return;
		}
		PassiveButton component2 = GetComponent<PassiveButton>();
		if ((bool)component2)
		{
			component2.OnClick?.Invoke();
		}
	}
}
