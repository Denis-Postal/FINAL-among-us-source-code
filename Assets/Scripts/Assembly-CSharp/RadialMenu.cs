using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RadialMenu : MonoBehaviour
{
	public struct CachedButtonObject
	{
		public TextMeshPro tr;

		public Transform textHandle;

		public RectTransform trRT;

		public GameObject gameObject;

		public PassiveButton button;

		public ButtonRolloverHandler rolloverHandler;

		public bool isNew;

		public PolygonCollider2D buttonCollider;

		public List<Vector2> colliderPoints;

		public int buttonIndex;

		public SpriteRenderer iconSR;

		private Vector3 baseTextPos;

		public Vector3 buttonDir;

		public CachedButtonObject(GameObject go, int bIndex)
		{
			tr = go.GetComponentInChildren<TextMeshPro>(includeInactive: true);
			textHandle = go.transform.GetChild(0);
			trRT = tr.GetComponent<RectTransform>();
			button = go.GetComponent<PassiveButton>();
			iconSR = go.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
			buttonCollider = button.GetComponent<PolygonCollider2D>();
			rolloverHandler = go.GetComponent<ButtonRolloverHandler>();
			gameObject = go;
			isNew = true;
			colliderPoints = new List<Vector2>();
			buttonIndex = bIndex;
			baseTextPos = tr.transform.localPosition;
			buttonDir = Vector3.zero;
		}

		public unsafe void AddIcon(Sprite iconSprite)
		{
			iconSR.enabled = true;
			iconSR.sprite = iconSprite;
			Vector3 localPosition = textHandle.localPosition;
			float t = Mathf.Abs(localPosition.y);
			float num = Mathf.Lerp(0.35f, 0.15f, t);
			localPosition.z = 0f;
			localPosition = localPosition.normalized;
			tr.transform.localPosition = baseTextPos + localPosition * num;
			iconSR.transform.localPosition = localPosition * (0f - num);
		}

		public unsafe void ResetIcon()
		{
			iconSR.enabled = false;
			tr.transform.localPosition = baseTextPos;
		}
	}

	private sealed class _003C_003Ec__DisplayClass28_0
	{
		public int tempButtonCount;

		public RadialMenu _003C_003E4__this;

		internal void _003CCacheButtons_003Eb__0()
		{
			RadialMenu radialMenu = _003C_003E4__this;
			if (ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick)
			{
				radialMenu.mouseSelectedButton = tempButtonCount;
			}
		}
	}

	public Mesh mesh;

	public GameObject perButtonTemplateObject;

	private MeshFilter mf;

	private MeshRenderer mr;

	private int radialDivisions = -1;

	public float sliceDegreesPerVert = 10f;

	public float textPositionDistance = 0.5f;

	public bool angleText;

	public int testRadialDivs = 5;

	private MaterialPropertyBlock mbp;

	private Controller cont= new Controller();

	public Transform arrowRotateHandle;

	public Transform arrowScaleHandle;

	public Transform inputGlyph;

	public List<CachedButtonObject> cachedButtons= new List<CachedButtonObject>();

	[NonSerialized]
	public Vector3[] buttonCenters;

	[NonSerialized]
	public Vector3[] buttonLeftEdges;

	[NonSerialized]
	public Vector3[] buttonRightEdges;

	private const float startOffset = -MathF.PI / 2f;

	public static int instances;

	[HideInInspector]
	public int prevSelectedButton= -1;

	[HideInInspector]
	public int mouseSelectedButton= -1;

	private const int cutoffLength = 12;

	public float horizontalSqueezedWidth;

	private void OnEnable()
	{
		mf = GetComponent<MeshFilter>();
		mr = GetComponent<MeshRenderer>();
		mbp = new MaterialPropertyBlock();
		instances++;
	}

	private void OnDisable()
	{
		instances--;
	}

	public void ButtonMouseOver(int which)
	{
		if (Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			mouseSelectedButton = which;
		}
	}

	public void ButtonMouseExit()
	{
		if (Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			mouseSelectedButton = -1;
		}
	}

	private unsafe void Update()
	{
		cont.Update();
		Player player = ReInput.players.GetPlayer(0);
		Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
		int num = -1;
		if (Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			num = mouseSelectedButton;
			if (arrowRotateHandle.gameObject.activeSelf)
			{
				arrowRotateHandle.gameObject.SetActive(value: false);
			}
		}
		else
		{
			mouseSelectedButton = -1;
			if (!arrowRotateHandle.gameObject.activeSelf)
			{
				arrowRotateHandle.gameObject.SetActive(value: true);
			}
		}
		float magnitude = axis2DRaw.magnitude;
		if (magnitude > 0.025f)
		{
			arrowScaleHandle.localScale = new Vector3(magnitude, 1f);
			axis2DRaw = axis2DRaw.normalized;
			float num2 = Mathf.Atan2(axis2DRaw.y, axis2DRaw.x);
			if (num2 < 0f)
			{
				num2 += MathF.PI * 2f;
			}
			arrowRotateHandle.localRotation = Quaternion.Euler(0f, 0f, num2 * 57.29578f);
			if (magnitude > 0.5f)
			{
				num2 += -MathF.PI / 2f;
				num = (int)((float)radialDivisions * (num2 / (MathF.PI * 2f) + 0.5f));
				if (num < 0)
				{
					num += radialDivisions;
				}
				else if (num >= radialDivisions)
				{
					num -= radialDivisions;
				}
			}
		}
		else
		{
			arrowScaleHandle.localScale = new Vector3(0f, 1f);
		}
		if (prevSelectedButton != num)
		{
			if (prevSelectedButton != -1)
			{
				cachedButtons[prevSelectedButton].button.OnMouseOut.Invoke();
			}
			if (num != -1)
			{
				cachedButtons[num].button.OnMouseOver.Invoke();
				if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
				{
					if (!inputGlyph.gameObject.activeSelf)
					{
						inputGlyph.gameObject.SetActive(value: true);
					}
				}
				else if (inputGlyph.gameObject.activeSelf)
				{
					inputGlyph.gameObject.SetActive(value: false);
				}
				Vector3 buttonDir = cachedButtons[num].buttonDir;
				buttonDir.z = inputGlyph.localPosition.z;
				inputGlyph.localPosition = buttonDir;
			}
			else if (inputGlyph.gameObject.activeSelf)
			{
				inputGlyph.gameObject.SetActive(value: false);
			}
		}
		if (num != -1 && player.GetButtonDown(11))
		{
			cachedButtons[num].button.ReceiveClickDown();
		}
		mr.GetPropertyBlock(mbp);
		mbp.SetInt("_SelectedSlice", num);
		mr.SetPropertyBlock(mbp);
		prevSelectedButton = num;
	}

	public CachedButtonObject[] CreateButtonsForStrings(string[] strings)
	{
		CacheButtons(strings.Length);
		BuildMesh(strings.Length);
		AlignTexts(strings);
		CachedButtonObject[] array = new CachedButtonObject[strings.Length];
		for (int i = 0; i < strings.Length; i++)
		{
			array[i] = cachedButtons[i];
		}
		return array;
	}

	private unsafe void CacheButtons(int numButtons)
	{
		while (cachedButtons.Count < numButtons)
		{
			int tempButtonCount = cachedButtons.Count;
			CachedButtonObject item = new CachedButtonObject(UnityEngine.Object.Instantiate(perButtonTemplateObject, transform), cachedButtons.Count);
			cachedButtons.Add(item);
			item.button.OnMouseOver.AddListener(() =>
			{
				ButtonMouseOver(tempButtonCount);
			});
			item.button.OnMouseOut.AddListener(() =>
			{
				ButtonMouseExit();
			});
		}
		for (int num = 0; num < numButtons; num++)
		{
			if (!cachedButtons[num].gameObject.activeSelf)
			{
				cachedButtons[num].gameObject.SetActive(value: true);
			}
		}
		for (int num2 = numButtons; num2 < cachedButtons.Count; num2++)
		{
			cachedButtons[num2].gameObject.SetActive(value: false);
		}
	}

	public float GetButtonMaxStringWidth(int whichButton, float scale)
	{
		CachedButtonObject cachedButtonObject = cachedButtons[whichButton];
		float y = cachedButtonObject.textHandle.transform.localPosition.y;
		float num = Mathf.Sqrt(1f - y * y);
		Vector3 vector = buttonLeftEdges[whichButton];
		Vector3 vector2 = buttonRightEdges[whichButton];
		Vector3 vector3 = new Vector3(0f - num, y);
		Vector3 vector4 = new Vector3(num, y);
		if (vector2.x < vector.x)
		{
			Vector3 vector5 = vector;
			vector = vector2;
			vector2 = vector5;
		}
		if (Mathf.Abs(vector.y) > 0.001f)
		{
			float num2 = y / vector.y;
			if (num2 > 0f && num2 < 1f)
			{
				vector3 = vector * num2;
			}
		}
		if (Mathf.Abs(vector2.y) > 0.001f)
		{
			float num3 = y / vector2.y;
			if (num3 > 0f && num3 < 1f)
			{
				vector4 = vector2 * num3;
			}
		}
		if (Mathf.Abs(y) <= 0.001f)
		{
			if (cachedButtonObject.textHandle.transform.localPosition.x > 0f)
			{
				vector3 = Vector3.zero;
				vector4 = Vector3.right;
			}
			else
			{
				vector3 = -Vector3.right;
				vector4 = Vector3.zero;
			}
		}
		float num4 = Mathf.Abs((vector3.x + vector4.x) * 0.5f - cachedButtonObject.textHandle.transform.localPosition.x) * 2f;
		return 0.95f * (Mathf.Abs(vector3.x - vector4.x) - num4) / scale;
	}

	private unsafe void AlignTexts(string[] strings)
	{
		float num = 1f / transform.localScale.x;
		horizontalSqueezedWidth = Mathf.Min(1f, MathF.PI * 2f / (float)strings.Length);
		for (int i = 0; i < strings.Length; i++)
		{
			Transform textHandle = cachedButtons[i].textHandle;
			textHandle.localPosition = buttonCenters[i];
			_ = strings[i].Length;
			float num2 = Mathf.Abs(((Vector2)textHandle.localPosition).normalized.y);
			num2 = num2 * num2 * num2;
			Vector3 localPosition = textHandle.localPosition;
			localPosition.x += (1f - num2) * 0.1f * Mathf.Sign(localPosition.x);
			textHandle.localPosition = localPosition;
			float buttonMaxStringWidth = GetButtonMaxStringWidth(i, num);
			Vector2 sizeDelta = cachedButtons[i].trRT.sizeDelta;
			sizeDelta.x = 19f + buttonMaxStringWidth;
			cachedButtons[i].trRT.sizeDelta = sizeDelta;
			cachedButtons[i].tr.text = strings[i];
			cachedButtons[i].buttonCollider.points = cachedButtons[i].colliderPoints.ToArray();
			cachedButtons[i].ResetIcon();
			CachedButtonObject value = cachedButtons[i];
			value.buttonDir = buttonCenters[i].normalized;
			if (cachedButtons[i].isNew)
			{
				value.isNew = false;
				textHandle.localScale *= num;
			}
			cachedButtons[i] = value;
			if (angleText)
			{
				if ((double)Mathf.Abs(textHandle.localPosition.x) <= 0.001)
				{
					textHandle.localRotation = Quaternion.identity;
					continue;
				}
				float num3 = ((!(textHandle.localPosition.x < 0f)) ? Mathf.Atan2(textHandle.localPosition.y, textHandle.localPosition.x) : Mathf.Atan2(0f - textHandle.localPosition.y, 0f - textHandle.localPosition.x));
				textHandle.localRotation = Quaternion.Euler(0f, 0f, num3 * 57.29578f);
			}
		}
	}

	private unsafe void BuildMesh(int newRadialDivisions)
	{
		if (!mesh)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh.Clear();
		}
		radialDivisions = newRadialDivisions;
		buttonCenters = new Vector3[radialDivisions];
		buttonLeftEdges = new Vector3[radialDivisions];
		buttonRightEdges = new Vector3[radialDivisions];
		float num = MathF.PI * 2f / (float)radialDivisions;
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Color> list3 = new List<Color>();
		List<Vector2> list4 = new List<Vector2>();
		float num2 = 360f / (float)radialDivisions;
		_ = MathF.PI * 2f / (float)radialDivisions;
		float num3 = 1f / (float)(radialDivisions - 1);
		int num4 = (int)(num2 / sliceDegreesPerVert);
		num4 = Mathf.Max(1, num4 - 2);
		float x = ((radialDivisions != 1) ? 0f : 100f);
		for (int i = 0; i < radialDivisions; i++)
		{
			float y = (float)i * num3;
			float num5 = (float)i * num + -MathF.PI / 2f;
			float num6 = (float)(i + 1) * num + -MathF.PI / 2f;
			float f = (num5 + num6) * 0.5f;
			buttonCenters[i] = new Vector3(Mathf.Cos(f), Mathf.Sin(f)) * textPositionDistance;
			cachedButtons[i].colliderPoints.Clear();
			Color item = Color.HSVToRGB(num5 / (MathF.PI * 2f), 1f, 1f);
			int count = list.Count;
			int num7 = count + 1;
			int num8 = num7 + 1;
			list.Add(Vector3.zero);
			list3.Add(item);
			list4.Add(new Vector2(0f, y));
			list.Add(new Vector3(Mathf.Cos(num5), Mathf.Sin(num5)));
			list.Add(new Vector3(Mathf.Cos(num6), Mathf.Sin(num6)));
			list3.Add(item);
			list3.Add(item);
			list4.Add(new Vector2(x, y));
			list4.Add(new Vector2(x, y));
			buttonLeftEdges[i] = list[num8];
			buttonRightEdges[i] = list[num7];
			Vector3 rhs = new Vector3(0f - list[num7].y, list[num7].x, 0f);
			Vector3 rhs2 = new Vector3(list[num8].y, 0f - list[num8].x, 0f);
			if (num4 == 0)
			{
				list2.Add(count);
				list2.Add(num8);
				list2.Add(num7);
			}
			else
			{
				int item2 = num7;
				for (int j = 0; j <= num4; j++)
				{
					int count2 = list.Count;
					float f2 = Mathf.Lerp(num5, num6, ((float)j + 1f) / (float)(num4 + 2));
					Vector3 vector = new Vector3(Mathf.Cos(f2), Mathf.Sin(f2));
					float num9 = Mathf.Min(Vector3.Dot(vector, rhs), Vector3.Dot(vector, rhs2));
					num9 *= 4.712389f;
					if (radialDivisions == 1)
					{
						num9 = 100f;
					}
					list.Add(vector);
					list3.Add(item);
					list4.Add(new Vector2(num9, y));
					list2.Add(count);
					list2.Add(count2);
					list2.Add(item2);
					item2 = count2;
				}
				list2.Add(count);
				list2.Add(num8);
				list2.Add(item2);
			}
			cachedButtons[i].colliderPoints.Add(list[count]);
			cachedButtons[i].colliderPoints.Add(list[num7]);
			for (int k = 0; k <= num4; k++)
			{
				cachedButtons[i].colliderPoints.Add(list[count + 3 + k]);
			}
			cachedButtons[i].colliderPoints.Add(list[num8]);
		}
		mesh.SetVertices(list);
		mesh.SetTriangles(list2.ToArray(), 0);
		mesh.SetColors(list3);
		mesh.SetUVs(0, list4.ToArray());
		if (!mf)
		{
			mf = GetComponent<MeshFilter>();
		}
		if (!mr)
		{
			mr = GetComponent<MeshRenderer>();
		}
		if (mbp == null)
		{
			mbp = new MaterialPropertyBlock();
		}
		mf.sharedMesh = mesh;
		mr.GetPropertyBlock(mbp);
		mbp.SetInt("_NumSlices", radialDivisions - 1);
		mr.SetPropertyBlock(mbp);
	}

	public RadialMenu()
	{ }

	private void _003CCacheButtons_003Eb__28_1()
	{
		//IL_0019: Expected I4, but got I8
		if (ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick)
		{
			mouseSelectedButton = -1;
		}
	}
}
