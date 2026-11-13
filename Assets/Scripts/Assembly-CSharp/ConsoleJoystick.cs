using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class ConsoleJoystick : MonoBehaviour, IVirtualJoystick
{
	public enum ConsoleInputState
	{
		Gameplay,
		Menu,
		Task,
		Sabotage,
		Vent,
		QuickChat,
		None
	}

	private Vector2 delta;

	private Vector2 deltaR;

	private static Player player;

	private static Rewired.Controller controller;

	private static Rewired.Controller prevController;

	public static ConsoleInputState inputState = ConsoleInputState.Menu;

	private static ConsoleInputState oldInputState = ConsoleInputState.Sabotage;

	private static int highlightedVentIndex = 0;

	public Vector2 DeltaL => delta;

	public Vector2 DeltaR => deltaR;

	public void ToggleVisuals(bool on)
	{
	}

	private void OnEnable()
	{
		RewiredControllerSupport.onPostControllersAssigned = (Action)Delegate.Combine(RewiredControllerSupport.onPostControllersAssigned, new Action(ReapplyCurrentInputState));
		if (inputState == ConsoleInputState.Menu)
		{
			SetMode_Gameplay();
		}
		else
		{
			ReapplyCurrentInputState();
		}
	}

	private void OnDisable()
	{
		RewiredControllerSupport.onPostControllersAssigned = (Action)Delegate.Remove(RewiredControllerSupport.onPostControllersAssigned, new Action(ReapplyCurrentInputState));
	}

	public static void ReapplyCurrentInputState()
	{
		Debug.Log("New controller connected, updating current input state");
		switch (inputState)
		{
		case ConsoleInputState.Gameplay:
			SetMode_Gameplay();
			break;
		case ConsoleInputState.Menu:
			SetMode_Menu();
			break;
		case ConsoleInputState.Sabotage:
			SetMode_Sabotage();
			break;
		case ConsoleInputState.Task:
			SetMode_Task();
			break;
		case ConsoleInputState.Vent:
			SetMode_Vent();
			break;
		case ConsoleInputState.QuickChat:
			SetMode_QuickChat();
			break;
		case ConsoleInputState.None:
			SetMode_None();
			break;
		}
	}

	public static void SetMode_MenuAdditive()
	{
		player = ReInput.players.GetPlayer(0);
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(2, enableMap: true);
		}
	}

	public static void SetMode_Gameplay()
	{
		inputState = ConsoleInputState.Gameplay;
		player = ReInput.players.GetPlayer(0);
		if ((bool)VirtualCursor.instance)
		{
			VirtualCursor.instance.gameObject.SetActive(value: false);
		}
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(1, enableMap: true);
			SetMapEnabled(2, enableMap: false);
			SetMapEnabled(3, enableMap: false);
			SetMapEnabled(4, enableMap: false);
		}
	}

	public static void SetMode_Menu()
	{
		inputState = ConsoleInputState.Menu;
		player = ReInput.players.GetPlayer(0);
		if ((bool)VirtualCursor.instance && (bool)ControllerManager.Instance && !ControllerManager.Instance.IsUiControllerActive)
		{
			VirtualCursor.instance.gameObject.SetActive(value: true);
		}
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(1, enableMap: false);
			SetMapEnabled(2, enableMap: true);
			SetMapEnabled(3, enableMap: false);
			SetMapEnabled(4, enableMap: false);
		}
		VirtualCursor.horizontalAxis = 9;
		VirtualCursor.verticalAxis = 10;
	}

	public static void SetMode_Task()
	{
		inputState = ConsoleInputState.Task;
		player = ReInput.players.GetPlayer(0);
		if ((bool)VirtualCursor.instance && !ControllerManager.Instance.IsUiControllerActive)
		{
			VirtualCursor.instance.gameObject.SetActive(value: true);
		}
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(1, enableMap: false);
			SetMapEnabled(2, enableMap: false);
			SetMapEnabled(3, enableMap: true);
			SetMapEnabled(4, enableMap: false);
		}
		VirtualCursor.horizontalAxis = 13;
		VirtualCursor.verticalAxis = 14;
	}

	public static void SetMode_Sabotage()
	{
		inputState = ConsoleInputState.Sabotage;
		player = ReInput.players.GetPlayer(0);
		if ((bool)VirtualCursor.instance)
		{
			VirtualCursor.instance.gameObject.SetActive(value: false);
		}
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(1, enableMap: true);
			SetMapEnabled(2, enableMap: false);
			SetMapEnabled(3, enableMap: true);
			SetMapEnabled(4, enableMap: false);
		}
		VirtualCursor.horizontalAxis = 16;
		VirtualCursor.verticalAxis = 17;
	}

	public static void SetMode_Vent()
	{
		inputState = ConsoleInputState.Vent;
		player = ReInput.players.GetPlayer(0);
		if ((bool)VirtualCursor.instance && !ControllerManager.Instance.IsUiControllerActive)
		{
			VirtualCursor.instance.gameObject.SetActive(value: false);
		}
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(1, enableMap: true);
			SetMapEnabled(2, enableMap: false);
			SetMapEnabled(3, enableMap: false);
			SetMapEnabled(4, enableMap: false);
		}
		highlightedVentIndex = -1;
		VirtualCursor.horizontalAxis = 16;
		VirtualCursor.verticalAxis = 17;
	}

	public static void SetMode_QuickChat()
	{
		inputState = ConsoleInputState.QuickChat;
		player = ReInput.players.GetPlayer(0);
		if ((bool)VirtualCursor.instance && !ControllerManager.Instance.IsUiControllerActive)
		{
			VirtualCursor.instance.gameObject.SetActive(value: true);
		}
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(1, enableMap: false);
			SetMapEnabled(2, enableMap: false);
			SetMapEnabled(3, enableMap: false);
			SetMapEnabled(4, enableMap: true);
		}
		VirtualCursor.horizontalAxis = 13;
		VirtualCursor.verticalAxis = 14;
	}

	public static void SetMode_None()
	{
		inputState = ConsoleInputState.None;
		player = ReInput.players.GetPlayer(0);
		IList<Joystick> joysticks = player.controllers.Joysticks;
		for (int i = 0; i < player.controllers.joystickCount; i++)
		{
			controller = (Rewired.Controller)joysticks[i];
			SetMapEnabled(1, enableMap: false);
			SetMapEnabled(2, enableMap: false);
			SetMapEnabled(3, enableMap: false);
			SetMapEnabled(4, enableMap: false);
		}
	}

	private static void SetMapEnabled(int rewiredCategoryIndex, bool enableMap)
	{
		if (controller != null)
		{
			player.controllers.maps.GetMap(controller, rewiredCategoryIndex, 0).enabled = enableMap;
		}
	}

	private void Update()
	{
		if (!PlayerControl.LocalPlayer)
		{
			return;
		}
		if (oldInputState != inputState)
		{
			if (DestroyableSingleton<HudManager>.Instance.consoleUIObjects != null && DestroyableSingleton<HudManager>.Instance.consoleUIObjects.Length != 0)
			{
				if (oldInputState != ConsoleInputState.None && DestroyableSingleton<HudManager>.Instance.consoleUIObjects[(int)oldInputState] != null)
				{
					DestroyableSingleton<HudManager>.Instance.consoleUIObjects[(int)oldInputState].SetActive(value: false);
				}
				if (inputState != ConsoleInputState.None && DestroyableSingleton<HudManager>.Instance.consoleUIObjects[(int)inputState] != null)
				{
					DestroyableSingleton<HudManager>.Instance.consoleUIObjects[(int)inputState].SetActive(value: true);
				}
			}
			oldInputState = inputState;
		}
		if (inputState == ConsoleInputState.Menu && ControllerManager.Instance.IsUiControllerActive != DestroyableSingleton<HudManager>.Instance.consoleUIObjects[(int)inputState].activeSelf)
		{
			DestroyableSingleton<HudManager>.Instance.consoleUIObjects[(int)inputState].SetActive(ControllerManager.Instance.IsUiControllerActive);
		}
		delta.x = player.GetAxis(2);
		delta.y = player.GetAxis(3);
		deltaR.x = player.GetAxis(54);
		deltaR.y = player.GetAxis(55);
		if (delta.sqrMagnitude > 1f)
		{
			delta = delta.normalized;
		}
		HandleHUD();
	}

	private void HandleHUD()
	{
		if ((bool)PlayerControl.LocalPlayer)
		{
			bool canMove = PlayerControl.LocalPlayer.CanMove;
			if (!canMove && inputState == ConsoleInputState.Gameplay)
			{
				SetMode_Menu();
				return;
			}
			if (canMove && inputState != ConsoleInputState.Sabotage && inputState != ConsoleInputState.Gameplay)
			{
				SetMode_Gameplay();
				return;
			}
		}
		if (inputState == ConsoleInputState.Vent)
		{
			if (AprilFoolsMode.ShouldFlipSkeld() && ShipStatus.Instance is SkeldShipStatus)
			{
				delta.x = 0f - delta.x;
			}
			if (!ControllerManager.Instance.IsUiControllerActive)
			{
				if (!Vent.currentVent)
				{
					SetMode_Gameplay();
				}
				else
				{
					bool flag = false;
					if (delta.sqrMagnitude > 0.25f)
					{
						flag = true;
						Vector2 normalized = delta.normalized;
						Vector2 zero = Vector2.zero;
						float num = float.NegativeInfinity;
						int num2 = -1;
						for (int i = 0; i < Vent.currentVent.Buttons.Length; i++)
						{
							if (Vent.currentVent.Buttons[i].isActiveAndEnabled)
							{
								zero = ((Vector2)Vent.currentVent.Buttons[i].transform.localPosition).normalized;
								float num3 = Vector2.Dot(normalized, zero);
								if (num2 == -1 || num3 > num)
								{
									num = num3;
									num2 = i;
								}
							}
						}
						if (num > 0.7f)
						{
							if (highlightedVentIndex != -1 && highlightedVentIndex < Vent.currentVent.Buttons.Length)
							{
								Vent.currentVent.Buttons[highlightedVentIndex].spriteRenderer.color = Color.white;
								highlightedVentIndex = -1;
							}
							highlightedVentIndex = num2;
							Vent.currentVent.Buttons[highlightedVentIndex].spriteRenderer.color = Color.red;
						}
						else if (highlightedVentIndex != -1 && highlightedVentIndex < Vent.currentVent.Buttons.Length)
						{
							Vent.currentVent.Buttons[highlightedVentIndex].spriteRenderer.color = Color.white;
							highlightedVentIndex = -1;
						}
					}
					else if (highlightedVentIndex != -1 && highlightedVentIndex < Vent.currentVent.Buttons.Length)
					{
						Vent.currentVent.Buttons[highlightedVentIndex].spriteRenderer.color = Color.white;
						highlightedVentIndex = -1;
					}
					if (!flag)
					{
						if (player.GetButtonDown(6))
						{
							DestroyableSingleton<HudManager>.Instance.UseButton.DoClick();
						}
						if (player.GetButtonDown(49))
						{
							DestroyableSingleton<HudManager>.Instance.AbilityButton.DoClick();
						}
						if (player.GetButtonDown(8))
						{
							DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.DoClick();
						}
						if (PlayerControl.LocalPlayer.Data != null && PlayerControl.LocalPlayer.Data.Role != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor && player.GetButtonDown(50))
						{
							DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.DoClick();
						}
					}
					else if (highlightedVentIndex != -1 && player.GetButtonDown(6))
					{
						Vent.currentVent.Buttons[highlightedVentIndex].spriteRenderer.color = Color.white;
						Vent.currentVent.Buttons[highlightedVentIndex].OnClick.Invoke();
						highlightedVentIndex = -1;
					}
				}
			}
		}
		else
		{
			if (player.GetButtonDown(49))
			{
				DestroyableSingleton<HudManager>.Instance.AbilityButton.DoClick();
			}
			if (player.GetButtonDown(8))
			{
				DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton.DoClick();
			}
			if (player.GetButtonDown(7))
			{
				DestroyableSingleton<HudManager>.Instance.ReportButton.DoClick();
			}
			if (player.GetButtonDown(6))
			{
				DestroyableSingleton<HudManager>.Instance.UseButton.DoClick();
			}
			if (PlayerControl.LocalPlayer.Data != null && PlayerControl.LocalPlayer.Data.Role != null)
			{
				if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && player.GetButtonDown(8))
				{
					DestroyableSingleton<HudManager>.Instance.KillButton.DoClick();
				}
				if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && player.GetButtonDown(50))
				{
					DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.DoClick();
				}
			}
		}
		if (player.GetButtonDown(5) && DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.TaskPanel.ToggleOpen();
		}
		if (player.GetButtonDown(1) && (!MatchInfoGuide.Instance || !MatchInfoGuide.Instance.IsActive))
		{
			if (DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen)
			{
				DestroyableSingleton<HudManager>.Instance.GameMenu.Close();
			}
			else
			{
				DestroyableSingleton<HudManager>.Instance.GameMenu.Open();
			}
		}
		if ((bool)MatchInfoGuide.Instance && (bool)ShipStatus.Instance && ShipStatus.Instance.CosmeticsCache.initialized && !DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen && (player.GetButtonDown(38) || (MatchInfoGuide.Instance.IsActive && player.GetButtonDown(12))))
		{
			MatchInfoGuide.Instance.ToggleActive();
		}
		if (player.GetButtonDown(4))
		{
			if (inputState == ConsoleInputState.Sabotage)
			{
				SetMode_Gameplay();
				if ((bool)MapBehaviour.Instance)
				{
					MapBehaviour.Instance.Close();
				}
			}
			else
			{
				DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(GameManager.Instance.GetMapOptions());
			}
		}
		if (player.GetButtonDown(35) && inputState == ConsoleInputState.Menu && (bool)MeetingHud.Instance && DestroyableSingleton<HudManager>.Instance.MapButton.isActiveAndEnabled)
		{
			DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(new MapOptions
			{
				Mode = MapOptions.Modes.Normal
			});
		}
		if (player.GetButtonDown(12) && !ControllerManager.Instance.IsUiControllerActive)
		{
			if ((bool)Minigame.Instance)
			{
				Minigame.Instance.Close();
			}
			else if ((bool)MapBehaviour.Instance)
			{
				MapBehaviour.Instance.Close();
			}
			if ((bool)Vent.currentVent)
			{
				SetMode_Vent();
			}
			else if (inputState != ConsoleInputState.Gameplay)
			{
				SetMode_Gameplay();
			}
		}
	}
}
