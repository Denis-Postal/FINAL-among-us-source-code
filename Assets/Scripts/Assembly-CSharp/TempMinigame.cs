using System.Collections;
using Cpp2ILInjected;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class TempMinigame : Minigame
{
	public TextMeshPro LogText;

	public TextMeshPro ReadingText;

	public IntRange LogRange;

	public IntRange ReadingRange;

	private int logValue;

	private int readingValue;

	public AudioClip ButtonSound;

	private float deltaSinceLastChangeNumber;

	public const float CHANGE_NUMBER_UPDATE_THRESHOLD_MIN = 0.05f;

	public unsafe override void Begin(PlayerTask task)
{
		base.Begin(task);
		logValue = LogRange.Next();
		readingValue = ReadingRange.Next();
		ReadingText.text = readingValue.ToString();
		ChangeNumber(0);
		SetupInput(disableCursor: true);
	}

	public unsafe void ChangeNumber(int dir)
{
		if (logValue != readingValue)
		{
			if (dir != 0 && Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ButtonSound, loop: false);
			}
			logValue += dir;
			LogText.text = logValue.ToString();
			if (logValue == readingValue)
			{
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose());
			}
			deltaSinceLastChangeNumber = 0f;
		}
	}

	private void Update()
{
		float axisRaw = ReInput.players.GetPlayer(0).GetAxisRaw(14);
		deltaSinceLastChangeNumber += Time.deltaTime;
		float num = 0.05f;
		int num2 = 0;
		if ((double)axisRaw > 0.9)
		{
			num = 0.05f;
			num2 = 1;
		}
		else if ((double)axisRaw > 0.7)
		{
			num = 0.1f;
			num2 = 1;
		}
		else if ((double)axisRaw > 0.5)
		{
			num = 0.2f;
			num2 = 1;
		}
		else if ((double)axisRaw > 0.4)
		{
			num = 0.3f;
			num2 = 1;
		}
		else if (axisRaw < -0.9f)
		{
			num = 0.05f;
			num2 = -1;
		}
		else if (axisRaw < -0.7f)
		{
			num = 0.1f;
			num2 = -1;
		}
		else if (axisRaw < -0.5f)
		{
			num = 0.2f;
			num2 = -1;
		}
		else if (axisRaw < -0.4f)
		{
			num = 0.3f;
			num2 = -1;
		}
		if (num2 != 0 && deltaSinceLastChangeNumber > num)
		{
			ChangeNumber(num2);
		}
	}
}
