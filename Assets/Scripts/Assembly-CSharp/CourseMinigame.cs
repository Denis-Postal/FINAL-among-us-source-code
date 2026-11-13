using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class CourseMinigame : Minigame
{
	[StructLayout((LayoutKind)2)]
	private struct UIntFloat
	{
		[FieldOffset(0)]
		public float FloatValue;

		[FieldOffset(0)]
		public int IntValue;

		public float GetFloat(byte[] bytes)
		{
			IntValue = bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
			return FloatValue;
		}

		public void GetBytes(float value, byte[] bytes)
		{ /* spliced-from-2019 */
			FloatValue = value;
			bytes[0] = (byte)(IntValue & 0xFF);
			bytes[1] = (byte)((IntValue >> 8) & 0xFF);
			bytes[2] = (byte)((IntValue >> 16) & 0xFF);
			bytes[3] = (byte)((IntValue >> 24) & 0xFF);
		}
	}

	public CourseStarBehaviour StarPrefab;

	public CourseStarBehaviour[] Stars;

	public SpriteRenderer DotPrefab;

	public Sprite DotLight;

	public SpriteRenderer[] Dots;

	public Collider2D Ship;

	public CourseStarBehaviour Destination;

	public Vector3[] PathPoints;

	public int NumPoints;

	public FloatRange XRange;

	public FloatRange YRange;

	public LineRenderer Path;

	public Controller myController= new Controller();

	public float lineTimer;

	private UIntFloat Converter;

	public AudioClip SetCourseSound;

	public AudioClip SetCourseLastSound;

	private TouchpadBehavior touchpad;

	private float initialCurVec;

	private float targetCurVec;

	public unsafe override void Begin(PlayerTask task)
	{
		base.Begin(task);
		PathPoints = new Vector3[NumPoints];
		Stars = new CourseStarBehaviour[NumPoints];
		Dots = new SpriteRenderer[NumPoints];
		for (int i = 0; i < PathPoints.Length; i++)
		{
			PathPoints[i].x = XRange.Lerp((float)i / ((float)PathPoints.Length - 1f));
			do
			{
				PathPoints[i].y = YRange.Next();
			}
			while (i > 0 && Mathf.Abs(PathPoints[i - 1].y - PathPoints[i].y) < YRange.Width / 4f);
			Dots[i] = Object.Instantiate(DotPrefab, transform);
			Dots[i].transform.localPosition = PathPoints[i];
			switch (i)
			{
			case 0:
				Dots[i].sprite = DotLight;
				continue;
			case 1:
				Ship.transform.localPosition = PathPoints[0];
				Ship.transform.eulerAngles = new Vector3(0f, 0f, Vector2.up.AngleSigned(PathPoints[1] - PathPoints[0]));
				break;
			}
			Stars[i] = Object.Instantiate(StarPrefab, transform);
			Stars[i].transform.localPosition = PathPoints[i];
			if (i == PathPoints.Length - 1)
			{
				Destination.transform.localPosition = PathPoints[i];
			}
		}
		Path.positionCount = PathPoints.Length;
		Path.SetPositions(PathPoints);
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	public unsafe void FixedUpdate()
	{
		float num = Converter.GetFloat(MyNormTask.Data);
		int num2 = (int)num;
		Vector2 vector = PathPoints[num2];
		myController.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			Vector2 vector2 = new Vector2(player.GetAxis(13), player.GetAxis(14));
			float magnitude = vector2.magnitude;
			if (magnitude > 0.1f)
			{
				if (num < (float)(PathPoints.Length - 1))
				{
					Vector2 vector3 = (Vector2)PathPoints[num2 + 1] - vector;
					Vector2 normalized = vector3.normalized;
					Vector2 normalized2 = vector2.normalized;
					float num3 = Vector2.Dot(normalized, normalized2);
					num = ((!(num3 > 0.7f)) ? Mathf.Max(num2, Mathf.Lerp(num, num2, Time.deltaTime * 5f)) : (num + magnitude * num3 * Time.deltaTime * 1.75f));
					Ship.transform.eulerAngles = new Vector3(0f, 0f, Vector2.up.AngleSigned(vector3));
				}
			}
			else if (touchpad.IsTouching())
			{
				if (num < (float)(PathPoints.Length - 1))
				{
					if (touchpad.IsFirstTouch())
					{
						initialCurVec = num;
						targetCurVec = Mathf.Floor(num) + 1f;
					}
					Vector2 vector4 = (Vector2)PathPoints[num2 + 1] - vector;
					Vector2 touchVector = touchpad.GetTouchVector();
					float num4 = Vector2.Dot(vector4.normalized, touchVector.normalized);
					if (num4 > 0.7f)
					{
						num = (touchVector.x + initialCurVec) * num4;
						if (num > targetCurVec)
						{
							num = (initialCurVec = targetCurVec + 0.01f);
							targetCurVec = Mathf.Floor(num) + 1f;
							touchpad.ResetTouch();
						}
					}
					else
					{
						num = Mathf.Max(num2, Mathf.Lerp(num, num2, Time.deltaTime * 5f));
					}
					if (num < initialCurVec)
					{
						num = initialCurVec;
					}
					Ship.transform.eulerAngles = new Vector3(0f, 0f, Vector2.up.AngleSigned(vector4));
				}
			}
			else
			{
				num = Mathf.Max(num2, Mathf.Lerp(num, num2, Time.deltaTime * 5f));
			}
			if (num < (float)(PathPoints.Length - 1))
			{
				float t = num - Mathf.Floor(num);
				int num5 = (int)num;
				Vector3 localPosition = Vector2.Lerp(PathPoints[num5], PathPoints[num5 + 1], t);
				localPosition.z = -1f;
				Ship.transform.localPosition = localPosition;
			}
			else
			{
				Vector3 localPosition2 = PathPoints[PathPoints.Length - 1];
				localPosition2.z = -1f;
				Ship.transform.localPosition = localPosition2;
			}
		}
		else
		{
			switch (myController.CheckDrag(Ship))
			{
			case DragState.Dragging:
				if (num < (float)(PathPoints.Length - 1))
				{
					Vector2 vector7 = (Vector2)PathPoints[num2 + 1] - vector;
					Vector2 vector8 = new Vector2(1f, vector7.y / vector7.x);
					Vector2 vector9 = (Vector2)transform.InverseTransformPoint(myController.DragPosition) - vector;
					if (vector9.x > 0f)
					{
						Vector2 vector10 = vector8 * vector9.x;
						if (Mathf.Abs(vector10.y - vector9.y) < 0.5f)
						{
							num = (float)num2 + Mathf.Min(1f, vector9.x / vector7.x);
							Vector3 localPosition5 = vector10 + vector;
							localPosition5.z = -1f;
							Ship.transform.localPosition = localPosition5;
							Ship.transform.localPosition = localPosition5;
							Ship.transform.eulerAngles = new Vector3(0f, 0f, Vector2.up.AngleSigned(vector7));
						}
						else
						{
							myController.Reset();
						}
					}
				}
				else
				{
					Vector3 localPosition6 = PathPoints[PathPoints.Length - 1];
					localPosition6.z = -1f;
					Ship.transform.localPosition = localPosition6;
				}
				break;
			case DragState.NoTouch:
				if (num < (float)(PathPoints.Length - 1))
				{
					Vector2 vector5 = (Vector2)PathPoints[num2 + 1] - vector;
					Vector2 vector6 = new Vector2(1f, vector5.y / vector5.x);
					num = Mathf.Max(num2, Mathf.Lerp(num, num2, Time.deltaTime * 5f));
					Vector3 localPosition3 = vector6 * (num - (float)num2) + vector;
					localPosition3.z = -1f;
					Ship.transform.localPosition = localPosition3;
				}
				else
				{
					Vector3 localPosition4 = PathPoints[PathPoints.Length - 1];
					localPosition4.z = -1f;
					Ship.transform.localPosition = localPosition4;
				}
				break;
			}
		}
		if ((int)num > num2 && (bool)Stars[num2 + 1])
		{
			Object.Destroy(Stars[num2 + 1].gameObject);
			Dots[num2 + 1].sprite = DotLight;
			if (num2 == PathPoints.Length - 2)
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(SetCourseLastSound, loop: false).volume = 0.7f;
				}
				Destination.Speed *= 5f;
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose());
			}
			else if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(SetCourseSound, loop: false).volume = 0.7f;
			}
		}
		Converter.GetBytes(num, MyNormTask.Data);
		SetLineDivision(num);
	}

	private void SetLineDivision(float curVec)
	{ /* spliced-from-2019 */
		int num = (int)curVec;
		float num2 = 0f;
		for (int i = 0; (float)i <= curVec && i < PathPoints.Length - 1; i++)
		{
			float num3 = Vector2.Distance(PathPoints[i], PathPoints[i + 1]);
			if (i == num)
			{
				num3 *= curVec - (float)i;
			}
			num2 += num3;
		}
		lineTimer -= Time.fixedDeltaTime;
		Vector2 value = new Vector2(lineTimer, 0f);
		Path.material.SetTextureOffset("_MainTex", value);
		Path.material.SetTextureOffset("_AltTex", value);
		Path.material.SetFloat("_Perc", num2 + lineTimer / 8f);
	}

	public CourseMinigame()
	{ }
}
