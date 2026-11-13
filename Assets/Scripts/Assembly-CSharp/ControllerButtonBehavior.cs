using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class ControllerButtonBehavior : MonoBehaviour
{
	public RewiredConstsEnum.Action Action;

	public GameObject requiredMenuObject;

	public List<string> requiredMenuNames;

	public List<string> restrictedMenuNames;

	public UiElement requiredSelectedObject;

	private Player player;

	private Action m_OnPressedEvent;

	public event Action OnPressedEvent
	{
		add
		{
			m_OnPressedEvent = (Action)System.Delegate.Combine(m_OnPressedEvent, value);
		}
		remove
		{
			m_OnPressedEvent = (Action)System.Delegate.Remove(m_OnPressedEvent, value);
		}
	}

	private void Start()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void Update()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	public ControllerButtonBehavior()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		List<string> list = new List<string>();
		requiredMenuNames = list;
		restrictedMenuNames = new List<string>();
	}
}
