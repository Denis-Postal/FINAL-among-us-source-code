using System;
using Cpp2ILInjected;
using PowerTools;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class ShieldMinigame : Minigame
{
	public Color OnColor= Color.white;

	public Color OffColor= Color.red;

	public SpriteRenderer[] Shields;

	public SpriteRenderer Gauge;

	private byte shields;

	public AudioClip ShieldOnSound;

	public AudioClip ShieldOffSound;

	public Transform selectedButtonHighlight;

	private Controller controller= new Controller();

	private int oldSelectedIndex= -1;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		shields = MyNormTask.Data[0];
		UpdateButtons();
		SetupInput(disableCursor: true);
	}

	public void ToggleShield(int i)
	{ /* spliced-from-2019 */
		if (MyNormTask.IsComplete)
		{
			return;
		}
		byte b = (byte)(1 << i);
		shields ^= b;
		MyNormTask.Data[0] = shields;
		if ((shields & b) != 0)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ShieldOnSound, loop: false);
			}
		}
		else if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(ShieldOffSound, loop: false);
		}
		if (shields == 127)
		{
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose());
			if (ShipStatus.Instance.ShieldsImages.Length != 0 && !ShipStatus.Instance.ShieldsImages[0].IsPlaying())
			{
				PlayerControl.LocalPlayer.RpcPlayAnimation(1);
			}
		}
	}

	private unsafe void Update()
	{
		controller.Update();
		if (Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			return;
		}
		Player player = ReInput.players.GetPlayer(0);
		Vector2 vector = new Vector2(player.GetAxis(13), player.GetAxis(14));
		int num = 0;
		if (vector.sqrMagnitude > 0.5f)
		{
			vector = vector.normalized;
			float num2 = float.NegativeInfinity;
			num = -1;
			Vector2 vector2 = Shields[0].transform.localPosition;
			for (int i = 1; i < Shields.Length; i++)
			{
				float num3 = Vector2.Dot(((Vector2)Shields[i].transform.localPosition - vector2).normalized, vector);
				if (num == -1 || num3 > num2)
				{
					num = i;
					num2 = num3;
				}
			}
		}
		if (oldSelectedIndex != num)
		{
			Vector3 localPosition = selectedButtonHighlight.transform.localPosition;
			Vector3 localPosition2 = Shields[num].transform.localPosition;
			localPosition.x = localPosition2.x;
			localPosition.y = localPosition2.y;
			selectedButtonHighlight.transform.localPosition = localPosition;
			oldSelectedIndex = num;
		}
		if (player.GetButtonDown(11))
		{
			ToggleShield(num);
		}
	}

	public void FixedUpdate()
	{
		UpdateButtons();
	}

	private unsafe void UpdateButtons()
	{ /* spliced-from-2019 */
		int num = 0;
		for (int i = 0; i < Shields.Length; i++)
		{
			bool flag = (shields & (1 << i)) == 0;
			if (!flag)
			{
				num++;
			}
			Shields[i].color = (flag ? OffColor : OnColor);
		}
		if (shields == 127)
		{
			Gauge.transform.Rotate(0f, 0f, Time.fixedDeltaTime * 45f);
			Gauge.color = new Color(1f, 1f, 1f, 1f);
		}
		else
		{
			float num2 = Mathf.Lerp(0.1f, 0.5f, (float)num / 6f);
			Gauge.color = new Color(1f, num2, num2, 1f);
		}
	}

	public ShieldMinigame()
	{ }
}
