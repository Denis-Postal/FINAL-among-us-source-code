using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Data;
using UnityEngine;

namespace AmongUs.Data.Settings
{
	// Offline remake: the project ships the keyboard/mouse Rewired shim (unity_shims/Rewired.cs), which has no joysticks,
	// controller maps, input behaviors or save-data types. The shipped class persists exactly those Rewired objects into
	// PlayerPrefs, so with the shim there is nothing to save or load. Every entry point is therefore a defined no-op.
	// Key rebinding is persisted by Rewired.KeyMap (PlayerPrefs "AmongUsRemake.KeyBindings.v1").
	// TODO(verify): restore the shipped XML/PlayerPrefs persistence (decomp_mono/Amongus/Data/Settings/RewiredUserDataStore_PlayerData.cs) if the real Rewired runtime is ever linked.
	public class RewiredUserDataStore_PlayerData : UserDataStore
	{
		private const string LEGACY_PLAYER_PREFS_KEY_PREFIX = "RewiredSaveData5";

		private const string logPrefix = "[RewiredUserDataStore_PlayerData]";

		private const string playerPrefsKeySuffix_controllerAssignments = "ControllerAssignments";

		private static readonly Logger logger = new Logger(Logger.Category.Input, "RewiredUserDataStore_PlayerData");

		[Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
		[SerializeField]
		private bool isEnabled = true;

		[SerializeField]
		[Tooltip("Should saved data be loaded on start?")]
		private bool loadDataOnStart = true;

		[SerializeField]
		[Tooltip("Should Player Keyboard assignments be saved and loaded?")]
		private bool loadKeyboardAssignments = true;

		[NonSerialized]
		private bool loadJoystickAssignments;

		[NonSerialized]
		private bool loadMouseAssignments;

		private string PlayerPrefsKey_ControllerAssignments => string.Format("{0}_{1}", LEGACY_PLAYER_PREFS_KEY_PREFIX, playerPrefsKeySuffix_controllerAssignments);

		private bool ShouldLoadControllerAssignments
		{
			get
			{
				if (!loadKeyboardAssignments && !loadMouseAssignments)
				{
					return loadJoystickAssignments;
				}
				return true;
			}
		}

		public override void Save()
		{
			if (!isEnabled)
			{
				logger.Warning("[RewiredUserDataStore_PlayerData] Component is disabled and will not save any data.", this);
			}
		}

		public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
		}

		public override void SaveControllerData(ControllerType controllerType, int controllerId)
		{
		}

		public override void SavePlayerData(int playerId)
		{
		}

		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
		}

		public override void Load()
		{
			if (!isEnabled)
			{
				logger.Warning("[RewiredUserDataStore_PlayerData] Component is disabled and will not load any data.", this);
			}
		}

		public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId)
		{
		}

		public override void LoadControllerData(ControllerType controllerType, int controllerId)
		{
		}

		public override void LoadPlayerData(int playerId)
		{
		}

		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
		}

		protected override void OnInitialize()
		{
			if (loadDataOnStart)
			{
				Load();
			}
		}

		protected override void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
		}

		protected override void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
		{
		}

		protected override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
		}

		public override void SaveControllerMap(int playerId, ControllerMap controllerMap)
		{
		}

		public override ControllerMap LoadControllerMap(int playerId, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId)
		{
			return null;
		}
	}
}
