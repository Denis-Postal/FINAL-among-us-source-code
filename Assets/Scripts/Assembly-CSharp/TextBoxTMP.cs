using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Text;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxTMP : MonoBehaviour, IFocusHolder
{
	public static readonly HashSet<char> SymbolChars= new HashSet<char>
	{
		'?', '!', ',', '.', '\'', ':', ';', '(', ')', '/',
		'\\', '%', '^', '&', '-', '=', '¿', '？', '#'
	};

	public static readonly HashSet<char> EmailChars= new HashSet<char> { '!', '~', '@', '.', '-', '_', '+' };

	public const char HiddenChar = '*';

	public bool allowAllCharacters;

	public string text;

	private string compoText = "";

	public int characterLimit = -1;

	[SerializeField]
	public TextMeshPro outputText;

	public SpriteRenderer Background;

	public MeshRenderer Pipe;

	[SerializeField]
	private TextMeshPro placeholderText;

	private float pipeBlinkTimer;

	public bool ClearOnFocus;

	public bool ClearOnlyPlaceholderOnFocus;

	public bool ForceUppercase;

	public Button.ButtonClickedEvent OnEnter;

	public Button.ButtonClickedEvent OnChange;

	public Button.ButtonClickedEvent OnFocusLost;

	public Button.ButtonClickedEvent OnFocus;

	private TouchScreenKeyboard keyboard;

	public bool AllowSymbols;

	public bool AllowEmail;

	public bool IpMode;

	public bool AllowPaste;

	public bool Hidden;

	private Collider2D[] colliders;

	private bool hasFocus;

	// text boxes that currently have focus; input shim ignores Space while any is focused (Space must not trigger Use/Confirm while typing)
	private static readonly System.Collections.Generic.HashSet<TextBoxTMP> focusedBoxes = new System.Collections.Generic.HashSet<TextBoxTMP>();

	public static bool AnyFocused
	{
		get
		{
			focusedBoxes.RemoveWhere((TextBoxTMP b) => b == null || !b.isActiveAndEnabled || !b.hasFocus);
			return focusedBoxes.Count > 0;
		}
	}

	private int caretPos;

	private float caretRepeatTimer;

	public float caretYOffset = 0.12f;

	public Color colorBackground= Color.green;

	private StringBuilder tempTxt= new StringBuilder();

	public SpriteRenderer sendButtonGlyph;

	public bool SendOnFullChars;

	public float TextHeight => outputText.GetNotDumbRenderedHeight();

	public void Start()
	{
		colliders = GetComponents<Collider2D>();
		DestroyableSingleton<PassiveButtonManager>.Instance.RegisterOne(this);
		if ((bool)Pipe)
		{
			Pipe.enabled = false;
		}
	}

	public void OnDestroy()
	{
		if (keyboard != null)
		{
			keyboard.active = false;
			keyboard = null;
		}
		if (DestroyableSingleton<PassiveButtonManager>.InstanceExists)
		{
			DestroyableSingleton<PassiveButtonManager>.Instance.RemoveOne(this);
		}
	}

	public void ResetField()
	{
		if ((bool)placeholderText)
		{
			placeholderText.gameObject.SetActive(value: true);
		}
		outputText.text = string.Empty;
	}

	public void Clear()
	{
		SetCaretPosition(0);
		SetText(string.Empty, string.Empty);
	}

	public void Update()
	{
		if (!enabled || !hasFocus)
		{
			return;
		}
		MoveCaret();
		if (AllowPaste && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.V))
		{
			string clipboardString = ClipboardHelper.GetClipboardString();
			if (!string.IsNullOrWhiteSpace(clipboardString))
			{
				SetText(text.Insert(caretPos, clipboardString));
			}
		}
		string inputString = Input.inputString;
		if (inputString.Length > 0 || compoText != Input.compositionString)
		{
			if (text == null || text == "Enter Name")
			{
				text = "";
			}
			SetText(text.Insert(caretPos, inputString), Input.compositionString);
		}
		if ((bool)Pipe && hasFocus)
		{
			pipeBlinkTimer += Time.deltaTime * 2f;
			Pipe.enabled = (int)pipeBlinkTimer % 2 == 0;
		}
	}

	public unsafe void GiveFocus()
	{
		if (!enabled)
		{
			return;
		}
		Input.imeCompositionMode = IMECompositionMode.On;
		if (hasFocus)
		{
			if (TouchScreenKeyboard.isSupported && (keyboard == null || !TouchScreenKeyboard.visible))
			{
				if (keyboard != null)
				{
					keyboard.active = false;
				}
				keyboard = TouchScreenKeyboard.Open(text);
			}
			return;
		}
		if (ClearOnFocus)
		{
			if ((bool)placeholderText)
			{
				placeholderText.gameObject.SetActive(value: false);
			}
			text = string.Empty;
			compoText = string.Empty;
			outputText.text = string.Empty;
			SetCaretPosition(0);
		}
		if (ClearOnlyPlaceholderOnFocus && (bool)placeholderText)
		{
			placeholderText.gameObject.SetActive(value: false);
		}
		if (OnFocus != null)
		{
			OnFocus.Invoke();
		}
		hasFocus = true;
		focusedBoxes.Add(this);
		if (TouchScreenKeyboard.isSupported)
		{
			keyboard = TouchScreenKeyboard.Open(text);
		}
		if ((bool)Background)
		{
			Background.color = colorBackground;
		}
		pipeBlinkTimer = 0f;
		if ((bool)Pipe)
		{
			SetPipePosition();
			SetCaretPosition(outputText.text.Length);
		}
	}

	public void ForceKeyboardClose()
	{
		if (keyboard != null)
		{
			keyboard.active = false;
			keyboard = null;
		}
	}

	public unsafe void LoseFocus()
	{
		// TODO(verify): from decomp9; Background colour (lost in decomp) taken from the 2019 baseline (Color.white)
		if (hasFocus)
		{
			Input.imeCompositionMode = IMECompositionMode.Off;
			if (compoText.Length > 0)
			{
				SetText(text + compoText);
				compoText = string.Empty;
			}
			hasFocus = false;
			focusedBoxes.Remove(this);
			if (keyboard != null)
			{
				keyboard.active = false;
				keyboard = null;
			}
			if ((bool)Background)
			{
				Background.color = Color.white;
			}
			if ((bool)Pipe)
			{
				Pipe.enabled = false;
			}
			if (OnFocusLost != null)
			{
				OnFocusLost.Invoke();
			}
			if ((bool)sendButtonGlyph)
			{
				sendButtonGlyph.enabled = true;
			}
		}
	}

	public bool CheckCollision(Vector2 pt)
	{
		// TODO(verify): from 2019 baseline
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].OverlapPoint(pt))
			{
				return true;
			}
		}
		return false;
	}

	public void SetText(string input, string inputCompo = "")
	{
		bool flag = false;
		AdjustCaretPosition(input.Length - this.text.Length);
		char c = ' ';
		tempTxt.Clear();
		for (int i = 0; i < input.Length; i++)
		{
			char c2 = input[i];
			if (c == ' ' && c2 == ' ')
			{
				AdjustCaretPosition(-1);
				continue;
			}
			if (c2 == '\r' || c2 == '\n')
			{
				flag = true;
			}
			if (c2 == '\b')
			{
				tempTxt.Length = Math.Max(tempTxt.Length - 1, 0);
				AdjustCaretPosition(-1);
			}
			if (ForceUppercase)
			{
				c2 = char.ToUpperInvariant(c2);
			}
			if (!IsCharAllowed(c2))
			{
				AdjustCaretPosition(-1);
				continue;
			}
			tempTxt.Append(c2);
			c = c2;
		}
		if (!tempTxt.ToString().Equals(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EnterName), StringComparison.OrdinalIgnoreCase) && characterLimit > 0 && !allowAllCharacters)
		{
			int length = tempTxt.Length;
			tempTxt.Length = Math.Min(tempTxt.Length, characterLimit);
			AdjustCaretPosition(-(length - tempTxt.Length));
		}
		input = tempTxt.ToString();
		if (!input.Equals(this.text) || !inputCompo.Equals(compoText))
		{
			this.text = input;
			compoText = inputCompo;
			string text = this.text;
			string text2 = compoText;
			if (Hidden)
			{
				text = "";
				for (int j = 0; j < this.text.Length; j++)
				{
					text += "*";
				}
			}
			outputText.text = text + text2;
			outputText.ForceMeshUpdate(ignoreActiveState: true, forceTextReparsing: true);
			if (keyboard != null)
			{
				keyboard.text = this.text;
			}
			OnChange.Invoke();
			if (tempTxt.Length == characterLimit && SendOnFullChars)
			{
				OnEnter.Invoke();
				LoseFocus();
			}
		}
		if (flag)
		{
			OnEnter.Invoke();
		}
		SetPipePosition();
	}

	public bool IsCharAllowed(char i)
	{
		if (allowAllCharacters || AllowSymbols)
		{
			return i >= ' ';
		}
		if (IpMode)
		{
			if (i >= '0' && i <= '9')
			{
				return true;
			}
			if (i == '.')
			{
				return true;
			}
			return false;
		}
		switch (i)
		{
		case ' ':
			return true;
		case 'A':
		case 'B':
		case 'C':
		case 'D':
		case 'E':
		case 'F':
		case 'G':
		case 'H':
		case 'I':
		case 'J':
		case 'K':
		case 'L':
		case 'M':
		case 'N':
		case 'O':
		case 'P':
		case 'Q':
		case 'R':
		case 'S':
		case 'T':
		case 'U':
		case 'V':
		case 'W':
		case 'X':
		case 'Y':
		case 'Z':
			return true;
		default:
			if (i >= 'a' && i <= 'z')
			{
				return true;
			}
			if (i >= '0' && i <= '9')
			{
				return true;
			}
			if (i >= 'À' && i <= 'ÿ')
			{
				return true;
			}
			if (i >= 'Ѐ' && i <= 'џ')
			{
				return true;
			}
			if (i >= '\u3040' && i <= '㆟')
			{
				return true;
			}
			if (i >= 'ⱡ' && i <= '힣')
			{
				return true;
			}
			if (AllowSymbols && SymbolChars.Contains(i))
			{
				return true;
			}
			if (AllowEmail && EmailChars.Contains(i))
			{
				return true;
			}
			return false;
		}
	}

	private unsafe void MoveCaret()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			caretRepeatTimer = 0f;
			pipeBlinkTimer = 0f;
			caretPos = Mathf.Clamp(caretPos - 1, 0, text.Length);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			caretRepeatTimer = 0f;
			pipeBlinkTimer = 0f;
			caretPos = Mathf.Clamp(caretPos + 1, 0, text.Length);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			caretRepeatTimer += Time.deltaTime;
			pipeBlinkTimer = 0f;
			if (caretRepeatTimer > 0.2f)
			{
				caretRepeatTimer = 0f;
				caretPos = Mathf.Clamp(caretPos - 1, 0, text.Length);
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			caretRepeatTimer += Time.deltaTime;
			pipeBlinkTimer = 0f;
			if (caretRepeatTimer > 0.2f)
			{
				caretRepeatTimer = 0f;
				caretPos = Mathf.Clamp(caretPos + 1, 0, text.Length);
			}
		}
		else
		{
			caretRepeatTimer = 0f;
		}
		if ((bool)Pipe)
		{
			Pipe.transform.localPosition = outputText.CursorPos(caretPos, caretYOffset);
		}
	}

	private void AdjustCaretPosition(int adjustment)
	{
		caretPos += adjustment;
		if (caretPos < 0)
		{
			caretPos = 0;
		}
	}

	private void SetCaretPosition(int index)
	{
		caretPos = index;
		if (caretPos <= 0)
		{
			caretPos = 0;
		}
	}

	private unsafe void SetPipePosition()
	{
		if ((bool)Pipe)
		{
			Pipe.transform.localPosition = outputText.CursorPos(caretPos, caretYOffset);
		}
	}

	public TextBoxTMP()
	{ }

	static TextBoxTMP()
	{ }
}
