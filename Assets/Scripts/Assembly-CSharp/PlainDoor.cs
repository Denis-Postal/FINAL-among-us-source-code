using System.Collections;
using Hazel;
using PowerTools;
using UnityEngine;

public class PlainDoor : OpenableDoor
{
	public bool Open;

	public BoxCollider2D myCollider;

	public Collider2D shadowCollider;

	public SpriteAnim animator;

	public AnimationClip OpenDoorAnim;

	public AnimationClip CloseDoorAnim;

	public AudioClip OpenSound;

	public AudioClip CloseSound;

	private float size;

	public override bool IsOpen => Open;

	protected virtual void Start()
	{
		Vector2 vector = myCollider.size;
		size = ((vector.x > vector.y) ? vector.y : vector.x);
		Open = myCollider.isTrigger;
		animator.Play(Open ? OpenDoorAnim : CloseDoorAnim, 1000f);
		UpdateShadow();
	}

	public override void SetDoorway(bool open)
	{
		if (Open == open)
		{
			return;
		}
		Open = open;
		myCollider.isTrigger = open;
		UpdateShadow();
		animator.Play(open ? OpenDoorAnim : CloseDoorAnim);
		StopAllCoroutines();
		if (!open)
		{
			Vector2 vector = myCollider.size;
			StartCoroutine(CoCloseDoorway(vector.x > vector.y));
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlayDynamicSound(name, CloseSound, loop: false, DoorDynamics, SoundManager.Instance.SfxChannel);
			}
			VibrationManager.Vibrate(2.5f, transform.position, 3f, 0f, VibrationManager.VibrationFalloff.None, CloseSound);
		}
		else
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlayDynamicSound(name, OpenSound, loop: false, DoorDynamics, SoundManager.Instance.SfxChannel);
			}
			VibrationManager.Vibrate(2.5f, transform.position, 3f, 0f, VibrationManager.VibrationFalloff.None, OpenSound);
		}
	}

	private void UpdateShadow()
	{
		if (shadowCollider != null)
		{
			shadowCollider.enabled = !Open;
		}
	}

	private IEnumerator CoCloseDoorway(bool isHort)
	{
		Vector2 s = myCollider.size;
		float i = 0f;
		if (isHort)
		{
			while (i < 0.1f)
			{
				i += Time.deltaTime;
				s.y = Mathf.Lerp(0.0001f, size, i / 0.1f);
				myCollider.size = s;
				yield return null;
			}
		}
		else
		{
			while (i < 0.1f)
			{
				i += Time.deltaTime;
				s.x = Mathf.Lerp(0.0001f, size, i / 0.1f);
				myCollider.size = s;
				yield return null;
			}
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
		writer.Write(Open);
	}

	public override void Deserialize(MessageReader reader)
	{
		SetDoorway(reader.ReadBoolean());
	}

	public override bool DoUpdate(float dt)
	{
		return false;
	}
}
