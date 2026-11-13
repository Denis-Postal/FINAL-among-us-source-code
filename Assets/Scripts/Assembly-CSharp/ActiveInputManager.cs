using System;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;

public class ActiveInputManager : DestroyableSingleton<ActiveInputManager>
{
	public enum InputType
	{
		Joystick,
		Keyboard,
		Touch
	}

	public InputType testCurrentControlType;

	public static InputType currentControlType = InputType.Keyboard;

	public double kChangeTime;

	public double mChangeTime;

	public double jChangeTime;

	public double tChangeTime;

	private Vector2 lastMousePosition ;

	public static Action CurrentInputSourceChanged;

	public Rewired.Controller lastUsedController;

	private static InputType PlatformDefault => InputType.Keyboard;

	public Rewired.Controller CurrentController => lastUsedController;

	private void Start()
	{
		ReInput.PlayerHelper players = ReInput.players;
		Player player = players.GetPlayer(0);
		PlayerActiveControllerChangedDelegate callback = OnLastActiveControllerChanged;
		player.controllers.AddLastActiveControllerChangedDelegate(callback);
	}

	private void Update()
	{
		UpdateActiveControlType();
	}

	private void OnLastActiveControllerChanged(Player player, Rewired.Controller controller)
	{
		// TODO(verify): reimplemented (decomp stub)
		lastUsedController = controller;
	}

	public void UpdateActiveControlType()
	{
		// TODO(verify): reimplemented (decomp stub)
		// TODO(verify): offline shim - keyboard/mouse unless a touch is present
		InputType newType = ShouldEnableTouch() ? InputType.Touch : InputType.Keyboard;
		if (newType != currentControlType)
		{
			currentControlType = newType;
			CurrentInputSourceChanged?.Invoke();
		}
	}

	private bool ShouldEnableTouch()
	{
		int touchCount = Input.touchCount;
		int num = touchCount ^ touchCount;
		int num2 = touchCount & num;
		bool flag = num2 < 0;
		bool flag2 = touchCount < 0;
		bool flag3 = touchCount == 0;
		bool flag4 = flag2 == flag;
		bool flag5 = !flag3;
		return flag5 & flag4;
	}

	public ActiveInputManager()
	{ }
}
