using System.Collections;
using Rewired;
using UnityEngine;

public class AlignGame : Minigame
{
	private Controller myController = new Controller();

	public FloatRange YRange = new FloatRange(-0.425f, 0.425f);

	public AnimationCurve curve;

	public LineRenderer centerline;

	public LineRenderer[] guidelines;

	public SpriteRenderer engine;

	public Collider2D col;

	private float pulseTimer;

	private bool wasPushingJoystick;

	private float initialY;

	private TouchpadBehavior touchpad;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		float v = FromByte(MyNormTask.Data[ConsoleId]);
		Vector3 localPosition = col.transform.localPosition;
		localPosition.y = YRange.Lerp(v);
		float num = YRange.ReverseLerp(localPosition.y);
		localPosition.x = curve.Evaluate(num);
		col.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(20f, -20f, num));
		engine.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(45f, -45f, num));
		col.transform.localPosition = localPosition;
		centerline.material.SetColor("_Color", Color.red);
		engine.color = Color.red;
		guidelines[0].enabled = false;
		guidelines[1].enabled = false;
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	public void Update()
	{
		centerline.material.SetTextureOffset("_MainTex", new Vector2(Time.time, 0f));
		guidelines[0].material.SetTextureOffset("_MainTex", new Vector2(Time.time, 0f));
		guidelines[1].material.SetTextureOffset("_MainTex", new Vector2(Time.time, 0f));
		if (amClosing != CloseState.None || ((bool)MyTask && MyNormTask.IsComplete))
		{
			return;
		}
		Vector3 localPosition = col.transform.localPosition;
		Player player = ReInput.players.GetPlayer(0);
		Vector2 vector = new Vector2(player.GetAxis(13), player.GetAxis(14));
		if (vector.magnitude > 0.01f)
		{
			wasPushingJoystick = true;
			float num = YRange.ReverseLerp(localPosition.y);
			float num2 = Mathf.Clamp01(num + vector.y * Time.deltaTime);
			localPosition.y = YRange.Lerp(num2);
			localPosition.x = curve.Evaluate(num2);
			col.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(20f, -20f, num2));
			engine.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(45f, -45f, num2));
			MyNormTask.Data[ConsoleId] = ToByte(num2);
			centerline.material.SetColor("_Color", ShouldComplete(MyNormTask.Data[ConsoleId]) ? Color.green : Color.red);
			if (Mathf.Abs(num2 - num) > 0.001f)
			{
				pulseTimer += Time.deltaTime * 25f;
				switch ((int)pulseTimer % 3)
				{
				case 0:
				case 1:
					engine.color = Color.red;
					break;
				case 2:
					engine.color = Color.clear;
					break;
				}
			}
			else
			{
				engine.color = Color.red;
			}
		}
		else if (touchpad.IsTouching())
		{
			wasPushingJoystick = true;
			if (touchpad.IsFirstTouch())
			{
				initialY = localPosition.y;
			}
			float y = touchpad.GetTouchVector().y;
			float num3 = YRange.ReverseLerp(initialY);
			float num4 = Mathf.Clamp01(num3 + y);
			localPosition.y = YRange.Lerp(num4);
			localPosition.x = curve.Evaluate(num4);
			col.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(20f, -20f, num4));
			engine.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(45f, -45f, num4));
			MyNormTask.Data[ConsoleId] = ToByte(num4);
			centerline.material.SetColor("_Color", ShouldComplete(MyNormTask.Data[ConsoleId]) ? Color.green : Color.red);
			if (Mathf.Abs(num4 - num3) > 0.001f)
			{
				pulseTimer += Time.deltaTime * 25f;
				switch ((int)pulseTimer % 3)
				{
				case 0:
				case 1:
					engine.color = Color.red;
					break;
				case 2:
					engine.color = Color.clear;
					break;
				}
			}
			else
			{
				engine.color = Color.red;
			}
		}
		else if (wasPushingJoystick)
		{
			wasPushingJoystick = false;
			if (ShouldComplete(MyNormTask.Data[ConsoleId]))
			{
				MyNormTask.NextStep();
				MyNormTask.Data[ConsoleId + 2] = 1;
				StartCoroutine(LockEngine());
				StartCoroutine(CoStartClose());
			}
			else
			{
				engine.color = Color.red;
			}
		}
		myController.Update();
		switch (myController.CheckDrag(col))
		{
		case DragState.TouchStart:
			pulseTimer = 0f;
			break;
		case DragState.Dragging:
		{
			Vector2 vector2 = myController.DragPosition - (Vector2)transform.position;
			float num5 = YRange.ReverseLerp(localPosition.y);
			localPosition.y = YRange.Clamp(vector2.y);
			float num6 = YRange.ReverseLerp(localPosition.y);
			localPosition.x = curve.Evaluate(num6);
			col.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(20f, -20f, num6));
			engine.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(45f, -45f, num6));
			MyNormTask.Data[ConsoleId] = ToByte(num6);
			centerline.material.SetColor("_Color", ShouldComplete(MyNormTask.Data[ConsoleId]) ? Color.green : Color.red);
			if (Mathf.Abs(num6 - num5) > 0.001f)
			{
				pulseTimer += Time.deltaTime * 25f;
				switch ((int)pulseTimer % 3)
				{
				case 0:
				case 1:
					engine.color = Color.red;
					break;
				case 2:
					engine.color = Color.clear;
					break;
				}
			}
			else
			{
				engine.color = Color.red;
			}
			break;
		}
		case DragState.Released:
			if (ShouldComplete(MyNormTask.Data[ConsoleId]))
			{
				MyNormTask.NextStep();
				MyNormTask.Data[ConsoleId + 2] = 1;
				StartCoroutine(LockEngine());
				StartCoroutine(CoStartClose());
			}
			else
			{
				engine.color = Color.red;
			}
			break;
		}
		col.transform.localPosition = localPosition;
	}

	private IEnumerator LockEngine()
	{
		int i = 0;
		while (i < 3)
		{
			guidelines[0].enabled = true;
			guidelines[1].enabled = true;
			yield return new WaitForSeconds(0.1f);
			guidelines[0].enabled = false;
			guidelines[1].enabled = false;
			yield return new WaitForSeconds(0.1f);
			int num = i + 1;
			i = num;
		}
		Color green = new Color(0f, 0.7f, 0f);
		yield return new WaitForLerp(1f, (float t) =>
		{
			engine.color = Color.Lerp(Color.white, green, t);
		});
		guidelines[0].enabled = true;
		guidelines[1].enabled = true;
	}

	public static float FromByte(byte b)
	{
		return (float)(int)b / 256f;
	}

	public static byte ToByte(float y)
	{
		return (byte)(y * 255f);
	}

	public static bool IsSuccess(byte b)
	{
		return b != 0;
	}

	public static bool ShouldComplete(byte b)
	{
		return (float)Mathf.Abs(b - 128) < 12.75f;
	}
}
