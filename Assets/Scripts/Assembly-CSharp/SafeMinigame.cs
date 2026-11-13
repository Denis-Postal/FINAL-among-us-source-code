using System;
using System.Linq;
using Rewired;
using TMPro;
using UnityEngine;

public class SafeMinigame : Minigame
{
	private const float LowTumblerThreshold = 5f;

	private const float HighTumblerThreshold = 7f;

	private const float ReverseDirThreshold = 0.15f;

	public TextMeshPro ComboText;

	public Collider2D Tumbler;

	public SpriteRenderer TumblerBehind;

	public Collider2D Spinner;

	public SpriteRenderer[] Arrows;

	private int[] combo = new int[3];

	private bool[] latched = new bool[3];

	private bool[] vibration = new bool[3];

	private Controller controller = new Controller();

	private float lastTumDir;

	private Vector2 lastMouseVec;

	private float spinTime;

	private float spinDel;

	private float spinVel;

	private float reversalBuffer;

	public AudioClip DialTurnSound;

	public AudioClip DialGoodSound;

	public AudioClip SpinnerStartSound;

	public AudioClip SpinnerFreeSound;

	public AudioClip SpinnerStopSound;

	private AudioSource loopSound;

	private bool prevHadLeftInput;

	private bool prevHadRightInput;

	private Vector2 prevLeftInput = Vector2.zero;

	private Vector2 prevRightInput = Vector2.zero;

	private float leftStickStartAngle;

	public float TopSpinRate = 360f;

	public void OnEnable()
	{
		Begin(null);
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		Spinner.GetComponent<SpriteRenderer>().color = Palette.DisabledGrey;
		do
		{
			for (int i = 0; i < combo.Length; i++)
			{
				combo[i] = IntRange.Next(8) + 1;
			}
		}
		while (combo[0] == combo[1] || combo[1] == combo[2]);
		Vector3 eulerAngles = Tumbler.transform.eulerAngles;
		eulerAngles.z = new FloatRange(0f, 360f).NextMinDistance(combo[0] * 45, 45f);
		Tumbler.transform.eulerAngles = eulerAngles;
		combo[1] = -combo[1];
		UpdateComboInstructions();
		loopSound = SoundManager.Instance.GetNamedSfxSource("spinnerLoop");
		loopSound.volume = 0f;
		loopSound.pitch = 0.5f;
		loopSound.loop = false;
		loopSound.clip = SpinnerFreeSound;
		SetupInput(disableCursor: true);
	}

	private void UpdateComboInstructions()
	{
		int num = latched.LastIndexOf((bool b) => b) + 2;
		ComboText.text = string.Join(" - ", from c in combo.Take(num)
			select Mathf.Abs(c) - 1);
		for (int num2 = 0; num2 < Arrows.Length; num2++)
		{
			Arrows[num2].enabled = num2 < num;
		}
	}

