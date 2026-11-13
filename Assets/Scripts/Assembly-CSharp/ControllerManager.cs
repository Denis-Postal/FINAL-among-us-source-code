using Object = UnityEngine.Object;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10)]
public class ControllerManager : MonoBehaviour
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Predicate<UiElement> _003C_003E9__41_0;

		public static Predicate<UiElement> _003C_003E9__42_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CClearDestroyedSelectableUiElements_003Eb__41_0(UiElement item)
		{
			return item == null;
		}

		internal bool _003CClearDestroyedSelectableUiElements_003Eb__42_0(UiElement item)
		{
			return item == null;
		}
	}

	public static ControllerManager Instance;

	[SerializeField]
	[HideInInspector]
	private int playerId;

	[SerializeField]
	private Player player;

	private const string DISCONNECT_POPUP_MENUNAME = "DisconnectPopup_Real";

	private const float DELTA_SINCE_LAST_BUTTON_HIGHLIGHT_THRESHOLD = 0.25f;

	private const float DELTA_SINCE_LAST_SLIDER_MOVEMENT_THRESHOLD = 0.1f;

	private bool inputDetectedLastFrame;

	[SerializeField]
	private GameObject PrefabMouseCursorForConsole;

	public GameObject ConsoleMouse;

	[HideInInspector]
	public bool IsButtonHeld;

	public ControllerUiElementsState CurrentUiState;

	public List<ControllerUiElementsState> CurrentUiStateStack;

	private float deltaSinceLastUiHighlight;

	private bool disabledVirtualCursor;

	private int frameDelaySinceInputChanged;

	public Dictionary<Vector2, AxisDirection> directions= new Dictionary<Vector2, AxisDirection>
	{
		{
			Vector2.up,
			AxisDirection.Up
		},
		{
			Vector2.down,
			AxisDirection.Down
		},
		{
			Vector2.left,
			AxisDirection.Left
		},
		{
			Vector2.right,
			AxisDirection.Right
		}
	};

	private readonly Logger logger= new Logger(Logger.Category.UI, "ControllerManager");

	private string currentScene;

	public bool IsUiControllerActive
	{
		get
		{
			if (CurrentUiState.BackButton != null || CurrentUiState.CurrentSelection != null || !string.IsNullOrEmpty(CurrentUiState.MenuName))
			{
				return true;
			}
			return false;
		}
	}

	private void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (!Instance)
		{
			Instance = this;
		}
		SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
		SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputTypeChanged));
	}

	private void SceneManager_activeSceneChanged(Scene from, Scene to)
	{
		currentScene = to.name;
	}

	private void SceneManager_sceneUnloaded(Scene scene)
	{
		if (currentScene == scene.name)
		{
			CurrentUiStateStack.Clear();
			ResetAll();
		}
	}

	private void Start()
	{
		if (VirtualCursor.instance != null)
		{
			VirtualCursor.instance.gameObject.SetActive(value: false);
		}
		player = ReInput.players.GetPlayer(playerId);
		ConsoleMouse = UnityEngine.Object.Instantiate(PrefabMouseCursorForConsole);
		if (ConsoleMouse != null)
		{
			ConsoleMouse.gameObject.SetActive(value: true);
			UnityEngine.Object.DontDestroyOnLoad(ConsoleMouse);
		}
	}

	public unsafe bool IsMenuActiveAtAll(string menuName)
	{
		if (!IsUiControllerActive)
		{
			return false;
		}
		if (CurrentUiState.MenuName == menuName)
		{
			return true;
		}
		foreach (ControllerUiElementsState item in CurrentUiStateStack)
		{
			if (item.MenuName == menuName)
			{
				return true;
			}
		}
		return false;
	}

	private unsafe void OnInputTypeChanged()
	{
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			if ((bool)CurrentUiState.CurrentSelection && CurrentUiState.CurrentSelection.isActiveAndEnabled)
			{
				CurrentUiState.CurrentSelection.ReceiveMouseOver();
			}
		}
		else if ((bool)CurrentUiState.CurrentSelection && CurrentUiState.CurrentSelection.isActiveAndEnabled)
		{
			foreach (ControllerUiElementsState item in CurrentUiStateStack)
			{
				if ((bool)item.CurrentSelection)
				{
					item.CurrentSelection.ReceiveMouseOut();
				}
			}
			CurrentUiState.CurrentSelection.ReceiveMouseOut();
		}
		frameDelaySinceInputChanged = 3;
	}

	public void NewScene(string menuName, UiElement backButton, UiElement defaultSelection, List<UiElement> selectableElements, bool gridNav = false)
	{
		GameObject gameObject = GameObject.Find("DisconnectPopup_Real");
		DisconnectPopup disconnectPopup = null;
		if (gameObject != null)
		{
			DisconnectPopup component = gameObject.GetComponent<DisconnectPopup>();
			if (component != null && component.isActiveAndEnabled)
			{
				disconnectPopup = component;
				logger.Warning("DisconnectPopup_Real menu found, restore after new scene.");
			}
		}
		ControllerUiElementsState controllerUiElementsState = new ControllerUiElementsState();
		controllerUiElementsState.EnforceGridNavigation = gridNav;
		controllerUiElementsState.BackButton = backButton;
		controllerUiElementsState.MenuName = menuName;
		controllerUiElementsState.IsScene = true;
		CurrentUiStateStack.Add(controllerUiElementsState);
		SetUpSelectables(controllerUiElementsState, defaultSelection, selectableElements);
		OpenTopmostMenu();
		if (CurrentUiState == controllerUiElementsState)
		{
			SetDefaultSelection(defaultSelection, selectableElements);
		}
		if ((bool)VirtualCursor.instance)
		{
			VirtualCursor.instance.gameObject.SetActive(value: false);
		}
		if (disconnectPopup != null && disconnectPopup.isActiveAndEnabled)
		{
			disconnectPopup.Show();
		}
	}

	public void OpenOverlayMenu(string menuName, UiElement backButton)
	{
		OpenOverlayMenu(menuName, backButton, null, new List<UiElement>());
	}

	public void OpenOverlayMenu(string menuName, UiElement backButton, UiElement defaultSelection)
	{
		OpenOverlayMenu(menuName, backButton, defaultSelection, new List<UiElement>());
	}

	public unsafe void OpenOverlayMenu(string menuName, UiElement backButton, UiElement defaultSelection, List<UiElement> selectableElements, bool gridNav = false)
	{
		// TODO(verify): reimplemented (decomp stub)
		// TODO(verify): simplified menu-stack bookkeeping (controller directional nav not reimplemented)
		if (CurrentUiStateStack == null) CurrentUiStateStack = new List<ControllerUiElementsState>();
		if (CurrentUiState == null)
		{
			CurrentUiState = new ControllerUiElementsState();
			CurrentUiState.IsScene = true;
		}
		CurrentUiStateStack.RemoveAll((ControllerUiElementsState s) => s.MenuName == menuName);
		if (!string.IsNullOrEmpty(CurrentUiState.MenuName) && CurrentUiState.MenuName != menuName)
		{
			CurrentUiStateStack.Add(CurrentUiState);
		}
		CurrentUiState = new ControllerUiElementsState();
		CurrentUiState.MenuName = menuName;
		CurrentUiState.BackButton = backButton;
		CurrentUiState.EnforceGridNavigation = gridNav;
		SetUpSelectables(CurrentUiState, defaultSelection, selectableElements);
	}

	public ControllerUiElementsState GetMenu(string menuName)
	{
		if (CurrentUiState != null && string.Equals(CurrentUiState.MenuName, menuName, StringComparison.OrdinalIgnoreCase))
		{
			return CurrentUiState;
		}
		for (int i = 0; i < CurrentUiStateStack.Count; i++)
		{
			if (string.Equals(CurrentUiStateStack[i].MenuName, menuName, StringComparison.OrdinalIgnoreCase))
			{
				return CurrentUiStateStack[i];
			}
		}
		logger.Error("OpenSpecificMenu - couldn't find specific menu:" + menuName);
		return null;
	}

	public void OpenSpecificMenu(string menuNameToOpen)
	{
		for (int i = 0; i < CurrentUiStateStack.Count; i++)
		{
			if (CurrentUiStateStack[i].MenuName == menuNameToOpen)
			{
				CurrentUiState = CurrentUiStateStack[i];
				HighlightSelection(CurrentUiState.CurrentSelection);
				return;
			}
		}
		logger.Warning("OpenSpecificMenu - couldn't find specific menu:" + menuNameToOpen);
	}

	private unsafe void OpenTopmostMenu()
	{
		ControllerUiElementsState currentUiState = CurrentUiState;
		CurrentUiState = null;
		float num = 0f;
		if (CurrentUiStateStack.Count > 0)
		{
			CurrentUiState = CurrentUiStateStack[0];
			UiElement uiElement = CurrentUiState.CurrentSelection;
			if (!uiElement && CurrentUiState.SelectableUiElements.Count > 0)
			{
				foreach (UiElement selectableUiElement in CurrentUiState.SelectableUiElements)
				{
					uiElement = selectableUiElement;
					if ((bool)uiElement && uiElement.isActiveAndEnabled)
					{
						break;
					}
				}
			}
			if (!uiElement)
			{
				uiElement = CurrentUiState.BackButton;
			}
			if (!uiElement)
			{
				logger.Warning($"Error: No button/back button for menu {CurrentUiState.MenuName}, {uiElement}");
				return;
			}
			num = uiElement.transform.position.z;
			CurrentUiState.zPos = num;
		}
		for (int i = 1; i < CurrentUiStateStack.Count; i++)
		{
			ControllerUiElementsState controllerUiElementsState = CurrentUiStateStack[i];
			UiElement uiElement2 = controllerUiElementsState.CurrentSelection;
			if (!uiElement2 && controllerUiElementsState.SelectableUiElements.Count > 0)
			{
				foreach (UiElement selectableUiElement2 in controllerUiElementsState.SelectableUiElements)
				{
					uiElement2 = selectableUiElement2;
					if ((bool)uiElement2 && uiElement2.isActiveAndEnabled)
					{
						break;
					}
				}
			}
			if (!uiElement2)
			{
				uiElement2 = controllerUiElementsState.BackButton;
			}
			if (!uiElement2)
			{
				logger.Error("Error: No button/back button for menu " + controllerUiElementsState.MenuName);
				continue;
			}
			float num2 = (controllerUiElementsState.zPos = uiElement2.transform.position.z);
			if (num2 <= num)
			{
				num = num2;
				CurrentUiState = controllerUiElementsState;
			}
		}
		if (CurrentUiState != null)
		{
			if (CurrentUiState != currentUiState)
			{
				HighlightSelection(CurrentUiState.CurrentSelection);
			}
			logger.Debug("Set current state to topmost menu " + CurrentUiState.MenuName);
		}
		else
		{
			CurrentUiState = currentUiState;
			ResetAll();
			logger.Info("There were no menu states to open");
		}
	}

	public void SetUpSelectables(ControllerUiElementsState state, UiElement defaultSelection, List<UiElement> selectables)
	{
		if (selectables != null)
		{
			if (selectables.Count == 0)
			{
				state.SelectableUiElements.Clear();
			}
			else
			{
				state.SelectableUiElements.AddRange(selectables);
			}
		}
		if (!(defaultSelection == null))
		{
			if (!state.SelectableUiElements.Contains(defaultSelection))
			{
				state.SelectableUiElements.Add(defaultSelection);
			}
			state.CurrentSelection = defaultSelection;
		}
	}

	public void SetDefaultSelection(UiElement defaultSelection, List<UiElement> selectableElements = null)
	{
		if (defaultSelection != null)
		{
			HighlightSelection(defaultSelection);
		}
	}

	public void SetBackButton(UiElement backButton)
	{
		if (backButton != null)
		{
			CurrentUiState.BackButton = backButton;
		}
	}

	public void UnsetBackButton(UiElement backButton)
	{
		if (CurrentUiState.BackButton == backButton)
		{
			CurrentUiState.BackButton = null;
		}
	}

	public void AddSelectableUiElement(UiElement uiElement, bool forceSelect = false)
	{
		if (!(uiElement == null))
		{
			if (!CurrentUiState.SelectableUiElements.Contains(uiElement))
			{
				CurrentUiState.SelectableUiElements.Add(uiElement);
			}
			if (forceSelect)
			{
				HighlightSelection(uiElement);
			}
		}
	}

	public void RemoveSelectableUiElement(UiElement uiElement)
	{
		if (!(uiElement == null))
		{
			CurrentUiState.SelectableUiElements.Remove(uiElement);
			if (CurrentUiState.CurrentSelection == uiElement)
			{
				CurrentUiState.CurrentSelection = null;
			}
		}
	}

	public void AddSelectableUiElement(UiElement uiElement, ControllerUiElementsState menu)
	{
		if (!(uiElement == null) && !menu.SelectableUiElements.Contains(uiElement))
		{
			menu.SelectableUiElements.Add(uiElement);
		}
	}

	public void RemoveSelectableUiElement(UiElement uiElement, ControllerUiElementsState menu)
	{
		if (!(uiElement == null))
		{
			menu.SelectableUiElements.Remove(uiElement);
			if (menu.CurrentSelection == uiElement)
			{
				menu.CurrentSelection = null;
			}
		}
	}

	public void ClearDestroyedSelectableUiElements()
	{
		if (CurrentUiState != null && CurrentUiState.SelectableUiElements != null && CurrentUiState.SelectableUiElements.Count != 0)
		{
			CurrentUiState.SelectableUiElements.RemoveAll((UiElement item) => item == null);
		}
	}

	public void ClearDestroyedSelectableUiElements(ControllerUiElementsState state)
	{
		if (state != null && state.SelectableUiElements != null && state.SelectableUiElements.Count != 0)
		{
			state.SelectableUiElements.RemoveAll((UiElement item) => item == null);
		}
	}

	public unsafe void CloseOverlayMenu(string menuName)
	{
		logger.Info("Menu: " + menuName + " => CloseOverlayMenu()");
		bool flag = false;
		if (CurrentUiState.CurrentSelection != null && CurrentUiState.MenuName == menuName)
		{
			CurrentUiState.CurrentSelection.ReceiveMouseOut();
		}
		if (CurrentUiStateStack.Count > 0)
		{
			if (string.Equals(CurrentUiState.MenuName, menuName, StringComparison.OrdinalIgnoreCase))
			{
				flag = true;
				CurrentUiStateStack.Remove(CurrentUiState);
				OpenTopmostMenu();
				HighlightSelection(CurrentUiState.CurrentSelection);
			}
			else
			{
				for (int i = 0; i < CurrentUiStateStack.Count; i++)
				{
					if (string.Equals(CurrentUiStateStack[i].MenuName, menuName, StringComparison.OrdinalIgnoreCase))
					{
						CurrentUiStateStack.RemoveAt(i);
						logger.Debug("Closed currently-inactive menu " + menuName);
						break;
					}
				}
			}
		}
		else if (IsUiControllerActive && string.Equals(CurrentUiState.MenuName, menuName, StringComparison.OrdinalIgnoreCase))
		{
			flag = true;
			ResetAll();
		}
		if (flag)
		{
			return;
		}
		List<ControllerUiElementsState> list = new List<ControllerUiElementsState>();
		foreach (ControllerUiElementsState item in CurrentUiStateStack)
		{
			if (string.Equals(item.MenuName, menuName, StringComparison.OrdinalIgnoreCase))
			{
				list.Add(item);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		foreach (ControllerUiElementsState item2 in list)
		{
			logger.Debug("Found menu: " + item2.MenuName + " in lower stack, removing.");
			CurrentUiStateStack.Remove(item2);
		}
	}

	public unsafe void CloseAndResetAll()
	{
		foreach (ControllerUiElementsState item in new List<ControllerUiElementsState>(CurrentUiStateStack))
		{
			PassiveUiElement passiveUiElement = item.BackButton as PassiveUiElement;
			if (passiveUiElement != null)
			{
				passiveUiElement.ReceiveClickDown();
			}
		}
		ResetAll();
	}

	public void ResetAll()
	{
		logger.Info("ResetAll()");
		if (CurrentUiState.CurrentSelection != null)
		{
			CurrentUiState.CurrentSelection.ReceiveMouseOut();
		}
		CurrentUiState.Reset();
		CurrentUiStateStack.Clear();
		deltaSinceLastUiHighlight = 0f;
	}

	private void Update()
	{
		if (frameDelaySinceInputChanged > 0)
		{
			frameDelaySinceInputChanged--;
		}
		else
		{
			if (!IsUiControllerActive || RadialMenu.instances != 0)
			{
				return;
			}
			if (!CurrentUiState.CurrentSelection || !CurrentUiState.CurrentSelection.isActiveAndEnabled)
			{
				PickTopSelectable();
			}
			if (player.GetButtonDown(12))
			{
				PassiveUiElement passiveUiElement = CurrentUiState.BackButton as PassiveUiElement;
				if (passiveUiElement != null)
				{
					passiveUiElement.ReceiveClickDown();
				}
				else
				{
					ButtonBehavior buttonBehavior = CurrentUiState.BackButton as ButtonBehavior;
					if (buttonBehavior != null)
					{
						buttonBehavior.ReceiveClick();
					}
				}
			}
			if (CurrentUiState.CurrentSelection == null)
			{
				return;
			}
			IsButtonHeld = player.GetButton(11);
			if (player.GetButtonDown(11))
			{
				PassiveUiElement passiveUiElement2 = CurrentUiState.CurrentSelection as PassiveUiElement;
				if (passiveUiElement2 != null && passiveUiElement2.isActiveAndEnabled)
				{
					passiveUiElement2.ReceiveClickDown();
				}
				else
				{
					ButtonBehavior buttonBehavior2 = CurrentUiState.CurrentSelection as ButtonBehavior;
					if (buttonBehavior2 != null && buttonBehavior2.isActiveAndEnabled)
					{
						buttonBehavior2.ReceiveClick();
					}
				}
			}
			else if (IsButtonHeld)
			{
				PassiveUiElement passiveUiElement3 = CurrentUiState.CurrentSelection as PassiveUiElement;
				if (passiveUiElement3 != null && passiveUiElement3.isActiveAndEnabled && passiveUiElement3.HandleRepeat)
				{
					passiveUiElement3.ReceiveRepeatDown();
				}
			}
			deltaSinceLastUiHighlight += Time.deltaTime;
			bool flag = false;
			bool buttonDown = player.GetButtonDown(25);
			bool buttonDown2 = player.GetButtonDown(26);
			bool buttonDown3 = player.GetButtonDown(27);
			bool buttonDown4 = player.GetButtonDown(28);
			float num = 0f;
			float num2 = 0f;
			if (buttonDown | buttonDown2)
			{
				num2 = (buttonDown ? 1 : (-1));
				flag = true;
			}
			else if (buttonDown3 | buttonDown4)
			{
				num = (buttonDown3 ? 1 : (-1));
				flag = true;
			}
			else
			{
				num = player.GetAxis(9);
				num2 = player.GetAxis(10);
			}
			if (Mathf.Abs(num) >= 0.3f || Mathf.Abs(num2) >= 0.3f)
			{
				SlideBar slideBar = CurrentUiState.CurrentSelection as SlideBar;
				if (slideBar != null)
				{
					if (slideBar.Vertical && num2 != 0f && (double)Mathf.Abs(num2) > 0.3)
					{
						VerticalAxisInputForSlideBar(num2, slideBar, flag);
						return;
					}
					if (num != 0f && (double)Mathf.Abs(num) > 0.3)
					{
						HorizontalAxisInputForSlideBar(num, slideBar, flag);
						return;
					}
				}
				if ((deltaSinceLastUiHighlight > 0.25f || !inputDetectedLastFrame) | flag)
				{
					UiElement uiElement = null;
					Vector2 origin = CurrentUiState.CurrentSelection.transform.position;
					PassiveUiElement passiveUiElement4 = CurrentUiState.CurrentSelection as PassiveUiElement;
					if (passiveUiElement4 != null && passiveUiElement4.Colliders.Length != 0)
					{
						origin = passiveUiElement4.Colliders[0].bounds.center;
					}
					Vector2 direction = new Vector3(num, num2);
					if (CurrentUiState.CurrentSelection.ControllerNav.mode == ControllerNavigation.Mode.Explicit)
					{
						AxisDirection direction2 = FindClosestDirection(direction);
						uiElement = FindUiElementByExplicitNavigation(CurrentUiState.CurrentSelection, CurrentUiState.CurrentSelection, direction2);
					}
					else
					{
						if (!CurrentUiState.EnforceGridNavigation)
						{
							float num3 = 0.15f;
							direction = direction.normalized;
							for (int i = 0; i < 4; i++)
							{
								uiElement = FindUiElementByRaycast(origin, direction, num3);
								if (uiElement != null)
								{
									break;
								}
								num3 *= 1.5f;
							}
						}
						AxisDirection direction3 = FindClosestDirection(direction);
						if (uiElement == null)
						{
							float orthogonalOffsetThreshold = (CurrentUiState.EnforceGridNavigation ? 0.25f : 100f);
							uiElement = FindUiElementByGridDirection(origin, direction3, orthogonalOffsetThreshold);
						}
					}
					if (uiElement != null)
					{
						HighlightSelection(uiElement);
					}
				}
				inputDetectedLastFrame = true;
			}
			else
			{
				deltaSinceLastUiHighlight = 0f;
				inputDetectedLastFrame = false;
			}
		}
	}

	private AxisDirection FindClosestDirection(Vector2 direction)
	{
		float num = float.MinValue;
		AxisDirection result = AxisDirection.None;
		foreach (KeyValuePair<Vector2, AxisDirection> direction2 in directions)
		{
			float num2 = Vector2.Dot(direction, direction2.Key);
			if (num2 > num)
			{
				num = num2;
				result = direction2.Value;
			}
		}
		return result;
	}

	private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2, Vector2 direction)
	{
		Vector2 to = vec2 - vec1;
		float num = ((vec2.y < vec1.y) ? (-1f) : 1f);
		return Vector2.Angle(direction, to) * num;
	}

	private UiElement FindUiElementByRaycast(Vector2 origin, Vector2 direction, float radius)
	{
		RaycastHit2D[] array = Physics2D.CircleCastAll(origin, radius, direction);
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit2D raycastHit2D = array[i];
			if (!((double)raycastHit2D.distance > 0.0) || !(raycastHit2D.collider != null) || !(raycastHit2D.transform.gameObject != CurrentUiState.CurrentSelection.gameObject))
			{
				continue;
			}
			GameObject gameObject = raycastHit2D.transform.gameObject;
			UiElement uiElement = gameObject.GetComponent<UiElement>();
			if (uiElement == null)
			{
				uiElement = gameObject.GetComponentInParent<UiElement>();
				if (uiElement == CurrentUiState.CurrentSelection)
				{
					uiElement = null;
				}
				if (uiElement != null)
				{
					PassiveUiElement passiveUiElement = uiElement as PassiveUiElement;
					if (!passiveUiElement || !passiveUiElement.Colliders.Contains(raycastHit2D.collider))
					{
						uiElement = null;
					}
				}
			}
			if (uiElement != null && CurrentUiState.SelectableUiElements.Contains(uiElement))
			{
				return uiElement;
			}
		}
		return null;
	}

	private UiElement FindUiElementByExplicitNavigation(UiElement start, UiElement from, AxisDirection direction)
	{
		UiElement uiElement = null;
		switch (direction)
		{
		case AxisDirection.Up:
			uiElement = from.ControllerNav.selectOnUp;
			break;
		case AxisDirection.Down:
			uiElement = from.ControllerNav.selectOnDown;
			break;
		case AxisDirection.Left:
			uiElement = from.ControllerNav.selectOnLeft;
			break;
		case AxisDirection.Right:
			uiElement = from.ControllerNav.selectOnRight;
			break;
		case AxisDirection.None:
			return null;
		}
		if ((bool)uiElement)
		{
			if (uiElement == start)
			{
				return null;
			}
			if (uiElement.isActiveAndEnabled)
			{
				return uiElement;
			}
			if (uiElement.ControllerNav.mode == ControllerNavigation.Mode.Explicit)
			{
				return FindUiElementByExplicitNavigation(start, uiElement, direction);
			}
		}
		return null;
	}

	private unsafe UiElement FindUiElementByGridDirection(Vector2 origin, AxisDirection direction, float orthogonalOffsetThreshold)
	{
		Vector2 rhs = Vector2.zero;
		Vector2 rhs2 = Vector2.zero;
		UiElement uiElement = null;
		switch (direction)
		{
		case AxisDirection.Down:
			rhs = new Vector2(0f, -1f);
			rhs2 = new Vector2(1f, 0f);
			break;
		case AxisDirection.Up:
			rhs = new Vector2(0f, 1f);
			rhs2 = new Vector2(1f, 0f);
			break;
		case AxisDirection.Left:
			rhs = new Vector2(-1f, 0f);
			rhs2 = new Vector2(0f, 1f);
			break;
		case AxisDirection.Right:
			rhs = new Vector2(1f, 0f);
			rhs2 = new Vector2(0f, 1f);
			break;
		case AxisDirection.None:
			return null;
		}
		float num = Vector2.Dot(origin, rhs);
		float num2 = Vector2.Dot(origin, rhs2);
		float num3 = float.MaxValue;
		float num4 = float.MaxValue;
		foreach (UiElement selectableUiElement in CurrentUiState.SelectableUiElements)
		{
			if (selectableUiElement == null || !selectableUiElement.isActiveAndEnabled || selectableUiElement == CurrentUiState.CurrentSelection)
			{
				continue;
			}
			float num5 = Vector2.Dot(selectableUiElement.gameObject.transform.position, rhs);
			if (num5 <= num)
			{
				continue;
			}
			float num6 = Vector2.Dot(selectableUiElement.gameObject.transform.position, rhs2);
			float num7 = Mathf.Abs(num2 - num6);
			if (!(num7 > orthogonalOffsetThreshold))
			{
				float num8 = num5 - num;
				if ((num8 <= num3 || Mathf.Abs(num8 - num3) <= 0.01f) && (num7 < num4 || Mathf.Abs(num7 - num4) <= 0.01f || num7 == 0f))
				{
					num3 = num8;
					num4 = num7;
					uiElement = selectableUiElement;
				}
			}
		}
		if (uiElement != null)
		{
			logger.Debug("FindUiElementByGridDirection: " + uiElement.gameObject.name);
		}
		return uiElement;
	}

	private unsafe UiElement FindUiElementByDistance(Vector2 origin, AxisDirection direction)
	{
		UiElement uiElement = null;
		float num = float.MinValue;
		switch (direction)
		{
		case AxisDirection.Up:
		{
			float y2 = origin.y;
			foreach (UiElement selectableUiElement in CurrentUiState.SelectableUiElements)
			{
				if (!selectableUiElement.isActiveAndEnabled)
				{
					continue;
				}
				Vector2 b4 = selectableUiElement.transform.position;
				if (y2 < b4.y)
				{
					float num5 = Vector2.Distance(origin, b4);
					if (uiElement == null || num5 < num)
					{
						uiElement = selectableUiElement;
						num = num5;
					}
				}
			}
			break;
		}
		case AxisDirection.Down:
		{
			float y = origin.y;
			foreach (UiElement selectableUiElement2 in CurrentUiState.SelectableUiElements)
			{
				if (!selectableUiElement2.isActiveAndEnabled)
				{
					continue;
				}
				Vector2 b3 = selectableUiElement2.transform.position;
				if (y > b3.y)
				{
					float num4 = Vector2.Distance(origin, b3);
					if (uiElement == null || num4 < num)
					{
						uiElement = selectableUiElement2;
						num = num4;
					}
				}
			}
			break;
		}
		case AxisDirection.Left:
		{
			float x2 = origin.x;
			foreach (UiElement selectableUiElement3 in CurrentUiState.SelectableUiElements)
			{
				if (!selectableUiElement3.isActiveAndEnabled)
				{
					continue;
				}
				Vector2 b2 = selectableUiElement3.transform.position;
				if (x2 > b2.x)
				{
					float num3 = Vector2.Distance(origin, b2);
					if (uiElement == null || num3 < num)
					{
						uiElement = selectableUiElement3;
						num = num3;
					}
				}
			}
			break;
		}
		case AxisDirection.Right:
		{
			float x = origin.x;
			foreach (UiElement selectableUiElement4 in CurrentUiState.SelectableUiElements)
			{
				if (!selectableUiElement4.isActiveAndEnabled)
				{
					continue;
				}
				Vector2 b = selectableUiElement4.transform.position;
				if (x < b.x)
				{
					float num2 = Vector2.Distance(origin, b);
					if (uiElement == null || num2 < num)
					{
						uiElement = selectableUiElement4;
						num = num2;
					}
				}
			}
			break;
		}
		}
		return uiElement;
	}

	private void VerticalAxisInputForSlideBar(float v, SlideBar slideBar, bool dpadDetected)
	{
		if ((deltaSinceLastUiHighlight > 0.1f) | dpadDetected)
		{
			if (v > 0f)
			{
				slideBar.ControllerIncrease();
			}
			else
			{
				slideBar.ControllerDecrease();
			}
			deltaSinceLastUiHighlight = 0f;
		}
	}

	private void HorizontalAxisInputForSlideBar(float h, SlideBar slideBar, bool dpadDetected)
	{
		if ((deltaSinceLastUiHighlight > 0.1f) | dpadDetected)
		{
			if (h > 0f)
			{
				slideBar.ControllerIncrease();
			}
			else
			{
				slideBar.ControllerDecrease();
			}
			deltaSinceLastUiHighlight = 0f;
		}
	}

	private void HighlightSelection(UiElement selection)
	{
		if (selection == null || !selection.isActiveAndEnabled)
		{
			if ((bool)selection)
			{
				logger.Warning($"Failed to highlight {selection.name}, selection.isActiveAndEnabled={selection.isActiveAndEnabled}");
			}
			else
			{
				logger.Warning("Failed to highlight, selection is null");
			}
			return;
		}
		if ((bool)CurrentUiState.CurrentSelection)
		{
			CurrentUiState.CurrentSelection.ReceiveMouseOut();
		}
		if (DestroyableSingleton<PassiveButtonManager>.InstanceExists
			&& ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			DestroyableSingleton<PassiveButtonManager>.Instance.LoseFocusForAll();
		}
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			selection.ReceiveMouseOver();
			if ((bool)selection.GetComponent<ColorChip>() || (bool)selection.GetComponent<AnnouncementPanel>() || (bool)selection.GetComponent<ViewSettingsTabButton>())
			{
				(selection as PassiveButton).OnClick.Invoke();
			}
		}
		CurrentUiState.CurrentSelection = selection;
		deltaSinceLastUiHighlight = 0f;
	}

	public void SetCurrentSelected(UiElement selection)
	{
		HighlightSelection(selection);
	}

	public unsafe void PickTopSelectable()
	{
		float num = -10000f;
		UiElement uiElement = null;
		foreach (UiElement selectableUiElement in CurrentUiState.SelectableUiElements)
		{
			if ((bool)selectableUiElement && selectableUiElement.isActiveAndEnabled && selectableUiElement.transform.position.y > num)
			{
				num = selectableUiElement.transform.position.y;
				uiElement = selectableUiElement;
			}
		}
		if ((bool)uiElement)
		{
			HighlightSelection(uiElement);
		}
	}

	public void PrintSelectableUiList()
	{
	}

	public void ClearSelectableUiList()
	{
	}

	public ControllerManager()
	{ }
}
