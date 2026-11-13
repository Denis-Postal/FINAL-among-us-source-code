using System.Collections;
using Hazel;
using UnityEngine;

public class ManualDoor : SomeKindaDoor
{
	public bool Opening;

	public BoxCollider2D myCollider;

	public Collider2D shadowCollider;

	public SpriteRenderer image;

	private float size;

	public float OpenDuration = 0.3f;

	private float openTimer;

	private float openValue;

	public AudioClip OpenSound;

	public AudioClip CloseSound;

	private void Awake()
	{
		Vector2 vector = myCollider.size;
		size = ((vector.x > vector.y) ? vector.y : vector.x);
		image.SetCooldownNormalizedUvs();
		Opening = myCollider.isTrigger;
	}

	private void Update()
	{
		if (Opening && openTimer < OpenDuration)
		{
			openTimer += Time.deltaTime;
			openValue = Mathf.SmoothStep(0f, 1f, openTimer / OpenDuration);
			image.material.SetFloat("_PercentY", openValue);
		}
		else if (!Opening && openTimer > 0f)
		{
			openTimer -= Time.deltaTime;
			openValue = Mathf.SmoothStep(0f, 1f, openTimer / OpenDuration);
			image.material.SetFloat("_PercentY", openValue);
		}
		if (shadowCollider != null)
		{
			bool flag = openValue <= 0.01f;
			if (shadowCollider.enabled != flag)
			{
				shadowCollider.enabled = flag;
			}
		}
	}

	public override void SetDoorway(bool open)
	{
		if (Opening == open)
		{
			return;
		}
		Opening = open;
		myCollider.isTrigger = open;
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

	public virtual void Serialize(MessageWriter writer)
	{
		writer.Write(Opening);
	}

	public virtual void Deserialize(MessageReader reader)
	{
		SetDoorway(reader.ReadBoolean());
	}
}
