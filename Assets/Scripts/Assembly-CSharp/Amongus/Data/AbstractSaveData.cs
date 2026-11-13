using System;
using System.Runtime.CompilerServices;
using System.Text;
using Cpp2ILInjected;
using Innersloth.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace AmongUs.Data
{
	public abstract class AbstractSaveData : AbstractSaveDataGroup
	{
		[Serializable]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9;

			public static Action<ISaveDataGroupControl> _003C_003E9__20_0;

			public static Action<ISaveDataGroupControl> _003C_003E9__20_1;

			public static Action<ISaveDataGroupControl> _003C_003E9__22_0;

			public static Action<ISaveDataGroupControl> _003C_003E9__22_1;

			static _003C_003Ec()
			{
				_003C_003Ec obj = new _003C_003Ec();
				_003C_003E9 = obj;
			}

			internal void _003CForceSave_003Eb__20_0(ISaveDataGroupControl group)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @180002A40");
			}

			internal void _003CForceSave_003Eb__20_1(ISaveDataGroupControl group)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @180002A40");
			}

			internal void _003CForceLoad_003Eb__22_0(ISaveDataGroupControl group)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @180002A40");
			}

			internal void _003CForceLoad_003Eb__22_1(ISaveDataGroupControl group)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @180002A40");
			}
		}

		private Action m_OnSavedEvent;

		private Action m_OnLoadedEvent;

		private bool groupsSubscribed;

		[JsonProperty("dataVersion")]
		protected int dataVersion;

		private const byte XOR_KEY = 212;

		public bool IsDirty { get; private set; }

		protected abstract AbstractSaveDataGroup[] Groups { get; }

		protected virtual int CurrentVersion => 0;

		public event Action OnSavedEvent
		{
			add
			{
				m_OnSavedEvent = (Action)Delegate.Combine(m_OnSavedEvent, value);
			}
			remove
			{
				m_OnSavedEvent = (Action)Delegate.Remove(m_OnSavedEvent, value);
			}
		}

		public event Action OnLoadedEvent
		{
			add
			{
				m_OnLoadedEvent = (Action)Delegate.Combine(m_OnLoadedEvent, value);
			}
			remove
			{
				m_OnLoadedEvent = (Action)Delegate.Remove(m_OnLoadedEvent, value);
			}
		}

		public AbstractSaveData()
		{
		}

		protected abstract void HandleSave();

		protected abstract void HandleLoad();

		protected abstract void HandleDelete();

		protected abstract bool HandleDoesFileExist();

		protected override void HandleChange()
		{
			// TODO(verify): reimplemented (decomp stub)
			IsDirty = true;
			base.HandleChange();
		}

		public bool DoesFileExist()
		{
			// TODO(verify): reimplemented (decomp stub)
			return HandleDoesFileExist();
		}

		public void Save()
		{
			if (IsDirty)
			{
				ForceSave();
			}
		}

		public void ForceSave()
		{
			SubscribeToGroups();
			// TODO(verify): reimplemented (decomp stub)
			ForEachGroup(g => g.SaveStart());
			OnSaveStart();
			HandleSave();
			IsDirty = false;
			OnSaveComplete();
			ForEachGroup(g => g.SaveComplete());
			m_OnSavedEvent?.Invoke();
		}

		public void Load()
		{
			// TODO(verify): decomp showed `if (IsDirty)`; always loading is the only sensible behaviour
			ForceLoad();
		}

		public void ForceLoad()
		{
			SubscribeToGroups();
			// TODO(verify): reimplemented (decomp stub)
			ForEachGroup(g => g.LoadStart());
			OnLoadStart();
			HandleLoad();
			CheckVersionMigration();
			IsDirty = false;
			OnLoadComplete();
			ForEachGroup(g => g.LoadComplete());
			m_OnLoadedEvent?.Invoke();
		}

		private void ForEachGroup(Action<ISaveDataGroupControl> action)
		{
			AbstractSaveDataGroup[] groups = Groups;
			if (groups != null)
			{
				Extensions.ForEach(groups, action);
			}
		}

		private void SubscribeToGroups()
		{
			if (groupsSubscribed)
			{
				return;
			}
			ForEachGroup(group =>
			{
				if (group is AbstractSaveDataGroup saveDataGroup)
				{
					saveDataGroup.OnChangedEvent += HandleChange;
				}
			});
			groupsSubscribed = true;
		}

		public void Delete()
		{
			// TODO(verify): reimplemented (decomp stub)
			HandleDelete();
		}

		private void CheckVersionMigration()
		{
			int currentVersion = CurrentVersion;
			if (dataVersion < currentVersion)
			{
				HandleVersionMigration(dataVersion);
			}
		}

		protected virtual void HandleVersionMigration(int version)
		{
		}

		protected static bool TrySaveToJsonFile(object value, string filename)
		{
			// TODO(verify): reimplemented (decomp stub)
			try
			{
				string path = System.IO.Path.Combine(Application.persistentDataPath, filename);
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
				System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(value));
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		protected unsafe static bool TryLoadFromJsonFile<T>(string filename, out T output)
		{
			// TODO(verify): reimplemented (decomp stub)
			output = default(T);
			try
			{
				string path = System.IO.Path.Combine(Application.persistentDataPath, filename);
				if (!System.IO.File.Exists(path)) return false;
				output = JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
				return output != null;
			}
			catch (Exception)
			{
				return false;
			}
		}

		protected static bool TryLoadAndOverwriteFromJsonFile<T>(T target, string filename)
		{
			// TODO(verify): reimplemented (decomp stub)
			try
			{
				string path = System.IO.Path.Combine(Application.persistentDataPath, filename);
				if (!System.IO.File.Exists(path)) return false;
				JsonConvert.PopulateObject(System.IO.File.ReadAllText(path), target);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		protected unsafe bool TrySerializeAndEncrypt(object target, out byte[] output)
		{
			// TODO(verify): reimplemented (decomp stub)
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(target));
				for (int i = 0; i < bytes.Length; i++) bytes[i] ^= XOR_KEY;
				output = bytes;
				return true;
			}
			catch (Exception)
			{
				output = null;
				return false;
			}
		}

		protected unsafe bool TryDecryptAndDeserialize<T>(ref T target, byte[] encryptedBytes)
		{
			// TODO(verify): reimplemented (decomp stub)
			try
			{
				byte[] bytes = (byte[])encryptedBytes.Clone();
				for (int i = 0; i < bytes.Length; i++) bytes[i] ^= XOR_KEY;
				JsonConvert.PopulateObject(Encoding.UTF8.GetString(bytes), target);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
