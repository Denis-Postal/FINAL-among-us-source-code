using System;
using System.Collections;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class SortableSample : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public SortableSample _003C_003E4__this;

		public Transform dropSpot;

		internal void _003CDropIntoContainer_003Eb__0()
		{
			Vector3 position = _003C_003E4__this.transform.position;
			position.z = dropSpot.position.z;
			_003C_003E4__this.transform.position = position;
		}

		internal void _003CDropIntoContainer_003Eb__1()
		{
			VibrationManager.Vibrate(0.3f, 0.3f, 0.1f, VibrationManager.VibrationFalloff.None, null, false, null);
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySoundImmediate(_003C_003E4__this.placeCorrectSfx, loop: false, 1f, 1f, null);
			}
		}
	}

	[SerializeField]
	public Collider2D coll;

	[SerializeField]
	public bool isMushroom;

	[SerializeField]
	private float distanceAboveJar;

	[SerializeField]
	private AudioClip pickUpSfx;

	[SerializeField]
	private AudioClip placeCorrectSfx;

	private Vector3 originalPosition;

	public Collider2D Collider => coll;

	public bool IsMushroom => isMushroom;

	private void Start()
	{
		originalPosition = base.transform.localPosition;
	}

	public void StartPickUp()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager instance = SoundManager.Instance;
			float pitch = default;
			AudioMixerGroup audioMixer = default;
			AudioSource audioSource = instance.PlaySoundImmediate(pickUpSfx, loop: false, 1f, pitch, audioMixer);
		}
	}

	public void SnapBackToOriginalPosition()
	{
		// decomp9: Slide2D(transform, localPosition, originalPosition, 0.25f) then re-enable collider
		coll.enabled = false;
		StartCoroutine(Effects.Sequence(new IEnumerator[] { Effects.Slide2D(base.transform, base.transform.localPosition, originalPosition, 0.25f), Effects.Action(delegate { coll.enabled = true; }) }));
	}

	public void DropIntoContainer(Transform dropSpot)
	{
		// exact from decomp_mono
		_003C_003Ec__DisplayClass13_0 cls = new _003C_003Ec__DisplayClass13_0();
		cls._003C_003E4__this = this;
		cls.dropSpot = dropSpot;
		coll.enabled = false;
		Vector3 localPosition = dropSpot.transform.localPosition;
		localPosition.x += UnityEngine.Random.Range(-0.5f, 0.5f);
		Vector3 vector = localPosition + new Vector3(0f, distanceAboveJar, 0f);
		StartCoroutine(Effects.Sequence(new IEnumerator[]
		{
			Effects.Slide2D(base.transform, base.transform.localPosition, vector, 0.4f),
			Effects.Action(cls._003CDropIntoContainer_003Eb__0),
			Effects.Slide2D(base.transform, vector, localPosition, 1f),
			Effects.Action(cls._003CDropIntoContainer_003Eb__1)
		}));
	}

	public void UpdatePosition(Vector3 pos)
	{
		float num = coll.transform.localPosition.y / 2f;
		pos.y -= num;
		base.transform.localPosition = pos;
	}

	private void _003CSnapBackToOriginalPosition_003Eb__12_0()
	{
		coll.enabled = true;
	}
}
