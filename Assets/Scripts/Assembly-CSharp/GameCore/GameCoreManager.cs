using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GameCore
{
	public class GameCoreManager : MonoBehaviour
	{
		public LocalizationProvider LocalizationProvider;

		public Action OnSuspend;

		public Action<double> OnResume;

		public Action<bool> OnResourceChange;

		private Thread dispatchJob;

		private bool stopExecution;

		private Dictionary<string, ulong> puid2xuid= new Dictionary<string, ulong>();

		private List<KeyValuePair<string, ulong>> xUIDsToCheckIfBlocked= new List<KeyValuePair<string, ulong>>();

		private HashSet<string> nonXuids= new HashSet<string>();

		private Dictionary<string, string> censoredTexts= new Dictionary<string, string>();

		public static GameCoreManager Instance { get; private set; }

		public UserManager UserManager { get; private set; }

		public NetworkManager NetworkManager { get; private set; }

		public SaveManager SaveManager { get; private set; }

		public StoreManager StoreManager { get; private set; }

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		public GameCoreManager()
		{ }
	}
}
