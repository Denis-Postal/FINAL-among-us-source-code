using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class SimonSaysGame : Minigame
{
	private Queue<int> operations = new Queue<int>();

	private const int FlashOp = 256;

	private const int AnimateOp = 128;

	private const int ReAnimateOp = 32;

	private const int FailOp = 64;

	private Color gray = new Color32(141, 141, 141, byte.MaxValue);

	private Color blue = new Color32(68, 168, byte.MaxValue, byte.MaxValue);

	private Color red = new Color32(byte.MaxValue, 58, 0, byte.MaxValue);

	private Color green = Color.green;

	public SpriteRenderer[] LeftSide;

	public SpriteRenderer[] Buttons;

	public SpriteRenderer[] LeftLights;

	public SpriteRenderer[] RightLights;

	private float flashTime = 0.25f;

	private float userButtonFlashTime = 0.175f;

	public AudioClip ButtonPressSound;

	public AudioClip FailSound;

	public Transform selectorHighlightObject;

	public float diagonalRoundingWidth = 0.6f;

	public float inputAngleIndex = -1f;

	public int roundDownIndex;

	public int roundUpIndex;

	private int[] orderedButtonIndices = new int[8] { 5, 2, 1, 0, 3, 6, 7, 8 };

	private int IndexCount
	{
		get
		{
			return MyNormTask.Data[0];
		}
		set
		{
			MyNormTask.Data[0] = (byte)value;
		}
	}

	private byte this[int idx]
	{
		get
		{
			return MyNormTask.Data[idx + 1];
		}
		set
		{
			MyNormTask.Data[idx + 1] = value;
		}
	}

	public override void Begin(PlayerTask task)
	{
		for (int i = 0; i < LeftSide.Length; i++)
		{
			LeftSide[i].color = Color.clear;
		}
		base.Begin(task);
		if (IndexCount == 0)
		{
			operations.Enqueue(128);
		}
		else
		{
			operations.Enqueue(32);
		}
		SetupInput(disableCursor: true);
		StartCoroutine(CoRun());
	}

	public void HitButton(int bIdx)
	{
		if (MyNormTask.IsComplete || MyNormTask.taskStep >= IndexCount)
		{
			return;
		}
		if (this[MyNormTask.taskStep] == bIdx)
		{
			MyNormTask.NextStep();
			SetLights(RightLights, MyNormTask.taskStep);
			if (MyNormTask.IsComplete)
			{
				SetLights(LeftLights, LeftLights.Length);
				for (int i = 0; i < Buttons.Length; i++)
				{
					SpriteRenderer spriteRenderer = Buttons[i];
					spriteRenderer.color = gray;
					StartCoroutine(FlashButton(-1, spriteRenderer, flashTime));
				}
				StartCoroutine(CoStartClose());
			}
			else
			{
				operations.Enqueue(0x100 | bIdx);
				if (MyNormTask.taskStep >= IndexCount)
				{
					operations.Enqueue(128);
				}
			}
		}
		else
		{
			IndexCount = 0;
			operations.Enqueue(64);
			operations.Enqueue(128);
		}
	}

	private IEnumerator CoRun()
	{
		while (true)
		{
			if (operations.Count > 0)
			{
				int num = operations.Dequeue();
				if (num.HasAnyBit(256))
				{
					int num2 = num & -257;
					yield return FlashButton(num2, Buttons[num2], userButtonFlashTime);
				}
				else if (num.HasAnyBit(128))
				{
					yield return CoAnimateNewLeftSide();
				}
				else if (num.HasAnyBit(32))
				{
					yield return CoAnimateOldLeftSide();
				}
				else if (num.HasAnyBit(64))
				{
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(FailSound, loop: false);
					}
					SetAllColor(red);
					yield return new WaitForSeconds(flashTime);
					SetAllColor(Color.white);
					yield return new WaitForSeconds(flashTime);
					SetAllColor(red);
					yield return new WaitForSeconds(flashTime);
					SetAllColor(Color.white);
					yield return new WaitForSeconds(flashTime / 2f);
				}
				continue;
			}
			Player player = ReInput.players.GetPlayer(0);
			Vector2 vector = new Vector2(player.GetAxis(13), player.GetAxis(14));
			int num3 = 4;
			if (vector.sqrMagnitude > 0.7f)
			{
				vector = vector.normalized;
				float num4 = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				if (num4 < 0f)
				{
					num4 += 360f;
				}
				num4 *= 0.0027777778f;
				inputAngleIndex = num4 * 8f;
				float num5 = inputAngleIndex - diagonalRoundingWidth;
				if (num5 < 0f)
				{
				}
				float num6 = inputAngleIndex - Mathf.Floor(inputAngleIndex);
				if ((int)inputAngleIndex % 2 == 1)
				{
					num3 = ((!(num6 < diagonalRoundingWidth)) ? ((int)(inputAngleIndex + 0.5f)) : ((int)inputAngleIndex));
				}
				else
				{
					num3 = ((!(num6 < 1f - diagonalRoundingWidth)) ? ((int)inputAngleIndex + 1) : ((int)inputAngleIndex));
				}
				roundUpIndex = Mathf.FloorToInt(inputAngleIndex + diagonalRoundingWidth);
				roundDownIndex = Mathf.FloorToInt(inputAngleIndex - diagonalRoundingWidth);
				num3 %= 8;
				num3 = orderedButtonIndices[num3];
			}
			if (player.GetButtonDown(11))
			{
				HitButton(num3);
				selectorHighlightObject.transform.localPosition = new Vector3(5000f, 5000f, selectorHighlightObject.transform.localPosition.z);
			}
			else
			{
				Vector3 localPosition = Buttons[num3].transform.localPosition;
				localPosition.z = selectorHighlightObject.transform.localPosition.z;
				selectorHighlightObject.transform.localPosition = localPosition;
			}
			yield return null;
		}
	}

	private void AddIndex(int idxToAdd)
	{
		this[IndexCount] = (byte)idxToAdd;
		IndexCount++;
	}

	private IEnumerator CoAnimateNewLeftSide()
	{
		SetLights(RightLights, 0);
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons[i].color = gray;
		}
		AddIndex(Buttons.RandomIdx());
		yield return CoAnimateOldLeftSide();
	}

	private IEnumerator CoAnimateOldLeftSide()
	{
		yield return new WaitForSeconds(1f);
		SetLights(LeftLights, IndexCount);
		int i = 0;
		while (i < IndexCount)
		{
			int num = this[i];
			yield return FlashButton(num, LeftSide[num], flashTime);
			yield return new WaitForSeconds(0.1f);
			int num2 = i + 1;
			i = num2;
		}
		MyNormTask.taskStep = 0;
		for (int j = 0; j < Buttons.Length; j++)
		{
			Buttons[j].color = Color.white;
		}
	}

	private IEnumerator FlashButton(int id, SpriteRenderer butt, float flashTime)
	{
		if (id > -1 && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(ButtonPressSound, loop: false).pitch = Mathf.Lerp(0.5f, 1.5f, (float)id / 9f);
		}
		Color c = butt.color;
		butt.color = blue;
		yield return new WaitForSeconds(flashTime);
		butt.color = c;
	}

	private void SetLights(SpriteRenderer[] lights, int num)
	{
		for (int i = 0; i < lights.Length; i++)
		{
			if (i < num)
			{
				lights[i].color = green;
			}
			else
			{
				lights[i].color = gray;
			}
		}
	}

	private void SetAllColor(Color color)
	{
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons[i].color = color;
		}
		for (int j = 0; j < RightLights.Length; j++)
		{
			RightLights[j].color = color;
		}
	}

	private void SetButtonColor(int i, Color color)
	{
		Buttons[i].color = color;
	}
}
