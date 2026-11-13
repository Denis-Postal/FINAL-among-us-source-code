using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class RewindTapeMinigame : Minigame
{
	private const float MaxTime = 83544f;

	private float targetTime;

	private float currentTime;

	public SpriteRenderer LeftWheel;

	public SpriteRenderer LeftTape;

	public SpriteRenderer RightWheel;

	public SpriteRenderer RightTape;

	public TextMeshPro TargetText;

	public TextMeshPro CurrentText;

	public SpriteRenderer RewindButton;

	public Sprite RewindNormal;

	public Sprite RewindDown;

	public SpriteRenderer FastFwdButton;

	public Sprite FastFwdNormal;

	public Sprite FastFwdDown;

	public SpriteRenderer PlayButton;

	public Sprite PlayNormal;

	public Sprite PlayDown;

	public SpriteRenderer PauseButton;

	public Sprite PauseNormal;

	public Sprite PauseDown;

	public SpriteRenderer RewindGlyph;

	public SpriteRenderer FastFwdGlyph;

	public SpriteRenderer PlayGlyph;

	public SpriteRenderer PauseGlyph;

	public float upGlyphYPos;

	public float pressedGlyphYPos;

	public Color upGlyphColor;

	public Color pressedGlyphColor;

	private float direction;

	public AudioClip buttonSound;

	public AudioClip playStartSound;

	public AudioClip playLoopSound;

	public AudioClip playStopSound;

	private AudioSource loopSound;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		targetTime = BitConverter.ToSingle(MyNormTask.Data, 0);
		currentTime = BitConverter.ToSingle(MyNormTask.Data, 4);
		UpdateText(TargetText, targetTime);
		UpdateText(CurrentText, currentTime);
		loopSound = SoundManager.Instance.GetNamedSfxSource("rewindLoop");
		loopSound.volume = 0f;
		loopSound.pitch = 0.5f;
		loopSound.loop = true;
		loopSound.clip = playLoopSound;
		loopSound.Play();
		SetupInput(disableCursor: true);
	}

	private void UpdateText(TextMeshPro targetText, float targetTime)
{
		int num = (int)(targetTime / 3600f);
		int num2 = (int)((targetTime - (float)(num * 3600)) / 60f);
		int num3 = (int)(targetTime % 60f);
		targetText.text = $"{num}:{num2:00}:{num3:00}";
	}

	public unsafe void Update()
{
		if (amClosing != CloseState.None)
		{
			return;
		}
		if (direction != 0f)
		{
			currentTime += direction * Time.deltaTime;
			if (currentTime < 0f)
			{
				currentTime = 0f;
				Pause();
			}
			if (currentTime > 83544f)
			{
				currentTime = 83544f;
				Pause();
			}
			if (Mathf.Abs(direction) < 120f)
			{
				if (direction < -1.05f)
				{
					direction -= 0.1f;
				}
				if (direction > 1.05f)
				{
					direction += 0.1f;
				}
			}
			UpdateText(CurrentText, currentTime);
			LeftWheel.transform.Rotate(0f, 0f, 5f * direction);
			RightWheel.transform.Rotate(0f, 0f, 5f * direction);
			float num = FloatRange.ReverseLerp(currentTime, 0f, 83544f);
			float num2 = Mathf.Lerp(0.52f, 1f, num);
			LeftTape.transform.localScale = new Vector3(num2, num2, num2);
			num2 = Mathf.Lerp(0.52f, 1f, 1f - num);
			LeftTape.transform.localScale = new Vector3(num2, num2, num2);
			if (Constants.ShouldPlaySfx())
			{
				loopSound.volume = Mathf.Lerp(loopSound.volume, 1f, Time.deltaTime);
				float b = Mathf.Lerp(0.5f, 1.25f, Mathf.Abs(direction) / 2f);
				loopSound.pitch = Mathf.Lerp(loopSound.pitch, b, Time.deltaTime);
			}
		}
		else if (Mathf.Abs(targetTime - currentTime) <= 1f)
		{
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose(0.5f));
		}
	}

	private void SetGlyphs(SpriteRenderer pressedGlyph)
	{
		SpriteRenderer[] array = new SpriteRenderer[4] { RewindGlyph, PlayGlyph, PauseGlyph, FastFwdGlyph };
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = array[i] == pressedGlyph;
			array[i].color = (flag ? pressedGlyphColor : upGlyphColor);
			SetYPos(array[i].transform, flag ? pressedGlyphYPos : upGlyphYPos);
		}
	}

	private void PlayButtonSound()
	{
		if (Constants.ShouldPlaySfx())
		{
			AudioMixerGroup audioMixer = default;
			SoundManager.Instance.PlaySound(buttonSound, loop: false, 1f, audioMixer);
		}
	}

	private unsafe void SetYPos(Transform t, float newYPos)
{
		Vector3 localPosition = t.localPosition;
		localPosition.y = newYPos;
		t.localPosition = localPosition;
	}

	public void Rewind()
{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(buttonSound, loop: false);
		}
		direction = -1.1f;
		RewindButton.sprite = RewindDown;
		PlayButton.sprite = PlayNormal;
		PauseButton.sprite = PauseNormal;
		FastFwdButton.sprite = FastFwdNormal;
		RewindGlyph.color = pressedGlyphColor;
		PlayGlyph.color = upGlyphColor;
		PauseGlyph.color = upGlyphColor;
		FastFwdGlyph.color = upGlyphColor;
		SetYPos(RewindGlyph.transform, pressedGlyphYPos);
		SetYPos(PlayGlyph.transform, upGlyphYPos);
		SetYPos(PauseGlyph.transform, upGlyphYPos);
		SetYPos(FastFwdGlyph.transform, upGlyphYPos);
	}

	public void FastForward()
{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(buttonSound, loop: false);
		}
		direction = 1.1f;
		RewindButton.sprite = RewindNormal;
		PlayButton.sprite = PlayNormal;
		PauseButton.sprite = PauseNormal;
		FastFwdButton.sprite = FastFwdDown;
		RewindGlyph.color = upGlyphColor;
		PlayGlyph.color = upGlyphColor;
		PauseGlyph.color = upGlyphColor;
		FastFwdGlyph.color = pressedGlyphColor;
		SetYPos(RewindGlyph.transform, upGlyphYPos);
		SetYPos(PlayGlyph.transform, upGlyphYPos);
		SetYPos(PauseGlyph.transform, upGlyphYPos);
		SetYPos(FastFwdGlyph.transform, pressedGlyphYPos);
	}

	public void Pause()
{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(buttonSound, loop: false);
		}
		direction = 0f;
		RewindButton.sprite = RewindNormal;
		PlayButton.sprite = PlayNormal;
		PauseButton.sprite = PauseDown;
		FastFwdButton.sprite = FastFwdNormal;
		loopSound.volume = 0f;
		loopSound.pitch = 0.5f;
		RewindGlyph.color = upGlyphColor;
		PlayGlyph.color = upGlyphColor;
		PauseGlyph.color = pressedGlyphColor;
		FastFwdGlyph.color = upGlyphColor;
		SetYPos(RewindGlyph.transform, upGlyphYPos);
		SetYPos(PlayGlyph.transform, upGlyphYPos);
		SetYPos(PauseGlyph.transform, pressedGlyphYPos);
		SetYPos(FastFwdGlyph.transform, upGlyphYPos);
	}

	public void Play()
{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(buttonSound, loop: false);
		}
		direction = 1f;
		RewindButton.sprite = RewindNormal;
		PlayButton.sprite = PlayDown;
		PauseButton.sprite = PauseNormal;
		FastFwdButton.sprite = FastFwdNormal;
		RewindGlyph.color = upGlyphColor;
		PlayGlyph.color = pressedGlyphColor;
		PauseGlyph.color = upGlyphColor;
		FastFwdGlyph.color = upGlyphColor;
		SetYPos(RewindGlyph.transform, upGlyphYPos);
		SetYPos(PlayGlyph.transform, pressedGlyphYPos);
		SetYPos(PauseGlyph.transform, upGlyphYPos);
		SetYPos(FastFwdGlyph.transform, upGlyphYPos);
	}

	public override void Close()
	{
		SoundManager.Instance.StopNamedSound("rewindLoop");
		BitConverter.GetBytes(targetTime).CopyTo(MyNormTask.Data, 0);
		BitConverter.GetBytes(currentTime).CopyTo(MyNormTask.Data, 4);
		base.Close();
	}
}
