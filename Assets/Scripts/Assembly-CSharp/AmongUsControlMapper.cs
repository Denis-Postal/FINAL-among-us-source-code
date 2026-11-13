using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Settings > Keyboard > Remap Controls ("Keyboard Bindings" page).
// The original page was built at runtime by the Rewired ControlMapper plugin, which does not exist in the offline remake
// (Rewired is replaced by unity_shims/Rewired.cs). This component fills the page's prefab layout (scroll area + Save / Restore Defaults)
// with one row per bindable action (two key slots each). TODO(verify): row layout/labels of the original page.
public class AmongUsControlMapper : MonoBehaviour
{
	private sealed class Row
	{
		public int action;
		public Button[] slotButtons = new Button[2];
		public TextMeshProUGUI[] slotTexts = new TextMeshProUGUI[2];
	}

	public UiElement backButton;

	private const int SlotCount = 2;

	private bool built;

	private readonly List<Row> rows = new List<Row>();

	private readonly Dictionary<int, KeyCode[]> working = new Dictionary<int, KeyCode[]>();

	private Row listeningRow;

	private int listeningSlot;

	private int listenStartFrame;

	private static readonly KeyCode[] bindableKeys = BuildBindableKeys();

	// capture/test hook: a key press injected by the editor capture tools
	public static KeyCode TestKey;

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, backButton, backButton, new List<UiElement> { backButton });
		EnsureEventSystem();
		if (!built)
		{
			Build();
		}
		LoadWorkingCopy();
		StopListening();
		RefreshTexts();
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		StopListening();
	}

	private static KeyCode[] BuildBindableKeys()
	{
		List<KeyCode> list = new List<KeyCode>();
		foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
		{
			if (value != KeyCode.None && value != KeyCode.Escape && value < KeyCode.Mouse0 && !list.Contains(value))
			{
				list.Add(value);
			}
		}
		return list.ToArray();
	}

	private static void EnsureEventSystem()
	{
		foreach (EventSystem existing in FindObjectsByType<EventSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
		{
			if (existing.isActiveAndEnabled && existing.GetComponent<BaseInputModule>() != null)
			{
				return;
			}
		}
		GameObject go = new GameObject("KeyBindingsEventSystem");
		go.AddComponent<EventSystem>();
		go.AddComponent<StandaloneInputModule>();
	}

	private void LoadWorkingCopy()
	{
		working.Clear();
		foreach (KeyValuePair<int, string> bindable in Rewired.KeyMap.Bindable)
		{
			working[bindable.Key] = (KeyCode[])Rewired.KeyMap.Positive(bindable.Key).Clone();
		}
	}

	private Transform Find(string path)
	{
		return transform.Find(path);
	}

	private void Build()
	{
		built = true;
		Transform panelTf = Find("Canvas/MainPageGroup/MainContent/MainContentInner/InputGridGroup");
		Transform doneTf = Find("Canvas/MainPageGroup/MainContent/MainContentInner/Buttons/DoneButton");
		Transform restoreTf = Find("Canvas/MainPageGroup/MainContent/MainContentInner/Buttons/RestoreDefaultsButton");
		TextMeshProUGUI heading = null;
		Transform headingTf = Find("Canvas/MainPageGroup/MainContent/MainContentInner/Heading");
		if (headingTf != null)
		{
			heading = headingTf.GetComponent<TextMeshProUGUI>();
		}
		if (panelTf == null || doneTf == null || restoreTf == null)
		{
			Debug.LogWarning("[KeyBindings] page layout not found; bindings UI not built");
			return;
		}
		Image buttonLook = doneTf.GetComponent<Image>();

		// The prefab's grid (HorizontalLayoutGroup + Rewired sized container) is empty and fights any layout; keep only its framed panel image
		// and put a self-contained scroll view inside it.
		HorizontalLayoutGroup panelLayout = panelTf.GetComponent<HorizontalLayoutGroup>();
		if (panelLayout != null)
		{
			panelLayout.enabled = false;
		}
		foreach (Transform child in panelTf)
		{
			child.gameObject.SetActive(false);
		}
		LayoutElement panelSize = panelTf.GetComponent<LayoutElement>();
		if (panelSize == null)
		{
			panelSize = panelTf.gameObject.AddComponent<LayoutElement>();
		}
		panelSize.minHeight = 749f;
		panelSize.preferredHeight = 749f;
		panelSize.flexibleHeight = 0f;
		GameObject viewGo = new GameObject("KeyScroll", typeof(RectTransform));
		RectTransform view = viewGo.GetComponent<RectTransform>();
		view.SetParent(panelTf, false);
		view.anchorMin = Vector2.zero;
		view.anchorMax = Vector2.one;
		view.offsetMin = new Vector2(24f, 16f);
		view.offsetMax = new Vector2(-24f, -16f);
		viewGo.AddComponent<RectMask2D>();
		GameObject contentGo = new GameObject("KeyRows", typeof(RectTransform));
		RectTransform content = contentGo.GetComponent<RectTransform>();
		content.SetParent(view, false);
		content.anchorMin = new Vector2(0f, 1f);
		content.anchorMax = new Vector2(1f, 1f);
		content.pivot = new Vector2(0.5f, 1f);
		content.anchoredPosition = Vector2.zero;
		content.sizeDelta = Vector2.zero;
		VerticalLayoutGroup layout = contentGo.AddComponent<VerticalLayoutGroup>();
		layout.spacing = 8f;
		layout.padding = new RectOffset(16, 16, 8, 8);
		layout.childControlWidth = true;
		layout.childControlHeight = true;
		layout.childForceExpandWidth = true;
		layout.childForceExpandHeight = false;
		ContentSizeFitter fitter = contentGo.AddComponent<ContentSizeFitter>();
		fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		ScrollRect scroll = viewGo.AddComponent<ScrollRect>();
		scroll.viewport = view;
		scroll.content = content;
		scroll.horizontal = false;
		scroll.vertical = true;
		scroll.movementType = ScrollRect.MovementType.Clamped;
		scroll.scrollSensitivity = 40f;
		// ScrollRect needs a raycastable graphic to receive drag/wheel input
		Image viewHit = viewGo.AddComponent<Image>();
		viewHit.color = new Color(0f, 0f, 0f, 0f);

		foreach (KeyValuePair<int, string> bindable in Rewired.KeyMap.Bindable)
		{
			Row row = new Row { action = bindable.Key };
			GameObject rowGo = new GameObject(bindable.Value + " Row", typeof(RectTransform));
			rowGo.transform.SetParent(content, false);
			HorizontalLayoutGroup hl = rowGo.AddComponent<HorizontalLayoutGroup>();
			hl.spacing = 16f;
			hl.childControlWidth = true;
			hl.childControlHeight = true;
			hl.childForceExpandWidth = false;
			hl.childForceExpandHeight = true;
			rowGo.AddComponent<LayoutElement>().preferredHeight = 56f;

			TextMeshProUGUI label = MakeText(rowGo.transform, "Label", bindable.Value, heading, 32f, TextAlignmentOptions.MidlineLeft);
			label.color = new Color(0.1f, 0.16f, 0.2f, 1f);
			label.gameObject.AddComponent<LayoutElement>().flexibleWidth = 1f;
			for (int slot = 0; slot < SlotCount; slot++)
			{
				int capturedSlot = slot;
				GameObject keyGo = new GameObject("Key" + (slot + 1), typeof(RectTransform));
				keyGo.transform.SetParent(rowGo.transform, false);
				LayoutElement le = keyGo.AddComponent<LayoutElement>();
				le.preferredWidth = 220f;
				Image img = keyGo.AddComponent<Image>();
				if (buttonLook != null)
				{
					img.sprite = buttonLook.sprite;
					img.type = buttonLook.type;
					img.color = buttonLook.color;
				}
				Button btn = keyGo.AddComponent<Button>();
				btn.targetGraphic = img;
				Row capturedRow = row;
				btn.onClick.AddListener(delegate { StartListening(capturedRow, capturedSlot); });
				TextMeshProUGUI keyText = MakeText(keyGo.transform, "Text", "", heading, 28f, TextAlignmentOptions.Center);
				keyText.color = Color.white;
				RectTransform ktr = keyText.rectTransform;
				ktr.anchorMin = Vector2.zero;
				ktr.anchorMax = Vector2.one;
				ktr.offsetMin = Vector2.zero;
				ktr.offsetMax = Vector2.zero;
				row.slotButtons[slot] = btn;
				row.slotTexts[slot] = keyText;
			}
			rows.Add(row);
		}

		AddButtonBehaviour(doneTf, Save);
		AddButtonBehaviour(restoreTf, RestoreDefaults);
	}

	private static TextMeshProUGUI MakeText(Transform parent, string objName, string text, TextMeshProUGUI styleFrom, float size, TextAlignmentOptions align)
	{
		GameObject go = new GameObject(objName, typeof(RectTransform));
		go.transform.SetParent(parent, false);
		TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
		if (styleFrom != null)
		{
			tmp.font = styleFrom.font;
			tmp.fontSharedMaterial = styleFrom.fontSharedMaterial;
			tmp.color = styleFrom.color;
		}
		tmp.fontSize = size;
		tmp.alignment = align;
		tmp.text = text;
		tmp.raycastTarget = false;
		return tmp;
	}

	private static void AddButtonBehaviour(Transform tf, UnityEngine.Events.UnityAction action)
	{
		Image img = tf.GetComponent<Image>();
		Button btn = tf.GetComponent<Button>();
		if (btn == null)
		{
			btn = tf.gameObject.AddComponent<Button>();
		}
		btn.targetGraphic = img;
		btn.onClick.RemoveAllListeners();
		btn.onClick.AddListener(action);
	}

	private void RefreshTexts()
	{
		foreach (Row row in rows)
		{
			KeyCode[] keys;
			working.TryGetValue(row.action, out keys);
			for (int slot = 0; slot < SlotCount; slot++)
			{
				bool isListening = listeningRow == row && listeningSlot == slot;
				row.slotTexts[slot].text = isListening ? "Press a key..." : ((keys != null && slot < keys.Length) ? KeyName(keys[slot]) : "-");
			}
		}
	}

	private static string KeyName(KeyCode key)
	{
		switch (key)
		{
		case KeyCode.UpArrow:
			return "Up Arrow";
		case KeyCode.DownArrow:
			return "Down Arrow";
		case KeyCode.LeftArrow:
			return "Left Arrow";
		case KeyCode.RightArrow:
			return "Right Arrow";
		case KeyCode.Return:
			return "Enter";
		case KeyCode.LeftShift:
			return "Left Shift";
		case KeyCode.RightShift:
			return "Right Shift";
		case KeyCode.LeftControl:
			return "Left Ctrl";
		case KeyCode.RightControl:
			return "Right Ctrl";
		case KeyCode.BackQuote:
			return "`";
		default:
			if (key >= KeyCode.Alpha0 && key <= KeyCode.Alpha9)
			{
				return ((char)(48 + (key - KeyCode.Alpha0))).ToString();
			}
			return key.ToString();
		}
	}

	private void StartListening(Row row, int slot)
	{
		listeningRow = row;
		listeningSlot = slot;
		listenStartFrame = Time.frameCount;
		RefreshTexts();
	}

	private void StopListening()
	{
		listeningRow = null;
		RefreshTexts();
	}

	// Escape cancels, Backspace/Delete clears the slot, any other key is assigned (and removed from other actions: one key, one action).
	private void Update()
	{
		if (listeningRow == null || Time.frameCount == listenStartFrame)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			StopListening();
			return;
		}
		KeyCode pressed = KeyCode.None;
		if (TestKey != KeyCode.None)
		{
			pressed = TestKey;
			TestKey = KeyCode.None;
		}
		else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
		{
			SetSlot(listeningRow.action, listeningSlot, KeyCode.None);
			StopListening();
			return;
		}
		if (pressed == KeyCode.None)
		{
			foreach (KeyCode key in bindableKeys)
			{
				if (Input.GetKeyDown(key))
				{
					pressed = key;
					break;
				}
			}
		}
		if (pressed == KeyCode.None)
		{
			return;
		}
		foreach (KeyValuePair<int, string> bindable in Rewired.KeyMap.Bindable)
		{
			KeyCode[] keys = working[bindable.Key];
			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i] == pressed && !(bindable.Key == listeningRow.action && i == listeningSlot))
				{
					SetSlot(bindable.Key, i, KeyCode.None);
				}
			}
		}
		SetSlot(listeningRow.action, listeningSlot, pressed);
		StopListening();
	}

	private void SetSlot(int action, int slot, KeyCode key)
	{
		List<KeyCode> keys = new List<KeyCode>(working[action]);
		while (keys.Count <= slot)
		{
			keys.Add(KeyCode.None);
		}
		keys[slot] = key;
		keys.RemoveAll((KeyCode k) => k == KeyCode.None);
		if (key == KeyCode.None)
		{
			// keep slot order stable when clearing the first slot: remaining key moves up
		}
		else
		{
			keys.Remove(key);
			keys.Insert(Mathf.Min(slot, keys.Count), key);
		}
		working[action] = keys.ToArray();
	}

	private void Save()
	{
		foreach (KeyValuePair<int, string> bindable in Rewired.KeyMap.Bindable)
		{
			Rewired.KeyMap.User[bindable.Key] = working[bindable.Key];
		}
		Rewired.KeyMap.SaveUser();
		Close();
	}

	private void RestoreDefaults()
	{
		foreach (KeyValuePair<int, string> bindable in Rewired.KeyMap.Bindable)
		{
			working[bindable.Key] = (KeyCode[])Rewired.KeyMap.DefaultKeys(bindable.Key).Clone();
		}
		StopListening();
	}

	private void Close()
	{
		Transform closeTf = Find("Canvas/Close");
		PassiveButton close = closeTf != null ? closeTf.GetComponent<PassiveButton>() : null;
		if (close != null)
		{
			close.OnClick.Invoke();
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
