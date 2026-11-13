using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class MonitorOxyMinigame : Minigame
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<BoxCollider2D, bool> _003C_003E9__16_0;

		public static Func<BoxCollider2D, bool> _003C_003E9__16_1;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CUpdate_003Eb__16_0(BoxCollider2D s)
		{
			return !s.enabled;
		}

		internal bool _003CUpdate_003Eb__16_1(BoxCollider2D s)
		{
			return !s.enabled;
		}
	}

	public SpriteRenderer[] Targets;

	public BoxCollider2D[] Sliders;

	public VerticalSpriteGauge[] Fills;

	public FloatRange YRange;

	public FloatRange[] RandomRanges;

	private Controller controller= new Controller();

	public AudioClip[] DragSounds;

	private AudioSource ActiveSound;

	public Transform selectorObject;

	private TouchpadBehavior touchpad;

	private float initialY;

	private int selectedIndex;

	private bool prevHadInput;

	private float selectCooldown;

	private bool isTouchInput;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		for (int i = 0; i < Sliders.Length; i++)
		{
			BoxCollider2D obj = Sliders[i];
			Vector3 localPosition = obj.transform.localPosition;
			localPosition.y = RandomRanges[i].Next();
			obj.transform.localPosition = localPosition;
			float value = YRange.ReverseLerp(localPosition.y);
			Fills[i].Value = value;
		}
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	public void Update()
	{
		controller.Update();
		Vector2 vector = ReInput.players.GetPlayer(0).GetAxis2DRaw(13, 14);
		if (touchpad.IsTouching())
		{
			vector = touchpad.GetTouchVector();
			vector.x *= 0.5f;
		}
		if (Mathf.Abs(vector.x) > 0.7f)
		{
			if (selectCooldown <= 0f)
			{
				selectedIndex += (int)Mathf.Sign(vector.x);
				selectedIndex = Mathf.Clamp(selectedIndex, 0, Sliders.Length - 1);
				selectorObject.SetParent(Sliders[selectedIndex].transform, worldPositionStays: false);
				if (prevHadInput)
				{
					if ((bool)ActiveSound)
					{
						ActiveSound.Stop();
					}
					prevHadInput = false;
				}
				selectCooldown = 0.2f;
			}
		}
		else
		{
			selectCooldown = 0f;
		}
		BoxCollider2D boxCollider2D = Sliders[selectedIndex];
		if (boxCollider2D.enabled && Mathf.Abs(vector.x) < 0.7f && !isTouchInput)
		{
			if (Mathf.Abs(vector.y) > 0.05f || touchpad.IsTouching())
			{
				if (!prevHadInput && Constants.ShouldPlaySfx())
				{
					ActiveSound = SoundManager.Instance.PlaySound(DragSounds[selectedIndex], loop: true, 0.7f, SoundManager.Instance.SfxChannel);
				}
				Vector3 localPosition = boxCollider2D.transform.localPosition;
				if (touchpad.IsTouching())
				{
					if (touchpad.IsFirstTouch())
					{
						initialY = boxCollider2D.transform.localPosition.y;
					}
					localPosition.y = vector.y + initialY;
				}
				else
				{
					localPosition.y += vector.y * 2f * Time.deltaTime;
				}
				localPosition.y = YRange.Clamp(localPosition.y);
				boxCollider2D.transform.localPosition = localPosition;
				float num = YRange.ReverseLerp(localPosition.y);
				Fills[selectedIndex].Value = num;
				if ((bool)ActiveSound)
				{
					ActiveSound.pitch = Mathf.Lerp(0.8f, 1.2f, num);
				}
				prevHadInput = true;
			}
			else
			{
				if (prevHadInput && (bool)ActiveSound)
				{
					ActiveSound.Stop();
				}
				SpriteRenderer spriteRenderer = Targets[selectedIndex];
				if (Mathf.Abs(boxCollider2D.transform.localPosition.y - spriteRenderer.transform.localPosition.y) < 0.1f)
				{
					boxCollider2D.enabled = false;
					spriteRenderer.color = Color.green;
					if (Sliders.All((BoxCollider2D s) => !s.enabled))
					{
						MyNormTask.NextStep();
						StartCoroutine(CoStartClose());
					}
					VibrationManager.Vibrate(0.2f, 0.2f, 0.2f);
				}
				prevHadInput = false;
			}
		}
		for (int num2 = 0; num2 < Sliders.Length; num2++)
		{
			BoxCollider2D boxCollider2D2 = Sliders[num2];
			if (!boxCollider2D2.enabled)
			{
				continue;
			}
			switch (controller.CheckDrag(boxCollider2D2))
			{
			case DragState.TouchStart:
				if (Constants.ShouldPlaySfx())
				{
					ActiveSound = SoundManager.Instance.PlaySound(DragSounds[num2], loop: true, 0.7f, SoundManager.Instance.SfxChannel);
				}
				isTouchInput = true;
				break;
			case DragState.Dragging:
			{
				Vector2 vector2 = controller.DragPosition - (Vector2)boxCollider2D2.transform.parent.position;
				Vector3 localPosition2 = boxCollider2D2.transform.localPosition;
				localPosition2.y = YRange.Clamp(vector2.y);
				boxCollider2D2.transform.localPosition = localPosition2;
				float num3 = YRange.ReverseLerp(localPosition2.y);
				Fills[num2].Value = num3;
				if ((bool)ActiveSound)
				{
					ActiveSound.pitch = Mathf.Lerp(0.8f, 1.2f, num3);
				}
				break;
			}
			case DragState.Released:
			{
				if ((bool)ActiveSound)
				{
					ActiveSound.Stop();
				}
				SpriteRenderer spriteRenderer2 = Targets[num2];
				if (Mathf.Abs(boxCollider2D2.transform.localPosition.y - spriteRenderer2.transform.localPosition.y) < 0.1f)
				{
					boxCollider2D2.enabled = false;
					spriteRenderer2.color = Color.green;
					if (Sliders.All((BoxCollider2D s) => !s.enabled))
					{
						MyNormTask.NextStep();
						StartCoroutine(CoStartClose());
					}
					VibrationManager.Vibrate(0.2f, 0.2f, 0.2f);
				}
				isTouchInput = false;
				break;
			}
			}
		}
	}

	public override void Close()
	{
		if ((bool)ActiveSound)
		{
			ActiveSound.Stop();
		}
		base.Close();
	}

	public MonitorOxyMinigame()
	{ }
}
