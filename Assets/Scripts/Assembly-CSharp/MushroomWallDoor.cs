using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;
using UnityEngine.Audio;

public class MushroomWallDoor : OpenableDoor
{
	[SerializeField]
	private BoxCollider2D wallCollider;

	[SerializeField]
	private Collider2D shadowColl;

	[SerializeField]
	private Collider2D bottomColl;

	[SerializeField]
	private MushroomWallMushroom[] mushrooms;

	[SerializeField]
	private AudioClip openSound;

	[SerializeField]
	private AudioClip closeSound;

	private bool open;

	private bool allowAudio;

	public override bool IsOpen => open;

	private void Start()
	{
		allowAudio = false;
		SetDoorway(open: true);
		allowAudio = true;
	}

	public override void SetDoorway(bool open)
	{
		if (this.open == open)
		{
			return;
		}
		this.open = open;
		wallCollider.isTrigger = open;
		shadowColl.enabled = !open;
		if (bottomColl != null)
		{
			bottomColl.enabled = !open;
		}
		MushroomWallMushroom[] array = mushrooms;
		foreach (MushroomWallMushroom mushroomWallMushroom in array)
		{
			if (open)
			{
				mushroomWallMushroom.Hide();
			}
			else
			{
				mushroomWallMushroom.Show();
			}
		}
		if (open)
		{
			if (Constants.ShouldPlaySfx() && allowAudio)
			{
				SoundManager.Instance.PlayDynamicSound(name, openSound, loop: false, DoorDynamics, SoundManager.Instance.SfxChannel);
			}
			VibrationManager.Vibrate(2.5f, transform.position, 3f, 0f, VibrationManager.VibrationFalloff.None, openSound);
		}
		else
		{
			if (Constants.ShouldPlaySfx() && allowAudio)
			{
				SoundManager.Instance.PlayDynamicSound(name, closeSound, loop: false, DoorDynamics, SoundManager.Instance.SfxChannel);
			}
			VibrationManager.Vibrate(2.5f, transform.position, 3f, 0f, VibrationManager.VibrationFalloff.None, closeSound);
		}
	}

	private void DoorDynamics(AudioSource source, float dt)
	{
		if (!PlayerControl.LocalPlayer)
		{
			source.volume = 0f;
			return;
		}
		Vector2 a = transform.position;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		float num = Vector2.Distance(a, truePosition);
		if (num > 4f)
		{
			source.volume = 0f;
			return;
		}
		float b = 1f - num / 4f;
		source.volume = Mathf.Lerp(source.volume, b, dt);
	}

	public override void Serialize(MessageWriter writer)
	{
		writer.Write(open);
	}

	public override void Deserialize(MessageReader reader)
	{
		SetDoorway(reader.ReadBoolean());
	}

	public override bool DoUpdate(float deltaTime)
	{
		return false;
	}
}