	private void Update()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		controller.Update();
		Player player = ReInput.players.GetPlayer(0);
		float delta = 0f;
		bool flag = latched.All((bool result) => result);
		if (!flag)
		{
			int num = latched.IndexOf((bool flag3) => !flag3);
			int num2 = combo[num] * 45;
			Vector3 eulerAngles = Tumbler.transform.eulerAngles;
			if (eulerAngles.z < 0f)
			{
				eulerAngles.z += 360f;
			}
			bool flag2 = false;
			if (Controller.currentTouchType == Controller.TouchType.Joystick)
			{
				Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
				float magnitude = axis2DRaw.magnitude;
				if (magnitude > 0.9f)
				{
					axis2DRaw /= magnitude;
					if (!prevHadLeftInput)
					{
						lastTumDir = 0f;
						leftStickStartAngle = Mathf.Atan2(axis2DRaw.y, axis2DRaw.x) - eulerAngles.z * (MathF.PI / 180f);
					}
					else
					{
						delta = Vector2.SignedAngle(prevLeftInput, axis2DRaw) * (MathF.PI / 180f);
						eulerAngles.z = (Mathf.Atan2(axis2DRaw.y, axis2DRaw.x) - leftStickStartAngle) * 57.29578f;
						Tumbler.transform.eulerAngles = eulerAngles;
						flag2 = true;
					}
					prevLeftInput = axis2DRaw;
					prevHadLeftInput = true;
				}
				else
				{
					if (prevHadLeftInput && num == combo.Length - 1)
					{
						if (AngleNear(eulerAngles.z + 45f, lastTumDir, num2, 7f))
						{
							latched[num] = true;
						}
						UpdateComboInstructions();
					}
					prevHadLeftInput = false;
				}
			}
			else
			{
				switch (controller.CheckDrag(Tumbler))
				{
				case DragState.TouchStart:
				{
					lastTumDir = 0f;
					Vector2 vector = controller.DragPosition - (Vector2)Tumbler.transform.position;
					vector.Normalize();
					leftStickStartAngle = Mathf.Atan2(vector.y, vector.x) - eulerAngles.z * (MathF.PI / 180f);
					lastMouseVec = vector;
					break;
				}
				case DragState.Dragging:
				{
					Vector2 to = controller.DragPosition - (Vector2)Tumbler.transform.position;
					to.Normalize();
					delta = Vector2.SignedAngle(lastMouseVec, to) * (MathF.PI / 180f);
					eulerAngles.z = (Mathf.Atan2(to.y, to.x) - leftStickStartAngle) * 57.29578f;
					flag2 = true;
					Tumbler.transform.eulerAngles = eulerAngles;
					lastMouseVec = to;
					break;
				}
				case DragState.Released:
					if (num == combo.Length - 1)
					{
						if (AngleNear(eulerAngles.z + 45f, lastTumDir, num2, 7f))
						{
							latched[num] = true;
						}
						UpdateComboInstructions();
					}
					break;
				}
			}
			if (flag2)
			{
				CheckTumblr(delta, eulerAngles.z, num, num2);
			}
			if (latched.All((bool result) => result))
			{
				spinDel = 0f;
				controller.ClearTouch();
				TumblerBehind.color = Palette.DisabledGrey;
				Tumbler.GetComponent<SpriteRenderer>().color = Palette.DisabledGrey;
				Spinner.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Vector2 axis2DRaw2 = player.GetAxis2DRaw(16, 17);
			float magnitude2 = axis2DRaw2.magnitude;
			if (magnitude2 > 0.9f)
			{
				axis2DRaw2 /= magnitude2;
				if (!prevHadRightInput)
				{
					spinTime = 0f;
				}
				else
				{
					spinTime += Time.deltaTime;
					Vector2 vector2 = Spinner.transform.position;
					(lastMouseVec - vector2).WheelAngle(controller.DragPosition - vector2);
					lastMouseVec = controller.DragPosition;
					float f = spinVel;
					float num3 = Vector2.SignedAngle(prevRightInput, axis2DRaw2);
					spinVel += num3 / (3f * spinTime);
					if (spinVel > TopSpinRate)
					{
						spinVel = TopSpinRate;
					}
					if (spinVel < 0f - TopSpinRate)
					{
						spinVel = 0f - TopSpinRate;
					}
					if (Mathf.Sign(f) != Mathf.Sign(spinVel) && Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(SpinnerStartSound, loop: false);
					}
				}
				prevRightInput = axis2DRaw2;
				prevHadRightInput = true;
			}
			else
			{
				prevHadRightInput = false;
			}
		}
		else
		{
			switch (controller.CheckDrag(Spinner))
			{
			case DragState.TouchStart:
				lastMouseVec = controller.DragPosition;
				spinTime = 0f;
				break;
			case DragState.Dragging:
			{
				spinTime += Time.deltaTime;
				Vector2 vector3 = Spinner.transform.position;
				float num4 = (lastMouseVec - vector3).WheelAngle(controller.DragPosition - vector3);
				lastMouseVec = controller.DragPosition;
				float f2 = spinVel;
				spinVel += num4 / (2.5f * spinTime);
				if (spinVel > TopSpinRate)
				{
					spinVel = TopSpinRate;
				}
				if (spinVel < 0f - TopSpinRate)
				{
					spinVel = 0f - TopSpinRate;
				}
				if (Mathf.Sign(f2) != Mathf.Sign(spinVel) && Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(SpinnerStartSound, loop: false);
				}
				break;
			}
			}
		}
		if (Constants.ShouldPlaySfx())
		{
			float num5 = Mathf.Min(1f, Mathf.Abs(spinVel / 60f));
			float volume = num5 * num5 * num5;
			loopSound.volume = volume;
			float b = Mathf.Lerp(0.5f, 1.25f, num5);
			loopSound.pitch = Mathf.Lerp(loopSound.pitch, b, Time.deltaTime);
		}
		float f3 = Spinner.transform.localEulerAngles.z - 180f;
		float num6 = spinVel * Time.deltaTime;
		Spinner.transform.Rotate(0f, 0f, num6);
		float f4 = Spinner.transform.localEulerAngles.z - 180f;
		if (Constants.ShouldPlaySfx() && Mathf.Sign(f3) != Mathf.Sign(f4))
		{
			loopSound.Play();
		}
		spinDel += num6;
		spinVel = Mathf.Lerp(spinVel, 0f, Time.deltaTime);
		if (flag && Mathf.Abs(spinDel) > 720f)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(SpinnerStopSound, loop: false);
			}
			MyNormTask.NextStep();
			Close();
		}
	}

	private void CheckTumblr(float delta, float tumRotZ, int unlatched, int expected)
	{
		float num = lastTumDir;
		float num2 = 0f - Mathf.Sign(delta);
		if (num2 != 0f && num != num2)
		{
			reversalBuffer += delta;
			if (Mathf.Abs(reversalBuffer) > 0.15f)
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(DialTurnSound, loop: false);
				}
				lastTumDir = 0f - Mathf.Sign(delta);
				reversalBuffer = 0f;
			}
		}
		if (num != 0f && lastTumDir != num)
		{
			if (AngleNear(tumRotZ + 45f, num, expected, 7f))
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(DialGoodSound, loop: false);
				}
				latched[unlatched] = true;
			}
			else
			{
				latched = new bool[3];
				vibration = new bool[3];
			}
			UpdateComboInstructions();
		}
		else
		{
			if (!AngleNear(tumRotZ + 45f, num, expected, 5f))
			{
				return;
			}
			latched[unlatched] = true;
			UpdateComboInstructions();
			latched[unlatched] = false;
			if (AngleNear(tumRotZ + 45f, num, expected, 7f))
			{
				reversalBuffer = 0.15f * (0f - num2);
				if (!vibration[unlatched])
				{
					VibrationManager.Vibrate(0.5f, 0.5f, 0.2f, VibrationManager.VibrationFalloff.Linear);
					vibration[unlatched] = true;
				}
			}
		}
	}

	public override void Close()
	{
		SoundManager.Instance.StopNamedSound("spinnerLoop");
		base.Close();
	}

	private bool AngleNear(float actual, float dir, float expected, float Threshold)
	{
		if (actual < 0f)
		{
			actual += 360f;
		}
		if (Mathf.Sign(dir) != Mathf.Sign(expected))
		{
			return false;
		}
		expected = Mathf.Abs(expected);
		if (actual < 90f && expected > 270f)
		{
			actual += 360f;
		}
		if (expected < 90f && actual > 270f)
		{
			expected += 360f;
		}
		if (actual >= expected - Threshold)
		{
			return actual <= expected + Threshold;
		}
		return false;
	}
}
