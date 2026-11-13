using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace AmongUs.Data.Settings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class InputSettingsData : AbstractSaveDataGroup
	{
		public const ControlTypes DEFAULT_INPUT_MODE = ControlTypes.Keyboard;

		public const bool DEFAULT_MOUSE_MOVEMENT_ENABLED = false;

		public const float DEFAULT_TOUCH_JOYSTICK_SIZE = 1f;

		private const string INIT_KEY = "initialization";

		private const string INIT_VALUE_DEFAULT = "uninitialized";

		private const string INIT_VALUE_INITIALIZED = "initialized";

		[JsonProperty("inputMode")]
		public ControlTypes inputMode = ControlTypes.Keyboard;

		[JsonProperty("mouseMovementEnabled")]
		private bool mouseMovementEnabled;

		[JsonProperty("touchJoystickSize")]
		public float touchJoystickSize = 1f;

		[JsonProperty("inputData")]
		public Dictionary<string, string> inputData = new Dictionary<string, string> { { "initialization", "uninitialized" } };

		public ControlTypes InputMode
		{
			get
			{
				return inputMode;
			}
			set
			{
				SetValue(ref inputMode, ValidateInputMode(value), OnInputModeChanged);
			}
		}

		public bool MouseMovementEnabled
		{
			get
			{
				return mouseMovementEnabled;
			}
			set
			{
				SetValue(ref mouseMovementEnabled, value, OnMouseMovementEnabledChanged);
			}
		}

		public float TouchJoystickSize
		{
			get
			{
				return touchJoystickSize;
			}
			set
			{
				SetValue(ref touchJoystickSize, value, OnTouchJoystickSizeChanged);
			}
		}

		public event Action OnInputModeChanged;

		public event Action OnMouseMovementEnabledChanged;

		public event Action OnTouchJoystickSizeChanged;

		public event Action OnInputDataChanged;

		protected override void OnLoadComplete()
		{
			inputMode = ValidateInputMode(inputMode);
		}

		protected override void OnSaveStart()
		{
			base.OnSaveStart();
			if (!inputData.ContainsKey("initialization"))
			{
				inputData.Add("initialization", "uninitialized");
			}
			inputData["initialization"] = "initialized";
		}

		protected override void OnSaveComplete()
		{
			base.OnSaveComplete();
			bool flag = false;
			foreach (string key in inputData.Keys)
			{
				PlayerPrefs.DeleteKey(key);
				flag = true;
			}
			if (flag)
			{
				PlayerPrefs.Save();
			}
		}

		private static ControlTypes ValidateInputMode(ControlTypes mode)
		{
			if (Application.isConsolePlatform)
			{
				return ControlTypes.Controller;
			}
			if (Application.isMobilePlatform && (mode == ControlTypes.Keyboard || mode == ControlTypes.Controller))
			{
				mode = ControlTypes.VirtualJoystick;
			}
			return mode;
		}

		public bool HasInputDataValue(string key)
		{
			if (!PlayerPrefs.HasKey(key))
			{
				return inputData.ContainsKey(key);
			}
			return true;
		}

		public string GetInputDataValue(string key)
		{
			if (PlayerPrefs.HasKey(key) && !inputData.ContainsKey(key))
			{
				string value = PlayerPrefs.GetString(key);
				inputData[key] = value;
			}
			if (!inputData.ContainsKey(key))
			{
				return null;
			}
			return inputData[key];
		}

		public void SetInputDataValue(string key, string value)
		{
			inputData[key] = value;
			OnInputDataChanged?.Invoke();
			HandleChange();
		}
	}
}
