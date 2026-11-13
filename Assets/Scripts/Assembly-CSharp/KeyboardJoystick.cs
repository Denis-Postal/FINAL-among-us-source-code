using System;
using System.Runtime.CompilerServices;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;

public class KeyboardJoystick : MonoBehaviour, IVirtualJoystick
{
	private Vector2 del;

	private static Player player;

	private static int hudHandledFrame = -1;

	private Controller myController;

	private Collider2D[] hitBuffer;

	private int touchId;

	// TODO(verify): decompile lost the getter body; returns del (movement vector). Hardening against a latched vector: zero while disabled / no local player.
	public Vector2 DeltaL => (isActiveAndEnabled && PlayerControl.LocalPlayer) ? del : Vector2.zero;

	private void OnDisable()
	{
		del = Vector2.zero;
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			del = Vector2.zero;
		}
	}

	public Vector2 DeltaR => Vector2.zero;

	public void ToggleVisuals(bool on)
	{
	}

	private void UpdateTouchMovement()
	{
		myController.Update();
		if (touchId > -1)
		{
			Controller.TouchState touchState = myController.Touches[touchId];
			if (touchState.IsDown)
			{
				Vector2 vector = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
				del = (touchState.Position - vector).normalized;
			}
			else
			{
				touchId = -1;
				del = Vector2.zero;
			}
			return;
		}
		for (int i = 0; i < myController.Touches.Length; i++)
		{
			Controller.TouchState touchState2 = myController.Touches[i];
			if (!touchState2.TouchStart)
			{
				continue;
			}
			bool flag = false;
			int num = Physics2D.OverlapPointNonAlloc(touchState2.Position, hitBuffer, Constants.NotShipMask);
			for (int j = 0; j < num; j++)
			{
				Collider2D collider2D = hitBuffer[j];
				if ((bool)collider2D.GetComponent<ButtonBehavior>() || (bool)collider2D.GetComponent<PassiveButton>())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				touchId = i;
				break;
			}
		}
	}

	// TODO(verify): rebuilt from decomp9 (action ids KBRight=40, KBLeft=39, KBUp=44, KBDown=42); UpdateTouchMovement/mouse movement from decomp_mono
	private unsafe void Update()
	{
		if (!PlayerControl.LocalPlayer)
		{
			del = Vector2.zero;
			return;
		}
		if (player == null)
		{
			player = ReInput.players.GetPlayer(0);
		}
		del = Vector2.zero;
		if (DataManager.Settings.Input.MouseMovementEnabled)
		{
			UpdateTouchMovement();
		}
		if (del == Vector2.zero)
		{
			if (player.GetButton(RewiredConsts.Action.KBRight))
			{
				del.x += 1f;
			}
			if (player.GetButton(RewiredConsts.Action.KBLeft))
			{
				del.x -= 1f;
			}
			if (player.GetButton(RewiredConsts.Action.KBUp))
			{
				del.y += 1f;
			}
			if (player.GetButton(RewiredConsts.Action.KBDown))
			{
				del.y -= 1f;
			}
		}
		if (hudHandledFrame != Time.frameCount) // fix4a: never process HUD hotkeys twice in one frame (duplicate joystick instances)
		{
			hudHandledFrame = Time.frameCount;
			HandleHud();
		}
		if (player.GetButtonDown(RewiredConsts.Action.MenuCancel))
		{
			// tasks3: ESC closes whichever window is open (order from decomp9 KeyboardJoystick.Update: minigame, map, customization, settings;
			// MatchInfoGuide added since its original close path is the MenuCancel action in ConsoleJoystick). The in-game menu closes itself in OptionsMenuBehaviour.Update.
			if (DestroyableSingleton<HudManager>.InstanceExists && DestroyableSingleton<HudManager>.Instance.GameMenu != null && DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen)
			{
			}
			else if ((bool)MatchInfoGuide.Instance && MatchInfoGuide.Instance.IsActive)
			{
				MatchInfoGuide.Instance.ToggleActive();
			}
			else if ((bool)Minigame.Instance)
			{
				Minigame.Instance.Close();
			}
			else if (DestroyableSingleton<HudManager>.InstanceExists && (bool)MapBehaviour.Instance && MapBehaviour.Instance.IsOpen)
			{
				MapBehaviour.Instance.Close();
			}
			else if ((bool)PlayerCustomizationMenu.Instance)
			{
				PlayerCustomizationMenu.Instance.Close(canMove: true);
			}
			else if ((bool)GameSettingMenu.Instance)
			{
				GameSettingMenu.Instance.Close();
			}
		}
		del.Normalize();
	}

	// TODO(verify): rebuilt from decomp9 + 2019 baseline
	private static void HandleHud()
	{
		if (!DestroyableSingleton<HudManager>.InstanceExists)
		{
			return;
		}
		HudManager hud = DestroyableSingleton<HudManager>.Instance;
		if (player.GetButtonDown(RewiredConsts.Action.ActionTertiary) && hud.ReportButton != null)
		{
			hud.ReportButton.DoClick();
		}
		if (player.GetButtonDown(RewiredConsts.Action.ActionPrimary) && hud.UseButton != null)
		{
			// roles3: the PET button takes the USE slot (and its key) when it is shown instead of USE (TODO(verify) original binding)
			if (hud.PetButton != null && hud.PetButton.isActiveAndEnabled)
			{
				hud.PetButton.DoClick();
			}
			else
			{
				hud.UseButton.DoClick();
			}
		}
		if (player.GetButtonDown(RewiredConsts.Action.ToggleMap) && !(hud.Chat != null && hud.Chat.IsOpenOrOpening) && GameManager.Instance != null)
		{
			hud.ToggleMapVisible(GameManager.Instance.GetMapOptions());
		}
		if (player.GetButtonDown(RewiredConsts.Action.ActionQuaternary) && hud.AbilityButton != null)
		{
			hud.AbilityButton.DoClick();
		}
		NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
		if (data != null && data.Role != null && data.Role.IsImpostor)
		{
			if (player.GetButtonDown(RewiredConsts.Action.ActionSecondary) && hud.KillButton != null)
			{
				hud.KillButton.DoClick();
			}
			if (player.GetButtonDown(RewiredConsts.Action.UseVent) && hud.ImpostorVentButton != null)
			{
				hud.ImpostorVentButton.DoClick();
			}
		}
	}

	public KeyboardJoystick()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		Controller controller = new Controller();
		myController = controller;
		hitBuffer = new Collider2D[20];
		touchId = -1;
	}
}
