// Offline keyboard/mouse input shim replacing the Rewired runtime. Action ids are the ones in RewiredConsts.Action
// (see KeyboardMap). TODO(verify): key bindings are an approximation of the default Among Us PC layout
// (WASD/arrows move, E/Space use, Q kill, R report, Tab map, Esc back).
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Rewired
{
	public enum ControllerType { Keyboard, Mouse, Joystick, Custom }
	public enum AxisCoordinateMode { Absolute, Relative }
	public enum Pole { Positive, Negative }
	public enum InputActionType { Axis, Button }
	public enum UpdateLoopType { Update, FixedUpdate, OnGUI }
	public enum ControllerElementType { Axis, Button }
	public enum ControllerPollingInfoType { Button, Axis }

	public static class KeyMap
	{
		// action id -> (positive keys, negative keys). Filled by name via RewiredConsts when available.
		// TODO(verify): action-id -> key mapping inferred from RewiredConsts.Action names (real Rewired maps not available).
		static readonly System.Collections.Generic.Dictionary<int, KeyCode[]> pos = new System.Collections.Generic.Dictionary<int, KeyCode[]>
		{
			{ RewiredConsts.Action.Pause, new[] { KeyCode.Escape } },
			{ RewiredConsts.Action.MoveHorizontal, new[] { KeyCode.D, KeyCode.RightArrow } },
			{ RewiredConsts.Action.MoveVertical, new[] { KeyCode.W, KeyCode.UpArrow } },
			{ RewiredConsts.Action.ToggleMap, new[] { KeyCode.Tab } },
			{ RewiredConsts.Action.KBRight, new[] { KeyCode.D, KeyCode.RightArrow } },
			{ RewiredConsts.Action.KBLeft, new[] { KeyCode.A, KeyCode.LeftArrow } },
			{ RewiredConsts.Action.KBUp, new[] { KeyCode.W, KeyCode.UpArrow } },
			{ RewiredConsts.Action.KBDown, new[] { KeyCode.S, KeyCode.DownArrow } },
			{ RewiredConsts.Action.ActionQuaternary, new[] { KeyCode.F } },
			{ RewiredConsts.Action.TogglePlayerList, new[] { KeyCode.G } },
			{ RewiredConsts.Action.ToggleTasks, new[] { KeyCode.T } },
			{ RewiredConsts.Action.ActionPrimary, new[] { KeyCode.E, KeyCode.Space } },   // use
			{ RewiredConsts.Action.UseVent, new[] { KeyCode.V } },
			{ RewiredConsts.Action.ActionSecondary, new[] { KeyCode.Q } },                // kill
			{ RewiredConsts.Action.ActionTertiary, new[] { KeyCode.R } },                 // report
			{ RewiredConsts.Action.MenuHorizontal, new[] { KeyCode.D, KeyCode.RightArrow } },
			{ RewiredConsts.Action.MenuVertical, new[] { KeyCode.W, KeyCode.UpArrow } },
			{ RewiredConsts.Action.MenuConfirm, new[] { KeyCode.Return, KeyCode.Space } },
			{ RewiredConsts.Action.MenuCancel, new[] { KeyCode.Escape } },
		};
		static readonly System.Collections.Generic.Dictionary<int, KeyCode[]> neg = new System.Collections.Generic.Dictionary<int, KeyCode[]>
		{
			{ RewiredConsts.Action.MoveHorizontal, new[] { KeyCode.A, KeyCode.LeftArrow } },
			{ RewiredConsts.Action.MoveVertical, new[] { KeyCode.S, KeyCode.DownArrow } },
			{ RewiredConsts.Action.MenuHorizontal, new[] { KeyCode.A, KeyCode.LeftArrow } },
			{ RewiredConsts.Action.MenuVertical, new[] { KeyCode.S, KeyCode.DownArrow } },
		};
		static readonly KeyCode[] none = new KeyCode[0];
		public static readonly System.Collections.Generic.HashSet<int> TestHeld = new System.Collections.Generic.HashSet<int>(); // capture/test hook: actions treated as held
		// capture/test hooks: IgnorePhysical makes the shim ignore the real keyboard; Pressed = one-shot GetButtonDown injections (value = frame requested)
		public static bool IgnorePhysical;
		public static readonly System.Collections.Generic.Dictionary<int, int> Pressed = new System.Collections.Generic.Dictionary<int, int>();
#if UNITY_EDITOR
		public static bool Key(KeyCode k) => !IgnorePhysical && !TypingBlocks(k) && Input.GetKey(k);
		public static bool KeyDown(KeyCode k) => !IgnorePhysical && !TypingBlocks(k) && Input.GetKeyDown(k);
		public static bool KeyUp(KeyCode k) => !IgnorePhysical && !TypingBlocks(k) && Input.GetKeyUp(k);
#else
		// built player: physical keys always work; IgnorePhysical/Pressed/TestHeld are capture-only (set from the editor MenuCapture tool)
		public static bool Key(KeyCode k) => !TypingBlocks(k) && Input.GetKey(k);
		public static bool KeyDown(KeyCode k) => !TypingBlocks(k) && Input.GetKeyDown(k);
		public static bool KeyUp(KeyCode k) => !TypingBlocks(k) && Input.GetKeyUp(k);
#endif
		// Space / Enter are handled by the focused text box (typing, OnEnter); they must not also drive Use / Confirm (quick chat, buttons)
		static bool TypingBlocks(KeyCode k) => (k == KeyCode.Space || k == KeyCode.Return || k == KeyCode.KeypadEnter) && TextBoxTMP.AnyFocused; // Enter sends the message via TextBoxTMP.OnEnter, not via the Confirm action
		public static bool ConsumePressed(int a) { if (Pressed.TryGetValue(a, out int f) && Time.frameCount - f <= 3) { Pressed.Remove(a); return true; } return false; }
		// ---- user key bindings (Settings > Keyboard > Remap Controls). Overrides replace the default keys of an action; stored in PlayerPrefs. ----
		public static readonly Dictionary<int, KeyCode[]> User = new Dictionary<int, KeyCode[]>();
		const string PrefKey = "AmongUsRemake.KeyBindings.v1";
		// actions shown in the Keyboard Bindings page (id, label); TODO(verify) labels/order of the original ControlMapper page
		public static readonly KeyValuePair<int, string>[] Bindable = new[]
		{
			new KeyValuePair<int, string>(RewiredConsts.Action.KBUp, "Move Up"),
			new KeyValuePair<int, string>(RewiredConsts.Action.KBDown, "Move Down"),
			new KeyValuePair<int, string>(RewiredConsts.Action.KBLeft, "Move Left"),
			new KeyValuePair<int, string>(RewiredConsts.Action.KBRight, "Move Right"),
			new KeyValuePair<int, string>(RewiredConsts.Action.ActionPrimary, "Use"),
			new KeyValuePair<int, string>(RewiredConsts.Action.ActionTertiary, "Report"),
			new KeyValuePair<int, string>(RewiredConsts.Action.ActionSecondary, "Kill"),
			new KeyValuePair<int, string>(RewiredConsts.Action.UseVent, "Vent"),
			new KeyValuePair<int, string>(RewiredConsts.Action.ActionQuaternary, "Ability"),
			new KeyValuePair<int, string>(RewiredConsts.Action.ToggleMap, "Map"),
			new KeyValuePair<int, string>(RewiredConsts.Action.ToggleTasks, "Tasks"),
			new KeyValuePair<int, string>(RewiredConsts.Action.TogglePlayerList, "Player List"),
		};
		public static KeyCode[] DefaultKeys(int a) => pos.TryGetValue(a, out var k) ? k : none;
		public static void LoadUser()
		{
			User.Clear();
			try
			{
				string raw = PlayerPrefs.GetString(PrefKey, "");
				foreach (string entry in raw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				{
					string[] kv = entry.Split(':');
					if (kv.Length != 2 || !int.TryParse(kv[0], out int id)) continue;
					var keys = new List<KeyCode>();
					foreach (string n in kv[1].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
						if (Enum.TryParse(n, out KeyCode kc)) keys.Add(kc);
					User[id] = keys.ToArray();
				}
			}
			catch (Exception e) { Debug.LogWarning("[KeyMap] could not load key bindings: " + e.Message); }
		}
		public static void SaveUser()
		{
			try
			{
				var sb = new System.Text.StringBuilder();
				foreach (var kv in User) sb.Append(kv.Key).Append(':').Append(string.Join("|", Array.ConvertAll(kv.Value, k => k.ToString()))).Append(';');
				PlayerPrefs.SetString(PrefKey, sb.ToString());
				PlayerPrefs.Save();
			}
			catch (Exception e) { Debug.LogWarning("[KeyMap] could not save key bindings: " + e.Message); }
		}
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void InitUserBindings() { LoadUser(); }
		public static Func<int, KeyCode[]> Positive = a => User.TryGetValue(a, out var u) ? u : (pos.TryGetValue(a, out var k) ? k : none);
		public static Func<int, KeyCode[]> Negative = a => neg.TryGetValue(a, out var k) ? k : none;
	}

	public class InputActionEventData { public int actionId; public Player player; public ControllerType controllerType; public float GetAxis() => 0f; public bool GetButtonDown() => false; }

	public class Controller
	{
		public virtual ControllerType type => ControllerType.Keyboard;
		public string name = "Keyboard";
		public int id;
		public string hardwareName => name;
		public Guid hardwareTypeGuid;
		public bool isConnected = true; public T GetExtension<T>() where T : class => null;
		public event Action<Controller> dummy;
		// mono splice: GameOptionButton waits on CurrentController.GetAnyButtonUp (the cpp2il dummy dll has it, this shim lacked it)
		static int _upFrame = -1; static bool _prevAny, _upNow;
		public bool GetAnyButton() => Input.anyKey;
		public bool GetAnyButtonDown() => Input.anyKeyDown;
		public bool GetAnyButtonUp()
		{
			if (_upFrame != Time.frameCount) { _upFrame = Time.frameCount; bool now = Input.anyKey; _upNow = _prevAny && !now; _prevAny = now; }
			return _upNow;
		}
	}
	public class Keyboard : Controller { }
	public class Mouse : Controller { public Vector2 screenPosition => Input.mousePosition; public Vector2 wheelDelta => Input.mouseScrollDelta; }
	public class Joystick : Controller
	{
		public override ControllerType type => ControllerType.Joystick;
		public Guid deviceInstanceGuid; public int[] axisCount; public bool supportsVibration; public int vibrationMotorCount;
		public void SetVibration(float left, float right) { }
		public void StopVibration() { }
		public T GetExtension<T>() where T : class => null;
		public bool unityJoystickId;
	}
	public class ControllerIdentifier { public ControllerType controllerType; public int controllerId; public Guid controllerGuid; public int controllerHardwareId; }
	public class ControllerPollingInfo { public bool success; public ControllerType controllerType; public int controllerId; public int elementIdentifierId; public ControllerElementType elementType; public Pole axisPole; public string elementIdentifierName = ""; public Controller controller; }
	public class ControllerElementTarget { public int elementIdentifierId; public Pole axisContribution; }
	public class ControllerStatusChangedEventArgs { public string name; public int controllerId; public ControllerType controllerType; public Controller controller; }
	public class InputBehavior { public string name = ""; public int id; }
	public class ElementAssignmentChange { }
	public class ElementAssignmentConflictInfo { }
	public class ElementAssignmentConflictCheck { }
	public class ActionElementMap
	{
		public int actionId; public int elementIdentifierId; public ControllerElementType elementType; public Pole axisContribution; public string elementIdentifierName = ""; public KeyCode keyCode; public Pole axisType; public int id;
		public string elementDisplayName => elementIdentifierName;
		public ControllerMap controllerMap;
	}
	public class ControllerMap
	{
		public int id; public int categoryId; public int layoutId; public ControllerType controllerType; public int controllerId; public string name = ""; public bool enabled = true;
		public IList<ActionElementMap> AllMaps = new List<ActionElementMap>();
		public ActionElementMap GetElementMap(int id) => null;
		public IList<ActionElementMap> ElementMapsWithAction(int actionId) => new List<ActionElementMap>();
		public IList<ActionElementMap> GetButtonMapsWithAction(int actionId) => new List<ActionElementMap>();
		public IList<ActionElementMap> GetAxisMapsWithAction(int actionId) => new List<ActionElementMap>();
		public int ButtonMapCount => 0;
		public int AxisMapCount => 0;
		public IList<ActionElementMap> ButtonMaps => new List<ActionElementMap>();
		public IList<ActionElementMap> AxisMaps => new List<ActionElementMap>();
	}
	public class Player
	{
		public int id; public string name = "Player";
		public bool isPlaying => true;
		public ControllerHelper_ controllers = new ControllerHelper_();
		public MapHelper_ controllers_maps => controllers.maps;
		public class MapHelper_
		{
			public ControllerMap GetMap(ControllerType t, int id, int categoryId, int layoutId) => dummyMap; public ControllerMap GetMap(Controller c, int categoryId, int layoutId) => dummyMap; public ControllerMap GetMap(Controller c, string category, string layout) => dummyMap; public ControllerMap GetMap(int controllerId, int categoryId, int layoutId) => dummyMap; static ControllerMap dummyMap = new ControllerMap();
			public IList<ControllerMap> GetMaps(ControllerType t, int id) => new List<ControllerMap>();
			public void SetMapsEnabled(bool b, string category) { }
			public void SetMapsEnabled(bool b, int category) { }
			public void SetMapsEnabled(bool b, ControllerType t, string category) { }
			public void SetMapsEnabled(bool b, ControllerType t, int category) { }
			public void SetMapsEnabled(bool b, ControllerType t, int category, int layout) { }
			public bool GetFirstMapInCategory(ControllerType t, int id, int cat) => false;
			public IEnumerable<ControllerMap> GetAllMaps() => new List<ControllerMap>();
			public IEnumerable<ControllerMap> GetAllMapsInCategory(int cat) => new List<ControllerMap>();
			public IEnumerable<ControllerMap> GetAllMapsInCategory(string cat) => new List<ControllerMap>();
			public void LoadDefaultMaps(ControllerType t) { }
			public void ClearMaps(ControllerType t, bool b) { }
			public int GetElementMapsWithAction(int actionId, bool skipDisabledMaps, List<ActionElementMap> results) => 0;
		}
		public class ControllerHelper_
		{
			public MapHelper_ maps = new MapHelper_();
			public Keyboard Keyboard => ReInput.Kb;
			public Mouse Mouse => ReInput.Ms;
			public IList<Joystick> Joysticks => new List<Joystick>();
			public int joystickCount => 0;
			public bool hasKeyboard => true;
			public bool hasMouse => true;
			public Controller GetLastActiveController() => ReInput.Kb;
			public Controller GetLastActiveController(ControllerType t) => ReInput.Kb;
			public Controller GetController(ControllerType t, int id) => t == ControllerType.Mouse ? (Controller)ReInput.Ms : ReInput.Kb;
			public IEnumerable<Controller> Controllers => new Controller[] { ReInput.Kb, ReInput.Ms };
			public int controllerCount => 2;
			public bool ContainsController(Controller c) => true;
			public bool ContainsController(ControllerType t, int id) => t != ControllerType.Joystick;
			public IEnumerable<Joystick> GetJoysticks() => new List<Joystick>();
			public Joystick GetJoystick(int i) => null;
			public void AddController(Controller c, bool removeFromOthers) { }
			public void RemoveController(Controller c) { }
			public void ClearAllControllers() { }
			public void AddLastActiveControllerChangedDelegate(PlayerActiveControllerChangedDelegate d) { }
			public void RemoveLastActiveControllerChangedDelegate(PlayerActiveControllerChangedDelegate d) { }
			public bool polling = true;
		}
		public bool GetButton(int a) { if (KeyMap.TestHeld.Contains(a)) return true; foreach (var k in KeyMap.Positive(a)) if (KeyMap.Key(k)) return true; return false; }
		public bool GetButtonDown(int a) { if (KeyMap.ConsumePressed(a)) return true; foreach (var k in KeyMap.Positive(a)) if (KeyMap.KeyDown(k)) return true; return false; }
		public bool GetButtonUp(int a) { foreach (var k in KeyMap.Positive(a)) if (KeyMap.KeyUp(k)) return true; return false; }
		public bool GetNegativeButton(int a) { foreach (var k in KeyMap.Negative(a)) if (KeyMap.Key(k)) return true; return false; }
		public bool GetNegativeButtonDown(int a) { foreach (var k in KeyMap.Negative(a)) if (KeyMap.KeyDown(k)) return true; return false; }
		public bool GetNegativeButtonUp(int a) { foreach (var k in KeyMap.Negative(a)) if (KeyMap.KeyUp(k)) return true; return false; }
		public bool GetButtonRepeating(int a) => GetButton(a);
		public bool GetButtonDoublePressDown(int a) => false;
		public float GetButtonTimePressed(int a) => 0f;
		public float GetAxis(int a) => (GetButton(a) ? 1f : 0f) - (GetNegativeButton(a) ? 1f : 0f);
		public float GetAxisRaw(int a) => GetAxis(a);
		public Vector2 GetAxis2D(int x, int y) => new Vector2(GetAxis(x), GetAxis(y));
		public Vector2 GetAxis2DRaw(int x, int y) => new Vector2(GetAxis(x), GetAxis(y));
		public float GetAxisPrev(int a) => 0f;
		public bool GetAnyButton() => Input.anyKey;
		public bool GetAnyButtonDown() => Input.anyKeyDown;
		public bool GetAnyButtonUp() => false;
		public bool GetAnyNegativeButton() => false;
		public bool GetAnyNegativeButtonDown() => false;
		public bool GetAnyNegativeButtonUp() => false;
		public bool GetAnyButton(int category) => Input.anyKey;
		public void AddInputEventDelegate(Action<InputActionEventData> d, UpdateLoopType loop) { }
		public void AddInputEventDelegate(Action<InputActionEventData> d, UpdateLoopType loop, int actionId) { }
		public void AddInputEventDelegate(Action<InputActionEventData> d, UpdateLoopType loop, InputActionEventType t) { }
		public void AddInputEventDelegate(Action<InputActionEventData> d, UpdateLoopType loop, InputActionEventType t, int actionId) { }
		public void RemoveInputEventDelegate(Action<InputActionEventData> d) { }
		public void SetVibration(int motorIndex, float level) { }
		public void SetVibration(int motorIndex, float level, float duration) { }
		public void StopVibration() { }
	}
	public delegate void PlayerActiveControllerChangedDelegate(Player player, Controller controller);
	public enum InputActionEventType { Update, ButtonPressed, ButtonJustPressed, ButtonJustReleased, ButtonPressedForTime, ButtonJustPressedForTime }
	public static class ReInput
	{
		internal static Keyboard Kb = new Keyboard();
		internal static Mouse Ms = new Mouse { name = "Mouse" };
		static Player p0 = new Player();
		public class PlayerHelper { public Player GetPlayer(int i) => p0; public Player GetPlayer(string n) => p0; public Player GetSystemPlayer() => p0; public IList<Player> Players => new List<Player> { p0 }; public IList<Player> AllPlayers => new List<Player> { p0 }; public int playerCount => 1; }
		public class ControllerHelper
		{
			public Player.ControllerHelper_ dummy;
			public IList<Joystick> Joysticks => new List<Joystick>();
			public IList<Joystick> joysticks => new List<Joystick>();
			public int joystickCount => 0;
			public Keyboard Keyboard => Kb;
			public Mouse Mouse => Ms;
			public IList<Controller> Controllers => new List<Controller> { Kb, Ms };
			public Controller GetController(ControllerType t, int id) => t == ControllerType.Mouse ? (Controller)Ms : Kb;
			public Controller GetLastActiveController() => Kb;
			public Joystick GetJoystick(int id) => null;
			public Joystick GetJoystick(Guid g) => null;
			public IList<Joystick> GetJoysticks() => new List<Joystick>();
			public bool polling = true;
			public bool IsJoystickAssigned(Joystick j) => false;
			public event Action<ControllerStatusChangedEventArgs> ControllerConnectedEvent_;
			public bool hasKeyboard => true; public bool hasMouse => true;
		}
		public class MappingHelper
		{
			public IList<InputBehavior> GetInputBehaviors() => new List<InputBehavior>();
			public InputBehavior GetInputBehavior(int id) => null; public InputBehavior GetInputBehavior(int playerId, int id) => null;
			public InputBehavior GetInputBehavior(string n) => null;
			public IList<InputAction> Actions => new List<InputAction>();
			public InputAction GetAction(int id) => null;
			public InputAction GetAction(string n) => null;
		}
		public class TimeHelper { public float unscaledDeltaTime => Time.unscaledDeltaTime; public float deltaTime => Time.deltaTime; }
		public static PlayerHelper players = new PlayerHelper();
		public static ControllerHelper controllers = new ControllerHelper();
		public static MappingHelper mapping = new MappingHelper();
		public static TimeHelper time = new TimeHelper();
		public static bool isReady => true;
		public static event Action InitializedEvent;
		public static event Action<ControllerStatusChangedEventArgs> ControllerConnectedEvent;
		public static event Action<ControllerStatusChangedEventArgs> ControllerDisconnectedEvent;
		public static event Action<ControllerStatusChangedEventArgs> ControllerPreDisconnectEvent;
		public static event Action<int> ControllerConnectedEvent2;
		public static string version_unityPro => "shim";
	}
	public class InputAction { public int id; public string name = ""; public string descriptiveName = ""; public InputActionType type; }
}

namespace Rewired.ControllerExtensions
{
	public interface IDualShock4Extension { void SetLightColor(Color c); void SetVibration(int motor, float level); Vector2 GetTouchPosition(int i); bool IsTouching(int i); int touchCount { get; } }
}
namespace Rewired.Utils { public static class Platform { } }
namespace Rewired.Utils.Libraries.TinyJson
{
	public static class JsonParser { public static T FromJson<T>(this string json) => default; public static object FromJson(this string json, Type t) => null; }
	public static class JsonWriter { public static string ToJson(this object o) => "{}"; }
}
namespace Rewired.Data
{
	public abstract class UserDataStore : MonoBehaviour
	{
		public class PlayerSaveData { public int id; }
		public bool isEnabled => enabled;
		public virtual void Save() { } public virtual void SaveControllerData(int playerId, ControllerType t, int id) { } public virtual void SaveControllerData(ControllerType t, int id) { }
		public virtual void SavePlayerData(int playerId) { } public virtual void SaveInputBehavior(int playerId, int behaviorId) { }
		public virtual void Load() { } public virtual void LoadControllerData(int playerId, ControllerType t, int id) { } public virtual void LoadControllerData(ControllerType t, int id) { }
		public virtual void LoadPlayerData(int playerId) { } public virtual void LoadInputBehavior(int playerId, int behaviorId) { }
		protected virtual void OnInitialize() { } protected virtual void OnControllerConnected(ControllerStatusChangedEventArgs a) { } protected virtual void OnControllerPreDisconnect(ControllerStatusChangedEventArgs a) { } protected virtual void OnControllerDisconnected(ControllerStatusChangedEventArgs a) { }
		public virtual void SaveControllerMap(int playerId, ControllerMap m) { }
		public virtual ControllerMap LoadControllerMap(int playerId, ControllerIdentifier id, int categoryId, int layoutId) => null;
		public virtual void LoadControllerMap(int playerId, ControllerMap m) { }
		public virtual string LoadControllerMapXml(int playerId, ControllerIdentifier id, int categoryId, int layoutId) => null;
		public virtual void LoadControllerMaps() { }
		protected virtual void Awake() { }
		public virtual bool LoadedAlready => true;
		public virtual void SaveControllerMaps(int playerId) { }
		public virtual int LoadOrder { get; }
	}
}
