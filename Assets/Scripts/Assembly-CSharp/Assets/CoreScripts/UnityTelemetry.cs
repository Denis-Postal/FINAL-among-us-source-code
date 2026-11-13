using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CoreScripts
{
	public class UnityTelemetry : DestroyableSingleton<UnityTelemetry>
	{
		private static readonly string[] ColorNames = new string[23]
		{
			"Red", "Blue", "Green", "Pink", "Orange", "Yellow", "Black", "White", "Purple", "Brown",
			"Cyan", "Lime", "Mahogany", "Rose", "Banana", "Gray", "Tan", "Lilac", "Aqua", "Cobalt",
			"Barf", "Salmon", "Chocolate"
		};

		private bool amHost;

		private bool gameStarted;

		private DateTime timeStarted;

		public void Init()
		{
			gameStarted = true;
		}

		public void StartGame(bool isHost, int playerCount, int impostorCount, NetworkModes gameMode, uint timesImpostor, uint gamesPlayed, uint crewStreak)
		{
			//IL_0086: Expected I4, but got Obj
			//IL_00aa: Expected I4, but got Obj
			//IL_00ce: Expected I4, but got Obj
			//IL_0111: Expected I4, but got Obj
			//IL_0135: Expected I4, but got Obj
			amHost = isHost;
			DateTime utcNow = DateTime.UtcNow;
			timeStarted = utcNow;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			RuntimePlatform platform = Application.platform;
			object value = (byte)gamesPlayed;
			dictionary.Add("Platform", value);
			object value2 = timesImpostor;
			dictionary.Add("TimesImpostor", value2);
			object obj = default;
			object value3 = (uint)(int)obj;
			dictionary.Add("CrewStreak", value3);
			object obj2 = default;
			object value4 = (uint)(int)obj2;
			dictionary.Add("GamesPlayed", value4);
			object obj3 = default;
			object value5 = (NetworkModes)obj3;
			dictionary.Add("GameMode", value5);
			if (amHost)
			{
				object obj4 = default;
				object value6 = (int)obj4;
				dictionary.Add("PlayerCount", value6);
				object obj5 = default;
				object value7 = (int)obj5;
				dictionary.Add("InfectedCount", value7);
			}
		}

		public void StartGameCosmetics(int colorId, string hatId, string skinId, string petId, string visorId, string nameplateId)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string[] colorNames = ColorNames;
			dictionary.Add("Color", (object)colorNames[colorId]);
			HatManager instance = DestroyableSingleton<HatManager>.Instance;
			HatData hatById = instance.GetHatById(hatId);
			string value = hatById.name;
			dictionary.Add("Hat", (object)value);
			HatManager instance2 = DestroyableSingleton<HatManager>.Instance;
			SkinData skinById = instance2.GetSkinById(skinId);
			string value2 = skinById.name;
			dictionary.Add("Skin", (object)value2);
			HatManager instance3 = DestroyableSingleton<HatManager>.Instance;
			string petId2 = default;
			PetData petById = instance3.GetPetById(petId2);
			string value3 = petById.name;
			dictionary.Add("Pet", (object)value3);
			HatManager instance4 = DestroyableSingleton<HatManager>.Instance;
			string visorId2 = default;
			VisorData visorById = instance4.GetVisorById(visorId2);
			string value4 = visorById.name;
			dictionary.Add("Visor", (object)value4);
			HatManager instance5 = DestroyableSingleton<HatManager>.Instance;
			string namePlateId = default;
			NamePlateData namePlateById = instance5.GetNamePlateById(namePlateId);
			string value5 = namePlateById.name;
			dictionary.Add("Nameplate", (object)value5);
		}

		public void WriteMeetingStarted(bool isEmergency)
		{
		}

		public void WriteMeetingEnded(float duration)
		{
		}

		public void WritePosition(byte playerNum, Vector2 worldPos)
		{
		}

		public void WriteMurder()
		{
		}

		public void WriteSabotageUsed(SystemTypes systemType)
		{
		}

		public void CardSwipeComplete(int attempts)
		{
		}

		public void WriteUse(byte playerNum, TaskTypes taskType, Vector3 worldPos)
		{
		}

		public void WriteCompleteTask(TaskTypes taskType)
		{
		}

		internal void WriteDisconnect(DisconnectReasons reason)
		{
		}

		public void EndGame(GameOverReason endReason)
		{
			//IL_001d: Expected I4, but got Obj
			//IL_0098: Expected F8, but got Obj
			if (gameStarted)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				object obj = default;
				object value = (GameOverReason)obj;
				dictionary.Add("Reason", value);
				if (amHost)
				{
					DateTime utcNow = DateTime.UtcNow;
					TimeSpan timeSpan = utcNow - timeStarted;
					TimeSpan timeSpan2 = default;
					double totalSeconds = timeSpan2.TotalSeconds;
					object obj2 = default;
					object value2 = (double)obj2;
					dictionary.Add("DurationSec", value2);
				}
			}
		}

		public void SendWho()
		{
		}

		public void WonGame(int colorId, string hatId, string skinId, string petId, string visorId, string nameplateId)
		{
		}
	}
}
