using Newtonsoft.Json;
﻿using System;
using System.Collections.Generic;
using AmongUs.Data.Legacy;
using Cpp2ILInjected;

namespace AmongUs.Data.Settings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class SettingsData : AbstractUserSaveData
	{
		public GameplaySettingsData gameplay = new GameplaySettingsData
		{
			screenShake = true
		};

		[JsonProperty("accessibility")]
		public AccessibilitySettingsData accessibility = new AccessibilitySettingsData();

		public AudioSettingsData audio = new AudioSettingsData
		{
			musicVolume = 0.6f,
			ambienceVolume = 0.6f,
			sfxVolume = 0.6f
		};

		[JsonProperty("video")]
		public VideoSettingsData video = new VideoSettingsData();

		[JsonProperty("language")]
		public LanguageSettingsData language = new LanguageSettingsData();

		public InputSettingsData input = new InputSettingsData
		{
			inputMode = ControlTypes.Keyboard,
			touchJoystickSize = 1f,
			inputData = new Dictionary<string, string> { { "initialization", "uninitialized" } }
		};

		[JsonProperty("multiplayer")]
		public MultiplayerSettingsData multiplayer = new MultiplayerSettingsData();

		protected override int CurrentVersion => 1;

		public GameplaySettingsData Gameplay => gameplay;

		public AccessibilitySettingsData Accessibility => accessibility;

		public AudioSettingsData Audio => audio;

		public VideoSettingsData Video => video;

		public InputSettingsData Input => input;

		public LanguageSettingsData Language => language;

		public MultiplayerSettingsData Multiplayer => multiplayer;

		protected override string FileName
		{
			get
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363D88B]");
				if ((nint)0 == 0)
				{
					_ = 1;
				}
				return "settings.amogus";
			}
		}

		protected override AbstractSaveDataGroup[] Groups
		{
			get
			{
				return new AbstractSaveDataGroup[] { gameplay, accessibility, audio, video, language, input, multiplayer }; // TODO(verify): order inferred
			}
		}

		protected unsafe override void HandleVersionMigration(int version)
		{
			// TODO(verify): migration steps lost in decompile; no-op (fresh offline save needs none)
		}

		public SettingsData()
		{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
			// Initialize groups before AbstractSaveData subscribes to their change events.
		}
	}
}
